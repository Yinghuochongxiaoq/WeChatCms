//根目录
var hidRootUrl = $("#hidRootNode").val();
var tree_id = "user_tree";
var user_id = 0;

/**
 * 处理权限
 */
function delPowerContent(id, userTree) {
    tree_id = userTree;
    user_id = id;
    var index = layer.load();
    $.ajax({
        url: hidRootUrl + "/SysSet/GetUserPowerList?id=" + id,
        type: "POST",
        success: function (dataStr) {
            var data = JSON.parse(dataStr);
            if (data && data.ResultCode == 0) {
                $.fn.zTree.init($("#" + tree_id), setting, data.Data);
            }
            else {
                layer.msg('查询失败');
            }
            layer.close(index);
        }
    });
    $("#powerWindow").modal("show");
}

/*
 * 权限树初始化设置
 */
var setting = {
    view: {
        dblClickExpand: false,//双击节点时，是否自动展开父节点的标识
        showLine: true,//是否显示节点之间的连线
        fontCss: { 'color': 'black', 'font-weight': 'bold' },//字体样式函数
        selectedMulti: false //设置是否允许同时选中多个节点
    },
    check: {
        chkStyle: "checkbox",//复选框类型
        enable: true //每个节点上是否显示 CheckBox
    },
    data: {
        simpleData: {//简单数据模式
            enable: true,
            idKey: "id",
            pIdKey: "pId",
            rootPId: ""
        }
    },
    callback: {
        beforeClick: function (treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj();
            if (treeNode.isParent) {
                zTree.expandNode(treeNode);//如果是父节点，则展开该节点
            } else {
                zTree.checkNode(treeNode, !treeNode.checked, true, true);//单击勾选，再次单击取消勾选
            }
        }
    }
};

/*
 * 测试数据
 */
var zNodes = [
    {
        id: 1,
        pId: 0,
        name: "test 1",
        open: false,
        checked: true,
        children: [
            { name: "子节点1" },
            { name: "子节点2" }
        ]
    },
    {
        id: 11,
        pId: 1,
        name: "test 1-1",
        open: true,
        checked: true
    },
    {
        id: 111,
        pId: 11,
        name: "test 1-1-1",
        checked: true
    },
    {
        id: 112,
        pId: 11,
        name: "test 1-1-2",
        checked: true
    },
    {
        id: 12,
        pId: 1,
        name: "test 1-2",
        checked: true,
        open: false
    }
];

/*
 * 保存
 */
function savePowerDataInfo() {
    var treeObj = $.fn.zTree.getZTreeObj(tree_id),
        nodes = treeObj.getCheckedNodes(true),
        v = [];
    for (var i = 0; i < nodes.length; i++) {
        v.push(nodes[i].id); //获取选中节点的值
    }
    var postData = {};
    postData.id = user_id;
    postData.listIds = v;
    var index = layer.load();
    $.ajax(hidRootUrl + "/SysSet/SaveUserPower", {
        type: "POST",
        data: postData,
        success: function (result) {
            if (result && result.ResultCode == 0) {
                parent.layer.msg("操作成功!");
                $("#powerWindow").modal("hide");
                Search(1);
            } else
                parent.layer.msg(result.Message);
        },
        error: function () {
            parent.layer.msg("系统异常");
        }
    });
    layer.close(index);
}