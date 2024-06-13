using AplicacaoWeb.Models.Dtos.Filme;

namespace AplicacaoWeb.Aplication
{
    public interface IFilmesApp : IApp<FilmeDto>
    {
        Task<FilmeDto> Add(FilmeWithArquiveDto obj);
        Task<FilmeDto> Update(int id, FilmeWithArquiveDto obj);
    }
}
