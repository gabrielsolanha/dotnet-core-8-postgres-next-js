using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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
                    }
                );
        }
    }
}
