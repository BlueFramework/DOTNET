var opt = window.NameSpace || {};

function init() {
    opt.init_Buttons();
}

opt.init_Buttons = function () {
    if (dataId == 0) {
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

//查询发放列表
opt.query = function () {
}


opt.add = function () {
}

//编辑对象
opt.edit = function (id) {
}

$.fn.datebox.defaults.formatter = function (date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '-' + m;
}