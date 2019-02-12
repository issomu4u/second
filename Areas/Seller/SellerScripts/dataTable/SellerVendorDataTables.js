/*
 *  Document    =  customDataTable.js
 *  Author      =  Skarth
 *  Description =  Custom javascript code used in Tables Datatables page
 */

var CorporateRecruitmentDataTable = function (controller, columns, displayLength) {
    debugger;
    var _this = this;
    _this.Table = null;
    _this.TableSelector = ".CorporateRecruitment-data-table";
    _this.Controller = controller;
    _this.KeyValue = null;
    _this.KeyValue1 = null;
    _this.KeyValue2 = null;
    _this.KeyValue3 = null;
    _this.KeyValue4 = null;
    _this.KeyValue5 = null;

    _this.CreateFormSelector = "#formCreate";
    _this.EditFormSelector = "#formEdit";
    _this.DeleteFormSelector = "#formDelete";

    _this.CreateEditButtonSelector = ".btnSave";

    _this.CreateActionUrl = '/' + _this.Controller + "/Register";
    _this.EditActionUrl = '/' + _this.Controller + "/Save";
    _this.DeleteActionUrl = '/' + _this.Controller + "/Delete";
    _this.GetDataUrl = '/' + _this.Controller + "/GetData";

    _this.CreateModalTitle = "Create " + _this.Controller;
    _this.EditModalTitle = "Edit " + _this.Controller;
    _this.DeleteModalTitle = "Delete " + _this.Controller;
    _this.ViewModalTitle = "View " + _this.Controller;
    _this.CancelModalTitle = "Cancel " + _this.Controller;

    _this.CreateModalSelector = "#" + _this.Controller.toLowerCase() + "-modal";
    _this.EditModalSelector = "#" + _this.Controller.toLowerCase() + "-modal";
    _this.ViewModalSelector = "#view-" + _this.Controller.toLowerCase() + "-modal";
    _this.DeleteModalSelector = "#delete-" + _this.Controller.toLowerCase() + "-modal";
    _this.CancelModalSelector = "#cancel-" + _this.Controller.toLowerCase() + "-modal";
    //_this.DeleteModalSelector = "#delete-view-modal";


    _this.ClearFields = function () { };
    _this.AfterLoad = function () { };
    _this.FillFields = function () { };
    _this.AddRecord = function () {
        $('.modal-title').html('<i class="fa fa-plus"></i> ' + _this.CreateModalTitle);
        $(_this.CreateEditButtonSelector).text('Create');
        $(_this.CreateFormSelector)[0].setAttribute('action', _this.CreateActionUrl);
        _this.ClearFields();
        $(_this.CreateModalSelector).modal('show');

    };

    _this.ViewRecord = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-file-o"></i> ' + _this.ViewModalTitle);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //$(_this.ViewModalSelector).modal('show');
    };
    _this.EditRecord = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-pencil"></i> ' + _this.EditModalTitle);
        //$(_this.CreateEditButtonSelector).text('Save');
        ////$(_this.EditFormSelector)[0].setAttribute('action', _this.EditActionUrl);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //_this.ResetValidator();
        //$(_this.EditModalSelector).modal('show');

    };
    _this.CopyRecord = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-pencil"></i> ' + _this.EditModalTitle);
        //$(_this.CreateEditButtonSelector).text('Save');
        ////$(_this.EditFormSelector)[0].setAttribute('action', _this.EditActionUrl);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //_this.ResetValidator();
        //$(_this.EditModalSelector).modal('show');

    };
    _this.DeleteRecord = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-times"></i> ' + _this.DeleteModalTitle);
        ////$(_this.DeleteFormSelector)[0].setAttribute('action', _this.DeleteActionUrl);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //$(_this.DeleteModalSelector).modal('show');
    };
    _this.PrintRecord = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-times"></i> ' + _this.DeleteModalTitle);
        ////$(_this.DeleteFormSelector)[0].setAttribute('action', _this.DeleteActionUrl);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //$(_this.DeleteModalSelector).modal('show');
    };

    _this.PrintBarCode = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-times"></i> ' + _this.DeleteModalTitle);
        ////$(_this.DeleteFormSelector)[0].setAttribute('action', _this.DeleteActionUrl);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //$(_this.DeleteModalSelector).modal('show');
    };
    _this.CancelRecord = function (row, data) {
        //$('.modal-title').html('<i class="fa fa-times"></i> ' + _this.DeleteModalTitle);
        ////$(_this.DeleteFormSelector)[0].setAttribute('action', _this.DeleteActionUrl);
        //_this.ClearFields();
        //_this.FillFields(row, data);
        //$(_this.DeleteModalSelector).modal('show');
    };

    _this.Create = function () {
        _this.SubmitCrud(_this.CreateFormSelector);
        _this.SubmitCrud(_this.EditFormSelector);
        _this.SubmitCrud(_this.DeleteFormSelector);
        _this.RenameDeleteModel(_this.Controller);
        var actionColumn = {
            'data': null,
            'title': "Actions",
            'width': "95px",
            'class': "text-center",
            'sortable': false,
            'defaultContent':
				'<div>' +
					'<a class="view-row btn btn-xs btn-primary icon-space" title="Views" data-toggle="modal" href="javascript:void(0);">' +
						'<i class="fa fa-file-o"></i>' +
					'</a>' +
					'<a class="edit-row btn btn-xs btn-default icon-space" title="Edit" data-toggle="modal" href="javascript:void(0);">' +
						'<i class="fa fa-pencil"></i>' +
					'</a>' +
					'<a class="delete-row btn btn-xs btn-danger icon-space" title="Delete" data-toggle="modal" href="javascript:void(0);">' +
						'<i class="fa fa-times"></i>' +
					'</a>' +
                  '<a class="create-row btn btn-xs btn-danger icon-space" title="Status" data-toggle="modal" href="javascript:void(0);">' +
						'<i class="fa fa-times"></i>' +
					'</a>' +
                    '<a class="cancel-row btn btn-xs btn-danger icon-space" title="Cancel" data-toggle="modal" href="javascript:void(0);">' +
						'<i class="fa fa-times"></i>' +
					'</a>' +
				'</div>'
        };
        //columns.push(actionColumn);
        _this.Columns = columns;

        ///* Initialize Bootstrap Datatables Integration */
        //App.datatables();

        /* Initialize Datatables */
        _this.Table = $(_this.TableSelector).DataTable({

            "processing": true,
            "serverSide": true,
            "scrollX": true,
            "ajax": {
                "url": _this.GetDataUrl,
                "method": "POST",
                "data": function (d) {
                    d.Key1 = _this.KeyValue1;
                    d.Key = _this.KeyValue;
                    d.Key2 = _this.KeyValue2;
                    d.Key3 = _this.KeyValue3;
                    d.Key4 = _this.KeyValue4;
                    d.Key5 = _this.KeyValue5;
                }
            },
            "columns": _this.Columns,
            "pagingType": "full_numbers",
            "paginate": {
                "first": "First page"
            },
            "language": {
                "lengthMenu": "  _MENU_ Per Page",
                "paginate": {
                    "previous": '<i class="fa fa-angle-left text-large-2"></i>',
                    "next": '<i class="fa fa-angle-right text-large-2"></i>',
                    "last": '<i class="fa fa-angle-double-right text-large-2"></i>',
                    "first": '<i class="fa fa-angle-double-left text-large-2"></i>'
                }
            },
            "displayLength": displayLength,
            "lengthMenu": [[5, 10, 20, 30], [5, 10, 20, 30]],
            "dom": '<"container"<"col-xs-3"l><"col-xs-6 text-center"<"#dtRefNew">><"col-xs-3"f>>t<"container"<"col-xs-4 text-left"i>r<"col-xs-8"p>>',
            "rowCallback": function (row, data) {
                $(row).find('.view').click(function () {
                    _this.ViewRecord(row, data);
                });
                $(row).find('.edit').click(function () {
                    _this.EditRecord(row, data);
                });
                $(row).find('.copy').click(function () {
                    _this.CopyRecord(row, data);
                });
                $(row).find('.delete').click(function () {
                    _this.DeleteRecord(row, data);
                });
                $(row).find('.print').click(function () {
                    _this.PrintRecord(row, data);
                });
                $(row).find('.create').click(function () {
                    _this.CreateRecord(row, data);
                });
                $(row).find('.cancel').click(function () {
                    _this.CancelRecord(row, data);
                });
                $(row).find('.printCode').click(function () {
                    _this.PrintBarCode(row, data);
                });
            },
            "drawCallback": function () {
                _this.AfterLoad();
            }
        });

        var button = $('.dtRefNew').html();
        $('#dtRefNew').html(button);
        $('.dtRefNew').remove();

        $(_this.TableSelector).on('draw.dt', function () {
            $(".btn-add-new-record").click(function () {
                //_this.AddRecord();
                _this.AddRecord()
                //$(_this.CreateModalSelector).modal('show');
            });
            $('.btn-refresh-table').click(function () {
                _this.ReloadTable();
            });
        });
    }

    _this.ReloadTable = function () {
        _this.Table.ajax.reload(null, false);
    }

    _this.SubmitCrud = function CRUD(selector) {
        $(selector).submit(function (e) {
            e.preventDefault();
            $form = $(this);
            if ($form.valid()) {
                $.ajax({
                    url: $form.prop("action"),
                    type: "POST",
                    data: $form.serialize(),
                    success: function (jqXHR) {
                        var statusCode = jqXHR.statusCode;

                        function genreateResponeBody(alertTypeHtml, data) {

                            var responseBody = $form.parent().find(".response-body");
                            responseBody.removeClass("hidden");
                            responseBody.css("display", "block");
                            responseBody.html("");
                            responseBody.html(alertTypeHtml + data + '</div>');
                        };

                        var successAlert = '<div class="alert alert-success alert-dismissable"><button aria-hidden="true" data-dismiss="alert" class="close" type="button">x</button><h4><i class="fa fa-check-circle"></i> Success</h4>';
                        var dangerAlert = '<div class="alert alert-danger alert-dismissable"><button aria-hidden="true" data-dismiss="alert" class="close" type="button">x</button><h4><i class="fa fa-times-circle"></i> Error</h4>';

                        switch (statusCode) {
                            case 201:
                                genreateResponeBody(successAlert, jqXHR.data);
                                break;
                            case 409:
                                genreateResponeBody(dangerAlert, jqXHR.data);
                                break;
                            default:
                                genreateResponeBody(dangerAlert, jqXHR.data);
                                break;
                        };
                        _this.ReloadTable();
                    },
                    error: function (jqXHR) {
                        alert(jqXHR.responseText);

                    }
                });
            }
        });
    };

    _this.RenameDeleteModel = function (controller) {
        $("#delete-view-modal form").prop("action", "/" + controller.toLowerCase() + "/delete");
        $("#delete-view-modal").prop("id", "delete-" + controller.toLowerCase() + "-modal");
    };

    _this.ResetValidator = function resetValidation() {

        $('.field-validation-error span').remove();
        //Removes validation from input-fields
        $('.input-validation-error').addClass('input-validation-valid');
        $('.input-validation-error').removeClass('input-validation-error');
        //Removes validation message after input-fields
        $('.field-validation-error').addClass('field-validation-valid');
        $('.field-validation-error').removeClass('field-validation-error');
        //Removes validation summary 
        $('.validation-summary-errors').addClass('validation-summary-valid');
        $('.validation-summary-errors').removeClass('validation-summary-errors');

    };

    //#region Comments    
    //_this.ShowAlert = function showAlert(selector, cssClass, message, time) {
    //	if (!selector) {
    //		selector = ".info-message";
    //	}

    //	if (isTimerRunning) {
    //		window.setTimeout(function () {
    //			showAlert(selector, cssClass, message, time);
    //		}, 1000);
    //		return;
    //	}

    //	$(selector).removeClass('alert-info');
    //	$(selector).removeClass('alert-error');
    //	$(selector).removeClass('alert-success');

    //	$(selector).addClass(cssClass);
    //	$(selector + ' strong').text(message);
    //	$(selector).slideDown(1000, function () {
    //		timerRef = window.setTimeout(function () {
    //			isTimerRunning = true;
    //			$(selector).slideUp(1000, function () {
    //				$(selector).removeClass(cssClass);
    //				$(selector + ' strong').text(" ");
    //				window.clearTimeout(timerRef);
    //				$(selector).hide();
    //				isTimerRunning = false;
    //			});
    //		}, time);
    //	});
    //}
    //#endregion
};

