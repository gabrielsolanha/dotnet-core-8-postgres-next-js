using AplicacaoWeb.Models.Dtos.Filme;

namespace AplicacaoWeb.Aplication
{
    public interface IFilmesApp : IApp<FilmeDto>
    {
        new Task<FilmeDto> Add(FilmeWithArquiveDto obj);
    }
}
