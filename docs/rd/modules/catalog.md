# 模块 Catalog

**一句话：** 耗材货架（轻量）。  
**表前缀：** `cat_`  
**路径：** `src/Modules/NXAI.Catalog/`  
**开工序号：** 12  
**第一版：** 后台维护 + 小程序只读，无订单无支付。

## 负责 / 不负责

- 负责：耗材 SKU、形态、建议配方 Code、适配型号。
- 不负责：当投料单元、当 SN 库存（壶走 Asset）、下单。

## 表

`cat_product`：SkuCode，Name，Form，ConsumableTypeCode，SuggestedRecipeCode，CompatibleModels，PackageQty，QtyUnit，Status，DetailJson（无医疗宣称）

## API

```text
GET|POST|PUT /api/console/products
GET          /api/portal/products
```

## ACL

V1 无。不发 MQTT。
