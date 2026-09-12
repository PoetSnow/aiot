# 模块 AfterSales

**一句话：** 设备售后工单。  
**表前缀：** `afs_`  
**路径：** `src/Modules/NXAI.AfterSales/`  
**开工序号：** 11（主链路通了再做）  
**依赖：** [modules/asset](asset.md)、[modules/device](device.md)、[modules/member](member.md)

## 负责 / 不负责

- 负责：咨询/维修/退货/换货工单、质保结论（开单时冻结）、处理日志。
- 不负责：MQTT、配方、自己存进销存。质保问 Asset（出库时间 + 型号月数）。

## 表

`afs_ticket`：TicketNo，Sn，MemberId?，DeviceId?，Type，Status，WarrantyValid，Symptom，CloseResult  
`afs_ticket_log`：Action，Remark，OperatorStaffId

开维修：发事件，Asset 把 SN → `Repairing`；仍绑定则 ACL 让 Device 解绑。  
结案：回库 / 换货 / 报废，经 Asset，不直接改 `ast_sn`。

## API

```text
GET|POST /api/console/tickets
POST     /api/console/tickets/{id}/accept
POST     /api/console/tickets/{id}/close
POST     /api/portal/tickets
GET      /api/portal/tickets
```

## ACL

- `IAssetSnGateway`：质保、请求改 SN 状态  
- `IDeviceGateway`：解绑  
- `IMemberGateway`：工单用户

## 事件

发布：`ticket.opened.v1`、`ticket.closed.v1`  
订阅：`asset.sn.outbound`（可选缓存质保起点）

## 红线

不发 MQTT。不直接 UPDATE `ast_sn`。
