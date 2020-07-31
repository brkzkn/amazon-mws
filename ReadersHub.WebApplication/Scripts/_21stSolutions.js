/******** _21stSolution Namespaces ********/
var _21stSolution = {};
_21stSolution = _21stSolution || {};
_21stSolution.core = _21stSolution.core || {};
_21stSolution.core.ajax = _21stSolution.core.ajax || {};
_21stSolution.core.log = _21stSolution.core.log || {};
_21stSolution.Messaging = {}
_21stSolution.Notification = {}
_21stSolution.Grid = {}
_21stSolution.Modal = {}
_21stSolution.Window = {}
_21stSolution.TopWin = window.top;
_21stSolution.TopDoc = _21stSolution.TopWin.document;

/***************************************/
_21stSolution.Window.ShowLoading = function () {
    $('#YerelFutbolOverlay').show();
}

_21stSolution.Window.HideLoading = function () {
    $('#YerelFutbolOverlay').hide();
}

/***************************************/

_21stSolution.PageReady = function () {
    console.log("page");
    _21stSolution.InitializeValidation();

}

_21stSolution.GetFormPostJsonDataFromControl = function (c) {
    var object = {};
    var input, inputName, inputVal;
    $(c).find('input').each(function () {
        input = $(this);
        inputName = input.prop('id');
        inputVal = input.val();
        if (inputName !== "") {
            switch (input.prop('type')) {
                case 'hidden':
                    if (input.attr("data") === "type-control") {
                        if (inputVal === "") {
                            object[inputName] = null;
                            break;
                        }
                        else {
                            object[inputName] = inputVal.split(',').map(Number);
                            break;
                        }
                    }
                    if (inputName && inputName !== "") {
                        object[inputName] = inputVal;
                        break;
                    }
                    break;
                case 'text':
                    object[inputName] = inputVal;
                    break;
                case 'password':
                    object[inputName] = inputVal;
                    break;
                case 'number':
                    object[inputName] = inputVal;
                    break;
                case 'checkbox':
                    inputName = input.prop('name');
                    if (input.is(':checked')) {
                        inputVal = input.val();
                        if (input.attr('checkbox-list') === 'true') {
                            if (!object[inputName]) {
                                object[inputName] = new Array();
                            }
                            object[inputName].push(inputVal);
                        }
                        else {
                            object[inputName] = inputVal;
                        }
                    }
                    break;
                case 'radio':
                    inputName = input.prop('name');
                    if (input.prop('checked') === true) {
                        inputVal = input.val();
                        object[inputName] = inputVal;
                    }
                    break;
                default:
                    break;
            }
        }
    });
    $(c).find('textarea').each(function () {
        input = $(this);
        inputName = input.prop('id');
        inputVal = input.val();
        if (inputName !== "") {
            object[inputName] = inputVal;
        }
    });

    $(c).find('select').each(function () {
        input = $(this);
        if (input.prop('multiple')) {
            inputName = input.prop('id');
            inputVal = input.val();
            if (inputVal != null && inputVal.length > 0) {
                object[inputName] = new Array();
                for (i = 0; i < inputVal.length; i++) {
                    object[inputName].push(inputVal[i]);
                }
            }
            else {
                object[inputName] = inputVal;
            }
        }
        else {
            inputName = input.prop('id');
            inputVal = input.val();
            if (inputName !== "") {
                object[inputName] = inputVal;
            }
        }
    });
    if (object !== {}) {
        return object;
    }
    else {
        return null;
    }
}

_21stSolution.GetFormPostJsonData = function (formId) {
    return _21stSolution.GetFormPostJsonDataFromControl($("#" + formId).get(0));
}


/******** _21stSolution Logging ********/

var logger = _21stSolution.core.log;

_21stSolution.core.log.debug = function (msg) {
    console.log("DEBUG: " + msg);
}

_21stSolution.core.log.info = function (msg) {
    console.log("INFO: " + msg);
}

