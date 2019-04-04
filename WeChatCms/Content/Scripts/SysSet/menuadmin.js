var resultVm = new Vue({
    el: '#resultTable',
    data: {
        menuList: []
    },
    computed: {},
    methods: {
        vueEditContent: function (id) {
            editContent(id);
        },
        vueDelCostContent: function (id) {
            delContent(id);
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
    el: '#form1',
    data: {
        pageIndex: pager.index,
        pageSize: pager.size
    }
});

/*
 * 获取菜单信息
 */
function LoadingActivityResultDetailDate() {
    var index = layer.load();
    searchVm.$data.pageIndex = $("#currentPageIndex").val();
    $.ajax({
        url: "/SysSet/GetMenuList",
        type: "POST",
        data: searchVm.$data,
        success: function (data) {
            if (data && data.ResultCode == 0) {
                resultVm.$data.menuList = data.Data.menuList;
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
        menu_model: {
            Id: 0,
            Name: "",
            Title: "",
            MenuType: "",
            Url: "",
            OrderNo: "",
            Icon: "",
            ParentId: 0
        },
        all_menu_list: []
    }
});

/**
 * 初始化模太窗口数据
 */
function initViewModel() {
    detailVm.$data.menu_model = {
        Id: 0,
        Name: "",
        Title: "",
        MenuType: "",
        Url: "",
        OrderNo: "",
        Icon: "",
        ParentId: 0
    };
    getAllMenuList();
}

/**
 * 新增模态窗口
 */
function newCostContent() {
    initViewModel();
    $("#detailWindow").modal("show");
}

/**
 * 获取所有的菜单信息
 */
function getAllMenuList() {
    $.ajax({
        url: "/SysSet/GetAllMenuList",
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                detailVm.$data.all_menu_list = data.Data;
            }
            else {
                layer.msg('查询失败');
            }
        }
    });
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
    $.ajax("/SysSet/GetMenuModel?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result && result.ResultCode == 0 && result.Data) {
                detailVm.$data.menu_model = {
                    Id: result.Data.Id,
                    Name: result.Data.Name,
                    Title: result.Data.Title,
                    MenuType: result.Data.MenuType,
                    OrderNo: result.Data.OrderNo,
                    Url: result.Data.Url,
                    Icon: result.Data.Icon,
                    ParentId: result.Data.ParentId
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
function saveMenuInfo() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax("/SysSet/SaveMenuInfo", {
        type: "POST",
        data: detailVm.$data.menu_model,
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("操作成功!");
                $("#detailWindow").modal("hide");
                Search(1);
            } else
                parent.layer.msg(result.Message);
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        },
        error: function () {
            parent.layer.msg("系统异常");
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

/*
 * 删除单个菜单
 */
function delContent(id) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    layer.msg('确定要删除？', {
        time: 0 //不自动关闭
        , btn: ['确定', '取消']
        , shade: 0.4
        , area: ["200px", "100px"]
        , yes: function (indexOne) {
            layer.close(indexOne);
            var index = layer.load();
            $.ajax("/SysSet/DelMenuModel?id=" + id, {
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
    });
}

/**
 *  展示图标选择框
 */
function showIconWindow() {
    $("#IconWindow").modal("show");
}

/*
 * 选择图标
 */
function chooseIcon(icon) {
    detailVm.$data.menu_model.Icon = icon;
    $("#IconWindow").modal("hide");
}
LoadingActivityResultDetailDate();