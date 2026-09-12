# 模块 System（已有）

**一句话：** 后台员工、角色、菜单、字典。  
**表前缀：** `sys_`  
**路径：** `src/Modules/NXAI.System`（已存在）  
**第一版：** 沿用，不往里面塞养生壶业务。

## 负责

员工登录、后台 JWT、`IUserService`、权限。

## 不负责

小程序用户、设备 SN、配方、任务、MQTT。

## 给别人

只暴露 `StaffUserId`。其他模块不读 `sys_user`。

## 鉴权

`/api/console` 的 JWT：`token_type=staff`。禁止写入 Device/Cooking 的 `MemberId`。

## 本模块 V1 不用改表

现有 `sys_user` / `sys_role` / `sys_menu` 等保持即可。
