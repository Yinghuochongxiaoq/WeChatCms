/*
Navicat MySQL Data Transfer

Source Server         : MySqlRoot
Source Server Version : 50709
Source Host           : localhost:3306
Source Database       : wechatdb

Target Server Type    : MYSQL
Target Server Version : 50709
File Encoding         : 65001

Date: 2019-03-28 22:26:57
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for syscontent
-- ----------------------------
DROP TABLE IF EXISTS `syscontent`;
CREATE TABLE `syscontent` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `CreateUserId` bigint(20) DEFAULT NULL COMMENT '创建者id（sysuser表id）',
  `Title` varchar(255) DEFAULT NULL COMMENT '标题',
  `Content` mediumtext COMMENT '文章内容',
  `IsDel` int(255) DEFAULT '0' COMMENT '是否已经删除0：未删除，1：已经删除',
  `ContentSource` varchar(255) DEFAULT NULL COMMENT '文章来源',
  `ContentType` varchar(255) DEFAULT NULL COMMENT '内容类型，关联文章类型',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of syscontent
-- ----------------------------
INSERT INTO `syscontent` VALUES ('1', '2019-03-22 14:03:04', '1', '五一放假调整通知', '<p>&nbsp; &nbsp; &nbsp; &nbsp;五一放假调整通知专题为您提供最新最全的五一放假调整通知相关法律知识，以及提供全国各地的五一放假调整通知最快捷律师在线为您提供相关的法律咨询.<br/></p>', '0', '华律网', '75b25bc195304ac381d95ebb00836be4');

-- ----------------------------
-- Table structure for sysdict
-- ----------------------------
DROP TABLE IF EXISTS `sysdict`;
CREATE TABLE `sysdict` (
  `Id` varchar(64) NOT NULL,
  `Value` varchar(100) DEFAULT NULL COMMENT '字典值',
  `Lable` varchar(100) DEFAULT NULL COMMENT '标签',
  `Type` varchar(100) DEFAULT NULL COMMENT '类型',
  `Description` varchar(100) DEFAULT NULL COMMENT '描述',
  `Sort` decimal(10,0) DEFAULT NULL COMMENT '排序值',
  `ParentId` varchar(64) DEFAULT NULL COMMENT '上级id',
  `CreateBy` varchar(64) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `Remarks` varchar(255) DEFAULT NULL COMMENT '备注',
  `IsDel` int(255) DEFAULT NULL COMMENT '是否已经删除0：未删除，1：已经删除',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of sysdict
-- ----------------------------
INSERT INTO `sysdict` VALUES ('75b25bc195304ac381d95ebb00836be4', '行业新闻', '行业新闻', 'ContentType', '行业新闻内容分类', '1', '0', '1', '2019-03-23 15:54:31', '行业新闻内容分类', '0');

-- ----------------------------
-- Table structure for sysmenumodel
-- ----------------------------
DROP TABLE IF EXISTS `sysmenumodel`;
CREATE TABLE `sysmenumodel` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `Name` varchar(255) DEFAULT NULL COMMENT '英文名称',
  `Title` varchar(255) DEFAULT NULL COMMENT '标题',
  `Icon` varchar(255) DEFAULT NULL COMMENT '图标',
  `Url` varchar(255) DEFAULT NULL COMMENT '连接',
  `OrderNo` varchar(255) DEFAULT NULL COMMENT '序号',
  `MenuType` varchar(255) DEFAULT NULL COMMENT '权限类型',
  `Remark` varchar(255) DEFAULT NULL COMMENT '备注',
  `HasPermission` bit(8) DEFAULT NULL COMMENT '是否有权限',
  `ParentId` int(11) DEFAULT NULL COMMENT '上级菜单id',
  `IsDel` int(255) DEFAULT '0' COMMENT '是否已经删除0：未删除，1：已经删除',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of sysmenumodel
-- ----------------------------
INSERT INTO `sysmenumodel` VALUES ('1', 'SystemSet', '系统设置', 'fa-cog', null, '0', '1001', null, '\0', '0', '0');
INSERT INTO `sysmenumodel` VALUES ('2', 'MenuSet', '菜单权限设置', null, '/SysSet/MenuSet', '1', '1002', '菜单权限设置', null, '1', '0');
INSERT INTO `sysmenumodel` VALUES ('3', 'MenuAdmin', '菜单配置', null, '/SysSet/MenuAdmin', '2', '1003', '菜单配置', null, '1', '0');
INSERT INTO `sysmenumodel` VALUES ('5', 'MenuUsers', '管理员列表', 'fa-users', '/SysSet/MenuUsers', '3', '1004', null, '\0', '1', '0');
INSERT INTO `sysmenumodel` VALUES ('6', 'ContentManage', '内容管理', 'fa-coffee', null, '4', '2001', null, '\0', '0', '0');
INSERT INTO `sysmenumodel` VALUES ('7', 'ContentEditPage', '内容编辑', null, '/Content/ContentEdit', '5', '2002', null, '\0', '6', '0');
INSERT INTO `sysmenumodel` VALUES ('8', 'ContentEditList', '内容列表', null, '/Content/ContentList', '6', '2003', null, '\0', '6', '0');
INSERT INTO `sysmenumodel` VALUES ('9', 'SysDicManage', '字典配置', null, '/SysSet/SysDicManage', '7', '1005', null, '\0', '1', '0');
INSERT INTO `sysmenumodel` VALUES ('10', 'ResourceManage', '资源配置', 'fa-folder', null, '8', '3001', null, '\0', '0', '0');
INSERT INTO `sysmenumodel` VALUES ('11', 'ResourceList', '资源列表', null, '/SysResource/ResourceList', '9', '3002', null, '\0', '10', '0');

-- ----------------------------
-- Table structure for sysresource
-- ----------------------------
DROP TABLE IF EXISTS `sysresource`;
CREATE TABLE `sysresource` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '资源id',
  `ResourceUrl` varchar(255) DEFAULT NULL COMMENT '资源链接',
  `ResourceType` int(255) DEFAULT NULL COMMENT '资源类型1：图片；2：视频',
  `ResourceRemark` varchar(255) DEFAULT NULL COMMENT '资源备注',
  `CreateBy` varchar(64) CHARACTER SET utf8 DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `IsDel` int(255) DEFAULT NULL COMMENT '是否已经删除0：未删除，1：已经删除',
  `Sort` int(255) DEFAULT NULL COMMENT '排序值',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of sysresource
-- ----------------------------
INSERT INTO `sysresource` VALUES ('5', '/Uploadfile/ShareDetailImage/20190328/6368940849557084865299367.jpg', '1', null, '1', '2019-03-28 22:21:39', '1', '0');

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser` (
  `Id` bigint(11) NOT NULL AUTO_INCREMENT COMMENT '主键，用户id',
  `UserName` varchar(255) DEFAULT NULL COMMENT '用户名',
  `Password` varchar(255) DEFAULT NULL COMMENT '密码',
  `Sex` int(255) DEFAULT NULL COMMENT '性别：0保密；1：男；2：女',
  `Birthday` datetime DEFAULT NULL COMMENT '出生日期',
  `HeadUrl` varchar(255) DEFAULT NULL COMMENT '头像地址',
  `TelPhone` varchar(255) DEFAULT NULL COMMENT '手机号',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  `CreateAuth` bigint(255) DEFAULT NULL COMMENT '创建人id',
  `UpdateAuth` bigint(11) DEFAULT NULL COMMENT '更新人id',
  `UserType` int(255) DEFAULT '0' COMMENT '用户类型0：普通用户；1：超级管理员；2：普通管理员',
  `IsDel` int(255) DEFAULT '0' COMMENT '是否已经删除0：未删除，1：已经删除',
  `TrueName` varchar(255) DEFAULT NULL COMMENT '真实姓名',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of sysuser
-- ----------------------------
INSERT INTO `sysuser` VALUES ('1', 'FreshMan', '11982e4228792c2c0c9a6e53261dbec4', '1', '1991-07-31 20:56:15', '/Uploadfile/ShareDetailImage/20190326/6368921088309469071603066.jpg', '18883257311', '2018-11-25 20:57:34', '2018-11-29 21:05:56', '1', '1', '1', '0', 'FreshMan');

-- ----------------------------
-- Table structure for sysusermenu
-- ----------------------------
DROP TABLE IF EXISTS `sysusermenu`;
CREATE TABLE `sysusermenu` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `UserId` int(11) DEFAULT NULL COMMENT '用户id',
  `MenuId` int(11) DEFAULT NULL COMMENT '菜单id',
  `IsDel` int(255) DEFAULT '0' COMMENT '是否已经删除0：未删除，1：已经删除',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of sysusermenu
-- ----------------------------
INSERT INTO `sysusermenu` VALUES ('22', '1', '1', '1');
INSERT INTO `sysusermenu` VALUES ('23', '1', '2', '1');
INSERT INTO `sysusermenu` VALUES ('24', '1', '3', '1');
INSERT INTO `sysusermenu` VALUES ('25', '1', '5', '1');
INSERT INTO `sysusermenu` VALUES ('26', '1', '9', '1');
INSERT INTO `sysusermenu` VALUES ('27', '1', '6', '1');
INSERT INTO `sysusermenu` VALUES ('28', '1', '7', '1');
INSERT INTO `sysusermenu` VALUES ('29', '1', '8', '1');
INSERT INTO `sysusermenu` VALUES ('30', '1', '1', '0');
INSERT INTO `sysusermenu` VALUES ('31', '1', '2', '0');
INSERT INTO `sysusermenu` VALUES ('32', '1', '3', '0');
INSERT INTO `sysusermenu` VALUES ('33', '1', '5', '0');
INSERT INTO `sysusermenu` VALUES ('34', '1', '9', '0');
INSERT INTO `sysusermenu` VALUES ('35', '1', '6', '0');
INSERT INTO `sysusermenu` VALUES ('36', '1', '7', '0');
INSERT INTO `sysusermenu` VALUES ('37', '1', '8', '0');
INSERT INTO `sysusermenu` VALUES ('38', '1', '10', '0');
INSERT INTO `sysusermenu` VALUES ('39', '1', '11', '0');
