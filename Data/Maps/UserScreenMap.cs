using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AplicacaoWeb.Data.Maps
{
    public class UserScreenMap : IEntityTypeConfiguration<UserScreen>
    {
        private readonly string _tableName;

        public UserScreenMap()
        {
            _tableName = "UserScreen";
        }

        public virtual void Configure(EntityTypeBuilder<UserScreen> builder)
        {
            if (!string.IsNullOrEmpty(_tableName)) builder.ToTable(_tableName);

            builder.Property(x => x.AccessLevel)
                .HasConversion(
                    v => (int)v,
                    i => (AccessLevel)Enum.ToObject(typeof(AccessLevel), i)
                );
        }
    }
}
