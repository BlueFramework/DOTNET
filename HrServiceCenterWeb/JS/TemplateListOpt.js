var TemplateListOpt = window.NameSpace || {};

function init() {
    var height = $('.container-layout').height() - 160;
    $('#dgContainer').height(height);
    $('#dg').datagrid('resize');
    TemplateListOpt.QueryTemplateList();
}

TemplateListOpt.QueryTemplateList = function () {
    $('#dg').datagrid('loading');
    $.ajax({
        url: '../Pay/GetTemplateList',
        type: "POST",
        dataType: "json",
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