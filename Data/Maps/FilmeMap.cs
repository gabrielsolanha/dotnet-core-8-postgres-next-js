using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace AplicacaoWeb.Data.Maps
{
    public class FilmeMap : BaseMap<Filme>
    {
        public FilmeMap() : base("Filmes")
        { }

        public override void Configure(EntityTypeBuilder<Filme> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Title).HasColumnName("Titulo").IsRequired();

            builder.HasIndex(x => x.Plate).IsUnique();

            builder.Property(x => x.UserResponsibleId).HasColumnName("User").IsRequired();
            builder.HasOne(x => x.UserResponsible).WithMany(x => x.Filmes).HasForeignKey(x => x.UserResponsibleId);
            builder.HasOne(x => x.Category).WithMany(x => x.Filmes).HasForeignKey(x => x.CategoryId);
            builder.HasMany(x => x.Images).WithOne(x => x.Filme).HasForeignKey(x => x.FilmeId);
        }
    }
}