_21stSolution.core.log.warn = function (msg) {
    console.log("WARN: " + msg);
}

_21stSolution.core.log.error = function (msg) {
    console.log("ERROR: " + msg);
}

/******** _21stSolution AJAX ********/

_21stSolution.core.ajax.post = function (url, settings) {

    logger.debug("post to url: " + url);

    if (settings && settings.data) {
        if (typeof (settings.data) === "object") {
            settings.data = JSON.stringify(settings.data);
        }

        logger.debug("Data to Send: " + settings.data);
    }

    var fnSuccess = settings.success;
    var fnError = settings.error;
    var fnComplete = settings.complete;
    var fnBeforeSend = settings.beforeSend;

    var preventDefaultSuccess = settings.preventDefaultSuccess ? true : false;
    var preventDefaultError = settings.preventDefaultError ? true : false;
    var preventDefaultBeforeSend = settings.preventDefaultBeforeSend ? true : false;
    var preventDefaultComplete = settings.preventDefaultComplete ? true : false;

    delete settings.success;
    delete settings.error;
    delete settings.complete;
    delete settings.beforeSend;

    var defaultSettings = {
        async: true,
        type: "POST",
        dataType: "json",
        contentType: 'application/json',

        success: function (data, textStatus, jqXhr) {

            logger.debug("onAjaxSuccess");
            _21stSolution.TopWin._21stSolution.Window.HideLoading();

            if (!preventDefaultSuccess) {


                if (data.RedirectUrl !== null && data.RedirectUrl !== "") {
                    window.location = data.RedirectUrl;
                }
            }

            var f = fnSuccess;
            if (f) {
                f.apply(this, [data, textStatus, jqXhr]);
            }
        },

        error: function (jqXhr, textStatus, errorThrown) {

            logger.debug("onAjaxError");
            _21stSolution.TopWin._21stSolution.Window.HideLoading();

            var resultObject = JSON.parse(jqXhr.responseText);

            if (!preventDefaultError) {
                _21stSolution.Ajax.Handle_21stSolutionPostError(resultObject);
            }

            var f = fnError;
            if (f) {

                f.apply(this, [resultObject, textStatus, errorThrown]);
            }
        },

        complete: function () {

            logger.debug("onAjaxComplete");
            if (!preventDefaultComplete) {
                _21stSolution.TopWin._21stSolution.Window.HideLoading();
            }

            var f = fnComplete;
            if (f) {
                f.apply(this, arguments);
            }
        },

        beforeSend: function () {

            logger.debug("onBeforeSend");

            if (!preventDefaultBeforeSend) {
                _21stSolution.TopWin._21stSolution.Window.ShowLoading();
            }

            var f = fnBeforeSend;
            if (f) {
                f.apply(this, arguments);
            }

            //$("#loading").show();
        }
    };

    var ajaxSettings = {};

    $.extend(ajaxSettings, defaultSettings);
    $.extend(ajaxSettings, settings);
    $.ajax(url, ajaxSettings);
};


/******** _21stSolution Messaging ********/

_21stSolution.Messaging.ShowConfirm = function (msg, okCallback, cancelCallback) {
    //alertify.confirm('ReadersHub', msg, okCallback, cancelCallback);


    bootbox.setDefaults({
        /**
         * @optional String
         * @default: en
         * which locale settings to use to translate the three
         * standard button labels: OK, CONFIRM, CANCEL
         */
        locale: "tr",
    });
    
    bootbox.confirm(msg, function (result) {
        if (result === null || result === false) {
            if (cancelCallback !== null)
                cancelCallback();
        } else {
            // result has a value
            okCallback();
        }
    });
}

