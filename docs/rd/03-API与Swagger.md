# HTTP API 与 Swagger

按**调用方**拆，不按模块名拆路由。基类已在 `NXAI.Shared.WebApi`。

| 端 | 前缀 | Group | 鉴权 |
|---|---|---|---|
| 后台 | `/api/console` | `console` 后台管理 API | 员工 JWT |
| 小程序 | `/api/portal` | `portal` 小程序 API | 会员 JWT |
| 设备 | `/api/device` | `device` 设备端 API | `X-Device-Token` |
| 调试 | `/api/internal` | `internal` | 仅 Development |

规则：

- 一个 Controller 只挂一套面。
- 后台继承 `ConsoleApiController`，小程序 `PortalApiController`，设备 `DeviceApiController`。
- 投放不进 HTTP。`api/device` 只做激活、换 Token、MQTT 鉴权回调。
- Swagger：console/portal 用 Bearer；device 补 `X-Device-Token`，不要只显示员工 JWT。

各模块自己的路径写在模块文件里，这里不重复罗列。
