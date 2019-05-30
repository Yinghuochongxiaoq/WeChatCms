//根目录
var hidRootUrl = $("#hidRootNode").val();

var searchVm = new Vue({
    el: '#form1',
    data: {
        searchModel: {
            starttime: '',
            endtime: '',
            costchannel: 0
        },
        allChannelList: []
    }
});

var statistical = new Vue({
    el: "#statistical",
    data: {
        statisticalModel: {
            CanPayAcount: 0,
            CostPayAcount: 0
        }
    }
});

/**
 * 获取所有的账户
 */
function getAllCostChannelList() {
    $.ajax(hidRootUrl + "/CostNote/GetCostChannel", {
        type: "POST",
        success: function (result) {
            if (result.ResultCode == 0 && result.Data) {
                searchVm.$data.allChannelList = result.Data;
            } else {
                parent.layer.msg(result.message);
            }
        },
        error: function () {
            parent.layer.msg("获取账户列表失败");
        }
    });
}

/**
 * 获取总的统计信息
 */
function getAllCanPayInfo() {
    //加载
    $(".loading-container").removeClass("loading-inactive");
    $.ajax(hidRootUrl + "/CostNote/GetStatisticalData", {
        type: "POST",
        data: searchVm.$data.searchModel,
        success: function (result) {
            if (result && result.ResultCode == 0) {
                statistical.$data.statisticalModel.CanPayAcount = result.Data.allCanPay;
                statistical.$data.statisticalModel.CostPayAcount = result.Data.allTypeCost;
                drawCanPayLine(result.Data.channelAcount);
                drawCostType(result.Data.costTypeList, result.Data.allTypeCost);
                parent.layer.msg("操作成功!");
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
 *绘制总余额图 堆叠图
 * @param {any} data
 */
function drawCanPayLine(data) {
    var dom = document.getElementById("costPay");
    var myChart = echarts.init(dom);
    var series = [];
    var legend = [];
    if (data && data.length > 0) {
        data.forEach(function (v, i) {
            series.push({
                name: v.CostChannelName,
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: [v.CostCount]
            });
            legend.push(v.CostChannelName);
        });
    }
    var option = {
        title: {
            text: '',
            subtext: '各账户分量'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            data: legend
        },
        grid: {
            left: '0%',
            right: '0%',
            bottom: '0%',
            containLabel: true
        },
        xAxis: {
            type: 'value'
        },
        yAxis: {
            type: 'category',
            data: ['余额']
        },
        series: series
    };
    myChart.setOption(option, true);
}

/**
 * 绘制消费分类图
 * @param {any} data
 * @param {any} allCost
 */
function drawCostType(data, allCost) {
    var dom = document.getElementById("costMonth");
    var myChart = echarts.init(dom);
    var xData = ['总费用'];
    var costData = [allCost];
    var helpShowData = [allCost];
    if (data && data.length > 0) {
        data.forEach(function (v, i) {
            xData.push(v.CostTypeName);
            costData.push(v.CostCount);
            helpShowData.push(helpShowData[i] - v.CostCount);
        });
    }
    helpShowData[0] = 0;
    var option = {
        title: {
            text: '',
            subtext: '分类消费'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            },
            formatter: function (params) {
                var tar = params[1];
                return tar.name + '<br/>' + tar.seriesName + ' : ' + tar.value;
            }
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            splitLine: { show: false },
            data: xData
        },
        yAxis: {
            type: 'value'
        },
        series: [
            {
                name: '辅助',
                type: 'bar',
                stack: '总量',
                itemStyle: {
                    normal: {
                        barBorderColor: 'rgba(0,0,0,0)',
                        color: 'rgba(0,0,0,0)'
                    },
                    emphasis: {
                        barBorderColor: 'rgba(0,0,0,0)',
                        color: 'rgba(0,0,0,0)'
                    }
                },
                data: helpShowData
            },
            {
                name: '生活费',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'inside'
                    }
                },
                data: costData
            }
        ]
    };
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
    }
}

function dayCost() {
    var dom = document.getElementById("dayCost");
    var myChart = echarts.init(dom);
    var app = {};
    option = null;
    function getVirtulData(year) {
        year = year || '2017';
        var date = +echarts.number.parseDate(year + '-01-01');
        var end = +echarts.number.parseDate((+year + 1) + '-01-01');
        var dayTime = 3600 * 24 * 1000;
        var data = [];
        for (var time = date; time < end; time += dayTime) {
            data.push([
                echarts.format.formatTime('yyyy-MM-dd', time),
                Math.floor(Math.random() * 10000)
            ]);
        }
        return data;
    }

    option = {
        title: {
            top: 30,
            left: 'left',
            text: '2016年某人每天的步数'
        },
        tooltip: {},
        visualMap: {
            min: 0,
            max: 10000,
            type: 'piecewise',
            orient: 'horizontal',
            left: 'left',
            top: 65,
            textStyle: {
                color: '#000'
            }
        },
        calendar: {
            top: 120,
            left: 30,
            right: 30,
            cellSize: ['auto', 13],
            range: '2016',
            itemStyle: {
                normal: { borderWidth: 0.5 }
            },
            yearLabel: { show: false }
        },
        series: {
            type: 'heatmap',
            coordinateSystem: 'calendar',
            data: getVirtulData(2016)
        }
    };
    ;
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
    }
}

getAllCostChannelList();
getAllCanPayInfo();
dayCost();