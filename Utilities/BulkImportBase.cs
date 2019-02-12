using System.Collections.Generic;
using System.Web;
using SellerVendor.Enums;
using System.IO;
using SellerVendor.Dtos;
using System.Data.Entity;
using SellerVendor.Areas.Seller.Models.DBContext;
using System.Linq;
using System;
using SellerVendor.Areas.Seller.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace SellerVendor.Utilities
{
    public abstract class BulkImportBase
    {
        protected IList<ImportFileDto> UploadFileOnServer(int sellerId, FileType fileType, HttpFileCollection files)
        {

            if (files == null || files.Count == 0)
                return null; //TODO: throw exception here and catch in previous 

            string filePath = string.Empty;

            if (FileType.Settlement == fileType)
                filePath = HttpContext.Current.Server.MapPath("~/UploadExcel/" + sellerId + "/settlement/");
            else if (FileType.SalesOrder == fileType)
                filePath = HttpContext.Current.Server.MapPath("~/UploadExcel/" + sellerId + "/sales/");
            else if (FileType.Tax == fileType)
                filePath = HttpContext.Current.Server.MapPath("~/UploadExcel/" + sellerId + "/tax/");
            else
                return null; //TODO: throw exception here and catch in previous 

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var importedFiles = new List<ImportFileDto>();


            foreach (string fileString in files.AllKeys)
            {
                HttpPostedFile file = files[fileString];
                file.SaveAs(filePath + file.FileName);
                importedFiles.Add(new ImportFileDto { FileName = file.FileName, Location = filePath, IsUploaded = true, Messages = new List<string> { "Uploaded on server." }, FileType = fileType });
            }
            return importedFiles;
        }

        protected void DumpFileIntoDatabase(int sellerId, MarketPlace marketplaceId, FileType fileType, IEnumerable<ImportFileDto> files)
        {
            using (var context = new SellerContext())
            {
                //TODO : change the table to filter data based on merket place and file type
                var columnsFromDb = context.ImportFileMappingEntity.Where(x => x.MarketPlaceId == (byte)marketplaceId && x.FileType == (byte)fileType).ToList();

                foreach (var file in files)
                {
                    file.MarketPlace = marketplaceId;

                    if (string.IsNullOrWhiteSpace(file.FileName) || string.IsNullOrWhiteSpace(file.Location))
                    {
                        file.Messages.Add(" Location or file name is not correct.");
                        continue;
                    }

                    var importFileHistoryDetails = context.ImportFileHistoryEntity
                        .FirstOrDefault(a => a.SellerId == sellerId && a.FileName == file.FileName
                        && a.FileType == (byte)fileType && a.MarketPlaceId == (byte)marketplaceId);

                    if (importFileHistoryDetails != null)
                    {
                        file.FileHistoryId = importFileHistoryDetails.Id;
                        file.Messages.Add(" File is already imported into database.");
                        file.IsDumpedIntoDb = true;
                        continue;
                    }

                    importFileHistoryDetails = new ImportFileHistoryEntity
                    {
                        SellerId = sellerId,
                        MarketPlaceId = (byte)marketplaceId,
                        FileName = file.FileName,
                        FilePath = file.Location,
                        FileType = (byte)file.FileType,
                        FileStatus = (int)FileStatus.Queued,
                        CreatedOnUtc = DateTime.UtcNow,
                        LastUpdatedOnUtc = DateTime.UtcNow
                    };
                    context.ImportFileHistoryEntity.Add(importFileHistoryDetails);
                    context.SaveChanges();

                    //TODO : Move that to db or config. its tab in case of Amazon
                    var delimiter = marketplaceId == MarketPlace.Amazon ? "\t" : ",";

                    var columnsFromFile = GetFileColumns(file.FullPath, delimiter);

                    var matchingColList = (from fileCol in columnsFromFile
                                           join dbCol in columnsFromDb
                                           on fileCol.ToUpper() equals dbCol.FileColumnName.ToUpper()
                                           select dbCol.TableColumnName);

                    //TODO : correct it to log into database
                    if (matchingColList == null || matchingColList.Count() == 0 || matchingColList.Count() != columnsFromFile.Count)
                    {
                        file.Messages.Add(matchingColList.Count() != columnsFromFile.Count ?
                                                   $"Out {columnsFromFile.Count} columns {matchingColList.Count()} are matching." :
                                                    " File type is not configured to import.");

                        importFileHistoryDetails.FileStatus = (byte)FileStatus.Failed;
                        importFileHistoryDetails.Message = string.Join(", ", file.Messages).TrimStart(',');
                        context.Entry(importFileHistoryDetails).State = EntityState.Modified;
                        context.SaveChanges();
                        continue;
                    }

                    var destinationTable = GetDestinationTable(marketplaceId, file.FileType);

                    var result = DumbInDatabase(file.FullPath, importFileHistoryDetails.Id, destinationTable, delimiter, matchingColList);

                    if (result)
                    {
                        file.FileHistoryId = importFileHistoryDetails.Id;
                        importFileHistoryDetails.FileStatus = (byte)FileStatus.Imported;
                        importFileHistoryDetails.Message = "Success";
                        file.IsDumpedIntoDb = true;
                    }
                    else
                    {
                        importFileHistoryDetails.FileStatus = (byte)FileStatus.Failed;
                        importFileHistoryDetails.Message = string.Join(", ", file.Messages).TrimStart(',');
                    }

                    context.Entry(importFileHistoryDetails).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
        protected ImportFileDto GetImportedFileToProcess(ImportFileDto file, int? sellerId)
        {
            using (var context = new SellerContext())
            {
                var fileHistoryId = file?.FileHistoryId;

                var importFileHistoryDetails = context.ImportFileHistoryEntity
                        .FirstOrDefault(a => (a.SellerId == sellerId || sellerId == null) && (fileHistoryId == null || a.Id == fileHistoryId)
                        && a.FileStatus == (byte)FileStatus.Imported);

                if (importFileHistoryDetails == null)
                {
                    file.Messages.Add("Not in import state.");
                    return file;
                }

                var fileInfo = file ?? new ImportFileDto
                {
                    FileHistoryId = importFileHistoryDetails.Id,
                    FileName = importFileHistoryDetails.FileName,
                    Location = importFileHistoryDetails.FilePath,
                    FileType = (FileType)importFileHistoryDetails.FileType,
                    MarketPlace = (MarketPlace)importFileHistoryDetails.MarketPlaceId
                };

                //importFileHistoryDetails.FileStatus = (byte)FileStatus.Inprogress;
                //importFileHistoryDetails.LastUpdatedOnUtc = DateTime.UtcNow;
                //importFileHistoryDetails.Message = "Inprogress";
                //context.Entry(importFileHistoryDetails).State = EntityState.Modified;
                //context.SaveChanges();
                return fileInfo;
            }
        }

        protected void ProcessPayTmSettlement(int importFileHistoryId)
        {
            ProcessCommand("spProcessPayTmSettlementFile_V1", importFileHistoryId);
        }

        protected void ProcessPayTmSalesOrder(int importFileHistoryId)
        {
            ProcessCommand("spProcessPayTmSalesOrderFile_V1", importFileHistoryId);
        }
        protected void ProcessPayTmTaxes(int importFileHistoryId)
        {
            ProcessCommand("spProcessPayTmTaxFile_V1", importFileHistoryId);
        }
        protected void ProcessCommand(string sp, int importFileHistoryId)
        {
            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["SellerConnection"].ToString()))
            {
                using (var cmd = new MySqlCommand(sp, conn))
                {
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "@fileId", Value = importFileHistoryId, Direction = ParameterDirection.Input });
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetDestinationTable(MarketPlace marketPlace, FileType fileType)
        {
            if (marketPlace == MarketPlace.Paytm && fileType == FileType.Settlement)
                return "PayTmSettlementImport";
            else if (marketPlace == MarketPlace.Paytm && fileType == FileType.SalesOrder)
                return "PayTmSalesOrderImport";
            else if (marketPlace == MarketPlace.Paytm && fileType == FileType.Tax)
                return "PayTmTaxImport";
            else
                return string.Empty;
        }
        private bool DumbInDatabase(string filePath, int fileHistoryId, string destinationTableName, string delimiter, IEnumerable<string> columns)
        {
            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["SellerConnection"].ToString()))
            {
                try
                {
                    var query = "LOAD DATA LOCAL INFILE '" + filePath.Replace(@"\", @"/") + "'" +
                        " INTO TABLE " + destinationTableName +
                        " FIELDS TERMINATED BY '" + delimiter.Trim() + "' " +
                        " ENCLOSED BY '\"'  LINES TERMINATED BY '\n'  IGNORE 1 LINES " +
                        " (" + string.Join(",", columns) + ")" +
                        " SET ImportFileHistoryId= " + fileHistoryId.ToString();

                    var cmd = new MySqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }
        private List<string> GetFileColumns(string filePathAndName, string delimiter)
        {
            List<string> columnHeaders;
            using (var fileStream = new FileStream(filePathAndName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    var headerRow = sr.ReadLine(); //split full file text into rows  
                    columnHeaders = headerRow.Split(new string[] { delimiter }, StringSplitOptions.None).Select(x => x.Replace("\r", "").Replace("\n", "").Trim()).ToList();
                }
            }
            return columnHeaders;
        }



    }
}