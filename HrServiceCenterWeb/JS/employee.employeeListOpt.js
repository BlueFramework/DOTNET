﻿var opt = window.NameSpace || {};

function init() {
    var height = $('.container-layout').height() - 180;
    $('#dgContainer').height(height);
    $('#dg').datagrid('resize');
    opt.query();
}


//查询用户列表
opt.query = function () {
    var url =  '../Employee/GetEmployeeList';
    var params = { companyId:0 };
    $('#dg').datagrid('loading');
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: params,
        success: function (data) {
            $('#dg').datagrid('loadData', data);
            $('#dg').datagrid('loaded');
        },
        error: function () {
            $('#dg').datagrid('loaded');
            $.messager.alert('提示', '查询出错！');
        }
    });
}

opt.selectCompany = function (company) {
    opt.query();
}


//删除操作
opt.delete = function (id) {
    $.messager.confirm('提示窗', '您确认删除吗?', function (event) {
        if (event) {
            var row = $('#dg').datagrid('getSelected');
            var id = row.PersonId;
            $.ajax({
                type: 'POST',
                url: "../Employee/DeleteEmployee",
                data: {
                    personId: id
                },
                dataType: "json",
                success: function (result) {
                    $.messager.alert('提示', result.data);
                    opt.query();
                }
            });
        }
        else {
            return;
        }
    });
}


opt.add = function () {
    var url = '../Employee/EmployeePage?id=0';
    self.location = url;
}

//编辑用户
opt.edit = function (id) {
    var row = $('#dg').datagrid('getSelected');
    if (row == null) {
        $.messager.alert('提示', '未选中任何数据!');
        return;
    }
    var id = row.PersonId;
    var url = '../Employee/EmployeePage?id=' + id;
    self.location = url;
}
