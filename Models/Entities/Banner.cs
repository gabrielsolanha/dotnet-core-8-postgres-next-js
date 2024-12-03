namespace AplicacaoWeb.Models.Entities
{
    public class Banner : BaseEntity
    {
        public virtual string? Title { get; set; }
        public virtual string? Description { get; set; }
        public virtual string? RedirectLink { get; set; }
        public virtual bool IsMobile { get; set; }
        public virtual string? Path { get; set; }
        public virtual string? ArquiveName { get; set; }
    }
}
