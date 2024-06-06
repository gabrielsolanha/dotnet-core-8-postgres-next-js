namespace AplicacaoWeb.Models.Entities
{
    public class Filme : BaseEntity
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string? Plate { get; set; }
        public virtual int UserResponsibleId { get; set; }
        public virtual User UserResponsible { get; set; }
        public virtual Category Category { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual IEnumerable<ImageUrlAndName>? Images { get; set; }
    }
}
