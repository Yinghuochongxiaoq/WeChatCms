//根目录
var hidRootUrl = $("#hidRootNode").val();

var validArr = [
    { id: 0, name: '停用' },
    { id: 1, name: '启用' }
];

/**
 * 查询结果
 */
var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: [],
        spendTypeNum: validArr
    },
    computed: {},
    methods: {
        vueEditContent: function (id) {
            editContent(id);
        },
        vueDelContent: function (id, isValid) {
            delContent(id, isValid);
        },
        vueValid: function (id) {
            var data = this.spendTypeNum.filter(function (item) {
                return item.id == id;
            });
            if (data && data.length > 0) {
                return data[0].name;
            }
            return "--";
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
            Name: "",
            IsValid: -1,
            pageIndex: pager.index,
            pageSize: pager.size
        },
        contentTypeNum: validArr
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
        url: hidRootUrl + "/CostNote/GetCostChannelPage",
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

/*
 * 分页查询功能
 */
function Search(pageIndex) {
    $("#currentPageIndex").val(pageIndex);
    LoadingActivityResultDetailDate();
}

/*
 * 停用单个菜单
 */
function delContent(id, isValid) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    var msg = "确定要停用？";
    if (isValid == 1) {
        msg = "确定要启用？";
    }
    layer.msg(msg, {
        time: 0 //不自动关闭
        , btn: ['确定', '取消']
        , shade: 0.4
        , area: ["200px", "100px"]
        , yes: function (indexOne) {
            layer.close(indexOne);
            var index = layer.load();
            $.ajax(hidRootUrl + "/CostNote/DelCostChannelModel?id=" + id + "&isValid=" + isValid, {
                type: "POST",
                success: function (result) {
                    if (result && result.ResultCode == 0) {
                        parent.layer.msg("处理成功");
                        Search(1);
                    } else {
                        parent.layer.msg(result.Message);
                    }
                    layer.close(index);
                },
                error: function () {
                    parent.layer.msg("处理错误");
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
    $.ajax(hidRootUrl + "/CostNote/GetCostChannelModel?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result && result.ResultCode == 0 && result.Data) {
                detailVm.$data.model = result.Data;
                $("#detailWindow").modal("show");
            } else {
                parent.layer.msg(result.Message);
            }
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
});

/**
 * 编辑
 */
var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        model: {
            CostChannelName: "",
            CostChannelNo: "",
            Id: 0,
            IsValid: 1,
            Sort: 0
        },
        validTypeNum: validArr
    }
});

/**
 * 初始化模太窗口数据
 */
function initViewModel() {
    detailVm.$data.model = {
        CostChannelName: "",
        CostChannelNo: "",
        Id: 0,
        IsValid: 1,
        Sort: 0
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
    $.ajax(hidRootUrl + "/CostNote/SaveCostChannelInfo", {
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