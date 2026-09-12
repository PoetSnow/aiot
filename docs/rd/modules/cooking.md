# 模块 Cooking

**一句话：** 一次制作的任务、状态机、指令记录。唯一允许对 Device 说「去投」的模块。  
**表前缀：** `ckg_`  
**路径：** `src/Modules/NXAI.Cooking/`  
**开工序号：** 9  
**必读：** [protocol/operation-order](../protocol/operation-order.md)、[protocol/mqtt](../protocol/mqtt.md)、[modules/device](device.md)、[modules/recipe](recipe.md)

## 负责 / 不负责

- 负责：校验、创建任务、一步一条指令、等终态 ACK、执行日志、是否允许投递重试。
- 不负责：改配方模板、改仓位绑定、售后、直接 MQTT。

## 状态机

```text
Created → Validating → Ready → Running → Steeping → Completed
                ↘ Rejected          ↘ Failed
Running / Steeping → Cancelled
```

创建时校验（全过才下发）：设备在线、型号兼容、每步 Target 能解析到唯一仓且模式匹配、PACKAGE 余量够。失败 → Rejected，零投放指令。

V1 Source 只用手动。执行只认 `RecipeSnapshotJson`。

## 表

`ckg_task`：MemberId，DeviceId，RecipeId/Code/Version，SnapshotJson，Status，CurrentStepNo，Epoch  
`ckg_task_step`：解析后的 SlotCode/Mode/Amount，CommandId，结果  
`ckg_command`：Id=commandId，IdempotencyKey=`{taskId}:{stepId}:{seq}`，IssuePolicy（DISPENSE=禁止换新 Id 重投）  
`ckg_execution_log`

一台设备同时最多一条未终态指令。下一步必须等上一步终态 ACK。

## API（portal）

```text
POST /api/portal/cooking-tasks
GET  /api/portal/cooking-tasks/{id}
POST /api/portal/cooking-tasks/{id}/cancel
```

## ACL

- `IRecipeGateway`：不可变快照  
- `IDeviceGateway`：能力/仓位、占任务锁、IssueCommand  
- `IInventoryGateway`：投放前余量只读  

## 事件

订阅：online/offline、command.acked、shadow.changed（**只看门狗，不因影子 80℃ 发立即投放**）  
发布：task.completed / task.failed  

HEAT SUCCESS（硬件本地达标）之后，才发带 `TEMP_GTE` 的 DISPENSE，禁止 IMMEDIATE 代替。

## 红线

不引用 MQTT 客户端。不并行两个 Running。上一步未 ACK 不发下一步投放。
