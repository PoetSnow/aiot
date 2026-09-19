# 模块 Device

**一句话：** 已激活设备、仓位、影子、MQTT、模拟器。云端**唯一**连 EMQX 的模块。  
**表前缀：** `dev_`  
**路径：** `src/Modules/NXAI.Device/`  
**开工序号：** 5–6，遥测骨架可同期留口  
**先读：** [protocol/mqtt](../protocol/mqtt.md)、[protocol/operation-order](../protocol/operation-order.md)、[protocol/telemetry](../protocol/telemetry.md)、[infra/emqx](../infra/emqx.md)、[modules/asset](asset.md)

## 负责 / 不负责

- 负责：绑定/配网、在线、影子、仓位事实、能力、MQTT 上下行、Outbox、模拟器、`ITelemetryStore` 写入。
- 不负责：入库出库、编配方、卖货、任务状态机（状态机在 Cooking，本模块只投递并回 ACK）。

## 绑定

ACL 问 Asset：SN 为 Outbound/Bound，且 Member 可认领。成功后建 Device + 按型号 SlotProfile 建仓，发 `device.bound`。

仓位「放了什么」以本模块为准。Inventory 只投影放置位置。

## 表

`dev_device` `dev_slot` `dev_shadow` `dev_capability` `dev_mqtt_outbox`  
遥测表 `dev_telemetry` 只通过 `ITelemetryStore` 访问，见 [protocol/telemetry](../protocol/telemetry.md)。

Device 关键字段：MemberId（不是员工）、DeviceSn、ModelCode、ActiveTaskId、ActiveEpoch  
Slot：SlotCode、SlotType、SupportedModes、BindingKind、MaterialCode / ConsumableId  
Shadow：当前水温/状态/version，一行覆盖  
Outbox：投递重试同一 commandId，直到 RECEIVED；已 RECEIVED 禁止当新投放重发

## API

```text
POST /api/portal/devices/bind
GET  /api/portal/devices
GET  /api/portal/devices/{id}
GET  /api/portal/devices/{id}/shadow
GET  /api/portal/devices/{id}/slots
PUT  /api/portal/devices/{id}/slots/{slotCode}/binding

GET  /api/console/devices
GET  /api/console/devices/{id}/shadow

POST /api/device/activate
POST /api/device/token/refresh
POST /api/device/mqtt-auth
GET  /api/device/time
```

## 对外（Cooking 用）

`IDeviceService` / Gateway 入参：查能力与仓位、`IssueCommand`（Cooking 本地类型）、占/放 ActiveTask。  
Cooking **不**引用 MQTT。

内部端口：`IMqttDeviceBus`、`ITelemetryStore`。

## ACL

- `IAssetSnGateway`：绑定校验  
- `IMemberGateway`：会员未注销  
- `IInventoryGateway`：绑仓时耗材属于该会员  

## 事件

发布：bound/unbound、online/offline、shadow.changed、slot.bound/unbound、command.acked  
订阅：`asset.sn.status.changed`（维修中拒绑）、`aftersales.ticket.opened`（解绑）

## 模拟器

MQTT Client，实现协议判定（epoch/stepNo/本地温度开火）。不要用 HTTP 假装设备。

## 红线

不做进销存。不因影子 80℃ 替 Cooking 直接投。Append 遥测失败不挡 ACK。
