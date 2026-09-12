# 操作时序（并发 / 网络）

MQTT 会重复、会后到。顺序不靠 Broker，靠状态机。写 Cooking、Device 下发、模拟器时读。

## 三条铁律

1. 一壶同时一个 ActiveTask。再点开始 → 拒绝。
2. 一任务同时一条在途 COMMAND。HEAT 没终态 ACK，不发 DISPENSE。
3. 设备只执行：`epoch` 相同且 `stepNo == 期望下一步`。其余 REJECTED。`STOP`（同 epoch）可打断。

新任务开始前先 STOP 旧世代，再 `ActiveEpoch++`。旧包全部作废。

## 设备收包顺序

1. commandId 见过 → 回放 ACK  
2. epoch 旧 → REJECTED  
3. stepNo 不对 → REJECTED  
4. 过期 → REJECTED  
5. STOP 同 epoch → 停机械，清空期望  
6. 通过 → 做一个动作  

硬件状态只允许：`IDLE → HEATING → READY → DISPENSING → STEEPING → IDLE`。

## 水温开火（防云端 80、壶里 70）

```text
云端：发 HEAT(80) → 等硬件 SUCCESS → 再发 DISPENSE(必须 TEMP_GTE 80)
硬件：本地 ≥80 才 HEAT SUCCESS；投料前再读探头，不到就等或到期拒
影子/遥测：只给界面和超时看门狗，不能扣扳机
```

禁止：看见影子 ≥80 就发无 Trigger 的立即投放。

## 模拟器必测

- 连点开始只有一个 Running  
- 重放同一 commandId，投放次数仍为 1  
- 旧 epoch 的 HEAT 被拒  
- 影子写成 80、本地 70 时收到 DISPENSE+TEMP_GTE：不得投  
