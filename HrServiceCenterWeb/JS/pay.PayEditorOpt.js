var opt = window.NameSpace || {};

function init() {
    opt.init_Buttons();
    if (dataId > 0) {
        opt.loadPayment();
    }
}

opt.init_Buttons = function () {
    if (dataId === 0) {
        $('#btnSave').attr('disabled', true);
        $('#btnExport').attr('disabled', true);
        $('#btnImport').attr('disabled', true);
        $('#btnSubmit').attr('disabled', true);
    }
    else {
        $('#btnCreate').attr('disabled', true);
    }
}
opt.autoName = function () {
    var date = $('#datebox').datebox('getText');
    var cmp = $('#cmpname').combobox('getText');
    var name = cmp + date + '工资表';
    $('#tempname').val(name);
}

opt.createPayment = function () {
    var o = HR.Form.getValues('formPayment');
    var url = '../Payment/CreatePayment';
    HR.Loader.show("loading...");
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(o),
        success: function (result) {
            if (result.success) {
                HR.Loader.hide();
                dataId = result.data;
                self.location = "../Pay/PayEditor?id=" + dataId;
            }
            else {
                $.messager.alert('提示', '保存失败！');
            }

        },
        error: function () {
            $.messager.alert('提示', '查询出错！');
        }
    });

}

opt.loadPayment = function () {
    var url = '../Payment/LoadPayment?payId=' + dataId;
    HR.Loader.show("loading...");
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: null,
        success: function (payment) {
            HR.Loader.hide();
            HR.Form.setValues('formPayment',payment);
            opt.createGrid(payment.Items, payment.Sheet);
        },
        error: function () {
            $.messager.alert('提示', '查询出错！');
        }
    });
}
opt.createGrid = function (items, table) {
    var obj = { "total": 2, "rows": table };
    var head1 = [
        { field: 'PersonId', title: 'ID',rowspan:2,width: 0},
        { field: 'PersonName', title: '姓名', rowspan:2, width: 80 },
        { field: 'PersonCode', title: '身份证', rowspan:2, width: 100 }
    ];
    var head2 = [];
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        var column = {};
        column.field = item.ItemName;
        column.title = item.ItemCaption;
        if (item.ParentId > 0) {
            column.width = 80;
            head2.push(column);
        }
        else {
            var colSpan = 1;
            for (var j = 0; j < items.length; j++) {
                if (items[j].ParentId === item.ItemId)
                    colSpan++;
            }
            if (colSpan === 1)
                column.rowspan = 2;
            else
                column.colspan = colSpan-1;
            head1.push(column);
        }

    }
    var columns = [head1, head2];

    $('#dg').datagrid({
        columns: columns,
        data: table,
        nowrap: false,
        rownumbers: false,
        singleSelect: true,
        collapsible: true,
        autoRowHeight: false,
        fitColumns: false,
        showFooter: true,
        frozenColumns: [[
        ]],
        onClickRow: function () {
        },
        onClickCell: function (rowIndex, field, value) {

        },
        striped: true
    });
}

opt.export = function () {
    var title = $('#dirTitle').val();
    var url = "../Payment/Export?payId=" + dataId;
    var params = {};

    HR.DownFile(url, params);
}
opt.import = function () {
    if (dataId === 0) return false;
    $('#winUpload').upload({
        multiple: true,
        params: { payId: dataId},
        ext: 'xlsx',
        url: '../Payment/Import?payId='+dataId,
        onAfterUpload: function (result) {
            if (result.success === true) {
                self.location.href = '../Pay/PayEditor?id=' + dataId;
            }
            else {
                alert(result.data);
            }
        }
    });

    $('#winUpload').upload('show');
}

$.fn.datebox.defaults.formatter = function (date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '-' + m;
}