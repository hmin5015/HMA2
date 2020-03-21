$(document).ready(function () {
    $("#grid").kendoGrid({
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Id: { type: "number" },
                        VersionTypeId: { type: "number" },
                        VersionName: { type: "string" },
                        VersionTypeName: { type: "string" },
                        FileName: { type: "string" },
                        FilePath: { type: "string" },
                        FileId: { type: "number" },
                        Memo: { type: "string" },
                        CreatedOn: { type: "datetime" }
                    }
                }
            },
            pageSize: 10
        },
        height: 550,
        scrollable: true,
        sortable: true,
        filterable: false,
        editable: 'inline',
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5,
            change: function (e) {
                setPageNo();
            }
        },
        columns: [
            {
                field: "Id",
                hidden: true
            },
            {
                field: "VersionTypeId",
                hidden: true
            },
            {
                field: "VersionTypeName",
                title: "Version Type",
                width: "70px",
                attributes: {
                    style: "text-align: center;"
                },
                headerAttributes: {
                    style: "text-align: center;"
                }
            },
            {
                field: "VersionName",
                title: "Version Name",
                width: "70px",
                attributes: {
                    style: "text-align: center;"
                },
                headerAttributes: {
                    style: "text-align: center;"
                }
            },
            {
                field: "FileName",
                title: "File Name",
                width: "150px",
                template: function (dataItem) {
                    return `<a download id='${dataItem.Id}' href='${dataItem.FilePath}'>${dataItem.FileName}</a>`;
                }
            },
            {
                field: "FilePath",
                hidden: true
            },
            {
                field: "FileId",
                hidden: true
            },
            {
                field: "Memo",
                width: "500px"
            },
            {
                field: "CreatedOn",
                title: "Create Date",
                width: "90px",
                attributes: {
                    style: "text-align: center;"
                },
                headerAttributes: {
                    style: "text-align: center;"
                },
                template: "#= kendo.toString(kendo.parseDate(CreatedOn, 'yyyy-MM-dd'), 'MM/dd/yyyy hh:mm:ss tt') #"
            },
            {
                command: { text: "DELETE", click: onDelete },
                title: "Manage",
                width: "60px",
                attributes: {
                    style: "text-align: center;"
                },
                headerAttributes: {
                    style: "text-align: center; font-weight: 700;"
                },
            }
        ]
    });

    let versionTypeId = parseInt($('.tab-wrapper li span.active')[0].parentNode.id);
    getVersions(versionTypeId);

    $("#files").kendoUpload({
        async: {
            saveUrl: "Version/Save",
            removeUrl: "Version/Remove",
            autoUpload: true
        },
        upload: onUpload,
        complete: onComplete
    });
});

function getVersionType(typeId) {
    switch (typeId) {
        case 1:
            return 'PDI';
        case 2:
            return 'BT';
        case 3:
            return 'DB';
        case 4:
            return 'PIM F/W';
        case 5:
            return 'GDS';
        case 6:
            return 'MM';
        case 7:
            return 'VCI F/W';
        default:
            return '';
    }
}

function onTabClick(versionType) {
    $(`.tab-wrapper li span`).removeClass('active');
    $(`.tab-wrapper li:eq(${versionType - 1}) span`).addClass('active');

    getVersions(versionType);
}

function getVersions(versionType) {
    $.ajax({
        url: '/Version/GetVersions',
        type: 'POST',
        dataType: 'json',
        data: {
            versionType: versionType
        },
        success: function (data) {
            // set Title
            $('.grid-title span')[0].textContent = getVersionType(versionType);

            // set grid data
            setGrid(data);

            // set Page No
            setPageNo();

            $('.form-wrapper').addClass('hide');
            $('.grid-wrapper').removeClass('hide');

            // clear localStorage
            localStorage.clear();
        },
        error: function (xhr, status, text) {
            // hide all modal
            hideAllModal();
        }
    });
}

function hideAllModal() {
    $('#confirm-modal').modal('hide');
    $('#message-modal').modal('hide');
}

function setPageNo() {
    let currentPage = $('span.k-link.k-pager-nav')[0].textContent,
        totalPage = $('.k-pager-last')[0].dataset.page;
        totalQty = $('.k-pager-info')[0].textContent.substr($('.k-pager-info')[0].textContent.indexOf("of") + 3, 2).trim();

    $('.page-no')[0].textContent = currentPage;
    $('.total-page')[0].textContent = totalPage;
    $('.total-qty')[0].textContent = totalQty;
}

