/*
  NXAI 养生壶 · System 表（从 netcoreai/docs/nx_devices_erp.sql 原样拷贝，含数据）
  库：nxai
*/
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for sys_config
-- ----------------------------
DROP TABLE IF EXISTS `sys_config`;
CREATE TABLE `sys_config`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `key` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Parameter key',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Parameter name',
  `value` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Parameter value',
  `remark` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Remark',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'System parameter' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_config
-- ----------------------------
INSERT INTO `sys_config` VALUES (654337157616325, 653335112912901, '2025-03-14 09:24:44.641988', 653335112912901, '2025-03-14 09:24:44.642019', 'weixin-key', '微信接口Key', '343sfsdfas', '微信接口配置');

-- ----------------------------
-- Table structure for sys_dictionary
-- ----------------------------
DROP TABLE IF EXISTS `sys_dictionary`;
CREATE TABLE `sys_dictionary`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `remark` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Dictionary' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dictionary
-- ----------------------------
INSERT INTO `sys_dictionary` VALUES (654231800692741, 653335112912901, '2025-03-14 02:16:02.842381', 653335112912901, '2025-03-14 02:16:02.847194', 'notice_type', '通知类型', '', 1);
INSERT INTO `sys_dictionary` VALUES (654240219889733, 653335112912901, '2025-03-14 02:50:18.256197', 653335112912901, '2025-03-14 02:50:50.768281', 'notice_level', '通知级别', '', 1);
INSERT INTO `sys_dictionary` VALUES (657672185329605, 653335112912901, '2025-03-23 19:35:00.432169', 653335112912901, '2025-03-23 19:37:07.391462', 'exchange_behavior', '客户金额变动类型', '', 1);
INSERT INTO `sys_dictionary` VALUES (657672310503365, 653335112912901, '2025-03-23 19:35:30.952618', 653335112912901, '2025-03-23 19:37:20.731602', 'exchage_status', '客户金额变动状态', '', 1);

-- ----------------------------
-- Table structure for sys_dictionary_data
-- ----------------------------
DROP TABLE IF EXISTS `sys_dictionary_data`;
CREATE TABLE `sys_dictionary_data`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `dictcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `label` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `value` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `tagtype` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` tinyint(1) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Dictionary data' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dictionary_data
-- ----------------------------
INSERT INTO `sys_dictionary_data` VALUES (654239258492997, 653335112912901, '2025-03-14 02:46:23.753258', 653335112912901, '2025-03-14 02:46:23.764096', 'notice_type', '系统升级', '1', 'success', 1, 1);
INSERT INTO `sys_dictionary_data` VALUES (654239345217605, 653335112912901, '2025-03-14 02:46:44.662244', 653335112912901, '2025-03-14 02:46:44.662315', 'notice_type', '系统维护', '2', 'warning', 1, 2);
INSERT INTO `sys_dictionary_data` VALUES (654239670661189, 653335112912901, '2025-03-14 02:48:04.130043', 653335112912901, '2025-03-14 02:48:17.928995', 'notice_type', '安全警告', '3', 'info', 1, 3);
INSERT INTO `sys_dictionary_data` VALUES (654239799701573, 653335112912901, '2025-03-14 02:48:35.619266', 653335112912901, '2025-03-14 02:48:35.619300', 'notice_type', '假期通知', '4', 'primary', 1, 4);
INSERT INTO `sys_dictionary_data` VALUES (654239873306693, 653335112912901, '2025-03-14 02:48:53.589871', 653335112912901, '2025-03-14 02:48:53.589908', 'notice_type', '公司新闻', '5', 'danger', 1, 5);
INSERT INTO `sys_dictionary_data` VALUES (654240025727045, 653335112912901, '2025-03-14 02:49:30.801847', 653335112912901, '2025-03-14 02:49:47.775236', 'notice_type', '其他', '7', 'info', 1, 99);
INSERT INTO `sys_dictionary_data` VALUES (654241282986053, 653335112912901, '2025-03-14 02:54:37.749547', 653335112912901, '2025-03-14 02:54:37.749570', 'notice_level', '高', 'H', 'danger', 1, 1);
INSERT INTO `sys_dictionary_data` VALUES (654241329270853, 653335112912901, '2025-03-14 02:54:49.049727', 653335112912901, '2025-03-14 02:55:31.410027', 'notice_level', '中', 'M', 'primary', 1, 2);
INSERT INTO `sys_dictionary_data` VALUES (654241459761221, 653335112912901, '2025-03-14 02:55:20.908143', 653335112912901, '2025-03-14 02:55:40.136094', 'notice_level', '低', 'L', 'info', 1, 3);
INSERT INTO `sys_dictionary_data` VALUES (657672538613701, 653335112912901, '2025-03-23 19:36:26.683774', 653335112912901, '2025-03-25 12:55:08.585748', 'exchange_behavior', '后台充值', '8000', 'success', 1, 0);
INSERT INTO `sys_dictionary_data` VALUES (657672875403205, 653335112912901, '2025-03-23 19:37:48.899555', 653335112912901, '2025-03-23 19:38:30.251295', 'exchage_status', '处理中', '2000', 'primary', 1, 0);
INSERT INTO `sys_dictionary_data` VALUES (657672927889349, 653335112912901, '2025-03-23 19:38:01.680144', 653335112912901, '2025-03-23 19:38:44.726467', 'exchage_status', '已完成', '2008', 'success', 1, 3);
INSERT INTO `sys_dictionary_data` VALUES (657673015670725, 653335112912901, '2025-03-23 19:38:23.111797', 653335112912901, '2025-03-23 19:38:48.499734', 'exchage_status', '已失败', '2016', 'danger', 1, 6);
INSERT INTO `sys_dictionary_data` VALUES (658281065769541, 653335112912901, '2025-03-25 12:52:32.845266', 653335112912901, '2025-03-25 12:55:21.807517', 'exchange_behavior', '订单交易', '8008', 'primary', 1, 1);

