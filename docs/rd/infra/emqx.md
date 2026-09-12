# EMQX（自建）

起 Broker 时读。协议正文在 [protocol/mqtt](../protocol/mqtt.md)。

## V1 形态

EMQX 5 开源，**单节点 Docker**。开发也用它，不用 Mosquitto。

| 端口 | 用途 |
|---|---|
| 1883 | MQTT |
| 18083 | Dashboard |
| 8083 | WebSocket（调试） |

认证：HTTP 回调 `POST /api/device/mqtt-auth`。ACL：`pot/{username}/#`。  
云端服务账号 `nxai-device-svc` 可订 `pot/#`，密码只在 Host 配置。  
一个 SN 一个会话，重复登录踢旧。

关掉规则引擎写 MySQL/Influx/Kafka。解析和幂等在 Device。

TLS、集群：真机/规模上来再说。

## 位置

```text
壶/模拟器 --1883--> EMQX --仅 Device 订阅--> 影子 + ITelemetryStore + CAP
RabbitMQ 只给 CAP，不要和 EMQX 混用
```

小程序不直连 EMQX，进度轮询影子即可。
