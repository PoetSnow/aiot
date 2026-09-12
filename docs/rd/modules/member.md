# 模块 Member

**一句话：** 小程序独立账号。  
**表前缀：** `mb_`  
**路径：** `src/Modules/NXAI.Member/`（四层，抄 System）  
**开工序号：** 2  
**依赖：** [00-架构总览](../00-架构总览.md)、[03-API与Swagger](../03-API与Swagger.md)

## 负责 / 不负责

- 负责：手机号或微信登录、会员 JWT、资料、注销。
- 不负责：后台登录、SN 库存、MQTT、配方。

## 表

`mb_member`

| 字段 | 类型 | 说明 |
|---|---|---|
| Id | long | 与 `sys_user.Id` **不是同一空间** |
| Mobile | string(16) | 可空，有值则唯一 |
| OpenId | string(64) | 可空，唯一 |
| UnionId | string(64) | 可空 |
| Nickname | string(32) | |
| Avatar | string(256) | |
| Status | int | 1 正常 / 0 禁用 / 2 注销 |
| LastLoginTime | DateTime? | |

不与 `sys_user` 做外键、不合并登录。

## JWT

`sub=MemberId`，`token_type=member`，`aud=portal`。只打 `/api/portal`。

## API（portal）

```text
POST /api/portal/auth/session
POST /api/portal/auth/session/refresh
GET  /api/portal/auth/profile
```

## 对外

`IMemberService`：按 Id 查是否存在、是否注销。Asset / Device / AfterSales 经 ACL 调用。

## ACL

本模块 V1 不调其他业务模块。

## 验收

员工 Token 访问上述接口失败；会员 Token 访问 `/api/console/**` 失败。
