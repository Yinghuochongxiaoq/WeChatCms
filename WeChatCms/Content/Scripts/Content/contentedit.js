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
            ContentSource: "",
            ContentFlag: "",
            Introduction: "",
            ContentDisImage: "",
            AttachmentFile: "",
            AttachmentFileSize: "",
            AttachmentFileName: ""
        },
        all_type_list: []
    },
    methods: {
        initImageUrl(url, type) {
            if (type == 1) {
                return hidRootUrl + '/Content/Images/attach_64.png';
            } else if (type == 0) {
                if (url) return url;
                return hidRootUrl + '/Content/Images/upload.png';
            }
        },
        fileClick(type) {
            if (type == 0) {
                document.getElementById('resource_upload_file').click();
            } else if (type == 1) {
                document.getElementById('attache_button').click();
            }
        },
        fileChange(el, type) {
            if (!el.target.files[0].size) return;
            this.fileList(el.target.files, type);
            el.target.value = '';
        },
        fileList(files, type) {
            for (let i = 0; i < files.length; i++) {
                if (type == 0) {
                    this.fileAdd(files[i]);
                }
                else if (type == 1) {
                    this.attacheFileAdd(files[i]);
                }

            }
        },
        fileAdd(file) {
            var reader = new FileReader();
            reader.vue = this;
            reader.readAsDataURL(file);
            reader.onload = function () {
                file.src = this.result;
            }
            //上传到服务器
            var formData = new FormData();
            formData.append("file", file);
            $.ajax({
                url: hidRootUrl + '/SysSet/PutImageToSys',
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data && data.ResultCode == 0) {
                        detailVm.$data.content_model.ContentDisImage = data.Message;
                        layer.msg('上传成功', { icon: 1 });
                    } else {
                        layer.msg(data.Message, { icon: 5 });
                    }
                }
            });
        },
        attacheFileAdd(file) {
            var reader = new FileReader();
            reader.vue = this;
            reader.readAsDataURL(file);
            reader.onload = function () {
                file.src = this.result;
            }
            //上传到服务器
            var formData = new FormData();
            formData.append("file", file);
            formData.append("type", "1");
            $.ajax({
                url: hidRootUrl + '/SysSet/PutImageToSys',
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data && data.ResultCode == 0) {
                        detailVm.$data.content_model.AttachmentFile = data.Message;
                        detailVm.$data.content_model.AttachmentFileName = data.Data.oldName;
                        detailVm.$data.content_model.AttachmentFileSize = data.Data.fileSize;
                        layer.msg('上传成功', { icon: 1 });
                    } else {
                        layer.msg(data.Message, { icon: 5 });
                    }
                }
            });
        }
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
        url: hidRootUrl + "/Content/AddContentInfo",
        type: "POST",
        data: detailVm.$data.content_model,
        success: function (data) {
            if (data && data.ResultCode == 0) {
                layer.msg(data.Message);
                window.location.href = hidRootUrl + "/Content/ContentList";
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
        url: hidRootUrl + "/Content/GetContentInfo",
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
        url: hidRootUrl + "/Content/GetContentType",
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                detailVm.$data.all_type_list = data.Data;
            }
        }
    });
}