_21stSolution.Messaging.ShowPrompt = function (msg, okCallback, cancelCallback) {

    var $txtDesc = $(".ajs-input");

    $txtDesc.removeClass("k-invalid");
    $txtDesc.val("");

    alertify.dialog('prompt')
            .set(
            {
                closable: false,
                transition: 'zoom',
                message: msg,
                'oncancel': cancelCallback,
                title: "",
                labels: { ok: "Onay", cancel: "Vazgeç" },
                'onok': function (evt, message) {
                    if (Modes.String.IsNullOrEmpty(message)) {
                        $(".ajs-input").addClass("k-invalid");
                        return false;
                    } else {
                        okCallback(evt, message);
                        return true;
                    }
                },
                onshow: function () {
                    $(".ajs-input").attr("onkeypress", "_21stSolution.TextBoxUpperCaseOnKeyPress(this,event)");
                    $(".ajs-input").attr("onpaste", "_21stSolution.TextBoxUpperCaseOnPaste(this,event)");
                }
            }).show();
}

_21stSolution.Messaging.ShowSuccess = function (msg, t) {
    alertify.success(msg);
    //_21stSolution.TopWin.toastr.success(msg);
}

_21stSolution.Messaging.ShowError = function (msg, t) {
    _21stSolution.TopWin.toastr.error(msg);
}

_21stSolution.Messaging.ShowWarning = function (msg, t) {

    if (t === null) {
        t = 60;
    }

    _21stSolution.TopWin.toastr.warning(msg, t);
}

_21stSolution.Messaging.ShowAlert = function (msg, t) {
    _21stSolution.TopWin.toastr.info(msg, t);
}

/******** _21stSolution Grid ********/

_21stSolution.Grid.Refresh = function (gridId, onComplete) {
    if (onComplete !== null)
        $("#" + gridId).DataTable().ajax.reload(onComplete);
    else
        $("#" + gridId).DataTable().ajax.reload();
};

_21stSolution.Grid.Render = function (gridSelector, url, ajaxError, settings) {

    var defaults = {
        paging: true,
        bFilter: false,
        bInfo: true,
        ordering: false,
        processing: true,
        select: false,
        serverSide: true,
        retrieve: true,
        ajax: {
            url: url,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: function (data) {
                return data = JSON.stringify(data);
            },
            error: function (errorInfo) {
                ajaxError(errorInfo);
            }
        },
        "aaSorting": [[0, "ASC"]],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": true,
        }],
        "language": {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun soralamasını aktifleştir"
            }
        }
    };

    _21stSolution.Notification.Clear();

    
    var tableSettings = {};

    $.extend(tableSettings, defaults);
    $.extend(tableSettings, settings);

    var table = $('#' + gridSelector).DataTable(tableSettings);

    var columns = table.settings()[0].aoColumns;
    var searchableColumnIndex = [];
    for (var i = 0; i < columns.length; i++) {
        if (table.settings()[0].aoColumns[i].bSearchable)
            searchableColumnIndex.push(i);
    }

    $('#' + gridSelector + ' tfoot th').each(function () {
        var footerIndex = $(this).index();
        if ($.inArray(footerIndex, searchableColumnIndex) >= 0) {
            var title = $('#' + gridSelector + ' thead th').eq($(this).index()).text();
            $(this).html('<input type="text" class="form-control input-sm full-width filter-input" placeholder="Filtrele ' + title + '" />');
        }
    });


    table.columns().every(function () {
        var that = this;
        var oldValue = '';
        $('.filter-input', this.footer()).on('focus', function () {
            oldValue = this.value;
        });

        $('.filter-input', this.footer()).on('blur', function () {
            if (oldValue !== this.value) {
                if (this.value === '' || this.value.length > 2) {
                    that.search(this.value).draw();
                }
            }
        });
    });

    $(".dataTables_filter").empty();
};


/******** Initialize Function ********/

