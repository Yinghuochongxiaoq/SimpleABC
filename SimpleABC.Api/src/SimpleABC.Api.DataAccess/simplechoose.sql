/*
Navicat MySQL Data Transfer

Source Server         : MySQl
Source Server Version : 50717
Source Host           : localhost:3306
Source Database       : simplechoose

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2017-06-09 17:59:26
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for chooseimageinfo
-- ----------------------------
DROP TABLE IF EXISTS `chooseimageinfo`;
CREATE TABLE `chooseimageinfo` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ImageName` varchar(255) NOT NULL,
  `ImageSize` bigint(20) NOT NULL,
  `ImagePath` varchar(255) NOT NULL,
  `ImageCreateTime` datetime NOT NULL,
  `ImageDriverFlag` varchar(255) DEFAULT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of chooseimageinfo
-- ----------------------------

-- ----------------------------
-- Table structure for chooseloginfo
-- ----------------------------
DROP TABLE IF EXISTS `chooseloginfo`;
CREATE TABLE `chooseloginfo` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ContentType` varchar(255) DEFAULT NULL COMMENT 'Request contentType',
  `ErrorMessage` mediumtext COMMENT 'error message',
  `InnerErrorMessage` mediumtext COMMENT 'Inner message.',
  `ErrorTypeFullName` mediumtext COMMENT 'Error type full name',
  `StackTrace` mediumtext COMMENT 'Error stack trace',
  `ErrorTime` datetime NOT NULL COMMENT 'product error time',
  `ErrorType` int(11) NOT NULL DEFAULT '0' COMMENT 'error type enum 0:error;1:warning;2:info',
  PRIMARY KEY (`Id`,`ErrorTime`,`ErrorType`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of chooseloginfo
-- ----------------------------
