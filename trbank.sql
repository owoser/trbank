/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MariaDB
 Source Server Version : 110002 (11.0.2-MariaDB)
 Source Host           : localhost:3306
 Source Schema         : trbank

 Target Server Type    : MariaDB
 Target Server Version : 110002 (11.0.2-MariaDB)
 File Encoding         : 65001

 Date: 05/08/2023 18:03:34
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for trbank
-- ----------------------------
DROP TABLE IF EXISTS `trbank`;
CREATE TABLE `trbank`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `money` int(11) UNSIGNED DEFAULT NULL,
  `xuid` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
