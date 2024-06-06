namespace AplicacaoWeb.Models.Entities
{
    public class ImageUrlAndName: BaseEntity
    {
        public virtual string Url { get; set; }
        public virtual string ArquiveName { get; set; }
        public virtual int FilmeId { get; set; }
        public virtual Filme Filme { get; set; }
    }
}
