# 智能养生壶 · 研发文档（按文件开工）

**接手先看：** [交接文档](../交接文档.md)（V1 已落地什么、怎么跑、和规格差在哪）。

规格已从一篇长文拆开。写代码时只打开**当前模块那一份**，不要整包通读。

产品原文：`docs/智能养生壶软件产品说明文档.docx`  
本文档版本：V1.2（2026-09-13）

## 先读这两份（约 15 分钟）

1. [00-架构总览](00-架构总览.md) — 边界、分层、三端、拍板结论
2. [01-开工顺序](01-开工顺序.md) — 按序号做，做完一项再开下一项

## 再按序号打开模块文件

| 序 | 模块 | 文件 | 第一版 |
|---|---|---|---|
| 已有 | System 后台员工 | [modules/system](modules/system.md) | 沿用 |
| 2 | Member 小程序账号 | [modules/member](modules/member.md) | 要做 |
| 4 | Asset 设备进销存 | [modules/asset](modules/asset.md) | 要做 |
| 5–6 | Device 在线设备 | [modules/device](modules/device.md) | 要做 |
| 7 | Recipe 配方 | [modules/recipe](modules/recipe.md) | 要做 |
| 8 | Inventory 用户耗材 | [modules/inventory](modules/inventory.md) | 要做 |
| 9 | Cooking 执行任务 | [modules/cooking](modules/cooking.md) | 要做 |
| 11 | AfterSales 售后 | [modules/aftersales](modules/aftersales.md) | 要做 |
| 12 | Catalog 耗材货架 | [modules/catalog](modules/catalog.md) | 轻量 |
| — | Recommend | [modules/recommend](modules/recommend.md) | **二期，不建** |

做到 Device / Cooking 时再读协议：

| 文件 | 何时读 |
|---|---|
| [02-共享内核](02-共享内核.md) | 写枚举、MQTT 信封时 |
| [03-API与Swagger](03-API与Swagger.md) | 写 Controller 时 |
| [04-集成事件](04-集成事件.md) | 写跨模块通知时 |
| [protocol/mqtt](protocol/mqtt.md) | 写 Device MQTT / 模拟器时 |
| [protocol/operation-order](protocol/operation-order.md) | 写下发与固件判定时 |
| [protocol/telemetry](protocol/telemetry.md) | 写影子落点 / `ITelemetryStore` 时 |
| [infra/emqx](infra/emqx.md) | 起 Broker 时 |

旧的单文件长文已改成索引，不再维护正文。
