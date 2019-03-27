/**
 * 查询结果
 */
var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: []
    },
    computed: {},
    methods: {
        vueEditContent: function (id) {
            editContent(id);
        },
        vueDelContent: function (id) {
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
        model: {
            label: "",
            type: "",
            pageIndex: 1,
            pageSize: 10
        },
        contentTypeNum: []
    }
});

/*
 * 获取菜单信息
 */
function LoadingActivityResultDetailDate() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    searchVm.$data.model.pageIndex = $("#currentPageIndex").val();
    $.ajax({
        url: "/SysSet/GetDicList",
        type: "POST",
        data: searchVm.$data.model,
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
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

/**
 * 加载所有的类型
 */
function GetAllType() {
    $.ajax({
        url: "/SysSet/GetAllDicType",
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                searchVm.$data.contentTypeNum = data.Data.typeList;
                detailVm.$data.all_parent_list = data.Data.parentList;
            }
        }
    });
}

/*
 * 分页查询功能
 */
function Search(pageIndex) {
    $("#currentPageIndex").val(pageIndex);
    LoadingActivityResultDetailDate();
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
            $.ajax("/SysSet/DelDicModel?id=" + id, {
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
 * 编辑跳转
 * @param {any} id
 */
function editContent(id) {
    if (!id) {
        parent.layer.msg("参数错误");
        return;
    }
    initViewModel();
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax("/SysSet/GetDicModel?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result && result.ResultCode == 0 && result.Data) {
                detailVm.$data.model = result.Data;
                $("#detailWindow").modal("show");
            } else
                parent.layer.msg(result.Message);
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        },
        error: function () {
            parent.layer.msg("编辑错误");
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

/**
初始化页面
*/
$(document).ready(function () {
    LoadingActivityResultDetailDate();
    GetAllType();
});

/**
 * 编辑
 */
var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        model: {
            Id: "",
            Value: "",
            Lable: "",
            Type: "",
            Description: "",
            Sort: "",
            Remarks: "",
            ParentId: 0
        },
        all_parent_list: []
    }
});

/**
 * 初始化模太窗口数据
 */
function initViewModel() {
    detailVm.$data.model = {
        Id: "",
        Value: "",
        Lable: "",
        Type: "",
        Description: "",
        Sort: "",
        Remarks: "",
        ParentId: 0
    };
}

/**
 * 新增模态窗口
 */
function newContent() {
    initViewModel();
    $("#detailWindow").modal("show");
}

/*
 * 保存
 */
function saveDataInfo() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax("/SysSet/SaveDicInfo", {
        type: "POST",
        data: detailVm.$data.model,
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