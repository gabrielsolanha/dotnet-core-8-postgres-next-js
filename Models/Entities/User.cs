namespace AplicacaoWeb.Models.Entities
{
    public class User : BaseEntity
    {
        public virtual string UserName { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string CallMeName { get; set; }
        public virtual string Email { get; set; }
        public virtual IEnumerable<Filme> Filmes { get; set; }
        public virtual IEnumerable<Screen> ScreensAcess { get; set; }
        public virtual string Password { get; set; }
    }
}
