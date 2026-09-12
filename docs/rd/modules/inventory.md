# 模块 Inventory

**一句话：** 用户手里的耗材实例（茶包、散装录入）。  
**表前缀：** `inv_`  
**路径：** `src/Modules/NXAI.Inventory/`  
**开工序号：** 8  
**依赖：** [modules/device](device.md)

## 负责 / 不负责

- 负责：耗材实例、账本余量、放置位置的**投影**。
- 不负责：仓位物理事实（在 Device）、商品货架（在 Catalog）、配方步骤。

不可拆茶包是一个投料单元，禁止让用户填内部克数再拆投。

## 表

`inv_consumable`：MemberId，ConsumableTypeCode（与配方 TargetCode 对齐），ProductId?，Form，Total/RemainQty，QtyUnit，Status，PlacedDeviceId/SlotCode（投影）

投放 SUCCESS 后订阅任务完成事件，`RemainQty - 1`。缺料不扣。

## API（portal）

```text
GET  /api/portal/consumables
POST /api/portal/consumables
POST /api/portal/consumables/{id}/place
```

放置：本模块 ACL 调 Device 绑仓，成功后再写投影。

## ACL

- `IDeviceSlotGateway`：申请绑定/解绑  
- `ICatalogGateway`：关联 SKU，可后补  

## 事件

订阅：slot.bound/unbound、cooking.task.completed/failed
