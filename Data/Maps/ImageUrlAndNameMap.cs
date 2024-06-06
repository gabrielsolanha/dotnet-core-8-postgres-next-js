using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace AplicacaoWeb.Data.Maps
{
    public class ImageUrlAndNameMap : BaseMap<ImageUrlAndName>
    {
        public ImageUrlAndNameMap() : base("ImageUrlAndNames")
        { }

        public override void Configure(EntityTypeBuilder<ImageUrlAndName> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FilmeId).HasColumnName("FilmId").IsRequired();
            builder.HasOne(x => x.Filme).WithMany(x => x.Images).HasForeignKey(x => x.FilmeId);

            builder.HasIndex(x => x.Url).IsUnique();
            builder.Property(x => x.IsDeleted).HasColumnName("Ativo");
        }
    }
}
