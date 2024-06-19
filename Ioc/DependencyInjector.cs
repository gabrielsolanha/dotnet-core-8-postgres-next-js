using AplicacaoWeb.Data.Repository;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Aplication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AplicacaoWeb.Service.Interfaces;
using AplicacaoWeb.Data.UnitOfWork;
using AplicacaoWeb.Models.Dtos.Filme;
using AplicacaoWeb.Service;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Aplication.Interfaces;


namespace AplicacaoWeb.IoC
{
    public static class DependencyInjector
    {
        public static void Register(IServiceCollection services)
        {
            RegisterServices(services);
            RegistrarRepositorios(services);
            RegisterApplication(services);
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            //Filme
            services.TryAddScoped<IRepositoryBase<Filme>, RepositoryBase<Filme>>();
            services.TryAddScoped<IFilmeRepository, FilmeRepository>();
            //User
            services.TryAddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.TryAddScoped<IUserRepository, UserRepository>();
            //Screen
            services.TryAddScoped<IRepositoryBase<Screen>, RepositoryBase<Screen>>();
            services.TryAddScoped<IScreenRepository, ScreenRepository>();
            //UserScreen
            services.TryAddScoped<IRepositoryBase<UserScreen>, RepositoryBase<UserScreen>>();
            //Category
            services.TryAddScoped<IRepositoryBase<Category>, RepositoryBase<Category>>();
            //ImageUrlAndName
            services.TryAddScoped<IRepositoryBase<ImageUrlAndName>, RepositoryBase<ImageUrlAndName>>();
            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void RegisterApplication(IServiceCollection services)
        {
            //Filme
            services.TryAddScoped<IApp<FilmeDto>, FilmesApp>();
            services.TryAddScoped<IFilmesApp, FilmesApp>();
            //User
            services.TryAddScoped<IApp<UserDto>, UsersApp>();
            services.TryAddScoped<IUsersApp, UsersApp>();
            //Category
            services.TryAddScoped<IApp<CategoryDto>, CategoriesApp>();
        }
        private static void RegisterServices(IServiceCollection services)
        {
            services.TryAddScoped<IS3Service, S3Service>();
            services.TryAddScoped<ITokenService, TokenService>();
            services.TryAddScoped<IAuthService, AuthService>();
        }

    }
}
