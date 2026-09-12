# 设备遥测切入口

时序**模型现在定**。第一版用 MySQL 实现，不上 TDengine。开火仍看 [operation-order](operation-order.md)。写 Device 时读。

## 三套存储

| 存 | 回答 | 形态 |
|---|---|---|
| `dev_shadow` | 现在几度 | 一行覆盖 |
| `ITelemetryStore` | 这段时间怎么走 | `(sn, metric, ts)` 追加 |
| `ckg_execution_log` | 这步成没成 | 稀疏事件 |

## 点

Ts=设备事件时间，ReceivedAt=服务器时间，Sn，Metric，ValueNum/Int/Text，TaskId?，Epoch?，Quality，Source。

V1 Metric：`water_temp`（≥5s，重复可丢）、`target_temp`/`work_state`/`online`（变化才写）、`power_w`/`fault_code`/`slot_occupied` 预留。

表 `dev_telemetry`，索引/唯一 `(sn, metric, ts)`，建议 7 天删除。策略在适配器，不改点模型。

## 端口

```text
ITelemetryStore
  AppendAsync(points)     // 失败只打日志，不回滚指令
  QueryAsync(sn, metric, from, to, taskId?, limit)
```

V1：`RelationalTelemetryStore`。以后：`TdengineTelemetryStore`，方法名不变。

只在 Device 解析 MQTT 后 Append。禁止 EMQX 直写表。禁止其他模块碰表。

## 第一版

要有：接口 + MySQL 适配器 + 至少 water_temp/work_state + console 查询。  
可以没有：曲线页、1Hz、TDengine。
