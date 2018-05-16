var opt = window.NameSpace || {};

function init() {
    opt.query();
}


//查询用户列表
opt.query = function () {
    var url =  '../Employee/GetCompanyList';
    var params = { query: $('#txtQuery').val() };
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


//删除操作
opt.delete = function (id) {
    $.messager.confirm('提示窗', '您确认删除吗?', function (event) {
        if (event) {
            $.ajax({
                type: 'POST',
                url: "../System/DeleteUser",
                data: {
                    UserId: id
                },
                dataType: "json",
                success: function (result) {
                    $.messager.alert('提示', result);
                    UserManageOpt.query();
                }
            });
        }
        else {
            return;
        }
    });
}


opt.add = function () {
}

//编辑用户
opt.edit = function (id) {
}
