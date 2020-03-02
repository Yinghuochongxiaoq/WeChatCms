//根目录
var hidRootUrl = $("#hidRootNode").val();
var resultVm = new Vue({
    el: '#resultTable',
    data: {
        dataList: [],
        sexTypeNum: [{ id: 2, name: '女' }, { id: 1, name: '男' }]
    },
    computed: {},
    methods: {
        vueEditContent: function (id) {
            editIdCardContent(id);
        },
        vueDelContent: function (id) {
            delContent(id);
        },
        vueTypeContent: function (typecode) {
            var data = this.sexTypeNum.filter(function (item) {
                return item.id == typecode;
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
            name: '',
            cardNumber: '',
            provinceCode: '',
            cityCode: '',
            countyCode: '',
            pageIndex: pager.index,
            pageSize: pager.size
        },
        provinceList: [],
        cityList: [],
        countyList: []
    },
    methods: {
        changeProvince(params) {
            getCityList(params);
        },
        changeCity(params) {
            getCountyList(params);
        }
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
        url: hidRootUrl + "/SysIdCardContent/GetList",
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
            $.ajax(hidRootUrl + "/SysIdCardContent/DeleteId?id=" + id, {
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
 * 获取省份列表
 */
function getProvinceList(pCode) {
    $.ajax({
        url: hidRootUrl + "/SysIdCardContent/GetProvinceType",
        data: { pCode: pCode },
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                searchVm.$data.provinceList = data.Data;
            }
        }
    });
}

/**
 * 获取市信息列表
 * @param {any} pCode
 */
function getCityList(pCode) {
    if (pCode == '0') {
        searchVm.$data.model.cityCode = '';
        searchVm.$data.model.countyCode = '';
        searchVm.$data.cityList = [];
        searchVm.$data.countyList = [];
        return;
    }
    $.ajax({
        url: hidRootUrl + "/SysIdCardContent/GetProvinceType",
        data: { pCode: pCode },
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                searchVm.$data.cityList = data.Data;
                searchVm.$data.countyList = [];
                searchVm.$data.model.cityCode = '';
                searchVm.$data.model.countyCode = '';
            }
        },
        error: function () {
            searchVm.$data.model.cityCode = '';
            searchVm.$data.model.countyCode = '';
            searchVm.$data.cityList = [];
            searchVm.$data.countyList = [];
        }
    });
}

/**
 * 获取县信息列表
 * @param {any} pCode
 */
function getCountyList(pCode) {
    if (pCode == '0') {
        searchVm.$data.model.countyCode = '';
        searchVm.$data.countyList = [];
        return;
    }
    $.ajax({
        url: hidRootUrl + "/SysIdCardContent/GetProvinceType",
        data: { pCode: pCode },
        type: "POST",
        success: function (data) {
            if (data && data.ResultCode == 0) {
                searchVm.$data.countyList = data.Data;
                searchVm.$data.model.countyCode = '';
            }
        },
        error: function () {
            searchVm.$data.model.countyCode = '';
            searchVm.$data.countyList = [];
        }
    });
}

var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        model: {
            id: '0',
            name: "",
            cardNumber: "",
            remarks: ""
        }
    }
});

/**
 * 新增模态窗口
 */
function newIdCardInfo() {
    initViewModel();
    $("#detailWindow").modal("show");
}

/**
 * 编辑模特窗口
 */
function editIdCardContent(id) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax(hidRootUrl + "/SysIdCardContent/GetSysIdCardInfo?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result.ResultCode == 0 && result.Data) {

                detailVm.$data.model.id = result.Data.Id;
                detailVm.$data.model.name = result.Data.Name;
                detailVm.$data.model.cardNumber = result.Data.CardNumber;
                detailVm.$data.model.remarks = result.Data.Remarks;
                $("#detailWindow").modal("show");
            } else {
                parent.layer.msg(result.message);
            }
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        },
        error: function () {
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

/**
 * 初始化模太窗口数据
 */
function initViewModel() {
    var d = new Date();
    detailVm.$data.model = {
        id: '0',
        name: "",
        cardNumber: "",
        remarks: ""
    };
}
/**
 * 保存
 */
function saveIdCardInfo() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax(hidRootUrl + "/SysIdCardContent/SaveIdCardNumberInfo", {
        type: "POST",
        data: detailVm.$data.model,
        success: function (result) {
            if (result.ResultCode == 0) {
                parent.layer.msg("操作成功!");
                $("#detailWindow").modal("hide");
                Search(1);
            } else {
                parent.layer.msg(result.Message);
            }
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        },
        error: function () {
            //取消加载
            $(".loading-container").addClass("loading-inactive");
        }
    });
}

/**
初始化页面
*/
$(document).ready(function () {
    getProvinceList('0');
    LoadingActivityResultDetailDate();
});