_21stSolution.InitializeValidation = function () {

    if ($('form').length > 0) {
        $('form').each(function () {
            var validator = $(this).data('validator');
            if (validator && validator.settings) {
                validator.settings.ignore = ".displaynone, .k-button";
            }
        });

        var elements = $("form").find("[data-role=combobox],[data-role=dropdownlist],[data-role=multiselect],[data-role=numerictextbox],[data-role=date],[data-role=datepicker],[data-role=datetimepicker]");

        var hasMutationEvents = ("MutationEvent" in window),
            MutationObserver = window.WebKitMutationObserver || window.MutationObserver;

        if (MutationObserver) {
            var observer = new MutationObserver(function (mutations) {
                var idx = 0,
                    mutation,
                    length = mutations.length;

                for (; idx < length; idx++) {
                    mutation = mutations[idx];
                    if (mutation.attributeName === "class") {
                        updateCssOnPropertyChange(mutation);
                    }
                }
            }),
            config = { attributes: true, childList: false, characterData: false };

            elements.each(function () {
                observer.observe(this, config);
            });
        } else if (hasMutationEvents) {
            elements.bind("DOMAttrModified", updateCssOnPropertyChange);
        } else {
            elements.each(function () {
                this.attachEvent("onpropertychange", updateCssOnPropertyChange);
            });
        }
    }
}

_21stSolution.Notification.ShowErrorMessage = function (id, msg) {
    var notificationDiv = $("#" + id);
    $(notificationDiv).css("display", "block");
    $(notificationDiv).removeClass("alert-success").addClass("alert-danger");
    $(notificationDiv).find('h4').html('<i class=\"icon fa fa-ban\"></i>');
    $(notificationDiv).find('i').after('Hata');
    $(notificationDiv).find('#content').html(msg);
}

_21stSolution.Notification.ShowSuccessMessage = function (id, msg) {
    var notificationDiv = $("#" + id);
    $(notificationDiv).css("display", "block");
    $(notificationDiv).removeClass("alert-danger").addClass("alert-success");
    $(notificationDiv).find('h4').html('<i class=\"icon fa fa-check\"></i>');
    $(notificationDiv).find('i').after('Başarılı');
    $(notificationDiv).find('#content').html(msg);
}

_21stSolution.Notification.Clear = function (id) {
    var notificationDiv = $("#" + id);
    $(notificationDiv).css("display", "none");
    //$(notificationDiv).html('');
}

_21stSolution.Modal.ChangeTitle = function (id, msg) {
    var $modal = $('#' + id);
    $modal.find('.modal-title').html(msg);
}

_21stSolution.core.ConvertModelToJSArray = function (model, jsArray) {

}

_21stSolution.core.DateConvertFromJSON = function (data) {
    if (data == null) return '1/1/1950';
    var r = /\/Date\(([0-9]+)\)\//gi
    var matches = data.match(r);
    if (matches == null) return '1/1/1950';
    var result = matches.toString().substring(6, 19);
    var epochMilliseconds = result.replace(
    /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
    '$1');
    var b = new Date(parseInt(epochMilliseconds));
    var c = new Date(b.toString());

    var curr_date = c.getDate().toString();
    curr_date = curr_date.length > 1 ? curr_date : '0' + curr_date;

    var curr_month = (c.getMonth() + 1).toString();
    curr_month = curr_month.length > 1 ? curr_month : '0' + curr_month;

    var curr_year = c.getFullYear().toString();

    var curr_h = c.getHours().toString();
    curr_h = curr_h.length > 1 ? curr_h : '0' + curr_h;

    var curr_m = c.getMinutes().toString();
    curr_m = curr_m.length > 1 ? curr_m : '0' + curr_m;

    var curr_s = c.getSeconds().toString();
    curr_s = curr_s.length > 1 ? curr_s : '0' + curr_m;

    var curr_offset = c.getTimezoneOffset() / 60
    var d = curr_date.toString() + '.' + curr_month.toString() + '.' + curr_year + " " + curr_h + ':' + curr_m + ':' + curr_s;
    return d;
}
