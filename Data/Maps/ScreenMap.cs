using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace AplicacaoWeb.Data.Maps
{
    public class ScreenMap : BaseMap<Screen>
    {
        public ScreenMap() : base("Screen")
        { }

        public override void Configure(EntityTypeBuilder<Screen> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ScreenName).HasColumnName("Name").IsRequired();

            builder.HasIndex(x => x.ScreenUrl).IsUnique();
            builder.HasIndex(x => x.ScreenName).IsUnique();
            builder.Property(x => x.IsDeleted).HasColumnName("Ativo");
        }
    }
}
