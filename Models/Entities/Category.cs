namespace AplicacaoWeb.Models.Entities
{
    public class Category:BaseEntity
    {
        public virtual string CategoryName { get; set; }
        public virtual IEnumerable<Filme>? Filmes { get; set; }
    }
}
