var all_resource_type = [{ Id: 1, name: '图片' }];
var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: [],
        dataIds: [],
        // 初始化全选按钮, 默认不选
        isCheckedAll: false
    },
    computed: {},
    methods: {
        //删除
        vueDelCostContent: function (id) {
            before_del(id);
        },
        //全选
        checkedAll() {
            //判断点击之前是否已经全选中
            this.isCheckedAll = this.dataList.length == this.dataIds.length;
            this.isCheckedAll = !this.isCheckedAll;
            if (this.isCheckedAll) {
                // 全选时
                this.dataIds = [];
                this.dataList.forEach(function (item) {
                    this.dataIds.push(item.Id);
                },
                    this);
            } else {
                this.dataIds = [];
            }
        },
        //单选
        checkedOne(id) {
            var idIndex = this.dataIds.indexOf(id);
            if (idIndex >= 0) {
                // 如果已经包含了该id, 则去除(单选按钮由选中变为非选中状态)
                this.dataIds.splice(idIndex, 1);
            } else {
                // 选中该checkbox
                this.dataIds.push(id);
                this.isCheckedAll = this.dataList.length == this.dataIds.length;
            }
        },
        //批量删除
        vueDelAll() {
            delAll();
        }
    }
});

/*
 * 分页方法名
 */
var pager = new PagerView('pager');
pager.itemCount = '0';
pager.index = '1';
pager.size = '20';
pager.methodName = "Search";
pager.render();

var searchVm = new Vue({
    el: "#form1",
    data: {
        pageIndex: pager.index,
        pageSize: pager.size,
        resourceType: 0,
        all_resource_type: all_resource_type
    }
});

/*
 * 获取用户
 */
function LoadingActivityResultDetailDate() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    searchVm.$data.pageIndex = $("#currentPageIndex").val();
    $.ajax({
        url: "/SysResource/ResourceListPage",
        type: "POST",
        data: searchVm.$data,
        success: function (data) {
            if (data && data.ResultCode == 0) {
                resultVm.$data.dataList = data.Data.dataList;
                resultVm.$data.dataIds = [];
                pager.itemCount = "" + data.Data.count;
                pager.index = $("#currentPageIndex").val();
                pager.render();
                layer.ready(function () {
                    layer.photos({
                        photos: '#Images'
                    });
                });
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

/**
 * 单个文件上传
 */
var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        detail_model: {
            Id: 0,
            ResourceUrl: "",
            ResourceType: 1,
            ResourceRemark: "",
            Sort: 0
        },
        all_resource_type: all_resource_type
    },
    methods: {
        fileClick() {
            document.getElementById('resource_upload_file').click();
        },
        fileChange(el) {
            if (!el.target.files[0].size) return;
            this.fileList(el.target.files);
            el.target.value = '';
        },
        fileList(files) {
            for (let i = 0; i < files.length; i++) {
                this.fileAdd(files[i]);
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
                url: '/SysSet/PutImageToSys',
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data && data.ResultCode == 0) {
                        detailVm.$data.detail_model.ResourceUrl = data.Message;
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
 * 初始化模太窗口数据
 */
function initViewModel() {
    detailVm.$data.detail_model = {
        Id: 0,
        ResourceUrl: "/Content/Images/upload.png",
        ResourceType: 1,
        ResourceRemark: "",
        Sort: 0
    };
}

/**
 * 新增模态窗口
 */
function newResource() {
    initViewModel();
    $("#detailWindow").modal("show");
}

/*
 * 保存
 */
function saveDataInfo() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax("/SysResource/SaveResourceInfo", {
        type: "POST",
        data: detailVm.$data.detail_model,
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

/**
 * 删除单个资源文件
 * @param {any} id
 */
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
 * 删除单个资源文件
 */
function delContent(id) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    do_del([id]);
}

/**
 * 批量删除资源文件
 */
function delAll() {
    if (resultVm.$data.dataIds.length < 1) {
        layer.msg('请选择需要删除的图片', { icon: 3 });
        return;
    }
    layer.msg('确定要删除？', {
        time: 0 //不自动关闭
        , btn: ['确定', '取消']
        , shade: 0.4
        , area: ["200px", "100px"]
        , yes: function (index) {
            layer.close(index);
            do_del(resultVm.$data.dataIds);
        }
    });
}

/**
 * 执行删除操作
 * @param {any} ids
 */
function do_del(ids) {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax("/SysResource/DelResourceModels", {
        type: "POST",
        data: { ids: ids },
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("删除成功");
                Search(1);
            } else {
                parent.layer.msg(result.Message);
            }
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        },
        error: function () {
            parent.layer.msg("删除错误");
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

layer.ready(function () {
    layer.photos({
        photos: '#Images'
    });
});
LoadingActivityResultDetailDate();