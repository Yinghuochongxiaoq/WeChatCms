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

 Date: 26/03/2019 16:16:10
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

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
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of syscontent
-- ----------------------------
INSERT INTO `syscontent` VALUES (1, '2019-03-22 14:03:04', 1, '五一放假调整通知', '<p>&nbsp; &nbsp; &nbsp; &nbsp;五一放假调整通知专题为您提供最新最全的五一放假调整通知相关法律知识，以及提供全国各地的五一放假调整通知最快捷律师在线为您提供相关的法律咨询.<br/></p>', 0, '华律网', '75b25bc195304ac381d95ebb00836be4');
INSERT INTO `syscontent` VALUES (2, '2019-03-23 15:58:42', 1, '根据数组对象的某个属性值找到指定的元素', '<p><span style=\"color: rgb(47, 47, 47); font-family: -apple-system, &quot;SF UI Text&quot;, Arial, &quot;PingFang SC&quot;, &quot;Hiragino Sans GB&quot;, &quot;Microsoft YaHei&quot;, &quot;WenQuanYi Micro Hei&quot;, sans-serif; font-weight: 700; background-color: rgb(255, 255, 255);\">filter() 方法将匹配元素集合缩减为匹配指定选择器的元素.该方法不改变原数组,返回的是筛选后满足条件的数组.</span></p><p><br/></p><pre class=\"brush:js;toolbar:false\">var&nbsp;datas&nbsp;=&nbsp;[\n&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;name&quot;:&nbsp;&quot;商品房&quot;,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;bianma&quot;:&nbsp;&quot;11&quot;\n&nbsp;&nbsp;&nbsp;&nbsp;},\n&nbsp;&nbsp;&nbsp;&nbsp;{&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;name&quot;:&nbsp;&quot;商铺&quot;,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&quot;bianma&quot;:&nbsp;&quot;12&quot;\n&nbsp;&nbsp;&nbsp;&nbsp;}\n&nbsp;]\n&nbsp;var&nbsp;data=&nbsp;datas.filter(function(item){&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;return&nbsp;item.bianma&nbsp;==&nbsp;&quot;12&quot;;&nbsp;\n})console.log(data);&nbsp;//&nbsp;[{name:&nbsp;&quot;商铺&quot;,&nbsp;bianma:&nbsp;&quot;12&quot;}]</pre><p><br/></p>', 0, '断线の风筝', '75b25bc195304ac381d95ebb00836be4');

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
INSERT INTO `sysdict` VALUES ('75b25bc195304ac381d95ebb00836be4', '行业新闻', '行业新闻', 'ContentType', '行业新闻内容分类', 1, '0', '1', '2019-03-23 15:54:31', '行业新闻内容分类', 0);

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
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB AUTO_INCREMENT = 30 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysusermenu
-- ----------------------------
INSERT INTO `sysusermenu` VALUES (7, 2, 1, 0);
INSERT INTO `sysusermenu` VALUES (8, 2, 5, 0);
INSERT INTO `sysusermenu` VALUES (22, 1, 1, 0);
INSERT INTO `sysusermenu` VALUES (23, 1, 2, 0);
INSERT INTO `sysusermenu` VALUES (24, 1, 3, 0);
INSERT INTO `sysusermenu` VALUES (25, 1, 5, 0);
INSERT INTO `sysusermenu` VALUES (26, 1, 9, 0);
INSERT INTO `sysusermenu` VALUES (27, 1, 6, 0);
INSERT INTO `sysusermenu` VALUES (28, 1, 7, 0);
INSERT INTO `sysusermenu` VALUES (29, 1, 8, 0);

SET FOREIGN_KEY_CHECKS = 1;
