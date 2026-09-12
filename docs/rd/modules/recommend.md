# 模块 Recommend（二期）

**第一版不建项目、不建表、不进 Host。**

以后：对话与推荐。只能 ACL 查询 Device/Inventory/Recipe/Catalog，用户确认后走 `ICookingTaskGateway`。仍不能直发 MQTT、不能绕过 Cooking 校验。

健康话术不得做成诊疗。

V1 小程序路径：选已发布配方 → `POST /api/portal/cooking-tasks`。
