/*
  NXAI 养生壶 V1 业务表（按当前实体自建，无业务种子数据）
  列名：EF UseLowerCaseNamingConvention（全小写、无下划线）
  标识符英文 / 注释中文
  Id 为雪花，非自增
  库：nxai
*/
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Member · mb_member
-- ----------------------------
DROP TABLE IF EXISTS `mb_member`;
CREATE TABLE `mb_member` (
  `id` bigint(20) NOT NULL,
  `mobile` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '手机号，有值则唯一',
  `openid` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '微信 OpenId，有值则唯一',
  `unionid` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '微信 UnionId',
  `nickname` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '昵称',
  `avatar` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '头像地址',
  `status` int(11) NOT NULL COMMENT '1 正常 / 0 禁用 / 2 注销',
  `lastlogintime` datetime(6) NULL DEFAULT NULL COMMENT '最近一次登录时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_mb_member_mobile`(`mobile`) USING BTREE,
  UNIQUE INDEX `ix_mb_member_openid`(`openid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '小程序会员，表 mb_member。Id 与 sys_user 不是同一空间。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_device_model
-- ----------------------------
DROP TABLE IF EXISTS `ast_device_model`;
CREATE TABLE `ast_device_model` (
  `id` bigint(20) NOT NULL,
  `modelcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '型号编码，全局唯一',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '型号名称',
  `warrantymonths` int(11) NOT NULL COMMENT '保修月数',
  `slotprofilejson` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '仓位配置 JSON，绑定设备时用来建仓',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_device_model_modelcode`(`modelcode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备型号，表 ast_device_model。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_warehouse
-- ----------------------------
DROP TABLE IF EXISTS `ast_warehouse`;
CREATE TABLE `ast_warehouse` (
  `id` bigint(20) NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '仓库编码，唯一',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '仓库名称',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_warehouse_code`(`code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '仓库，表 ast_warehouse。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_sn
-- ----------------------------
DROP TABLE IF EXISTS `ast_sn`;
CREATE TABLE `ast_sn` (
  `id` bigint(20) NOT NULL,
  `sn` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'SN，全局唯一',
  `modelcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '型号编码',
  `warehouseid` bigint(20) NOT NULL COMMENT '当前所在仓库。出库后仍保留最后仓库便于追溯',
  `status` int(11) NOT NULL COMMENT 'SN 状态',
  `memberid` bigint(20) NULL DEFAULT NULL COMMENT '出库挂接的会员 Id。不是 sys_user.Id',
  `stockinid` bigint(20) NULL DEFAULT NULL COMMENT '最近一次入库单 Id',
  `stockoutid` bigint(20) NULL DEFAULT NULL COMMENT '最近一次出库单 Id',
  `inboundat` datetime(6) NULL DEFAULT NULL COMMENT '入库确认时间',
  `outboundat` datetime(6) NULL DEFAULT NULL COMMENT '出库确认时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_sn_sn`(`sn`) USING BTREE,
  INDEX `ix_ast_sn_warehouseid_modelcode_status`(`warehouseid`, `modelcode`, `status`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备 SN 台账，表 ast_sn。库存 = 按仓+型号统计 Status=InStock。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_stock_in
-- ----------------------------
DROP TABLE IF EXISTS `ast_stock_in`;
CREATE TABLE `ast_stock_in` (
  `id` bigint(20) NOT NULL,
  `billno` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '业务单号',
  `warehouseid` bigint(20) NOT NULL COMMENT '入库仓库',
  `status` int(11) NOT NULL COMMENT '单据状态。未审禁止改 SN',
  `remark` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '备注',
  `confirmedat` datetime(6) NULL DEFAULT NULL COMMENT '确认时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_stock_in_billno`(`billno`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '入库单，表 ast_stock_in。未确认禁止改 SN 台账。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_stock_in_line
-- ----------------------------
DROP TABLE IF EXISTS `ast_stock_in_line`;
CREATE TABLE `ast_stock_in_line` (
  `id` bigint(20) NOT NULL,
  `stockinid` bigint(20) NOT NULL COMMENT '所属入库单',
  `sn` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '待入库 SN',
  `modelcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '型号编码',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_stock_in_line_stockinid_sn`(`stockinid`, `sn`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '入库单行，表 ast_stock_in_line。一行一个 SN。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_stock_out
-- ----------------------------
DROP TABLE IF EXISTS `ast_stock_out`;
CREATE TABLE `ast_stock_out` (
  `id` bigint(20) NOT NULL,
  `billno` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '业务单号',
  `warehouseid` bigint(20) NOT NULL COMMENT '出库仓库',
  `status` int(11) NOT NULL COMMENT '单据状态。未审禁止改 SN',
  `memberid` bigint(20) NULL DEFAULT NULL COMMENT '出库挂接会员。可空',
  `remark` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '备注',
  `confirmedat` datetime(6) NULL DEFAULT NULL COMMENT '确认时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_stock_out_billno`(`billno`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '出库单，表 ast_stock_out。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Asset · ast_stock_out_line
-- ----------------------------
DROP TABLE IF EXISTS `ast_stock_out_line`;
CREATE TABLE `ast_stock_out_line` (
  `id` bigint(20) NOT NULL,
  `stockoutid` bigint(20) NOT NULL COMMENT '所属出库单',
  `sn` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '待出库 SN',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ast_stock_out_line_stockoutid_sn`(`stockoutid`, `sn`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '出库单行，表 ast_stock_out_line。一行一个 SN。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Device · dev_device
-- ----------------------------
DROP TABLE IF EXISTS `dev_device`;
CREATE TABLE `dev_device` (
  `id` bigint(20) NOT NULL,
  `memberid` bigint(20) NOT NULL COMMENT '主人会员 Id',
  `devicesn` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备 SN，全局唯一',
  `modelcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '型号编码，来自 Asset',
  `activetaskid` bigint(20) NULL DEFAULT NULL COMMENT '当前在途任务。一壶同时一个 ActiveTask',
  `activeepoch` int(11) NOT NULL COMMENT '当前任务世代。旧 epoch 包一律作废',
  `devicetokenhash` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备访问令牌 SHA256 十六进制。明文只下发一次',
  `activatedat` datetime(6) NULL DEFAULT NULL COMMENT '最近激活时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_dev_device_devicesn`(`devicesn`) USING BTREE,
  INDEX `ix_dev_device_memberid`(`memberid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '已绑定设备，表 dev_device。MemberId 不是员工 Id。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Device · dev_slot
-- ----------------------------
DROP TABLE IF EXISTS `dev_slot`;
CREATE TABLE `dev_slot` (
  `id` bigint(20) NOT NULL,
  `deviceid` bigint(20) NOT NULL COMMENT '所属设备',
  `slotcode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '仓位码，如 S3',
  `slottype` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '仓位类型：BULK / PACKAGE / CARTRIDGE',
  `supportedmodes` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '支持的投料模式，逗号分隔',
  `bindingkind` int(11) NOT NULL COMMENT '绑定种类：0 空仓 / 1 物料 / 2 耗材实例',
  `materialcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '物料编码',
  `consumableid` bigint(20) NULL DEFAULT NULL COMMENT '耗材实例 Id',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_dev_slot_deviceid_slotcode`(`deviceid`, `slotcode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备仓位事实，表 dev_slot。放了什么以本表为准。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Device · dev_shadow
-- ----------------------------
DROP TABLE IF EXISTS `dev_shadow`;
CREATE TABLE `dev_shadow` (
  `id` bigint(20) NOT NULL,
  `deviceid` bigint(20) NOT NULL COMMENT '所属设备，一对一',
  `version` int(11) NOT NULL COMMENT '影子版本，设备上报递增',
  `online` tinyint(1) NOT NULL COMMENT '是否在线',
  `watertemp` decimal(18, 4) NULL DEFAULT NULL COMMENT '当前水温。开火仍看硬件本地探头',
  `workstate` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工作状态，默认 IDLE',
  `currenttaskid` bigint(20) NULL DEFAULT NULL COMMENT '设备当前任务',
  `currentepoch` int(11) NULL DEFAULT NULL COMMENT '设备当前世代',
  `currentstepno` int(11) NULL DEFAULT NULL COMMENT '设备当前步骤号',
  `slotsoccupiedjson` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '硬件感知的仓位占用 JSON',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_dev_shadow_deviceid`(`deviceid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备影子，表 dev_shadow。一行覆盖，只给界面和看门狗，不当投料扳机。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Device · dev_capability
-- ----------------------------
DROP TABLE IF EXISTS `dev_capability`;
CREATE TABLE `dev_capability` (
  `id` bigint(20) NOT NULL,
  `deviceid` bigint(20) NOT NULL COMMENT '所属设备，一对一',
  `payloadjson` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '仓、模式、actions 的 JSON',
  `reportedat` datetime(6) NULL DEFAULT NULL COMMENT '最近上报时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_dev_capability_deviceid`(`deviceid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备能力快照，表 dev_capability。上线必报，Cooking 按此校验。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Device · dev_mqtt_outbox
-- ----------------------------
DROP TABLE IF EXISTS `dev_mqtt_outbox`;
CREATE TABLE `dev_mqtt_outbox` (
  `id` bigint(20) NOT NULL,
  `deviceid` bigint(20) NOT NULL COMMENT '所属设备',
  `commandid` bigint(20) NOT NULL COMMENT '指令 Id。已 RECEIVED 禁止当新投放重发',
  `payloadjson` varchar(8000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'COMMAND 信封 JSON',
  `status` int(11) NOT NULL COMMENT '投递状态：0 Pending / 1 Received',
  `retrycount` int(11) NOT NULL COMMENT '重试次数',
  `lastattemptat` datetime(6) NULL DEFAULT NULL COMMENT '最近一次投递时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_dev_mqtt_outbox_commandid`(`commandid`) USING BTREE,
  INDEX `ix_dev_mqtt_outbox_deviceid_status`(`deviceid`, `status`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = 'MQTT 下行 Outbox，表 dev_mqtt_outbox。重试同一 commandId，直到 RECEIVED。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Device · dev_telemetry
-- ----------------------------
DROP TABLE IF EXISTS `dev_telemetry`;
CREATE TABLE `dev_telemetry` (
  `id` bigint(20) NOT NULL,
  `sn` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备 SN',
  `metric` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '指标名，如 water_temp',
  `ts` datetime(6) NOT NULL COMMENT '设备事件时间',
  `receivedat` datetime(6) NOT NULL COMMENT '服务器接收时间',
  `valuenum` decimal(18, 4) NULL DEFAULT NULL COMMENT '数值',
  `valuetext` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '文本值，如 IDLE',
  `taskid` bigint(20) NULL DEFAULT NULL COMMENT '关联任务',
  `epoch` int(11) NULL DEFAULT NULL COMMENT '世代',
  `quality` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '质量',
  `source` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '来源，如 mqtt',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_dev_telemetry_sn_metric_ts`(`sn`, `metric`, `ts`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '遥测点，表 dev_telemetry。只由 ITelemetryStore 写入。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Recipe · rcp_material
-- ----------------------------
DROP TABLE IF EXISTS `rcp_material`;
CREATE TABLE `rcp_material` (
  `id` bigint(20) NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '物料编码，与配方 TargetCode、耗材类型对齐',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '名称',
  `defaultmode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '默认投料模式，如 PACKAGE',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_rcp_material_code`(`code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '物料主数据，表 rcp_material。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Recipe · rcp_recipe
-- ----------------------------
DROP TABLE IF EXISTS `rcp_recipe`;
CREATE TABLE `rcp_recipe` (
  `id` bigint(20) NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '配方编码',
  `version` int(11) NOT NULL COMMENT '版本号，从 1 起',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '名称',
  `status` int(11) NOT NULL COMMENT '0 草稿 / 1 已发布 / 2 被取代',
  `scenetags` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '场景标签，逗号分隔',
  `compatiblemodels` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '适配型号，逗号分隔。空表示不限',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_rcp_recipe_code_version`(`code`, `version`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '配方模板，表 rcp_recipe。方案 A：Code+Version 多行。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Recipe · rcp_recipe_step
-- ----------------------------
DROP TABLE IF EXISTS `rcp_recipe_step`;
CREATE TABLE `rcp_recipe_step` (
  `id` bigint(20) NOT NULL,
  `recipeid` bigint(20) NOT NULL COMMENT '所属配方行',
  `stepno` int(11) NOT NULL COMMENT '步骤序号，从 1 起',
  `action` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '动作：HEAT / DISPENSE / KEEP_WARM / STOP',
  `triggertype` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '触发：TEMP_GTE / DELAY / IMMEDIATE。HEAT 用 TEMP_GTE',
  `tempcelsius` decimal(18, 4) NULL DEFAULT NULL COMMENT 'TEMP_GTE 摄氏度',
  `delayseconds` int(11) NULL DEFAULT NULL COMMENT 'DELAY 秒数',
  `targetkind` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '目标种类，投料为 MATERIAL',
  `targetcode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '目标编码，如 TEA_PACK。Cooking 解析到仓位',
  `mode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '投料模式',
  `amountvalue` decimal(18, 4) NULL DEFAULT NULL COMMENT '数量',
  `amountunit` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '单位 PACK / G / PCS',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_rcp_recipe_step_recipeid_stepno`(`recipeid`, `stepno`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '配方步骤，表 rcp_recipe_step。禁止写死仓位号。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Inventory · inv_consumable
-- ----------------------------
DROP TABLE IF EXISTS `inv_consumable`;
CREATE TABLE `inv_consumable` (
  `id` bigint(20) NOT NULL,
  `memberid` bigint(20) NOT NULL COMMENT '主人会员 Id',
  `consumabletypecode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '类型编码，与配方 TargetCode 对齐',
  `productid` bigint(20) NULL DEFAULT NULL COMMENT '货架商品 Id，可空',
  `form` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '形态',
  `totalqty` decimal(18, 4) NOT NULL COMMENT '初始数量',
  `remainqty` decimal(18, 4) NOT NULL COMMENT '剩余数量',
  `qtyunit` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '数量单位',
  `status` int(11) NOT NULL COMMENT '0 可用 / 1 已用尽',
  `placeddeviceid` bigint(20) NULL DEFAULT NULL COMMENT '放置设备投影',
  `placedslotcode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '放置仓位投影',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `ix_inv_consumable_memberid`(`memberid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '会员耗材实例，表 inv_consumable。放置位置只是投影，仓位事实在 Device。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Cooking · ckg_task
-- ----------------------------
DROP TABLE IF EXISTS `ckg_task`;
CREATE TABLE `ckg_task` (
  `id` bigint(20) NOT NULL,
  `memberid` bigint(20) NOT NULL COMMENT '会员 Id',
  `deviceid` bigint(20) NOT NULL COMMENT '设备 Id',
  `recipeid` bigint(20) NOT NULL COMMENT '配方行 Id',
  `recipecode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '配方编码',
  `recipeversion` int(11) NOT NULL COMMENT '配方版本',
  `snapshotjson` varchar(8000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '不可变快照 JSON',
  `status` int(11) NOT NULL COMMENT '任务状态',
  `currentstepno` int(11) NOT NULL COMMENT '当前步骤号',
  `epoch` int(11) NOT NULL COMMENT '任务世代',
  `rejectreason` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '校验失败原因',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `ix_ckg_task_deviceid`(`deviceid`) USING BTREE,
  INDEX `ix_ckg_task_memberid`(`memberid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '制作任务，表 ckg_task。执行只认快照。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Cooking · ckg_task_step
-- ----------------------------
DROP TABLE IF EXISTS `ckg_task_step`;
CREATE TABLE `ckg_task_step` (
  `id` bigint(20) NOT NULL,
  `taskid` bigint(20) NOT NULL COMMENT '任务 Id',
  `stepno` int(11) NOT NULL COMMENT '步骤序号',
  `action` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '动作',
  `slotcode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '解析后的仓位。HEAT 可空',
  `mode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '投料模式',
  `amountvalue` decimal(18, 4) NULL DEFAULT NULL COMMENT '数量',
  `amountunit` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '单位',
  `triggertype` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '触发',
  `tempcelsius` decimal(18, 4) NULL DEFAULT NULL COMMENT '目标温度',
  `commandid` bigint(20) NULL DEFAULT NULL COMMENT '当前指令 Id',
  `consumableid` bigint(20) NULL DEFAULT NULL COMMENT '耗材实例',
  `result` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '步骤结果',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ckg_task_step_taskid_stepno`(`taskid`, `stepno`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '任务步骤，表 ckg_task_step。已解析到仓位。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Cooking · ckg_command
-- ----------------------------
DROP TABLE IF EXISTS `ckg_command`;
CREATE TABLE `ckg_command` (
  `id` bigint(20) NOT NULL,
  `taskid` bigint(20) NOT NULL COMMENT '任务 Id',
  `stepid` bigint(20) NOT NULL COMMENT '步骤 Id',
  `deviceid` bigint(20) NOT NULL COMMENT '设备 Id',
  `seq` int(11) NOT NULL COMMENT '投递序号',
  `idempotencykey` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '幂等键 {taskId}:{stepId}:{seq}',
  `action` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '动作',
  `issuepolicy` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'DISPENSE 禁止换新 Id 重投',
  `result` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最近结果',
  `issuedat` datetime(6) NOT NULL COMMENT '下发时间，看门狗用',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_ckg_command_idempotencykey`(`idempotencykey`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '下发指令，表 ckg_command。Id 即 commandId。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Cooking · ckg_execution_log
-- ----------------------------
DROP TABLE IF EXISTS `ckg_execution_log`;
CREATE TABLE `ckg_execution_log` (
  `id` bigint(20) NOT NULL,
  `taskid` bigint(20) NOT NULL COMMENT '任务 Id',
  `commandid` bigint(20) NULL DEFAULT NULL COMMENT '指令 Id',
  `message` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '说明',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `ix_ckg_execution_log_taskid`(`taskid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '执行日志，表 ckg_execution_log。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- AfterSales · afs_ticket
-- ----------------------------
DROP TABLE IF EXISTS `afs_ticket`;
CREATE TABLE `afs_ticket` (
  `id` bigint(20) NOT NULL,
  `ticketno` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工单号',
  `sn` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备 SN',
  `memberid` bigint(20) NULL DEFAULT NULL COMMENT '会员 Id',
  `deviceid` bigint(20) NULL DEFAULT NULL COMMENT '设备会话 Id，解绑后可空',
  `type` int(11) NOT NULL COMMENT '类型：0 咨询 / 1 维修 / 2 退货 / 3 换货',
  `status` int(11) NOT NULL COMMENT '状态：0 已开单 / 1 已受理 / 2 已结案',
  `warrantyvalid` tinyint(1) NOT NULL COMMENT '开单时冻结的质保结论',
  `symptom` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '症状',
  `closeresult` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '结案结果：return / scrap / replace',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_afs_ticket_ticketno`(`ticketno`) USING BTREE,
  INDEX `ix_afs_ticket_sn`(`sn`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '售后工单，表 afs_ticket。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- AfterSales · afs_ticket_log
-- ----------------------------
DROP TABLE IF EXISTS `afs_ticket_log`;
CREATE TABLE `afs_ticket_log` (
  `id` bigint(20) NOT NULL,
  `ticketid` bigint(20) NOT NULL COMMENT '工单 Id',
  `action` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '动作',
  `remark` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '备注',
  `operatorstaffid` bigint(20) NOT NULL COMMENT '操作员工 Id。会员开单为 0',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `ix_afs_ticket_log_ticketid`(`ticketid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '工单日志，表 afs_ticket_log。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Catalog · cat_product
-- ----------------------------
DROP TABLE IF EXISTS `cat_product`;
CREATE TABLE `cat_product` (
  `id` bigint(20) NOT NULL,
  `skucode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'SKU 编码',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '名称',
  `form` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '形态，如 PACKAGE',
  `consumabletypecode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '耗材类型，与配方 TargetCode 对齐',
  `suggestedrecipecode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '建议配方编码',
  `compatiblemodels` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '适配型号，逗号分隔',
  `packageqty` decimal(18, 4) NOT NULL COMMENT '每份数量',
  `qtyunit` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '数量单位',
  `status` int(11) NOT NULL COMMENT '0 下架 / 1 上架',
  `detailjson` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '详情 JSON。禁止医疗宣称',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `ix_cat_product_skucode`(`skucode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '耗材货架 SKU，表 cat_product。不当投料单元，也不当 SN 库存。' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Seed · 主仓（对应 AssetSchemaHostedService）
-- ----------------------------
INSERT INTO `ast_warehouse` (`id`, `code`, `name`) VALUES (1, 'MAIN', '主仓');

SET FOREIGN_KEY_CHECKS = 1;
