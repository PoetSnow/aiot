using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository.EfCore;

namespace NXAI.Device.Repository;

/// <summary>设备模块表映射。只登记 dev_ 前缀表。不登记遥测表，走 ITelemetryStore。</summary>
public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Device>().ToTable("dev_device");
        modelBuilder.Entity<Entities.DeviceSlot>().ToTable("dev_slot");
        modelBuilder.Entity<Entities.DeviceShadow>().ToTable("dev_shadow");
        modelBuilder.Entity<Entities.DeviceCapability>().ToTable("dev_capability");
        modelBuilder.Entity<Entities.DeviceMqttOutbox>().ToTable("dev_mqtt_outbox");
        modelBuilder.Entity<Entities.DeviceTelemetry>().ToTable("dev_telemetry");
    }
}
