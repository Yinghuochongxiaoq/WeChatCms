var sexEnum = ['保密', '男', '女'];
var userTypeEnum = ['普通用户', '超级管理员', '普通管理员'];
var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: [],
        sexEnum: sexEnum,
        userTypeEnum: userTypeEnum
    },
    computed: {},
    methods: {
        vueEditContent: function (id) {
            editContent(id);
        },
        vueDelCostContent: function (id) {
            before_del(id);
        },
        vueResetContent: function (id) {
            resetPassword(id);
        },
        vuePowerContent: function (id, userTree) {
            delPowerContent(id, userTree);
        }
    }
});

/*
 * 分页方法名
 */
var pager = new PagerView('pager');
pager.itemCount = '0';
pager.index = '1';
pager.size = '10';
pager.methodName = "Search";
pager.render();

var searchVm = new Vue({
    data: {
        pageIndex: 1
    }
});

/*
 * 获取用户
 */
function LoadingActivityResultDetailDate() {
    var index = layer.load();
    searchVm.$data.pageIndex = $("#currentPageIndex").val();
    $.ajax({
        url: "/SysSet/GetUserList",
        type: "POST",
        data: searchVm.$data,
        success: function (data) {
            if (data && data.ResultCode == 0) {
                resultVm.$data.dataList = data.Data.dataList;
                pager.itemCount = "" + data.Data.count;
                pager.index = $("#currentPageIndex").val();
                pager.render();
            }
            else {
                layer.msg('查询失败');
            }
        }
    });
    layer.close(index);
}

/*
 * 分页查询功能
 */
function Search(pageIndex) {
    $("#currentPageIndex").val(pageIndex);
    LoadingActivityResultDetailDate();
}

var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        detail_model: {
            Id: 0,
            Sex: "",
            Birthday: "",
            UserName: "",
            TrueName: "",
            TelPhone: "",
            UserType: 2,
            CouldChangeName: false
        },
        all_list: [{ Id: 0, name: '保密' }, { Id: 1, name: '男' }, { Id: 2, name: '女' }],
        //0：普通用户；1：超级管理员；2：普通管理员
        all_user_type: [{ Id: 0, name: '普通用户' }, { Id: 1, name: '超级管理员' }, { Id: 2, name: '普通管理员' }]
    }
});

/**
 * 初始化模太窗口数据
 */
function initViewModel() {
    detailVm.$data.detail_model = {
        Id: 0,
        Sex: "0",
        Birthday: "",
        UserName: "",
        TrueName: "",
        TelPhone: "",
        UserType: 2,
        CouldChangeName: false
    };
}

/**
 * 新增模态窗口
 */
function newCostContent() {
    initViewModel();
    $("#detailWindow").modal("show");
}

/**
 * 编辑模态窗口
 */
function editContent(id) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    initViewModel();
    var index = layer.load();
    $.ajax("/SysSet/GetUserModel?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result && result.ResultCode == 0 && result.Data) {
                detailVm.$data.detail_model = {
                    Id: result.Data.Id,
                    Sex: result.Data.Sex,
                    Birthday: result.Data.Birthday,
                    UserName: result.Data.UserName,
                    TrueName: result.Data.TrueName,
                    TelPhone: result.Data.TelPhone,
                    UserType: result.Data.UserType,
                    CouldChangeName: true
                };
                $("#detailWindow").modal("show");
            } else
                parent.layer.msg(result.Message);
        },
        error: function () {
            parent.layer.msg("编辑错误");
        }
    });
    layer.close(index);
}

/*
 * 保存
 */
function saveDataInfo() {
    var index = layer.load();
    $.ajax("/SysSet/SaveDataInfo", {
        type: "POST",
        data: detailVm.$data.detail_model,
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("操作成功!");
                $("#detailWindow").modal("hide");
                Search(1);
            } else
                parent.layer.msg(result.Message);
        },
        error: function () {
            parent.layer.msg("系统异常");
        }
    });
    layer.close(index);
}

function before_del(id) {
    layer.msg('确定要删除？', {
        time: 0 //不自动关闭
        , btn: ['确定', '取消']
        , shade: 0.4
        , area: ["200px", "100px"]
        , yes: function (index) {
            layer.close(index);
            delContent(id);
        }
    });
}

/*
 * 删除
 */
function delContent(id) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    var index = layer.load();
    $.ajax("/SysSet/DelModel?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("删除成功");
                Search(1);
            } else {
                parent.layer.msg(result.Message);
            }
            layer.close(index);
        },
        error: function () {
            parent.layer.msg("删除错误");
            layer.close(index);
        }
    });
}
/*
 * 重置密码
 */
function resetPassword(id) {
    var index = layer.load();
    $.ajax("/SysSet/ResetPassword?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("操作成功");
                Search(1);
            } else {
                parent.layer.msg(result.Message);
            }
            layer.close(index);
        },
        error: function () {
            parent.layer.msg("删除错误");
            layer.close(index);
        }
    });
}

LoadingActivityResultDetailDate();