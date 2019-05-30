//根目录
var hidRootUrl = $("#hidRootNode").val();
var inOrOutArr = [
    { id: 0, name: '支出' },
    { id: 1, name: '收入' },
    { id: 2, name: '资金转移' }
];
//分页对象
var pager = new PagerView('pager');
pager.itemCount = '0';
pager.index = '1';
pager.size = '10';
pager.methodName = "Search"; //分页方法名
pager.render();

var searchVm = new Vue({
    el: '#form1',
    data: {
        costcontentmodel: {
            starttime: '',
            endtime: '',
            costtype: -1,
            spendtype: -1,
            costthing: '',
            costchannel: -1,
            pageindex: pager.index,
            pagesize: pager.size
        },
        staticcosttypenum: [],
        inorout: inOrOutArr,
        costchannellist: []
    }, methods: {
        changeSpendType(params) {
            getCostType(params);
        }
    }
});

var vm = new Vue({
    el: '#resultTable',
    data: {
        costcontentlist: [],
        costtypeenum: [],
        inorout: inOrOutArr,
        statisticsModel: {
            allCouldCost: 0,
            allOutCost: 0,
            allInCost: 0
        }
    },
    computed: {},
    methods: {
        vueEditCostContent: function (id) {
            editCostContent(id);
        },
        vueTypeContent: function (typeId) {
            var data = this.costtypeenum.filter(function (item) {
                return item.Id == typeId;
            });
            if (data && data.length > 0) {
                return data[0].Name;
            }
            return "--";
        },
        vueInOrOut: function (id) {
            var data = this.inorout.filter(function (item) {
                return item.id == id;
            });
            if (data && data.length > 0) {
                return data[0].name;
            }
            return "--";
        }
    }
});

var detailVm = new Vue({
    el: '#detailWindow',
    data: {
        costcontentmodel: {
            Cost: '',
            CostAddress: "",
            CostChannel: 0,
            CostThing: "",
            CostTime: "",
            CostType: -1,
            Id: 0,
            SpendType: 0,
            LinkCostChannel: 0
        },
        staticcosttypenum: [],
        costchannellist: [],
        inorout: inOrOutArr
    },
    computed: {
        costTypeCmputed: function () {
            var spendType = this.costcontentmodel.SpendType;
            return this.staticcosttypenum.filter(function (item) {
                return item.SpendType == spendType;
            });
        }
    },
    methods: {
        vueTypeContent: function (typeId) {
            var data = this.costtypeenum.filter(function (item) {
                return item.Id == typeId;
            });
            if (data && data.length > 0) {
                return data[0].Name;
            }
            return "未知";
        }
    }
});

/**
 *查询方法
 */
function LoadingActivityResultDetailDate() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    searchVm.$data.costcontentmodel.pageindex = $("#currentPageIndex").val();
    $.ajax({
        url: hidRootUrl + "/CostNote/GetCostPage",
        type: "POST",
        data: searchVm.$data.costcontentmodel,
        success: function (data) {
            if (data && data.ResultCode == 0 && data.Data) {
                vm.$data.costcontentlist = data.Data.dataList;
                vm.$data.statisticsModel = data.Data.statisticsModel;
                pager.itemCount = data.Data.count;
                pager.index = $("#currentPageIndex").val();
                pager.render();
                //取消加载
                $(".loading-container").addClass("loading-inactive");
            }
            else {
                parent.layer.msg('查询失败');
                //取消加载
                $(".loading-container").addClass("loading-inactive");
            }
        }
    });
}

/**
 * 新增模态窗口
 */
function newCostContent() {
    initViewModel();
    $("#detailWindow").modal("show");
}

/**
 * 编辑模特窗口
 */
function editCostContent(id) {
    if (!id || id < 1) {
        parent.layer.msg("参数错误");
        return;
    }
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax(hidRootUrl + "/CostNote/GetCostModel?id=" + id, {
        type: "POST",
        success: function (result) {
            if (result.ResultCode == 0 && result.Data) {
                detailVm.$data.costcontentmodel = result.Data;
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
    detailVm.$data.costcontentmodel = {
        Cost: 0,
        CostAddress: "",
        CostChannel: 0,
        CostThing: "",
        CostTime: d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds(),
        CostType: -1,
        Id: 0,
        SpendType: 0
    };
}

function saveCostContentInfo() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax(hidRootUrl + "/CostNote/addCostInfo", {
        type: "POST",
        data: detailVm.$data.costcontentmodel,
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

//分页查询功能
function Search(pageIndex) {
    $("#currentPageIndex").val(pageIndex);
    LoadingActivityResultDetailDate();
}

/**
 * 获取消费类型
 */
function getCostType(spendType) {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    searchVm.$data.costcontentmodel.costtype = -1;
    $.ajax(hidRootUrl + "/CostNote/GetCostType?spendType=" + spendType, {
        type: "POST",
        success: function (result) {
            if (result.ResultCode == 0 && result.Data) {
                searchVm.$data.staticcosttypenum = result.Data;
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
 * 获取消费类型
 */
function getAllCostType() {
    $.ajax(hidRootUrl + "/CostNote/GetCostType?spendType=-1", {
        type: "POST",
        success: function (result) {
            if (result.ResultCode == 0 && result.Data) {
                vm.$data.costtypeenum = result.Data;
                detailVm.$data.staticcosttypenum = result.Data;
            } else {
                parent.layer.msg(result.message);
            }
        },
        error: function () {
            parent.layer.msg("获取类型失败");
        }
    });
}

function getAllCostChannelList() {
    $.ajax(hidRootUrl + "/CostNote/GetCostChannel", {
        type: "POST",
        success: function (result) {
            if (result.ResultCode == 0 && result.Data) {
                detailVm.$data.costchannellist = result.Data;
                searchVm.$data.costchannellist = result.Data;
            } else {
                parent.layer.msg(result.message);
            }
        },
        error: function () {
            parent.layer.msg("获取账户列表失败");
        }
    });
}

getAllCostType();
getAllCostChannelList();
LoadingActivityResultDetailDate();