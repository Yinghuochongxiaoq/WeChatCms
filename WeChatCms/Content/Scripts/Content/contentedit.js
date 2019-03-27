;
//根目录
var hidRootUrl = $("#hidRootNode").val();
//实例化编辑器
//建议使用工厂方法getEditor创建和引用编辑器实例，如果在某个闭包下引用该编辑器，直接调用UE.getEditor('editor')就能拿到相关的实例
var ue = UE.getEditor('shareContent');
/**
初始化页面
*/
$(document).ready(function () {
    init();
});

var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        content_model: {
            Id: 0,
            Title: "",
            ContentType: "",
            Content: "",
            ContentSource: ""
        },
        all_type_list: []
    }
});

/**
 * 初始化
 */
function init() {
    getTypeList();
    getContentInfo();
}

/**
 * 添加内容信息
 */
function addContentInfo() {
    var contentInfo = ue.getContent();
    detailVm.$data.content_model.Content = contentInfo;
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax({
        url: "/Content/AddContentInfo",
        type: "POST",
        data: detailVm.$data.content_model,
        success: function (data) {
            if (data && data.ResultCode == 0) {
                layer.msg(data.Message);
                window.location.href = "/Content/ContentList";
            }
            else {
                layer.msg('处理失败');
            }
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

/**
 * 获取初始化内容信息
 */
function getContentInfo() {
    var id = $("#id").val();
    $.ajax({
        url: "/Content/GetContentInfo",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data && data.ResultCode == 0) {
                detailVm.$data.content_model = data.Data;
                //初始化编辑器内容
                ue.ready(function () {
                    ue.setContent(detailVm.$data.content_model.Content);
                });
            }
            else {
                layer.msg('查询失败');
            }
        }
    });
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
                detailVm.$data.all_type_list = data.Data;
            }
        }
    });
}