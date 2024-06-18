using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AplicacaoWeb.Data.Maps
{
    public class UserMap : BaseMap<User>
    {
        public UserMap() : base("User")
        { }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.UserName).HasColumnName("UserName").IsRequired();
            builder.Property(x => x.Password).HasColumnName("Password").IsRequired();
            builder.HasIndex(x => x.Telefone).IsUnique();
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.IsDeleted).HasColumnName("Ativo");

            builder.HasMany(x => x.ScreensAcess)
                .WithMany(x => x.UsersAuthorized)
                .UsingEntity<UserScreen>(
                    join => join
                        .HasOne(us => us.Screens)
                        .WithMany()
                        .HasForeignKey(us => us.ScreenId),
                    join => join
                        .HasOne(us => us.Users)
                        .WithMany()
                        .HasForeignKey(us => us.UserId),
                    join =>
                    {
                        join.ToTable("UserScreen");
                        join.HasKey(us => new { us.ScreenId, us.UserId });
                        join.Property(us => us.ScreenId).IsRequired();
                        join.Property(us => us.UserId).IsRequired();
                        join.Property(us => us.AccessLevel)
                            .HasConversion(
                                v => (int)v,
                                i => (AccessLevel)Enum.ToObject(typeof(AccessLevel), i));
                    }
                );
        }
    }
}
