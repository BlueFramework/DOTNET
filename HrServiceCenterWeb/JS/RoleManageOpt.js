var RoleManageOpt = window.NameSpace || {};

var oldRoleName;

function init() {
    RoleManageOpt.Query();
    $('#edit').window({
        onBeforeClose: function () {
            $("#roleId").val("");
            $("#txtName").val("");
            $("#txtDesc").val("");
            $('#txtName').validatebox('enableValidation');
            $('#txtDesc').validatebox('enableValidation');
        },
        onOpen: function () {
            $('#txtName').validatebox('validate');
            $('#txtDesc').validatebox('validate');
        }
    })
}

//查询操作
RoleManageOpt.Query = function () {
    var obj = HR.Form.getValues('pnlSearch');
    var que = $("#uname").val();
    $('#dg').datagrid({
        url: '../System/RolesQuery?userName=' + que,
        fitColumns: true,
        rownumbers: true,
        singleSelect: true,
        autoRowHeight: false,
        pagination: true,
        nowrap: false,
        pageSize: 10,
        striped: true,
        idField: 'RoleId',
        queryParams: obj,
        loadFilter: pagerFilter,
        method: 'post',
        onLoadSuccess: function (data) {
            //self.parent.after_loadIframe();
        },
        onLoadError: function (data) {
            $.messager.alert('错误提示', '加载数据失败，请重试！');
        }
    });
}

//删除操作
RoleManageOpt.Delete = function (id) {
    $.messager.confirm('删除角色', '您确认删除吗?', function (event) {
        if (event) {
            $.ajax({
                url: '../System/DeleteRole',
                type: 'POST',
                datatype: 'json',
                data: { RoleId: id },
                success: function (data) {
                    RoleManageOpt.Query();
                    $.messager.alert('失败', data);
                },
                error: function (data) {
                    $.messager.alert('失败', '删除失败', 'error');
                }
            });
        }
        else {
            return;
        }
    });
}
//新增角色
RoleManageOpt.AddRole = function () {
    $('#edit').panel({ title: "新增角色" });
    $("#roleId").val("0");
    $('#edit').window('open');
}
//编辑角色
RoleManageOpt.EditRole = function (id) {
    $('#edit').panel({ title: "编辑角色" });
    $("#roleId").val(id);
    $('#edit').window('open');
    RoleManageOpt.LoadRole();
}

//加载某一个角色的信息
RoleManageOpt.LoadRole = function () {
    $.ajax({
        type: 'POST',
        url: "../System/LoadRole",
        async: false,
        data: {
            RoleId: $("#roleId").val()
        },
        dataType: "json",
        success: function (result) {
            HR.Form.setValues('edit', result);
            oldRoleName = $("#txtName").val();
        }
    });
}

//新增/编辑角色提交
RoleManageOpt.EditRoleBtn = function () {
    var success = $("#edit").form('validate');//验证
    if (success) {
        if ($("#roleId").val() != "0") {
            Url = "../System/UpdateOnlyRole?oldName=" + oldRoleName;
        }
        else {
            var Url = "../System/AddRole";
        }
        var obj = HR.Form.getValues('edit');
        $.ajax({
            type: 'POST',
            url: Url,
            data: obj,
            dataType: "json",
            success: function (result) {
                $('#edit').window('close');
                $.messager.alert('提示', result);
                RoleManageOpt.Query();
            }
        });
    }
}

//编辑列
function formatActions(val, row) {
    var name = "'" + row.RoleName + "'";
    var id = row.RoleId;
    var deletes = '<span title="删除" style="margin-left:20px; "><a href="javascript:void(0)" onclick="RoleManageOpt.Delete(' + id + ')"><i class="fa fa-trash fa-lg" aria-hidden="true"></i></a></span>';
    var edit = '<span title="编辑" "><a href="javascript:void(0)" onclick="RoleManageOpt.EditRole(' + id + ')"><i class="fa fa-pencil fa-lg" aria-hidden="true"></i></a></span>';
    var group = '<span title="分配组" style="margin-left:20px;"><a href="../SysManage/Group?RoleId=' + id + '" onclick=""><i class="fa fa-user-plus fa-lg" aria-hidden="true"></i></a></span>';
    //var menu = '<span title="分配功能" style="margin-left:20px;"><a href="../SysManage/PowersPage?RoleId=' + id + '" onclick=""><i class="fa fa-briefcase fa-lg" aria-hidden="true"></i></a></span>';
    var menu = '<span title="分配功能" style="margin-left:20px;"><a href="javascript:void(0)" onclick="RoleManageOpt.InitTreeData(' + id + ')"><i class="fa fa-briefcase fa-lg" aria-hidden="true"></i></a></span>';
    //var data = '<span title="分配地市" style="margin-left:20px;"><a href="../SysManage/DatasPage?RoleId=' + id + '" onclick=""><i class="fa fa-globe fa-lg" aria-hidden="true"></i></a></span>';
    //var data = '<span title="分配地市" style="margin-left:20px;"><a href="javascript:void(0)" onclick="RoleManageOpt.RightGridData(' + id + ')"><i class="fa fa-globe fa-lg" aria-hidden="true"></i></a></span>';
    var html = '<div style="margin:0 auto; display: inline-block !important; display: inline;">' + edit + group + menu + deletes + '</div>';
    return html;
}

