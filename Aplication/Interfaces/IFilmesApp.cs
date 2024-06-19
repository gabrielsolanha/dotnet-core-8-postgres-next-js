using AplicacaoWeb.Models.Dtos.Filme;

namespace AplicacaoWeb.Aplication.Interfaces
{
    public interface IFilmesApp : IApp<FilmeDto>
    {
        Task<FilmeDto> Add(FilmeWithArquiveDto obj, string changeMaker);
        Task<FilmeDto> Update(int id, FilmeWithArquiveDto obj, string changeMaker);
    }
}
