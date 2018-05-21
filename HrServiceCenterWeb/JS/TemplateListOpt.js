var TemplateListOpt = window.NameSpace || {};

function init() {
    TemplateListOpt.QueryTemplateList();
}

TemplateListOpt.QueryTemplateList = function () {
    $('#dg').datagrid({
        url: '../Pay/GetTemplateList',
        type: 'json',
        method: 'post',
        onLoadError: function (data) {
            $.messager.alert('提示', '模板列表加载失败，请重试！');
        }
    })
}