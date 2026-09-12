# MQTT 协议 pot.cmd.v1

写 Device / 模拟器时读。Broker 搭建见 [infra/emqx](../infra/emqx.md)。乱序规则见 [operation-order](operation-order.md)。

日常：Wi-Fi + MQTT。BLE 只配网。只有 Device 模块连 Broker。

设备登录：`clientId=pot-{sn}`，`username=sn`，`password=deviceToken`。只能碰 `pot/{sn}/#`。Token 用 `api/device` 签发/轮换。

## Topic

```text
down  pot/{sn}/down/command | shadow/get | capability/get
up    pot/{sn}/up/ack | shadow | event | capability
```

command/ack：QoS 1。

## 信封

```json
{ "schema": "pot.cmd.v1", "msgId": 1937..., "ts": 1760000000, "sn": "POT20260001", "type": "COMMAND", "payload": {} }
```

type：`COMMAND` / `ACK` / `SHADOW` / `EVENT` / `CAPABILITY`。Id 用 long，可附 `msgIdStr`。

## COMMAND

一条指令一个动作。禁止一条里塞加热+抽水+关机。

```json
{
  "commandId": 1937..., "taskId": 1937..., "stepId": 1937...,
  "epoch": 7, "stepNo": 2, "seq": 1,
  "idempotencyKey": "{taskId}:{stepId}:{seq}",
  "action": "DISPENSE",
  "slotCode": "S3", "mode": "PACKAGE",
  "amount": { "value": 1, "unit": "PACK" },
  "trigger": { "type": "TEMP_GTE", "tempCelsius": 80 },
  "expireAt": 1760003600
}
```

- 见过的 `commandId`：回放 ACK，不重做。
- `HEAT`：本地探头达标才 SUCCESS。
- `DISPENSE` + `TEMP_GTE`：再测一次本地水温，不用云端影子。
- `WEIGHT` 只下发目标克数，硬件自己称。

## ACK

`RECEIVED` → `RUNNING` → 唯一终态（`SUCCESS|FAILED|MISSING|EXCEPTION|REJECTED`）。带实际 `waterTemp` 仅日志。

## SHADOW

当前值：version、online、水温、workState、currentTaskId/Epoch/StepNo、slots.occupied（硬件感知）。绑定关系以云端仓位为准。

## EVENT / CAPABILITY

事件：`SLOT_EMPTY` `SLOT_JAM` `OVERHEAT` `SENSOR_ERROR` `BOIL_DRY`。  
上线必报能力（仓、模式、actions）。Cooking 按此校验。

## 幂等

未收到 RECEIVED：可重发同一 commandId。  
已 RECEIVED：禁止新 commandId 再投。失败只能取消或开新任务。  
超时 120s：Cooking Failed + 另发 STOP。
