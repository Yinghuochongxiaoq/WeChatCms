var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: [],
        contentTypeNum: []
    },
    computed: {},
    methods: {
        vueEditContent: function (id) {
            editContent(id);
        },
        vueDelContent: function (id) {
            delContent(id);
        },
        vueTypeContent: function (typecode) {
            var data = this.contentTypeNum.filter(function (item) {
                return item.Id == typecode;
            });
            if (data && data.length > 0) {
                return data[0].Lable;
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
            title: '',
            starttime: '',
            endtime: '',
            contentType: '',
            contentSource: '',
            pageIndex: 1
        },
        contentTypeNum: []
    }
});

/*
 * 获取菜单信息
 */
function LoadingActivityResultDetailDate() {
    //取消加载
    $(".loading-container").removeClass("loading-inactive");
    searchVm.$data.model.pageIndex = $("#currentPageIndex").val();
    $.ajax({
        url: "/Content/GetList",
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
            $.ajax("/Content/DeleteId?id=" + id, {
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
    window.location.href = "/Content/ContentEdit?id=" + id;
}

/**
 * 获取文章类型
 */
function getTypeList() {
    $.ajax({
        url: "/Content/GetContentType",
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                searchVm.$data.contentTypeNum = data.Data;
                resultVm.$data.contentTypeNum = data.Data;
            }
        }
    });
}
/**
初始化页面
*/
$(document).ready(function () {
    getTypeList();
    LoadingActivityResultDetailDate();
});
