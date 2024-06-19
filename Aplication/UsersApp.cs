using AplicacaoWeb.Aplication.Interfaces;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Data.UnitOfWork;
using AplicacaoWeb.Domain;
using AplicacaoWeb.Mapper;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Enums;
using AplicacaoWeb.Service.Interfaces;

namespace AplicacaoWeb.Aplication
{
    public class UsersApp : IUsersApp
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IS3Service iS3Service;
        private readonly IAuthService authService;
        private readonly IRepositoryBase<Screen> screenRepository;
        private readonly IRepositoryBase<UserScreen> userScreenRepository;

        public UsersApp(IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IRepositoryBase<Screen> screenRepository,
            IS3Service iS3Service,
            IAuthService authService,
            IRepositoryBase<UserScreen> userScreenRepository
            )
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.screenRepository = screenRepository ?? throw new ArgumentNullException(nameof(screenRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.userScreenRepository = userScreenRepository ?? throw new ArgumentNullException(nameof(userScreenRepository));
            this.iS3Service = iS3Service ?? throw new ArgumentNullException(nameof(iS3Service));
        }
        public async Task<UserDto> Add(UserDto userDto, string changeMaker)
        {
            try
            {
                unitOfWork.BeginTransaction();
                var mapper = new UserMapper();
                User user = mapper.MapperFromDto(userDto);
                var hashService = new GenerateHashService();
                user.Password = hashService.GenerateHashedPassword(userDto.Pass);
                user.CreatedBy = changeMaker;
                user.CreatedAt = DateTime.UtcNow;
                userRepository.Add(user);
                await unitOfWork.SaveAsync();

                var existingUser = userRepository.GetAllWhen(u => u.UserName == userDto.UserName).FirstOrDefault();
                if (userDto.Views != null)
                {
                    foreach (var view in userDto.Views)
                    {
                        var screen = screenRepository.GetAllWhen(s => s.ScreenName == view.Name).FirstOrDefault();
                        if (screen != null)
                        {
                            var userScreen = new UserScreen
                            {
                                UserId = existingUser.Id,
                                ScreenId = screen.Id,
                                AccessLevel = StringToAccessLevel(view.AcessLevel)
                            };
                            userScreenRepository.Add(userScreen);
                        }

                    }
                }
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();

                return await authService.MapperToDtoFromIdAsync(user.Id);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public static AccessLevel StringToAccessLevel(string accessLevelString)
        {
            AccessLevel accessLevel = 0;
            if (string.IsNullOrEmpty(accessLevelString))
            {
                return accessLevel;
            }

            var levels = accessLevelString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var level in levels)
            {
                if (Enum.TryParse(level.Trim(), out AccessLevel parsedLevel))
                {
                    accessLevel |= parsedLevel;
                }
                else
                {
                    throw new ArgumentException($"Invalid access level: {level}");
                }
            }

            return accessLevel;
        }
        public async Task Delete(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                User user = userRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("Nenhum user encontrado.");
                }
                foreach (var deletar in user.ScreensAcess)
                {
                    var db = userScreenRepository.GetAllWhen(x => x.ScreenId == deletar.Id && x.UserId == id).FirstOrDefault() ?? throw new Exception($"Houve um erro ao fazer a operação: Falha ao recuperar Imagem no data base");
                    userScreenRepository.Delete(db);
                }
                userRepository.Delete(user);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
                return;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public async Task<UserDto> Get(int id)
        {
            var user = await userRepository.GetUsersByIdAsync(id);

            if (user == null)
            {
                throw new Exception("Nenhum user encontrado.");
            }

            return await authService.MapperToDtoFromIdAsync(user.Id);
        }

        public IEnumerable<UserDto> List(PaginationDto<UserDto> filtro)//username telefone callmename email 
        {
            var source = userRepository.GetAllWhen(x => (!filtro.Filter.Id.HasValue || x.Id == filtro.Filter.Id) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.UserName) || x.UserName.ToLower().Contains(filtro.Filter.UserName.ToLower())) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.Telefone) || x.Telefone.ToLower().Contains(filtro.Filter.Telefone.ToLower())) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.CallMeName) || x.CallMeName.ToLower().Contains(filtro.Filter.CallMeName.ToLower())) &&
                                                         x.IsDeleted == filtro.Filter.IsDeleted);
            return MapFilter(source, filtro);

        }

        private IEnumerable<UserDto> MapFilter(IEnumerable<User> users, PaginationDto<UserDto> filtro)
        {
            var paginacao = Pagination(users, filtro);
            var mapper = new UserMapper();

            foreach (var item in paginacao)
            {
                yield return mapper.MapperToDto(item);
            }
        }
        private IEnumerable<User> Pagination(IEnumerable<User> users, PaginationDto<UserDto> filtro)
        {

            var total = users.Count();
            users = users.Skip((filtro.Page) * filtro.ItemCount).Take(filtro.ItemCount);
            filtro.ItemCount = total;

            return users;
        }

        public async Task<UserDto> Update(int id, UserDto userdto, string changeMaker)
        {

            unitOfWork.BeginTransaction();
            var mapper = new UserMapper();
            var existingUser = await userRepository.GetUsersByIdAsync(id);
            User user = mapper.MapperFromDtoToUpdate(userdto, existingUser);
            if (string.IsNullOrEmpty(userdto.Pass))
            {
                var hashService = new GenerateHashService();
                user.Password = hashService.GenerateHashedPassword(userdto.Pass);
            }
            user.UpdatedDate = DateTime.UtcNow;
            user.UpdatedBy = changeMaker;
            userRepository.Update(user);
            await unitOfWork.SaveAsync();


            if (userdto.Views != null)
            {

                foreach (var deletar in existingUser.ScreensAcess)
                {
                    var db = userScreenRepository.GetAllWhen(x => x.ScreenId == deletar.Id && x.UserId == id).FirstOrDefault() ?? throw new Exception($"Houve um erro ao fazer a operação: Falha ao recuperar Imagem no data base");
                    userScreenRepository.Delete(db);
                }
                await unitOfWork.SaveAsync();

                foreach (var view in userdto.Views)
                {
                    var screen = screenRepository.GetAllWhen(s => s.ScreenName == view.Name).FirstOrDefault();
                    if (screen != null)
                    {
                        var userScreen = new UserScreen
                        {
                            UserId = existingUser.Id,
                            ScreenId = screen.Id,
                            AccessLevel = StringToAccessLevel(view.AcessLevel)
                        };
                        userScreenRepository.Add(userScreen);
                    }

                }
                await unitOfWork.SaveAsync();
            }
            unitOfWork.Commit();

            return mapper.MapperToDto(user);
        }

        public async Task<UserDto> GetUserPublicInfo(int id)
        {
            var user = await userRepository.GetUsersByIdAsync(id);

            if (user == null)
            {
                throw new Exception("Nenhum user encontrado.");
            }

            var mapper = new UserMapper();
            return mapper.MapperToDto(user);
        }

    }
}