-- ----------------------------
-- Table structure for sys_eventtracker
-- ----------------------------
DROP TABLE IF EXISTS `sys_eventtracker`;
CREATE TABLE `sys_eventtracker`  (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_sys_eventtracker_eventid_trackername`(`eventid`, `trackername`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_eventtracker
-- ----------------------------

-- ----------------------------
-- Table structure for sys_menu
-- ----------------------------
DROP TABLE IF EXISTS `sys_menu`;
CREATE TABLE `sys_menu`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `parentid` bigint(20) NOT NULL COMMENT 'Parent menu ID',
  `parentids` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Parent menu ID path',
  `name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Name',
  `perm` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Permission code',
  `routename` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Route name',
  `routepath` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Route path',
  `type` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Menu type',
  `component` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Component configuration',
  `visible` tinyint(1) NOT NULL COMMENT 'Visible',
  `redirect` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Redirect route path',
  `icon` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Icon',
  `keepalive` tinyint(1) NOT NULL COMMENT 'Enable page caching',
  `alwaysshow` tinyint(1) NOT NULL COMMENT 'Always show when there is only one child route',
  `params` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Route parameters',
  `ordinal` int(11) NOT NULL COMMENT 'Ordinal',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Menu' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_menu
-- ----------------------------
INSERT INTO `sys_menu` VALUES (653342080185029, 1000000000000, '2025-03-11 13:55:46.028479', 653335112912901, '2026-06-08 11:56:38.188649', 0, '[0]', '系统管理', '', '', '/system', 'CATALOG', 'Layout', 1, '/system/user', 'role', 1, 1, '', 10);
INSERT INTO `sys_menu` VALUES (653342584296133, 1000000000000, '2025-03-11 13:57:49.016162', 653335112912901, '2025-03-14 09:10:34.425720', 653342080185029, '[0][653342080185029]', '菜单管理', '', 'Menu', 'menu', 'MENU', 'system/menu/index', 1, '', 'menu', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (653343418946245, 1000000000000, '2025-03-11 14:01:12.781334', 653335112912901, '2025-03-12 02:18:57.594467', 653342584296133, '[0][653342080185029][653342584296133]', '菜单新增', 'menu-create', '', '', 'BUTTON', '', 1, '', '', 1, 1, '', 1);
INSERT INTO `sys_menu` VALUES (653389651534725, 653335112912901, '2025-03-11 17:09:20.259624', 653335112912901, '2025-03-12 02:18:51.063178', 653342584296133, '[0][653342080185029][653342584296133]', '菜单修改', 'menu-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '111=111', 2);
INSERT INTO `sys_menu` VALUES (653524636169093, 653335112912901, '2025-03-12 02:18:35.485594', 653335112912901, '2025-03-12 02:18:35.490303', 653342584296133, '[0][653342080185029][653342584296133]', '菜单删除', 'menu-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (653525628846981, 653335112912901, '2025-03-12 02:22:37.641550', 653335112912901, '2025-03-12 17:18:54.181204', 653342080185029, '[0][653342080185029]', '角色管理', '', 'Role', 'role', 'MENU', 'system/role/index', 1, '', 'el-icon-Trophy', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (653525880882053, 653335112912901, '2025-03-12 02:23:39.176354', 653335112912901, '2025-03-12 02:23:39.176387', 653525628846981, '[0][653342080185029][653525628846981]', '角色新增', 'role-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (653525982651269, 653335112912901, '2025-03-12 02:24:04.021653', 653335112912901, '2025-03-12 02:24:04.021694', 653525628846981, '[0][653342080185029][653525628846981]', '角色修改', 'role-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (653526164857733, 653335112912901, '2025-03-12 02:24:48.546878', 653335112912901, '2025-03-12 02:24:48.546926', 653525628846981, '[0][653342080185029][653525628846981]', '角色删除', 'role-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (653526703580037, 653335112912901, '2025-03-12 02:27:00.032542', 653335112912901, '2025-03-15 01:31:28.153432', 653525628846981, '[0][653342080185029][653525628846981]', '角色查询', 'role-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (653688386045317, 653335112912901, '2025-03-12 13:24:53.568100', 1000000000000, '2026-07-14 10:48:26.772844', 653342080185029, '[0][653342080185029]', '机构管理', '', 'Dept', 'dept', 'MENU', 'system/dept/index', 0, '', 'el-icon-CoffeeCup', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (653688962147717, 653335112912901, '2025-03-12 13:27:13.929212', 653335112912901, '2025-03-12 13:27:13.929284', 653688386045317, '[0][653342080185029][653688386045317]', '机构新增', 'org-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (653689048724869, 653335112912901, '2025-03-12 13:27:35.065392', 653335112912901, '2025-03-12 13:29:55.810715', 653688386045317, '[0][653342080185029][653688386045317]', '机构修改', 'org-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (653689299236229, 653335112912901, '2025-03-12 13:28:36.228932', 653335112912901, '2025-03-12 13:30:04.669707', 653688386045317, '[0][653342080185029][653688386045317]', '机构删除', 'org-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (653689402680709, 653335112912901, '2025-03-12 13:29:01.481851', 653335112912901, '2025-03-15 01:30:38.237695', 653688386045317, '[0][653342080185029][653688386045317]', '机构查询', 'org-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (653689923114373, 653335112912901, '2025-03-12 13:31:08.537851', 653335112912901, '2025-03-15 01:34:27.281727', 653342584296133, '[0][653342080185029][653342584296133]', '菜单查询', 'menu-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (653745302407109, 653335112912901, '2025-03-12 17:16:29.199235', 653335112912901, '2025-03-12 17:18:47.314183', 653342080185029, '[0][653342080185029]', '用户管理', '', 'User', 'user', 'MENU', 'system/user/index', 1, '', 'el-icon-Avatar', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (653745429395397, 653335112912901, '2025-03-12 17:16:59.877507', 653335112912901, '2025-03-12 17:16:59.877621', 653745302407109, '[0][653342080185029][653745302407109]', '用户新增', 'user-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (653745520240581, 653335112912901, '2025-03-12 17:17:22.056285', 653335112912901, '2025-03-12 17:17:22.056480', 653745302407109, '[0][653342080185029][653745302407109]', '用户修改', 'user-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (653756020922437, 653335112912901, '2025-03-12 18:00:05.906431', 653335112912901, '2025-03-15 01:32:11.120967', 653745302407109, '[0][653342080185029][653745302407109]', '用户查询', 'user-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (653787841784005, 653335112912901, '2025-03-12 20:09:34.640689', 653335112912901, '2025-03-12 20:09:34.644133', 653745302407109, '[0][653342080185029][653745302407109]', '重置密码', 'user-reset-password', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 5);
INSERT INTO `sys_menu` VALUES (653804678302149, 653335112912901, '2025-03-12 21:18:05.045862', 653335112912901, '2025-03-12 21:18:05.050174', 653745302407109, '[0][653342080185029][653745302407109]', '用户删除', 'user-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 6);
INSERT INTO `sys_menu` VALUES (653805800937925, 653335112912901, '2025-03-12 21:22:39.031803', 653335112912901, '2025-03-12 21:22:39.031846', 653745302407109, '[0][653342080185029][653745302407109]', '用户导入', 'user-import', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 7);
INSERT INTO `sys_menu` VALUES (653805917993413, 653335112912901, '2025-03-12 21:23:07.610400', 653335112912901, '2025-03-12 21:23:07.610542', 653745302407109, '[0][653342080185029][653745302407109]', '用户导出', 'user-export', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 8);
INSERT INTO `sys_menu` VALUES (654225897013253, 653335112912901, '2025-03-14 01:52:01.907929', 653335112912901, '2025-03-14 01:53:03.674597', 653342080185029, '[0][653342080185029]', '字典管理', '', 'Dict', 'dict', 'MENU', 'system/dict/index', 1, '', 'el-icon-Discount', 1, 0, '', 5);
INSERT INTO `sys_menu` VALUES (654226073489413, 653335112912901, '2025-03-14 01:52:44.618434', 653335112912901, '2025-03-14 01:52:44.618516', 654225897013253, '[0][654225897013253]', '字典新增', 'dict-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (654226273665029, 653335112912901, '2025-03-14 01:53:33.486091', 653335112912901, '2025-03-14 01:54:08.179769', 654225897013253, '[0][653342080185029][654225897013253]', '字典修改', 'dict-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (654226371764229, 653335112912901, '2025-03-14 01:53:57.431053', 653335112912901, '2025-03-14 01:53:57.431184', 654225897013253, '[0][653342080185029][654225897013253]', '字典删除', 'dict-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (654226547773445, 653335112912901, '2025-03-14 01:54:40.402966', 653335112912901, '2025-03-15 01:34:59.384276', 654225897013253, '[0][653342080185029][654225897013253]', '字典查询', 'dict-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (654227048943621, 653335112912901, '2025-03-14 01:56:42.757506', 653335112912901, '2025-03-14 03:00:46.258079', 653342080185029, '[0][653342080185029]', '字典数据', '', 'DictData', 'dict-data', 'MENU', 'system/dict/data', 0, '', 'el-icon-CollectionTag', 1, 0, '', 6);
INSERT INTO `sys_menu` VALUES (654227246395397, 653335112912901, '2025-03-14 01:57:30.967744', 653335112912901, '2025-03-14 01:57:30.967803', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据新增', 'dictdata-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (654227348602885, 653335112912901, '2025-03-14 01:57:55.923311', 653335112912901, '2025-03-14 01:57:55.923356', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据修改', 'dictdata-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (654227455537157, 653335112912901, '2025-03-14 01:58:22.024924', 653335112912901, '2025-03-14 01:58:22.024962', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据删除', 'dictdata-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (654227565006853, 653335112912901, '2025-03-14 01:58:48.754711', 653335112912901, '2025-03-15 01:35:45.770369', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据查询', 'dictdata-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (654243405697093, 653335112912901, '2025-03-14 03:03:16.107668', 1000000000000, '2026-07-14 10:48:34.720798', 653342080185029, '[0][653342080185029]', '系统配置', '', 'Config', 'config', 'MENU', 'system/config/index', 0, '', 'el-icon-Setting', 1, 0, '', 7);
INSERT INTO `sys_menu` VALUES (654332804104837, 653335112912901, '2025-03-14 09:07:02.281041', 653335112912901, '2025-03-14 09:07:02.289492', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置新增', 'sysconfig-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (654332952756869, 653335112912901, '2025-03-14 09:07:38.205504', 653335112912901, '2025-03-14 09:07:47.036752', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置修改', 'sysconfig-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (654333157950085, 653335112912901, '2025-03-14 09:08:28.289495', 653335112912901, '2025-03-14 09:08:28.289582', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置删除', 'sysconfig-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (654333310177925, 653335112912901, '2025-03-14 09:09:05.451614', 653335112912901, '2025-03-15 01:36:15.355682', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置查询', 'sysconfig-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (654442132821701, 653337659433029, '2025-03-14 16:31:53.539063', 1000000000000, '2026-07-14 10:48:45.454335', 0, '[0]', '运维管理', '', '', '/maint', 'CATALOG', 'Layout', 0, '', 'el-icon-Opportunity', 1, 1, '', 11);
INSERT INTO `sys_menu` VALUES (654442722764485, 653337659433029, '2025-03-14 16:34:17.507347', 653335112912901, '2025-03-23 19:31:39.244337', 654442132821701, '[0][654442132821701]', '操作日志', '', 'OperateLog', 'operatelog', 'MENU', 'maint/log/index', 1, '', 'el-icon-Search', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (654455068768133, 653335112912901, '2025-03-14 17:24:32.167647', 653335112912901, '2025-03-15 01:36:42.106666', 654442722764485, '[0][654442132821701][654442722764485]', '操作日志查询', 'operationlog-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (654523403131077, 653335112912901, '2025-03-14 22:02:35.101176', 653335112912901, '2025-03-23 19:31:34.280401', 654442132821701, '[0][654442132821701]', '登录日志', '', 'LoginLog', 'loginlog', 'MENU', 'maint/log/loginlog', 1, '', 'el-icon-ChatLineRound', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (654523795179717, 653335112912901, '2025-03-14 22:04:10.598917', 653335112912901, '2025-03-15 01:37:04.449633', 654523403131077, '[0][654442132821701][654523403131077]', '登录日志查询', 'loginlog-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (654788688007877, 653335112912901, '2025-03-15 16:02:03.031386', 653335112912901, '2025-03-15 16:02:03.039044', 653525628846981, '[0][653342080185029][653525628846981]', '角色权限设置', 'role-setperms', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 5);
INSERT INTO `sys_menu` VALUES (657665676666821, 653335112912901, '2025-03-23 19:08:31.507308', 653335112912901, '2026-06-08 11:57:39.503337', 0, '[0]', '客户管理', '', '', '/cust', 'CATALOG', 'Layout', 1, '/cust/customer', 'el-icon-Basketball', 1, 1, '', 4);
INSERT INTO `sys_menu` VALUES (657666023151557, 653335112912901, '2025-03-23 19:09:55.957689', 653335112912901, '2025-03-23 19:16:31.039329', 657665676666821, '[0][657665676666821]', '客户信息', '', 'Customer', 'customer', 'MENU', 'cust/index', 1, '', 'el-icon-CircleCheck', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (657666337880005, 653335112912901, '2025-03-23 19:11:12.794987', 653335112912901, '2025-03-23 19:11:12.795007', 657666023151557, '[0][657665676666821][657666023151557]', '客户新增', 'customer-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (657666452023237, 653335112912901, '2025-03-23 19:11:40.660263', 653335112912901, '2025-03-23 19:11:40.660293', 657666023151557, '[0][657665676666821][657666023151557]', '客户查询', 'customer-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (657666599712709, 653335112912901, '2025-03-23 19:12:16.718766', 653335112912901, '2025-03-23 19:12:16.718793', 657666023151557, '[0][657665676666821][657666023151557]', '客户充值', 'customer-recharge', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (657666958747589, 653335112912901, '2025-03-23 19:13:44.372250', 653335112912901, '2025-03-23 19:17:01.672635', 657665676666821, '[0][657665676666821]', '充值记录', '', 'TransactionLog', 'transactionlog', 'MENU', 'cust/transactionlog', 1, '', 'el-icon-Grape', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (657667115956165, 653335112912901, '2025-03-23 19:14:22.752792', 653335112912901, '2025-03-23 19:14:22.752820', 657666958747589, '[0][657665676666821][657666958747589]', '充值查询', 'customer-search-transactionlog', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (657668727089093, 653335112912901, '2025-03-23 19:20:56.104578', 653335112912901, '2025-03-23 19:31:28.031597', 654442132821701, '[0][654442132821701]', '客户中心CAP', '', 'CustCapDashboard', 'eventbus-cust-dashboard', 'MENU', 'maint/eventbus/cust-cap-dashboard', 1, '', 'el-icon-Ship', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (657670470342597, 653335112912901, '2025-03-23 19:28:01.718644', 653335112912901, '2025-03-23 19:31:45.945099', 654442132821701, '[0][654442132821701]', 'Loki日志', '', '', 'http://62.234.187.128:9010', 'EXTLINK', '', 1, '', 'el-icon-Cherry', 1, 0, '', 8);
INSERT INTO `sys_menu` VALUES (657670977337285, 653335112912901, '2025-03-23 19:30:05.477607', 653335112912901, '2025-03-23 19:30:05.477628', 654442132821701, '[0][654442132821701]', '容器管理', '', '', 'http://62.234.187.128:9000', 'EXTLINK', '', 1, '', 'el-icon-Compass', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812653792118021, 653335112912901, '2026-06-04 17:56:46.836115', 653335112912901, '2026-06-08 11:57:20.478469', 0, '[0]', '基础资料', '', '', '/masterdata', 'CATALOG', 'Layout', 1, '', 'el-icon-DataBoard', 1, 1, '', 9);
INSERT INTO `sys_menu` VALUES (812654410413317, 653335112912901, '2026-06-04 17:59:17.694203', 653335112912901, '2026-06-04 17:59:29.271663', 812653792118021, '[0][812653792118021]', '物料管理', '', 'material', 'material', 'MENU', 'masterdata/material/index', 1, '', 'el-icon-Suitcase', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812890595538181, 653335112912901, '2026-06-05 10:00:20.074680', 653335112912901, '2026-06-05 10:00:20.074695', 812653792118021, '[0][812653792118021]', '物料分类', '', 'materialCategory', 'materialCategory', 'MENU', 'masterdata/materialCategory/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812890810012933, 653335112912901, '2026-06-05 10:01:12.436406', 653335112912901, '2026-06-05 10:01:12.436418', 812653792118021, '[0][812653792118021]', '计量单位', '', 'unit', 'uni', 'MENU', 'masterdata/unit/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812890927891717, 653335112912901, '2026-06-05 10:01:41.215381', 653335112912901, '2026-06-05 10:01:54.504556', 812653792118021, '[0][812653792118021]', '供应商管理', '', 'supplier', 'supplier', 'MENU', 'masterdata/supplier/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812912411931909, 653335112912901, '2026-06-05 11:29:06.416716', 653335112912901, '2026-06-05 11:29:06.418349', 812653792118021, '[0][812653792118021]', '仓库管理', '', 'warehouse', 'warehouse', 'MENU', 'masterdata/warehouse/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812955682157829, 653335112912901, '2026-06-05 14:25:10.441732', 653335112912901, '2026-06-08 10:43:55.151797', 0, '[0]', '供应链', '', '', '/supplyChain', 'CATALOG', 'Layout', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812956047721733, 653335112912901, '2026-06-05 14:26:39.614588', 653335112912901, '2026-06-05 14:26:39.614638', 812955682157829, '[0][812955682157829]', '采购单', '', 'purchaseOrder', 'purchaseOrder', 'MENU', 'supplychain/purchaseOrder/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812956163073285, 653335112912901, '2026-06-05 14:27:07.774720', 653335112912901, '2026-06-08 11:55:12.501273', 812955682157829, '[0][812955682157829]', '采购入库', '', 'stockInbound', 'stockInbound', 'MENU', 'supplychain/stockInbound/index', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (812956383307013, 653335112912901, '2026-06-05 14:28:01.543290', 653335112912901, '2026-06-08 11:57:29.737217', 0, '[0]', '产品管理', '', 'product', '/product', 'CATALOG', 'Layout', 1, '', '', 1, 0, '', 8);
INSERT INTO `sys_menu` VALUES (812956596425989, 653335112912901, '2026-06-05 14:28:53.572206', 653335112912901, '2026-06-05 14:29:06.294184', 812956383307013, '[0][812956383307013]', 'Bom产品（生产）', '', 'bom', 'bom', 'MENU', 'product/bom/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812956861986053, 653335112912901, '2026-06-05 14:29:58.408467', 653335112912901, '2026-06-05 16:07:31.762505', 812956383307013, '[0][812956383307013]', '销售套装', '', 'bomsale', 'bomsale', 'MENU', 'product/bom/sale', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812957308704005, 653335112912901, '2026-06-05 14:31:47.469091', 653335112912901, '2026-06-05 14:31:47.469103', 812956383307013, '[0][812956383307013]', '产品管理（成品）', '', 'finishedProduct', 'finishedProduct', 'MENU', 'product/finishedProduct/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (812982941320453, 653335112912901, '2026-06-05 16:16:05.523048', 653335112912901, '2026-06-08 11:57:01.581106', 0, '[0]', '生产管理', '', '', '/manufacturing', 'CATALOG', 'Layout', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (812983083382021, 653335112912901, '2026-06-05 16:16:40.117006', 653335112912901, '2026-06-05 16:16:40.117098', 812982941320453, '[0][812982941320453]', '生产单', '', 'workOrder', 'workOrder', 'MENU', 'manufacturing/workOrder/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (813963541873925, 653335112912901, '2026-06-08 10:46:09.871733', 653335112912901, '2026-06-08 11:55:17.635440', 812955682157829, '[0][812955682157829]', '库存查询', '', 'inventoryBalance', 'inventoryBalance', 'MENU', 'supplychain/inventoryBalance/index', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (813963702568197, 653335112912901, '2026-06-08 10:46:49.095472', 653335112912901, '2026-06-08 11:55:05.023515', 812955682157829, '[0][812955682157829]', '序列号查询', '', 'inventorySerial', 'inventorySerial', 'MENU', 'supplychain/inventorySerial/index', 1, '', '', 1, 0, '', 5);
INSERT INTO `sys_menu` VALUES (813963861050629, 653335112912901, '2026-06-08 10:47:27.789182', 653335112912901, '2026-06-08 11:55:26.586552', 812955682157829, '[0][812955682157829]', '采购退货', '', 'purchaseReturn', 'purchaseReturn', 'MENU', 'supplychain/purchaseReturn/index', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (813964328215813, 653335112912901, '2026-06-08 10:49:21.844915', 653335112912901, '2026-06-08 11:55:50.523569', 812955682157829, '[0][812955682157829]', '销售出库', '', 'stockOutbound', 'stockOutbound', 'MENU', 'supplychain/stockOutbound/index', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (813964515616005, 653335112912901, '2026-06-08 10:50:07.594195', 653335112912901, '2026-06-08 11:56:03.442979', 812982941320453, '[0][812982941320453]', '生产退料', '', 'materialReturn', 'materialReturn', 'MENU', 'supplychain/materialReturn/index', 1, '', '', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (813964643558661, 653335112912901, '2026-06-08 10:50:38.831725', 653335112912901, '2026-06-08 11:56:08.326021', 812982941320453, '[0][812982941320453]', '产品测试', '', 'productUnit', 'productUnit', 'MENU', 'manufacturing/productUnit/index', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (813964725294341, 653335112912901, '2026-06-08 10:50:58.785094', 653335112912901, '2026-06-08 11:57:12.710452', 0, '[0]', '销售', '', 'sales', '/sales', 'CATALOG', 'Layout', 1, '', '', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (813964847981829, 653335112912901, '2026-06-08 10:51:28.736820', 653335112912901, '2026-06-08 10:51:28.736832', 813964725294341, '[0][813964725294341]', '销售订单', '', 'order', 'order', 'MENU', 'sales/order/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (814007767123205, 653335112912901, '2026-06-08 13:46:07.153168', 653335112912901, '2026-06-08 13:46:07.155241', 812982941320453, '[0][812982941320453]', '生产领料', '', 'materialIssue', 'materialIssue', 'MENU', 'manufacturing/materialIssue/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (814648632751365, 653335112912901, '2026-06-10 09:13:48.432230', 653335112912901, '2026-06-10 09:29:18.612381', 812653792118021, '[0][812653792118021]', '系统设置', '', 'aiRegionConfig', 'aiRegionConfig', 'MENU', 'masterdata/aiRegionConfig/index', 1, '', '', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (815425963181317, 1000000000000, '2026-06-12 13:56:46.374657', 1000000000000, '2026-06-12 13:57:00.370895', 812982941320453, '[0][812982941320453]', '生产入库', '', 'stockInbound', 'stockInbound', 'MENU', 'manufacturing/stockInbound/index', 1, '', '', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (920442132821701, 0, '2026-07-08 16:27:33.613068', 1000000000000, '2026-07-13 16:02:28.372459', 0, '[0]', '客户中心', '', 'CustomerCenter', '/customer', 'CATALOG', 'Layout', 1, 'customer-list', 'user', 0, 1, '', 45);
INSERT INTO `sys_menu` VALUES (920442132821711, 0, '2026-07-08 16:27:33.613942', 0, '2026-07-08 16:27:33.613942', 920442132821701, '[0][920442132821701]', '历史客户', 'customer-search', 'CustomerList', 'customer-list', 'MENU', 'customer/customer/index', 1, '', 'peoples', 1, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821712, 0, '2026-07-08 16:27:33.614643', 0, '2026-07-08 16:27:33.614643', 920442132821711, '[0][920442132821701][920442132821711]', '新增客户', 'customer-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821713, 0, '2026-07-08 16:27:33.614643', 0, '2026-07-08 16:27:33.614643', 920442132821711, '[0][920442132821701][920442132821711]', '修改客户', 'customer-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821714, 0, '2026-07-08 16:27:33.614643', 0, '2026-07-08 16:27:33.614643', 920442132821711, '[0][920442132821701][920442132821711]', '删除客户', 'customer-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821731, 0, '2026-07-08 16:27:33.617488', 1000000000000, '2026-07-13 16:04:29.681732', 920442132821701, '[0][920442132821701]', '推荐类型', 'diagnosis-dimension-search', 'DiagnosisMatchDimension', 'diagnosis-match-dimension', 'MENU', 'customer/diagnosis-match-dimension/index', 1, '', 'connection', 1, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821732, 0, '2026-07-08 16:27:33.618304', 1000000000000, '2026-07-13 16:07:16.681286', 920442132821731, '[0][920442132821701][920442132821731]', '新增推荐类型', 'diagnosis-dimension-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821733, 0, '2026-07-08 16:27:33.618304', 1000000000000, '2026-07-13 16:07:25.414361', 920442132821731, '[0][920442132821701][920442132821731]', '修改推荐类型', 'diagnosis-dimension-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821734, 0, '2026-07-08 16:27:33.618304', 1000000000000, '2026-07-13 16:07:32.539843', 920442132821731, '[0][920442132821701][920442132821731]', '删除推荐类型', 'diagnosis-dimension-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821735, 0, '2026-07-08 16:27:33.618304', 1000000000000, '2026-07-13 16:07:49.318622', 920442132821731, '[0][920442132821701][920442132821731]', '新增诊断结果', 'diagnosis-dimension-item-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 4);
INSERT INTO `sys_menu` VALUES (920442132821736, 0, '2026-07-08 16:27:33.618304', 1000000000000, '2026-07-13 16:07:57.552648', 920442132821731, '[0][920442132821701][920442132821731]', '修改诊断结果', 'diagnosis-dimension-item-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 5);
INSERT INTO `sys_menu` VALUES (920442132821737, 0, '2026-07-08 16:27:33.618304', 1000000000000, '2026-07-13 16:08:05.494707', 920442132821731, '[0][920442132821701][920442132821731]', '删除诊断结果', 'diagnosis-dimension-item-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 6);
INSERT INTO `sys_menu` VALUES (920442132821741, 0, '2026-07-08 16:27:33.619257', 1000000000000, '2026-07-13 16:02:41.626686', 920442132821701, '[0][920442132821701]', '医疗字典', 'medical-dict-search', 'MedicalDict', 'medical-dict', 'MENU', 'customer/medical-dict/index', 1, '', 'collection', 1, 0, '', 4);
INSERT INTO `sys_menu` VALUES (920442132821742, 0, '2026-07-08 16:27:33.620018', 1000000000000, '2026-07-13 16:08:21.172487', 920442132821741, '[0][920442132821701][920442132821741]', '新增字典', 'medical-dict-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821743, 0, '2026-07-08 16:27:33.620018', 1000000000000, '2026-07-13 16:08:29.042652', 920442132821741, '[0][920442132821701][920442132821741]', '修改字典', 'medical-dict-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821744, 0, '2026-07-08 16:27:33.620018', 1000000000000, '2026-07-13 16:08:36.096480', 920442132821741, '[0][920442132821701][920442132821741]', '删除字典', 'medical-dict-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821745, 0, '2026-07-08 16:27:33.620018', 1000000000000, '2026-07-13 16:08:46.390764', 920442132821741, '[0][920442132821701][920442132821741]', '新增字典项', 'medical-dict-item-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 4);
INSERT INTO `sys_menu` VALUES (920442132821746, 0, '2026-07-08 16:27:33.620018', 1000000000000, '2026-07-13 16:08:53.148270', 920442132821741, '[0][920442132821701][920442132821741]', '修改字典项', 'medical-dict-item-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 5);
INSERT INTO `sys_menu` VALUES (920442132821747, 0, '2026-07-08 16:27:33.620018', 1000000000000, '2026-07-13 16:09:01.219228', 920442132821741, '[0][920442132821701][920442132821741]', '删除字典项', 'medical-dict-item-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 6);
INSERT INTO `sys_menu` VALUES (920442132821751, 0, '2026-07-09 17:08:42.030366', 1000000000000, '2026-07-13 16:05:03.961859', 920442132821701, '[0][920442132821701]', '推荐方案', 'regulation-strategy-search', 'RegulationStrategy', 'regulation-strategy', 'MENU', 'customer/regulation-strategy/index', 1, '', 'guide', 1, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821752, 0, '2026-07-09 17:08:42.031090', 1000000000000, '2026-07-13 16:05:52.426402', 920442132821751, '[0][920442132821701][920442132821751]', '新增方案', 'regulation-strategy-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821753, 0, '2026-07-09 17:08:42.031090', 1000000000000, '2026-07-13 16:06:00.385300', 920442132821751, '[0][920442132821701][920442132821751]', '修改方案', 'regulation-strategy-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821754, 0, '2026-07-09 17:08:42.031090', 1000000000000, '2026-07-13 16:06:08.406254', 920442132821751, '[0][920442132821701][920442132821751]', '删除方案', 'regulation-strategy-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821755, 0, '2026-07-09 17:08:42.031090', 1000000000000, '2026-07-13 16:06:17.604215', 920442132821751, '[0][920442132821701][920442132821751]', '发布方案', 'regulation-strategy-publish', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 4);
INSERT INTO `sys_menu` VALUES (920442132821756, 0, '2026-07-09 17:08:42.031090', 1000000000000, '2026-07-13 16:06:38.107581', 920442132821751, '[0][920442132821701][920442132821751]', '高级规则', 'regulation-strategy-advanced', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 5);
INSERT INTO `sys_menu` VALUES (920442132821761, 0, '2026-07-13 15:02:11.611120', 1000000000000, '2026-07-13 16:03:55.996397', 920442132821701, '[0][920442132821701]', '输出维度', 'regulation-output-channel-search', 'RegulationOutputChannel', 'regulation-output-channel', 'MENU', 'customer/regulation-output-channel/index', 1, '', 'grid', 1, 0, '', 5);
INSERT INTO `sys_menu` VALUES (920442132821762, 0, '2026-07-13 15:02:11.611827', 1000000000000, '2026-07-13 16:09:12.224347', 920442132821761, '[0][920442132821701][920442132821761]', '新增输出维度', 'regulation-output-channel-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821763, 0, '2026-07-13 15:02:11.611827', 1000000000000, '2026-07-13 16:09:18.739720', 920442132821761, '[0][920442132821701][920442132821761]', '修改输出维度', 'regulation-output-channel-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821764, 0, '2026-07-13 15:02:11.611827', 1000000000000, '2026-07-13 16:09:27.528604', 920442132821761, '[0][920442132821701][920442132821761]', '删除输出维度', 'regulation-output-channel-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821771, 0, '2026-07-13 17:30:26.785149', 0, '2026-07-13 17:30:26.785149', 920442132821701, '[0][920442132821701]', '收费包', 'regulation-ent-sku-search', 'RegulationEntitlementSku', 'regulation-entitlement-sku', 'MENU', 'customer/regulation-entitlement-sku/index', 1, '', 'money', 1, 0, '', 6);
INSERT INTO `sys_menu` VALUES (920442132821772, 0, '2026-07-13 17:30:26.786543', 0, '2026-07-13 17:30:26.786543', 920442132821771, '[0][920442132821701][920442132821771]', '新增收费包', 'regulation-ent-sku-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821773, 0, '2026-07-13 17:30:26.786543', 0, '2026-07-13 17:30:26.786543', 920442132821771, '[0][920442132821701][920442132821771]', '修改收费包', 'regulation-ent-sku-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821774, 0, '2026-07-13 17:30:26.786543', 0, '2026-07-13 17:30:26.786543', 920442132821771, '[0][920442132821701][920442132821771]', '删除收费包', 'regulation-ent-sku-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821781, 0, '2026-07-13 17:30:26.787674', 0, '2026-07-13 17:30:26.787674', 920442132821701, '[0][920442132821701]', '权益订单', 'regulation-ent-order-search', 'RegulationEntitlementOrder', 'regulation-entitlement-order', 'MENU', 'customer/regulation-entitlement-order/index', 1, '', 'shopping-cart', 1, 0, '', 7);
INSERT INTO `sys_menu` VALUES (920442132821782, 0, '2026-07-13 17:30:26.788829', 0, '2026-07-13 17:30:26.788829', 920442132821781, '[0][920442132821701][920442132821781]', '手动开单', 'regulation-ent-order-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821783, 0, '2026-07-13 17:30:26.788829', 0, '2026-07-13 17:30:26.788829', 920442132821781, '[0][920442132821701][920442132821781]', '撤销权益', 'regulation-ent-grant-revoke', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821791, 0, '2026-07-14 10:46:30.718646', 0, '2026-07-14 10:46:30.718646', 920442132821701, '[0][920442132821701]', 'LLM 模型', 'llm-profile-search', 'LlmProfile', 'llm-profile', 'MENU', 'customer/llm-profile/index', 1, '', 'cpu', 1, 0, '', 8);
INSERT INTO `sys_menu` VALUES (920442132821792, 0, '2026-07-14 10:46:30.719910', 0, '2026-07-14 10:46:30.719910', 920442132821791, '[0][920442132821701][920442132821791]', '新增模型', 'llm-profile-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821793, 0, '2026-07-14 10:46:30.719910', 0, '2026-07-14 10:46:30.719910', 920442132821791, '[0][920442132821701][920442132821791]', '修改模型', 'llm-profile-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821794, 0, '2026-07-14 10:46:30.719910', 0, '2026-07-14 10:46:30.719910', 920442132821791, '[0][920442132821701][920442132821791]', '删除模型', 'llm-profile-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821795, 0, '2026-07-14 10:46:30.719910', 0, '2026-07-14 10:46:30.719910', 920442132821791, '[0][920442132821701][920442132821791]', '绑定客户', 'llm-profile-bind', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 4);
INSERT INTO `sys_menu` VALUES (920442132821801, 0, '2026-07-24 17:16:06.710380', 0, '2026-07-24 17:16:06.710380', 920442132821701, '[0][920442132821701]', 'LLM 提示词', 'llm-prompt-search', 'LlmPrompt', 'llm-prompt', 'MENU', 'customer/llm-prompt/index', 1, '', 'edit', 1, 0, '', 9);
INSERT INTO `sys_menu` VALUES (920442132821802, 0, '2026-07-24 17:16:06.719958', 0, '2026-07-24 17:16:06.719958', 920442132821801, '[0][920442132821701][920442132821801]', '新增提示词', 'llm-prompt-create', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 1);
INSERT INTO `sys_menu` VALUES (920442132821803, 0, '2026-07-24 17:16:06.719958', 0, '2026-07-24 17:16:06.719958', 920442132821801, '[0][920442132821701][920442132821801]', '修改提示词', 'llm-prompt-update', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 2);
INSERT INTO `sys_menu` VALUES (920442132821804, 0, '2026-07-24 17:16:06.719958', 0, '2026-07-24 17:16:06.719958', 920442132821801, '[0][920442132821701][920442132821801]', '删除提示词', 'llm-prompt-delete', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 3);
INSERT INTO `sys_menu` VALUES (920442132821805, 0, '2026-07-24 17:16:06.719958', 0, '2026-07-24 17:16:06.719958', 920442132821801, '[0][920442132821701][920442132821801]', '绑定客户', 'llm-prompt-bind', '', '', 'BUTTON', '', 1, '', '', 0, 0, '', 4);

-- ----------------------------
-- Table structure for sys_organization
-- ----------------------------
DROP TABLE IF EXISTS `sys_organization`;
CREATE TABLE `sys_organization`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `parentid` bigint(20) NOT NULL,
  `parentids` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `code` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` tinyint(1) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Department' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_organization
-- ----------------------------
INSERT INTO `sys_organization` VALUES (653725132137989, 653335112912901, '2025-03-12 15:54:24.703512', 653335112912901, '2025-03-12 15:54:24.709316', 0, '[0]', 'microsoft', '微软中国有限公司', 1, 1);
INSERT INTO `sys_organization` VALUES (653726253618757, 653335112912901, '2025-03-12 15:58:58.512981', 653335112912901, '2025-03-12 15:58:58.517780', 653725132137989, '[0]', 'office', 'office事业部', 1, 1);
INSERT INTO `sys_organization` VALUES (653726372808261, 653335112912901, '2025-03-12 15:59:27.369192', 653335112912901, '2025-03-12 15:59:27.369244', 653725132137989, '[0]', 'database', '数据库事业部', 1, 2);
INSERT INTO `sys_organization` VALUES (653726516561477, 653335112912901, '2025-03-12 16:00:02.464592', 653335112912901, '2025-03-12 16:10:30.737262', 653725132137989, '[0]', 'tools', '开发工具事业部', 1, 3);
INSERT INTO `sys_organization` VALUES (653729175238277, 653335112912901, '2025-03-12 16:10:51.633050', 653335112912901, '2025-03-12 16:10:51.633839', 653726516561477, '[0]', 'vscode', 'vscode', 1, 1);

-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE `sys_role`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `datascope` int(11) NOT NULL,
  `status` tinyint(1) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Role' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_role
-- ----------------------------
INSERT INTO `sys_role` VALUES (653344679641925, 1000000000000, '2025-03-11 14:06:20.618864', 653335112912901, '2025-03-12 11:45:34.554147', '系统管理员', 'administrator', 0, 1, 1);
INSERT INTO `sys_role` VALUES (653682250118405, 653335112912901, '2025-03-12 12:59:55.116604', 653335112912901, '2025-03-24 23:57:04.926460', '访问者', 'guest', 2, 1, 1);

-- ----------------------------
-- Table structure for sys_role_menu_relation
-- ----------------------------
DROP TABLE IF EXISTS `sys_role_menu_relation`;
CREATE TABLE `sys_role_menu_relation`  (
  `id` bigint(20) NOT NULL,
  `menuid` bigint(20) NOT NULL,
  `roleid` bigint(20) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Menu-role relation' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_role_menu_relation
-- ----------------------------
INSERT INTO `sys_role_menu_relation` VALUES (658096326596997, 653342080185029, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326596998, 653688386045317, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326596999, 653689402680709, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597000, 653525628846981, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597001, 653526703580037, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597002, 653745302407109, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597003, 653756020922437, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597004, 653342584296133, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597005, 653689923114373, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597006, 654225897013253, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597007, 654226547773445, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597008, 654227048943621, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597009, 654227565006853, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597010, 654243405697093, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597011, 654333310177925, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597012, 654442132821701, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597013, 657670977337285, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597014, 657668727089093, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597015, 654523403131077, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597016, 654523795179717, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597017, 654442722764485, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597018, 654455068768133, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597019, 657670470342597, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597020, 657665676666821, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326597021, 657666023151557, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326601093, 657666452023237, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326601094, 657666958747589, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (658096326601095, 657667115956165, 653682250118405);
INSERT INTO `sys_role_menu_relation` VALUES (826704465780805, 812955682157829, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854533, 812956047721733, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854534, 812956163073285, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854535, 813963861050629, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854536, 813964328215813, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854537, 813963541873925, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854538, 813963702568197, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854539, 812982941320453, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854540, 812983083382021, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854541, 814007767123205, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854542, 813964515616005, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854543, 813964643558661, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854544, 815425963181317, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854545, 813964725294341, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854546, 813964847981829, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854547, 657665676666821, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854548, 657666023151557, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854549, 657666337880005, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854550, 657666599712709, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854551, 657666452023237, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854552, 657666958747589, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854553, 657667115956165, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854554, 812956383307013, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854555, 812957308704005, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465854556, 812956596425989, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858629, 812956861986053, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858630, 812653792118021, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858631, 812890595538181, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858632, 814648632751365, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858633, 812654410413317, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858634, 812890927891717, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858635, 812890810012933, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858636, 812912411931909, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858637, 653342080185029, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858638, 653688386045317, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858639, 653688962147717, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858640, 653689048724869, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858641, 653689299236229, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858642, 653689402680709, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858643, 653525628846981, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858644, 653525880882053, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858645, 653525982651269, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858646, 653526164857733, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858647, 653526703580037, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858648, 654788688007877, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858649, 653745302407109, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858650, 653745429395397, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858651, 653745520240581, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858652, 653756020922437, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858653, 653787841784005, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858654, 653804678302149, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858655, 653805800937925, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858656, 653805917993413, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858657, 653342584296133, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858658, 653343418946245, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858659, 653389651534725, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858660, 653524636169093, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858661, 653689923114373, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858662, 654225897013253, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858663, 654226073489413, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858664, 654226273665029, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858665, 654226371764229, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858666, 654226547773445, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858667, 654227048943621, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858668, 654227246395397, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858669, 654227348602885, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858670, 654227455537157, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858671, 654227565006853, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858672, 654243405697093, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858673, 654332804104837, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858674, 654332952756869, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858675, 654333157950085, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858676, 654333310177925, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858677, 654442132821701, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858678, 657670977337285, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858679, 657668727089093, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858680, 654523403131077, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858681, 654523795179717, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858682, 654442722764485, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858683, 654455068768133, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858684, 657670470342597, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858685, 920442132821701, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858686, 920442132821711, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465858687, 920442132821712, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862725, 920442132821713, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862726, 920442132821714, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862727, 920442132821751, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862728, 920442132821752, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862729, 920442132821753, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862730, 920442132821754, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862731, 920442132821755, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862732, 920442132821756, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862733, 920442132821731, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862734, 920442132821732, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862735, 920442132821733, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862736, 920442132821734, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862737, 920442132821735, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862738, 920442132821736, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862739, 920442132821737, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862740, 920442132821741, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862741, 920442132821742, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862742, 920442132821743, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862743, 920442132821744, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862744, 920442132821745, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862745, 920442132821746, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862746, 920442132821747, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862747, 920442132821761, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862748, 920442132821762, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862749, 920442132821763, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862750, 920442132821764, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862751, 920442132821771, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862752, 920442132821772, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862753, 920442132821773, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862754, 920442132821774, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862755, 920442132821781, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862756, 920442132821782, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862757, 920442132821783, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862758, 920442132821791, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862759, 920442132821792, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862760, 920442132821793, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862761, 920442132821794, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (826704465862762, 920442132821795, 653344679641925);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822701, 920442132821701, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822711, 920442132821711, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822712, 920442132821712, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822713, 920442132821713, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822714, 920442132821714, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822731, 920442132821731, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822732, 920442132821732, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822733, 920442132821733, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822734, 920442132821734, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822735, 920442132821735, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822736, 920442132821736, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822737, 920442132821737, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822741, 920442132821741, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822742, 920442132821742, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822743, 920442132821743, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822744, 920442132821744, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822745, 920442132821745, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822746, 920442132821746, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822747, 920442132821747, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822751, 920442132821751, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822752, 920442132821752, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822753, 920442132821753, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822754, 920442132821754, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822755, 920442132821755, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822756, 920442132821756, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822761, 920442132821761, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822762, 920442132821762, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822763, 920442132821763, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822764, 920442132821764, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822791, 920442132821791, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822792, 920442132821792, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822793, 920442132821793, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822794, 920442132821794, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822795, 920442132821795, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822801, 920442132821801, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822802, 920442132821802, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822803, 920442132821803, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822804, 920442132821804, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132822805, 920442132821805, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132831762, 920442132821762, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132831763, 920442132821763, 1);
INSERT INTO `sys_role_menu_relation` VALUES (920442132831764, 920442132821764, 1);

-- ----------------------------
-- Table structure for sys_role_user_relation
-- ----------------------------
DROP TABLE IF EXISTS `sys_role_user_relation`;
CREATE TABLE `sys_role_user_relation`  (
  `id` bigint(20) NOT NULL,
  `userid` bigint(20) NOT NULL,
  `roleid` bigint(20) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'User-role relation' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_role_user_relation
-- ----------------------------
INSERT INTO `sys_role_user_relation` VALUES (658284478040646, 658284478040645, 653682250118405);
INSERT INTO `sys_role_user_relation` VALUES (658284667353670, 658284667353669, 653682250118405);
INSERT INTO `sys_role_user_relation` VALUES (658284849875525, 658284256315973, 653682250118405);
INSERT INTO `sys_role_user_relation` VALUES (658284876995141, 653337659433029, 653344679641925);
INSERT INTO `sys_role_user_relation` VALUES (658284902369861, 653335112912901, 653344679641925);
INSERT INTO `sys_role_user_relation` VALUES (658285201709637, 658285103086149, 653344679641925);

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user`  (
  `id` bigint(20) NOT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT 'Deletion flag',
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NOT NULL,
  `modifytime` datetime(6) NOT NULL,
  `account` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Account',
  `avatar` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Avatar path',
  `birthday` datetime(6) NULL DEFAULT NULL COMMENT 'Birthday',
  `deptid` bigint(20) NOT NULL COMMENT 'Department ID',
  `email` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'email',
  `name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Name',
  `password` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Password',
  `mobile` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Mobile number',
  `salt` varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'Password salt',
  `gender` int(11) NOT NULL COMMENT 'Gender',
  `status` tinyint(1) NOT NULL COMMENT 'Status',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'Administrator' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO `sys_user` VALUES (653335112912901, 0, 1000000000000, '2025-03-11 13:27:25.038287', 653335112912901, '2025-03-25 13:40:52.000000', 'alpha2008', '', '2025-03-11 14:11:08.062000', 653725132137989, 'user@example.com', '余大猫', '3B6791E9AB14DFF278A3C2AF4B97F1E9', '', '846vm', 2, 1);
INSERT INTO `sys_user` VALUES (653337659433029, 0, 1000000000000, '2025-03-11 13:37:46.724327', 653335112912901, '2025-03-25 13:40:48.000000', 'alpha2009', '', '2025-03-11 13:24:22.360000', 653726253618757, 'user@example.com', '余大猫', 'DE325BC108FD5B42D45500C3720F6628', '19964946688', '7vkvf', 2, 1);
INSERT INTO `sys_user` VALUES (653838533808773, 1, 653335112912901, '2025-03-12 23:35:50.884954', 653335112912901, '2025-03-12 23:36:58.000000', 'test1', '', NULL, 653725132137989, 'alphacn@foxmail.com', '测试用户', '86A961C57306BAC569B9FBB4EC6BA638', '', 'xfgiq', 0, 1);
INSERT INTO `sys_user` VALUES (653838713631365, 1, 653335112912901, '2025-03-12 23:36:34.421153', 653335112912901, '2025-03-12 23:36:58.000000', 'test2', '', NULL, 653726253618757, 'alphacn@foxmail.com', '测试用户2', '0C54EFB2BAF6F1A13E2F39195D6AA8EB', '18898666555', 'so32e', 2, 1);
INSERT INTO `sys_user` VALUES (653838923670149, 1, 653335112912901, '2025-03-12 23:37:25.701911', 653335112912901, '2025-03-12 23:37:29.000000', 'test1', '', NULL, 653726253618757, 'alphacn@foxmail.com', '测试用户1', '2A04D26E4CA62451402A1AC6F7515667', '', '3j8mi', 0, 0);
INSERT INTO `sys_user` VALUES (658284069145157, 1, 653335112912901, '2025-03-25 13:04:46.170019', 653335112912901, '2025-03-25 13:06:45.000000', 'alpha2000', '', NULL, 653725132137989, 'alpha2000@tom.com', '2000', '60861703F6F7102ECF0DBB09CABD841B', '', 'v3pfy', 1, 1);
INSERT INTO `sys_user` VALUES (658284256315973, 0, 653335112912901, '2025-03-25 13:05:31.786621', 653335112912901, '2025-03-25 13:40:45.000000', 'alpha2010', '', '2025-03-11 13:24:22.360000', 653725132137989, '2010@tom.com', '2010', 'C82AF7A87AE221D4D5C2624025BA318B', '', 'zi9j6', 1, 1);
INSERT INTO `sys_user` VALUES (658284478040645, 0, 653335112912901, '2025-03-25 13:06:25.922623', 653335112912901, '2025-03-25 13:40:41.000000', 'alpha2011', '', '2025-03-11 13:24:22.360000', 653725132137989, '2011@tom.com', '2011', '4B75645227A161690BD0CDEAD09B3067', '', '1v7ho', 1, 1);
INSERT INTO `sys_user` VALUES (658284667353669, 0, 653335112912901, '2025-03-25 13:07:12.139040', 653335112912901, '2025-03-25 13:40:37.000000', 'alpha2012', '', '2025-03-11 13:24:22.360000', 653725132137989, '2012@tom.com', '2012', '1011AD5C3D8665905E9358E385349C5B', '', '4ix1k', 1, 1);
INSERT INTO `sys_user` VALUES (658285103086149, 0, 653335112912901, '2025-03-25 13:08:58.517647', 653335112912901, '2025-03-25 13:40:32.000000', 'alpha2007', '', NULL, 653725132137989, '2007@tom.com', '余大猫', '6DA705722E3CB40890B7CEA884821F7C', '', '3wjku', 2, 1);

SET FOREIGN_KEY_CHECKS = 1;
