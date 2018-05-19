var opt = window.NameSpace || {};

function init() {
    opt.query();
}


//查询信息
opt.query = function () {
    if (dataId == 0) return;
    var url = '../Company/GetCompany';
    var params = { id: dataId };
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: params,
        success: function (data) {
            HR.Form.setValues('formCompany', data);
        },
        error: function () {
            $.messager.alert('提示', '查询出错！');
        }
    });
}

opt.add = function () {
    var url = '../Company/CompanyPage?id=0';
    self.location = url;
}

// 保存
opt.save = function () {
    var o = HR.Form.getValues('formCompany');
    o.CompanyId = dataId;
    var url = '../Company/SaveCompany';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(o),
        success: function (data) {
            if (data.success) {
                self.location.href = "../Company/CompanyList";
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
// 充值
opt.recharge = function () {
    $('#divRecharge').show();
    
}
opt.saveRecharge = function () {

}
//删除操作
opt.delete = function (id) {

}

opt.addPosition = function () {
   
}

opt.removePosition = function () {

}