//加载地市列表
RoleManageOpt.RightGridData = function (id) {
    $('#win').window('open');
    $("#roleGroup").val(id);
    $('#dg1').datagrid({
        url: '../Account/GetAllCitys',
        rownumbers: true,
        singleSelect: false,
        autoRowHeight: false,
        pagination: false,
        method: 'post',
        onBeforeLoad: function (params) {
        },
        onLoadSuccess: function (data) {
            $.ajax({
                type: 'POST',
                url: "../SysManage/LoadRoleDatas",
                async: false,
                data: { RoleId: $("#roleGroup").val() },
                dataType: "json",
                success: function (result) {
                    if (result != "") {
                        var arr = new Array();
                        arr = result.split(',');
                        $.each(data.rows, function (index, item) {
                            //if (right.DataRights.indexOf(item.AreaId) > -1) {
                            //    $('#dg').datagrid('checkRow', index);
                            //}
                            $.each(arr, function (i, n) {
                                if (arr[i] == item.GroupID) {
                                    $('#dg1').datagrid('checkRow', index);
                                }
                            });
                        });
                    }
                }
            });
        },
        onLoadError: function (data) {
            $.messager.alert('错误提示', '加载数据失败，请重试！', 'error');
        }
    });
}
//获取选中行的AreaId
RoleManageOpt.GetGirdData = function () {
    var rows = $('#dg1').datagrid('getSelections');
    if (rows.length == 0) {
        return "";
    }
    else {
        var re = "";
        rows.forEach(function (item) {
            re += "," + item.GroupID;
        });
    }
    return re.substr(1);
}

RoleManageOpt.DataSave = function () {
    var DataRight = RoleManageOpt.GetGirdData();
    //if (DataRight == "") {
    //    $.messager.alert('警告','请至少为该角色分配一项地市权限！','warning');
    //    return;
    //}
    var url = '../SysManage/SaveRoleDatas';
    $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            RoleId: $("#roleGroup").val(),
            Datas: DataRight
        },
        success: function (data) {
            if (data == "1") {
                $.messager.alert('成功', '成功为该角色分配地市权限', 'info', function () {

                })
            }
            else {
                $.messager.alert('失败', '分配失败，请联系管理员', 'error');
            }
        },
        error: function () {
            $.messager.alert('错误提示', '分配失败，请联系管理员', 'error');
        }
    });
}

//加载功能列表
RoleManageOpt.InitTreeData = function (roleId) {
    $('#menu').window('open');
    $("#roleMenu").val(roleId);
    $('#tree').tree({
        url: '../SysManage/LoadMenus',
        checkbox: true,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#tree').tree('select', node.target);
        },
        onLoadSuccess: function (data) {
            $.ajax({
                type: 'POST',
                url: "../SysManage/GetRoleMenus",
                async: false,
                data: { RoleId: $("#roleMenu").val() },
                dataType: "json",
                success: function (result) {
                    if (result != "") {
                        var menuarr = result.split(",");
                        menuarr.forEach(function (item) {
                            if (item.length > 1) {
                                var node = $('#tree').tree('find', item);
                                $('#tree').tree('check', node.target);
                            }
                        });
                    }
                }
            });
        },
        onLoadError: function (data) {
            $.messager.alert('错误提示', '加载数据失败，请重试！');
        }
    });
}

RoleManageOpt.MenuSave = function () {
    var MenuRight = RoleManageOpt.GetTreeData();
    //if (MenuRight == "") {
    //    $.messager.alert('警告','请至少为该角色分配一项菜单权限！','warning');
    //    return;
    //}
    var url = '../SysManage/SaveRoleMenus';
    $.ajax({
        url: url,
        type: 'POST',
        datatype: 'json',
        data: {
            RoleId: $("#roleMenu").val(),
            Menus: MenuRight
        },
        success: function (data) {
            if (data == "1") {
                $.messager.alert('成功', '成功为该角色分配菜单权限', 'info', function () {

                })
            }
            else {
                $.messager.alert('失败', '分配失败，请联系管理员', 'error');
            }
        },
        error: function () {
            $.messager.alert('错误提示', '分配失败，请联系管理员', 'error');
        }
    });
}
//获取选中树形菜单ID
RoleManageOpt.GetTreeData = function () {
    var nodes = $('#tree').tree('getChecked', ['checked', 'indeterminate']);
    if (nodes.length == 0) {
        return "";
    }
    else {
        var re = "";
        nodes.forEach(function (item) {
            re += "," + item.id;
        });
    }
    return re.substr(1);
}