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

 Date: 11/04/2019 14:53:46
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;


-- ----------------------------
-- Table structure for costchannel
-- ----------------------------
DROP TABLE IF EXISTS `costchannel`;
CREATE TABLE `costchannel` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CostChannelName` varchar(255) DEFAULT NULL COMMENT '渠道名称',
  `CostChannelNo` varchar(255) DEFAULT NULL COMMENT '渠道账号',
  `IsDel` int(255) DEFAULT NULL COMMENT '1:启用；0：删除',
  `IsValid` int(255) DEFAULT '1' COMMENT '1:启用；0：停用',
  `UserId` bigint(11) DEFAULT NULL COMMENT '用户id',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `CreateUserId` bigint(11) DEFAULT NULL,
  `UpdateUserId` bigint(11) DEFAULT NULL,
  `Sort` int(255) DEFAULT '0' COMMENT '排序值',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of costchannel
-- ----------------------------

-- ----------------------------
-- Table structure for costcontent
-- ----------------------------
DROP TABLE IF EXISTS `costcontent`;
CREATE TABLE `costcontent` (
  `Id` bigint(11) NOT NULL AUTO_INCREMENT,
  `Cost` decimal(10,2) DEFAULT NULL,
  `UserId` bigint(11) DEFAULT NULL,
  `CostAddress` varchar(255) DEFAULT NULL,
  `CostTime` datetime DEFAULT NULL,
  `CostThing` varchar(255) DEFAULT NULL,
  `CostTypeName` varchar(255) DEFAULT NULL COMMENT '消费类型快照',
  `CostType` int(11) DEFAULT NULL,
  `CostYear` int(255) DEFAULT NULL,
  `CostMonth` int(255) DEFAULT NULL,
  `SpendType` int(255) NOT NULL DEFAULT '-1' COMMENT '2：转移；1:收入；0：支出',
  `CostChannel` bigint(255) DEFAULT NULL COMMENT '渠道id',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `CreateUserId` bigint(11) DEFAULT NULL,
  `UpdateUserId` bigint(11) DEFAULT NULL,
  `CostChannelName` varchar(255) DEFAULT NULL COMMENT '渠道名称',
  `CostChannelNo` varchar(255) DEFAULT NULL COMMENT '渠道账号',
  `CostInOrOut` bit(1) DEFAULT NULL COMMENT '1:入账；0：出账',
  `LinkCostId` bigint(11) DEFAULT NULL COMMENT '关联记录id',
  `LinkCostChannelName` varchar(255) DEFAULT NULL COMMENT '关联渠道名称',
  `LinkCostChannelNo` varchar(255) DEFAULT NULL COMMENT '关联渠道账号',
  `LinkCostChannel` bigint(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of costcontent
-- ----------------------------

-- ----------------------------
-- Table structure for costtype
-- ----------------------------
DROP TABLE IF EXISTS `costtype`;
CREATE TABLE `costtype` (
  `Id` bigint(11) NOT NULL AUTO_INCREMENT COMMENT '主键，类型id',
  `Name` varchar(255) DEFAULT NULL COMMENT '类型名称',
  `IsValid` int(11) DEFAULT '1' COMMENT '是否有效',
  `IsDel` int(255) DEFAULT '0' COMMENT '是否删除',
  `Sort` int(255) DEFAULT '0' COMMENT '排序值',
  `SpendType` int(255) NOT NULL COMMENT '1:收入；0：支出',
  `UserId` bigint(11) DEFAULT NULL COMMENT '用户id',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `CreateUserId` bigint(11) DEFAULT NULL,
  `UpdateUserId` bigint(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of costtype
-- ----------------------------
 
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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for syscontentviewinfo
-- ----------------------------
DROP TABLE IF EXISTS `syscontentviewinfo`;
CREATE TABLE `syscontentviewinfo`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '创建时间',
  `ContentId` bigint(20) NULL DEFAULT NULL COMMENT '文章id',
  `Ip` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '浏览ip',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Browser` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '浏览器类型',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `sysdict` VALUES ('24bdffa061314b87a3f34942301d8713', 'BlogIndexPageSlider', '博客首页广告轮播', 'AdvertiseType', '博客首页广告轮播', 4, '0', '1', '2019-04-11 12:21:23', '博客首页广告轮播', 0);
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

CREATE TABLE `wechatdb`.`WeChatAccount`  (
  `Id` bigint(0) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `OpenId` varchar(255) NULL COMMENT '微信OpenId',
  `NickName` varchar(255) NULL COMMENT '昵称获取微信授权的昵称或重命名',
  `AvatarUrl` varchar(255) NULL COMMENT '头像',
  `Gender` int(255) NULL COMMENT '性别1:女；2:男',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IsDel` int(255) NULL DEFAULT NULL COMMENT '是否已经删除0：未删除，1：已经删除',
  `AccountId` bigint(0) NULL DEFAULT NULL COMMENT '关联的用户id',
  PRIMARY KEY (`Id`)
);

SET FOREIGN_KEY_CHECKS = 1;