function setGrid(data) {
    var grid = $("#grid").data("kendoGrid");
    var dataSource = new kendo.data.DataSource({
        data: data,
        pageSize: 10
    });
    grid.setDataSource(dataSource);
    grid.dataSource.read();
}

function onAdd() {
    $('.form-wrapper').removeClass('hide');
    $('.grid-wrapper').addClass('hide');

    $('#versionName').val(null);
    $('#memo').val(null);
    $('.k-icon.k-i-close.k-i-x').click();

    setTimeout(() => {
        let title = `${$('#grid tbody tr:eq(0)')[0].cells[2].textContent} - Add [Current Version : ${$('#grid tbody tr:eq(0)')[0].cells[3].textContent}]`;
        $('.add-new-title')[0].textContent = title;
        $('#versionName').focus();
    }, 500);
}

function onCancel() {
    $('.form-wrapper').addClass('hide');
    $('.grid-wrapper').removeClass('hide');
}

function onDelete(e) {
    $('#confirm-modal').modal('show');
    let dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    localStorage.setItem('dataItem', JSON.stringify(dataItem));
}

function onDeleteVersion(e) {
    let dataItem = JSON.parse(localStorage.getItem('dataItem')),
        versionId = dataItem.Id,
        fileId = dataItem.FileId,
        versionTypeId = dataItem.VersionTypeId;

    $.ajax({
        url: '/Version/DeleteVersion',
        type: 'DELETE',
        dataType: 'json',
        data: {
            versionId: versionId,
            fileId: fileId
        },
        success: function (data) {
            getVersions(versionTypeId);

            $('#confirm-modal').modal('hide');
            $('#message-modal').modal('show');
        },
        error: function (xhr, status, text) {
            getVersions(versionTypeId);
        }
    });
}

function onSave() {
    let versionDto = JSON.parse(localStorage.getItem('versionDto')),
        fileName = versionDto[0].FileName,
        filePath = versionDto[0].FilePath,
        fileExtension = versionDto[0].VersionFileMapping.File.FileExtension,
        versionTypeId = versionDto[0].VersionTypeId,
        versionName = versionDto[0].VersionName,
        memo = versionDto[0].Memo,
        fileBase64String = versionDto[0].VersionFileMapping.File.FileBase64String;

    $.ajax({
        url: '/Version/SetVersion',
        type: 'POST',
        dataType: 'json',
        data: {
            fileName: fileName,
            filePath: filePath,
            fileExtension: fileExtension,
            versionTypeId: versionTypeId,
            versionName: versionName,
            memo: memo,
            fileBase64String: fileBase64String
        },
        success: function (data) {
            getVersions(versionTypeId);
        },
        error: function (xhr, status, text) {
            getVersions(versionTypeId);
        }
    });
}

function onUpload(e) {
    let versionDto = getFileInfo(e);
    localStorage.setItem('versionDto', JSON.stringify(versionDto));
}

function onComplete(e) {
    getBase64String(e);
}

function getFileInfo(e) {
    return $.map(e.files, function (file) {
        let versionTypeId = parseInt($('.tab-wrapper li span.active')[0].parentNode.id),
            versionType = getVersionType(versionTypeId),
            filePath = `\\files\\${versionType}\\${file.name}`;

        let versionDto = {
            VersionTypeId: versionTypeId,
            VersionTypeName: versionType,
            VersionName: $('#versionName').val(),
            FileName: file.name,
            FilePath: filePath,
            Memo: $('#memo').val(),
            VersionFileMapping: {
                VersionId: null,
                FileId: null,
                File: {
                    FileName: file.name,
                    FilePath: filePath,
                    FileExtension: file.extension,
                    FileBase64String: ''
                }
            }
        };
        return versionDto;
    });
}

function getBase64String(e) {
    let reader = new FileReader();
    let file = document.querySelector('input[type=file]').files[0];
    reader.addEventListener("load", function () {
        let versionDto = JSON.parse(localStorage.getItem('versionDto'));
        versionDto[0].VersionFileMapping.File.FileBase64String = reader.result;
        localStorage.setItem('versionDto', JSON.stringify(versionDto));
    }, false);
    
    if (file) {
        reader.readAsDataURL(file);
    }
}