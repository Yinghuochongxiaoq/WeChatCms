/*
 Navicat Premium Data Transfer

 Source Server         : MySqlLocalHost
 Source Server Type    : MySQL
 Source Server Version : 50724
 Source Host           : localhost:3306
 Source Schema         : wechatdb

 Target Server Type    : MySQL
 Target Server Version : 50724
 File Encoding         : 65001

 Date: 09/04/2019 17:20:47
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for customercomment
-- ----------------------------
DROP TABLE IF EXISTS `customercomment`;
CREATE TABLE `customercomment`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IsDel` int(255) NULL DEFAULT 0 COMMENT '是否已经删除0：未删除，1：已经删除',
  `Content` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '内容',
  `CustomerName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '姓名',
  `CustomerPhone` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '电话',
  `CustomerEmail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '邮箱',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `HasDeal` int(255) NULL DEFAULT NULL COMMENT '是否已经处理0：未处理，1：已处理',
  `DealResult` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '处理结果',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysadvertisement
-- ----------------------------
DROP TABLE IF EXISTS `sysadvertisement`;
CREATE TABLE `sysadvertisement`  (
  `Id` bigint(255) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `AdvertiName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告名称',
  `AdvertiType` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告类型(对应字典表中的主键)',
  `AdvertiTip` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告语',
  `CreateBy` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsDel` int(255) NULL DEFAULT NULL COMMENT '是否已经删除0：未删除，1：已经删除',
  `Sort` decimal(10, 0) NULL DEFAULT NULL COMMENT '排序值',
  `ResourceUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '关联资源url',
  `AdvertiUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告链接地址',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for syscontent
-- ----------------------------
DROP TABLE IF EXISTS `syscontent`;
CREATE TABLE `syscontent`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `CreateUserId` bigint(20) NULL DEFAULT NULL COMMENT '创建者id（sysuser表id）',
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '标题',
  `Content` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '文章内容',
  `IsDel` int(255) NULL DEFAULT 0 COMMENT '是否已经删除0：未删除，1：已经删除',
  `ContentSource` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '文章来源',
  `ContentType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '内容类型，关联文章类型',
  `Introduction` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '简介',
  `ContentFlag` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '标签',
  `ContentDisImage` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '文章封面图',
  `AttachmentFile` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '附件地址',
  `AttachmentFileSize` double NULL DEFAULT NULL COMMENT '附件大小',
  `AttachmentFileName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '附件名称',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of syscontent
-- ----------------------------
INSERT INTO `syscontent` VALUES (1, '2019-03-22 14:03:04', 1, '五一放假调整通知', '<p>&nbsp; &nbsp; &nbsp; &nbsp;五一放假调整通知专题为您提供最新最全的五一放假调整通知相关法律知识，以及提供全国各地的五一放假调整通知最快捷律师在线为您提供相关的法律咨询.<br/></p>', 0, '华律网', '75b25bc195304ac381d95ebb00836be4', NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `syscontent` VALUES (2, '2019-03-23 15:58:42', 1, '根据数组对象的某个属性值找到指定的元素', '<p><span style=\"color: rgb(47, 47, 47); font-family: -apple-system, &quot;SF UI Text&quot;, Arial, &quot;PingFang SC&quot;, &quot;Hiragino Sans GB&quot;, &quot;Microsoft YaHei&quot;, &quot;WenQuanYi Micro Hei&quot;, sans-serif; font-weight: 700; background-color: rgb(255, 255, 255);\">filter() 方法将匹配元素集合缩减为匹配指定选择器的元素.该方法不改变原数组,返回的是筛选后满足条件的数组.</span></p><p><br/></p><pre class=\"brush:js;toolbar:false\">var&nbsp;datas&nbsp;=&nbsp;[\n&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;name&quot;:&nbsp;&quot;商品房&quot;,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;bianma&quot;:&nbsp;&quot;11&quot;\n&nbsp;&nbsp;&nbsp;&nbsp;},\n&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;name&quot;:&nbsp;&quot;商铺&quot;,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;bianma&quot;:&nbsp;&quot;12&quot;\n&nbsp;&nbsp;&nbsp;&nbsp;}\n&nbsp;]\n&nbsp;var&nbsp;data=&nbsp;datas.filter(function(item){&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return&nbsp;item.bianma&nbsp;==&nbsp;&quot;12&quot;;&nbsp;\n})console.log(data);&nbsp;//&nbsp;[{name:&nbsp;&quot;商铺&quot;,&nbsp;bianma:&nbsp;&quot;12&quot;}]</pre><p><br/></p>', 0, '断线の风筝', '75b25bc195304ac381d95ebb00836be4', 'filter() 方法将匹配元素集合缩减为匹配指定选择器的元素.该方法不改变原数组,返回的是筛选后满足条件的数组.', 'filter', NULL, NULL, NULL, NULL);
INSERT INTO `syscontent` VALUES (3, '2019-04-09 14:19:40', 1, '英国：让自动驾驶“一路绿灯”', '<p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\"><strong>L4上路测试，L5概念据实</strong></p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　根据驾驶自动化程度，国际汽车工程师协会(SAE International)将自动驾驶汽车分为6级：L0指无自动化驾驶(即人工驾驶)；L1是驾驶辅助；L2为部分自动化驾驶；L3是有条件自动化驾驶；L4是指高度自动驾驶；L5是完全自动驾驶。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　目前，L3已经在许多国家的限定地区内行驶。作为全球首款L4级自动驾驶汽车，百度Apollo在去年7月量产下线后，许多厂商的L4陆续开始测试。英国豪华轿车生产商阿斯顿·马丁(拉贡达)总裁安迪·帕尔马博士预计，L4将在2020年—2021年上路，L5将于2030年前上路。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　XPI模拟公司产品专家考利向科技日报记者展示了他们设计的一款L5自动驾驶轿车的虚拟模型。在这款带有四个发动机、可前后双向行驶的自动驾驶轿车里，只有平躺的乘员座椅，乘员向汽车发出指令后，就可以毫不操心地到达指定地点。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　SMMT最新研究显示，英国在自动驾驶技术商业化方面已经领先于全球竞争对手，并将英国排在德国、美国、日本和韩国等其他主要汽车生产国之前，称其将成为CAVs大规模出现的全球主要市场。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　<strong>自动驾驶技术发展在英国一路绿灯</strong></p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　根据SMMT的研究，到2030年，自动驾驶汽车将给英国带来每年620亿英镑的市场机会，使之成为全球CAVs的最大市场，并将给英国创造约42万个新的就业岗位。同时，得益于使用驾驶辅助和自动驾驶技术的巨大安全效益，英国可以在未来10年里预防4.7万起严重事故，挽救3900人的生命。自动驾驶汽车还将提高上班族生活质量，让他们每年节省一周以上的路途劳累时间。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　英国商业、能源与产业部负责科研与创新事务的部长克里斯·斯基德莫尔在4日的会议上说，自动驾驶汽车的发展需要来自各个方面的合作，而不仅仅只是政府的责任。根据产业发展战略，在今年晚些时候，政府将支持日产等公司与英国大学、高速公路和相关公司合作，开启英国有史以来最复杂的自动控制汽车行程。他指出，政府的支持计划还包括建立创新实验室，以应对自动驾驶船舶和智能航运带来的法规挑战。另外，政府还加强“监管储备”，以针对诸如会飞的出租车和自动系统等创新技术制定合理的监管政策。他认为，英国政府在自动驾驶汽车的管理、组织和投资方面都做好了准备。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　<strong>脱欧让英国汽车产业未来充满变数</strong></p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　为变现英国在自动驾驶汽车领域的巨大潜力，实现在2021年让自动驾驶汽车在英国上路的雄心，SMMT建议政府尽快更新道路交通法规，使4G覆盖所有公路网，鼓励地方政府与行业合作，加强城市流动服务，特别是对未来产生重要影响的国际法规进行协调。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　脱欧大戏仍在上演，业内人士则对脱欧可能给英国汽车产业带来的伤害担忧无比。如果脱欧后没能与欧盟达成相关协议，那么今后英国制造和进口汽车的成本将大幅提高，会极大地削弱市场竞争力，还将对英国的声誉造成持久损害，使自动驾驶汽车当前积攒的优势面临削弱的风险。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　因此，SMMT指出，英国必须有序地退出欧盟，达成一项既支持行业合作、又支持技术合作(尤其是在数据方面)的协议。</p><p style=\"margin-top: 0px; margin-bottom: 0px; padding: 5px 10px; color: rgb(51, 51, 51); letter-spacing: 1px; font-family: 微软雅黑; font-size: 15px; white-space: normal; background-color: rgb(238, 242, 246);\">　　SMMT首席执行官迈克·霍斯表示，脱欧损害了英国在全球政治稳定方面的声誉，浪费了宝贵的时间和投资，英国汽车产业的成功取决于有利的脱欧协议，以及协调的产业合作和无摩擦的贸易政策。</p><p><br/></p>', 0, '今日视点', '75b25bc195304ac381d95ebb00836be4', '在英国汽车制造商和贸易商协会(SMMT)4月4日主办的有关“联网与自动驾驶汽车(CAVs)”技术与产业发展大会上，科技日报记者了解到，英国政府和汽车业界对自动驾驶汽车的技术发展和上路行驶高度重视，未来10年，英国有望成为全球自动驾驶汽车的头号市场。', '自动驾驶', '/Uploadfile/ShareDetailImage/20190409/6369041627600007735418011.png', '/Uploadfile/ShareDetailImage/20190409/6369041649467512263512750.rar', 2028, 'GeekUninstaller.rar');

-- ----------------------------
-- Table structure for sysdict
-- ----------------------------
DROP TABLE IF EXISTS `sysdict`;
CREATE TABLE `sysdict`  (
  `Id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '字典值',
  `Lable` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '标签',
  `Type` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '类型',
  `Description` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '描述',
  `Sort` decimal(10, 0) NULL DEFAULT NULL COMMENT '排序值',
  `ParentId` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '上级id',
  `CreateBy` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsDel` int(255) NULL DEFAULT NULL COMMENT '是否已经删除0：未删除，1：已经删除',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysdict
-- ----------------------------
INSERT INTO `sysdict` VALUES ('406f080a2d7b41c38e9b930275bf2022', 'FeelLife', '生活感悟', 'ContentType', '生活感悟文章', 2, NULL, '1', '2019-04-09 15:16:40', '生活感悟文章', 0);
INSERT INTO `sysdict` VALUES ('75b25bc195304ac381d95ebb00836be4', 'Technology', '杂谈', 'ContentType', '生活杂谈内容分类', 1, NULL, '1', '2019-03-23 15:54:31', '杂谈内容分类', 0);
INSERT INTO `sysdict` VALUES ('872bf5c7005941e98091d71010cfaca1', 'OpenSourceArea', '开源专区', 'ContentType', '开源专区文章', 3, NULL, '1', '2019-04-09 15:15:33', '开源专区文章', 0);

-- ----------------------------
-- Table structure for sysmenumodel
-- ----------------------------
DROP TABLE IF EXISTS `sysmenumodel`;
CREATE TABLE `sysmenumodel`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '英文名称',
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '标题',
  `Icon` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '图标',
  `Url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '连接',
  `OrderNo` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '序号',
  `MenuType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '权限类型',
  `Remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `HasPermission` bit(8) NULL DEFAULT NULL COMMENT '是否有权限',
  `ParentId` int(11) NULL DEFAULT NULL COMMENT '上级菜单id',
  `IsDel` int(255) NULL DEFAULT 0 COMMENT '是否已经删除0：未删除，1：已经删除',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 16 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysmenumodel
-- ----------------------------
INSERT INTO `sysmenumodel` VALUES (1, 'SystemSet', '系统设置', 'fa-cog', NULL, '0', '1001', NULL, b'00000000', 0, 0);
INSERT INTO `sysmenumodel` VALUES (2, 'MenuSet', '菜单权限设置', NULL, '/SysSet/MenuSet', '1', '1002', '菜单权限设置', NULL, 1, 0);
INSERT INTO `sysmenumodel` VALUES (3, 'MenuAdmin', '菜单配置', NULL, '/SysSet/MenuAdmin', '2', '1003', '菜单配置', NULL, 1, 0);
INSERT INTO `sysmenumodel` VALUES (5, 'MenuUsers', '管理员列表', 'fa-users', '/SysSet/MenuUsers', '3', '1004', NULL, b'00000000', 1, 0);
INSERT INTO `sysmenumodel` VALUES (6, 'ContentManage', '内容管理', 'fa-coffee', NULL, '4', '2001', NULL, b'00000000', 0, 0);
INSERT INTO `sysmenumodel` VALUES (7, 'ContentEditPage', '内容编辑', NULL, '/Content/ContentEdit', '5', '2002', NULL, b'00000000', 6, 0);
INSERT INTO `sysmenumodel` VALUES (8, 'ContentEditList', '内容列表', NULL, '/Content/ContentList', '6', '2003', NULL, b'00000000', 6, 0);
INSERT INTO `sysmenumodel` VALUES (9, 'SysDicManage', '字典配置', NULL, '/SysSet/SysDicManage', '7', '1005', NULL, b'00000000', 1, 0);
INSERT INTO `sysmenumodel` VALUES (10, 'ResourceManage', '资源管理', 'fa-folder', NULL, NULL, '3001', NULL, b'00000000', 0, 0);
INSERT INTO `sysmenumodel` VALUES (11, 'ResourceList', '资源列表', NULL, '/SysResource/ResourceList', '1', '3002', NULL, b'00000000', 10, 0);
INSERT INTO `sysmenumodel` VALUES (12, 'AdvertiseManage', '广告设置', 'fa-tags', NULL, '20', '4001', NULL, b'00000000', 0, 0);
INSERT INTO `sysmenumodel` VALUES (13, 'AdvertiseList', '广告列表', NULL, '/SysAdvertise/SysAdvertiseList', '1', '4002', NULL, b'00000000', 12, 0);
INSERT INTO `sysmenumodel` VALUES (14, 'CustomerCommentManage', '客户留言', 'fa-comment', NULL, '10', '5001', NULL, b'00000000', 0, 0);
INSERT INTO `sysmenumodel` VALUES (15, 'CustomerCommentList', '留言列表', NULL, '/CustomerComment/CustomerCommentList', '11', '5002', NULL, b'00000000', 14, 0);

-- ----------------------------
-- Table structure for sysresource
-- ----------------------------
DROP TABLE IF EXISTS `sysresource`;
CREATE TABLE `sysresource`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '资源id',
  `ResourceUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '资源链接',
  `ResourceType` int(255) NULL DEFAULT NULL COMMENT '资源类型1：图片；2：视频',
  `ResourceRemark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '资源备注',
  `CreateBy` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `IsDel` int(255) NULL DEFAULT NULL COMMENT '是否已经删除0：未删除，1：已经删除',
  `Sort` int(255) NULL DEFAULT NULL COMMENT '排序值',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser`  (
  `Id` bigint(11) NOT NULL AUTO_INCREMENT COMMENT '主键，用户id',
  `UserName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '用户名',
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '密码',
  `Sex` int(255) NULL DEFAULT NULL COMMENT '性别：0保密；1：男；2：女',
  `Birthday` datetime(0) NULL DEFAULT NULL COMMENT '出生日期',
  `HeadUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '头像地址',
  `TelPhone` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '手机号',
  `CreateTime` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime(0) NULL DEFAULT NULL COMMENT '修改时间',
  `CreateAuth` bigint(255) NULL DEFAULT NULL COMMENT '创建人id',
  `UpdateAuth` bigint(11) NULL DEFAULT NULL COMMENT '更新人id',
  `UserType` int(255) NULL DEFAULT 0 COMMENT '用户类型0：普通用户；1：超级管理员；2：普通管理员',
  `IsDel` int(255) NULL DEFAULT 0 COMMENT '是否已经删除0：未删除，1：已经删除',
  `TrueName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '真实姓名',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysuser
-- ----------------------------
INSERT INTO `sysuser` VALUES (1, 'FreshMan', '11982e4228792c2c0c9a6e53261dbec4', 1, '1991-07-31 20:56:15', '/Uploadfile/ShareDetailImage/20190326/6368921088309469071603066.jpg', '18883257311', '2018-11-25 20:57:34', '2018-11-29 21:05:56', 1, 1, 1, 0, 'FreshMan');
INSERT INTO `sysuser` VALUES (2, 'YangHongJun', '11982e4228792c2c0c9a6e53261dbec4', 2, '2016-11-29 21:06:15', NULL, '18523989760', '2018-11-29 21:06:41', '2018-11-29 21:06:41', 1, 1, 2, 0, '杨洪俊');

-- ----------------------------
-- Table structure for sysusermenu
-- ----------------------------
DROP TABLE IF EXISTS `sysusermenu`;
CREATE TABLE `sysusermenu`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `UserId` int(11) NULL DEFAULT NULL COMMENT '用户id',
  `MenuId` int(11) NULL DEFAULT NULL COMMENT '菜单id',
  `IsDel` int(255) NULL DEFAULT 0 COMMENT '是否已经删除0：未删除，1：已经删除',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 54 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysusermenu
-- ----------------------------
INSERT INTO `sysusermenu` VALUES (7, 2, 1, 0);
INSERT INTO `sysusermenu` VALUES (8, 2, 5, 0);
INSERT INTO `sysusermenu` VALUES (22, 1, 1, 1);
INSERT INTO `sysusermenu` VALUES (23, 1, 2, 1);
INSERT INTO `sysusermenu` VALUES (24, 1, 3, 1);
INSERT INTO `sysusermenu` VALUES (25, 1, 5, 1);
INSERT INTO `sysusermenu` VALUES (26, 1, 9, 1);
INSERT INTO `sysusermenu` VALUES (27, 1, 6, 1);
INSERT INTO `sysusermenu` VALUES (28, 1, 7, 1);
INSERT INTO `sysusermenu` VALUES (29, 1, 8, 1);
INSERT INTO `sysusermenu` VALUES (30, 1, 10, 1);
INSERT INTO `sysusermenu` VALUES (31, 1, 11, 1);
INSERT INTO `sysusermenu` VALUES (32, 1, 1, 1);
INSERT INTO `sysusermenu` VALUES (33, 1, 2, 1);
INSERT INTO `sysusermenu` VALUES (34, 1, 3, 1);
INSERT INTO `sysusermenu` VALUES (35, 1, 5, 1);
INSERT INTO `sysusermenu` VALUES (36, 1, 9, 1);
INSERT INTO `sysusermenu` VALUES (37, 1, 6, 1);
INSERT INTO `sysusermenu` VALUES (38, 1, 7, 1);
INSERT INTO `sysusermenu` VALUES (39, 1, 8, 1);
INSERT INTO `sysusermenu` VALUES (40, 1, 10, 0);
INSERT INTO `sysusermenu` VALUES (41, 1, 11, 0);
INSERT INTO `sysusermenu` VALUES (42, 1, 1, 0);
INSERT INTO `sysusermenu` VALUES (43, 1, 2, 0);
INSERT INTO `sysusermenu` VALUES (44, 1, 3, 0);
INSERT INTO `sysusermenu` VALUES (45, 1, 5, 0);
INSERT INTO `sysusermenu` VALUES (46, 1, 9, 0);
INSERT INTO `sysusermenu` VALUES (47, 1, 14, 0);
INSERT INTO `sysusermenu` VALUES (48, 1, 15, 0);
INSERT INTO `sysusermenu` VALUES (49, 1, 12, 0);
INSERT INTO `sysusermenu` VALUES (50, 1, 13, 0);
INSERT INTO `sysusermenu` VALUES (51, 1, 6, 0);
INSERT INTO `sysusermenu` VALUES (52, 1, 7, 0);
INSERT INTO `sysusermenu` VALUES (53, 1, 8, 0);

SET FOREIGN_KEY_CHECKS = 1;
