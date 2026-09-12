# 模块 Recipe

**一句话：** 可执行配方模板和物料主数据。  
**表前缀：** `rcp_`  
**路径：** `src/Modules/NXAI.Recipe/`  
**开工序号：** 7  
**依赖：** [02-共享内核](../02-共享内核.md)

## 负责 / 不负责

- 负责：物料、配方、步骤、发布（方案 A）。
- 不负责：MQTT、扣耗材、进销存。步骤不写死仓位号，只写 TargetCode。

配方是模板。Cooking 引用时打不可变快照，执行只认快照。

## 版本（方案 A）

- 唯一键 `(Code, Version)`。
- 同一 Code 最多一条 Published，旧版 Superseded。
- 只改 Draft。发布：旧 Published → Superseded，本行 Version = max+1。
- 回滚：复制旧版为新 Draft 再发布。不建 history 表，不准改历史行。

## 表

`rcp_material`：Code，Name，DefaultMode  
`rcp_recipe`：Code，Version，Status(Draft/Published/Superseded)，SceneTags，CompatibleModels  
`rcp_recipe_step`：StepNo，Action，Trigger（温度用 TempCelsius，时间用 DelaySeconds），TargetKind/TargetCode，Mode，Amount

## API（console + portal 只读已发布）

```text
GET|POST|PUT /api/console/materials
GET|POST|PUT /api/console/recipes
POST         /api/console/recipes/{id}/publish
GET          /api/portal/recipes
GET          /api/portal/recipes/{code}
```

对外：按 Code 取当前 Published 快照；按 RecipeId 取某一版快照。

## ACL

`IDeviceCapabilityGateway` 可后补（保存时校验型号）。V1 可不做。

## 红线

步骤禁止写 slotCode。不发 MQTT。
