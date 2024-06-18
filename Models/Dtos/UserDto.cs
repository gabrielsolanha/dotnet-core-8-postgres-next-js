using AplicacaoWeb.Models.Dtos.Filme;

namespace AplicacaoWeb.Models.Dtos
{
    public class UserDto : BaseDto
    {
        public virtual string? UserName { get; set; }
        public virtual string? Telefone { get; set; }
        public virtual string? CallMeName { get; set; }
        public virtual string? Email { get; set; }
        public virtual IEnumerable<FilmeDto>? Filmes { get; set; }
        public virtual IEnumerable<ScreenDto>? ScreensAcess { get; set; }
    }
}
