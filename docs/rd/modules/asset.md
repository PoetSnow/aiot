# 模块 Asset

**一句话：** 设备实物进销存（型号、仓库、入出库、SN）。  
**表前缀：** `ast_`  
**路径：** `src/Modules/NXAI.Asset/`  
**开工序号：** 4  
**依赖：** [modules/member](member.md)

## 负责 / 不负责

- 负责：型号、仓库、入库、出库、SN 台账。库存 = 按仓+型号统计 `InStock`。
- 不负责：水温、仓位、MQTT、影子。出库 ≠ 创建设备会话。

## SN 状态

`Created` → `InStock` → `Outbound` → `Bound`  
售后：`Repairing` / `Returned` / `Replaced` / `Scrapped`

未入库不能激活；未出库小程序不能绑定。

## 表

- `ast_device_model`：ModelCode 唯一，WarrantyMonths，SlotProfileJson
- `ast_warehouse`：Code，Name
- `ast_sn`：Sn 全局唯一，ModelCode，WarehouseId，Status，MemberId?，入出库单/时间
- `ast_stock_in` / `ast_stock_in_line`
- `ast_stock_out` / `ast_stock_out_line`

入库确认：SN 未占用 → `InStock`。  
出库确认：必须 `InStock` → `Outbound`，可带 MemberId。  
未审单据禁止改 SN。

## API（console）

```text
GET|POST /api/console/device-models
GET      /api/console/warehouses
POST     /api/console/stock-ins
POST     /api/console/stock-ins/{id}/confirm
POST     /api/console/stock-outs
POST     /api/console/stock-outs/{id}/confirm
GET      /api/console/asset-sns
GET      /api/console/asset-sns/{sn}
```

对外给 Device：校验「已出库且可被该 Member 认领」。

## ACL

- `IMemberGateway`：出库挂会员时确认会员存在。

## 事件

发布：`pot.asset.sn.inbound.v1`、`outbound.v1`、`status.changed.v1`  
订阅：`pot.device.bound/unbound`、`pot.aftersales.ticket.*`（改 SN 状态，经本模块服务，不让售后直接 UPDATE 本表）

## 红线

Device / AfterSales 不得直接改 `ast_sn`。本模块不发 MQTT。
