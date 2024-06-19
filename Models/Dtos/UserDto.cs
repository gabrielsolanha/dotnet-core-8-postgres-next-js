using AplicacaoWeb.Models.Dtos.Filme;
using AplicacaoWeb.Models.Dtos.Responses;

namespace AplicacaoWeb.Models.Dtos
{
    public class UserDto : BaseDto
    {
        public virtual string? UserName { get; set; }
        public virtual string? Telefone { get; set; }
        public virtual string? Pass { get; set; }
        public virtual string? CallMeName { get; set; }
        public virtual string? Email { get; set; }
        public virtual IEnumerable<FilmeDto>? Filmes { get; set; }
        public virtual IEnumerable<ScreenDto>? ScreensAcess { get; set; }
        public virtual List<ViewResponse>? Views { get; set; }
    }
}
