namespace AplicacaoWeb.Models.Dtos
{
    public class ImageUrlAndNameDto: BaseDto
    {
        public virtual string Url { get; set; }
        public virtual string ArquiveName { get; set; }
        public virtual int FilmeId { get; set; }
    }
}
