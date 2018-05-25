var opt = window.NameSpace || {};

function init() {
    if (dataId > 0) {
        opt.query();
    }
    opt.setControlState();
}


//查询信息
opt.query = function () {
    
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
opt.setControlState = function () {
    if (dataId == 0) {
        $('[name="onOldData"]').attr("disabled", true);
    }
    else {
        $('[name="onOldData"]').removeAttr("disabled");

    }
}
// 保存
opt.save = function () {
    var o = HR.Form.getValues('formCompany');
    o.CompanyId = dataId;
    var url = '../Company/SaveCompany';
    HR.Loader.show("loading...");
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(o),
        success: function (data) {
            if (data.success) {
                HR.Loader.hide();
                dataId = data.id;
                opt.setControlState();
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
    if (dataId == 0) {
        $.messager.alert('提示', '请先保存公司信息！');
        return;
    }
    var money = $('#nbRecharge').textbox('getValue');
    var url = '../Company/SaveRecharge';
    HR.Loader.show("loading...");
    var param = { companyId: dataId, money: money };
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: param,
        success: function (data) {
            if (data.success) {
                HR.Loader.hide();
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
//删除操作
opt.delete = function (id) {

}

opt.addPosition = function () {
   
}

opt.removePosition = function () {

}
