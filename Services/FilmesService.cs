using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Data.Repository.Interfaces;
using AplicacaoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoWeb.Services
{
    public class FilmesService:IService<Filme>
    {
        private readonly IFilmeRepository filmeRepository;

        public FilmesService(IFilmeRepository filmeRepository)
        {
            this.filmeRepository = filmeRepository ?? throw new ArgumentNullException(nameof(filmeRepository));
        }

        public async Task<Filme> Add(Filme filme)
        {
            filme.CreatedAt = DateTime.UtcNow;
            filmeRepository.Add(filme);
            await filmeRepository.SaveAsync();

            return filme;
        }

        public async Task Delete(int id)
        {
            Filme filme = await filmeRepository.GetFilmesByIdAsync(id);

            if (filme == null)
            {
                throw new Exception("Nenhum filme encontrado.");
            }
            filmeRepository.Delete(filme);
            await filmeRepository.SaveAsync();

            return;
        }

        public async Task<Filme> Get(int id)
        {
            Filme filme = await filmeRepository.GetFilmesByIdAsync(id);

            if (filme == null)
            {
                throw new Exception("Nenhum filme encontrado.");
            }

            return filme;
        }

        public async Task<IEnumerable<Filme>> List() 
        {
            return await filmeRepository.GetFilmesAllAsync();
        }

        public async Task Update(int id, Filme filme)
        {
            try
            {
                filmeRepository.Update(filme);
                await filmeRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!filmeRepository.FilmeExists(id))
                {
                    throw new Exception("Nenhum filme encontrado.");
                }
                else
                {
                    throw;
                }
            }
        }

    }
}
