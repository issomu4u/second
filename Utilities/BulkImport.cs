using SellerVendor.Dtos;
using SellerVendor.Enums;
using System;
using System.Collections.Generic;
using System.Web;

namespace SellerVendor.Utilities
{
    public class BulkImport : BulkImportBase
    {
        public static object Mutex = new object();

        public IList<ImportFileDto> ImportFiles(int sellerId, byte marketPlaceId, FileType fileType)
        {
            var files = HttpContext.Current.Request.Files;

            var fileList = UploadFileOnServer(sellerId, fileType, files);

            MarketPlace marketPlace;

            // TODO check how to handle it for unwanted market place

            Enum.TryParse(marketPlaceId.ToString(), true, out marketPlace);

            DumpFileIntoDatabase(sellerId, marketPlace, fileType, fileList);

            return fileList;
        }

        //TOdO : make is to threadsafe while picking the file ;
        public ImportFileDto ProcessFile(ImportFileDto file, int? sellerId)
        {
            ImportFileDto fileToProcess;

            lock (Mutex)
            {
                fileToProcess = GetImportedFileToProcess(file, sellerId);
            }

            if (fileToProcess == null)
            {
                //TODO Prepare message and return;
                return file;
            }

            if (fileToProcess.FileType == FileType.Settlement && fileToProcess.MarketPlace == MarketPlace.Paytm)
            {
                ProcessPayTmSettlement(fileToProcess.FileHistoryId);
                file.Messages.Add("Processed Successfully.");
            }
            else if (fileToProcess.FileType == FileType.SalesOrder && fileToProcess.MarketPlace == MarketPlace.Paytm)
            {
                ProcessPayTmSalesOrder(fileToProcess.FileHistoryId);
                file.Messages.Add("Processed Successfully.");
            }
            else if (fileToProcess.FileType == FileType.Tax && fileToProcess.MarketPlace == MarketPlace.Paytm)
            {
                ProcessPayTmTaxes(fileToProcess.FileHistoryId);
                file.Messages.Add("Processed Successfully.");
            }

            return file;
        }

    }
}