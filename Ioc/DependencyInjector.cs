using AplicacaoWeb.Data.Repository;
using AplicacaoWeb.Data.Repository.Interfaces;
using AplicacaoWeb.Models;
using AplicacaoWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;


namespace AplicacaoWeb.IoC
{
    public static class DependencyInjector
    {
        public static void Register(IServiceCollection services)
        {
            RegisterServices(services);
            RegistrarRepositorios(services);
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddScoped<IRepositoryBase<Filme>, RepositoryBase<Filme>>();
            services.TryAddScoped<IFilmeRepository, FilmeRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.TryAddScoped<IService<Filme>, FilmesService>();
        }

    }
}
