var opt = window.NameSpace || {};

function init() {
    if (dataId > 0) {
        opt.query();
    }
    else {
        if (CompanyId>0)
            $('#cmbCompany').combobox('setValue', CompanyId);
    }
}

opt.positions = new Array();
//查询信息
opt.query = function () {

    var url = '../Employee/GetEmployee?personId=' + dataId;
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: null,
        success: function (data) {
            HR.Form.setValues('formEmployee', data);
        },
        error: function () {
            $.messager.alert('提示', '查询出错！');
        }
    });
}

opt.add = function () {
    var companyId = $('#cmbCompany').combobox('getValue');
    var url = '../Employee/EmployeePage?id=0&companyId=' + companyId;
    self.location = url;
}
// 保存
opt.save = function () {
    var validate = false;
    var companyId = $('#cmbCompany').combobox('getValue');
    var companyText = $('#cmbCompany').combobox('getText');
    var companyList = $('#cmbCompany').combobox('getData');
    companyList.forEach(function (item,i) {
        if (item.CompanyId == companyId) {
            validate = true;
        }
    });
    if (!validate) {
        $.messager.alert('警告', '请从下拉列表中选择单位！');
        return;
    }
    var validate = $('#formEmployee').form('validate');
    if (!validate) {
        $.messager.alert('警告', '请填写人员必填信息！');
        return;
    }
    var o = HR.Form.getValues('formEmployee');
    o.PersonId = dataId;
    var url = '../Employee/SaveEmployee';
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
opt.exit = function (id) {
    self.location.href = '../Employee/EmployeeList';
}
