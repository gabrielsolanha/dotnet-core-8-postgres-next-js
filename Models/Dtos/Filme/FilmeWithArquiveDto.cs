namespace AplicacaoWeb.Models.Dtos.Filme
{
    public class FilmeWithArquiveDto : BaseDto
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string? Plate { get; set; }
        public virtual int UserResponsibleId { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual IEnumerable<IFormFile> ImageFiles { get; set; }
    }


}
