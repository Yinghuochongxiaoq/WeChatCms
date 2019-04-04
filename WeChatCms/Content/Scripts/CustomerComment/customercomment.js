var dealType = [{ id: 1, name: "已处理" }, { id: 0, name: "未处理" }];
/**
 * 查询结果
 */
var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: [],
        contentTypeNum: dealType
    },
    computed: {},
    methods: {
        vueDelContent: function (id) {
            delContent(id);
        },
        dealCommont: function (id) {
            layer.prompt({ title: '处理结果', formType: 2 }, function (text, index) {
                dealComment(id, text, index);
            });
        },
        vueTypeContent: function (hasDeal) {
            var data = this.contentTypeNum.filter(function (item) {
                return item.id == hasDeal;
            });
            if (data && data.length > 0) {
                return data[0].name;
            }
            return "未知";
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
            hasDeal: 0,
            starttime: "",
            endtime: "",
            pageIndex: pager.index,
            pageSize: pager.size
        },
        contentTypeNum: dealType
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
        url: "/CustomerComment/CustomerCommentListPage",
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
 * 删除
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
            $.ajax("/CustomerComment/DelResourceModels", {
                type: "POST",
                data: { ids: [id] },
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
 * 处理留言
 * @param {any} id
 * @param {any} dealResult
 * @param {any} handler
 */
function dealComment(id, dealResult, handler) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误", { icon: 5 });
        return;
    }
    $.ajax("/CustomerComment/DealResultModels", {
        type: "POST",
        data: { id: id, dealResult: dealResult },
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("处理成功");
                Search(1);
            } else {
                parent.layer.msg(result.Message, { icon: 1 });
            }
            layer.close(handler);
        },
        error: function () {
            parent.layer.msg("处理错误", { icon: 5 });
            layer.close(handler);
        }
    });
}

/**
初始化页面
*/
$(document).ready(function () {
    LoadingActivityResultDetailDate();
});