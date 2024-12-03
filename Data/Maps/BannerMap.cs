using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace AplicacaoWeb.Data.Maps
{
    public class BannerMap : BaseMap<Banner>
    {
        public BannerMap() : base("Banners")
        { }

        public override void Configure(EntityTypeBuilder<Banner> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Title).HasColumnName("Titulo").IsRequired();

            builder.HasIndex(x => x.Path).IsUnique();

            builder.Property(x => x.ArquiveName).HasColumnName("Nome do Arquivo").IsRequired();
        }
    }
}
