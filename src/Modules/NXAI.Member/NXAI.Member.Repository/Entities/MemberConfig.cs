using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NXAI.Member.Repository.Entities;

/// <summary>mb_member 列长与唯一索引。手机号、OpenId 有值时各只能对应一名会员。</summary>
public class MemberConfig : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(x => x.Mobile).HasMaxLength(Member.Mobile_MaxLength);
        builder.Property(x => x.OpenId).HasMaxLength(Member.OpenId_MaxLength);
        builder.Property(x => x.UnionId).HasMaxLength(Member.UnionId_MaxLength);
        builder.Property(x => x.Nickname).HasMaxLength(Member.Nickname_MaxLength).IsRequired();
        builder.Property(x => x.Avatar).HasMaxLength(Member.Avatar_MaxLength).IsRequired();
        builder.HasIndex(x => x.Mobile).IsUnique();
        builder.HasIndex(x => x.OpenId).IsUnique();
    }
}
