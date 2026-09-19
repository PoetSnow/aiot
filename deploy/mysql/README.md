# MySQL 脚本

库名 `nxai`。按文件名顺序执行。标识符英文，注释中文。

## 文件

| 文件 | 内容 |
| --- | --- |
| `001_system.sql` | System 表 + 数据。从 `netcoreai/docs/nx_devices_erp.sql` 原样拷贝 `sys_*` |
| `002_v1_modules.sql` | V1 养生壶业务表，按当前实体自建。仅种子 `ast_warehouse` 主仓 `MAIN` |

## 约定

- 不使用 EF Migrations。Schema 以本目录脚本为准。
- V1 表 `id` 为雪花，非自增；无审计列（`EfEntity` 只有 `Id`）。
- 列名跟随 `UseLowerCaseNamingConvention`：全小写、无下划线（如 `memberid`，不是 `member_id`）。
- 模块之间不加外键。同步调用走本模块 Gateway，不靠库约束跨模块。
- `002` 含 `DROP TABLE IF EXISTS`，空库或重建时用；已有业务数据不要重复跑。

## 执行示例

```bash
mysql -u root -p nxai < deploy/mysql/001_system.sql
mysql -u root -p nxai < deploy/mysql/002_v1_modules.sql
```
