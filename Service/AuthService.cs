using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Domain;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Dtos.Requests;
using AplicacaoWeb.Models.Dtos.Responses;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Enums;
using AplicacaoWeb.Service.Interfaces;

namespace AplicacaoWeb.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IRepositoryBase<UserScreen> userScreenRepository;
        private readonly ITokenService tokenService;

        public AuthService(
           IUserRepository userRepository,
            ITokenService tokenService,
        IRepositoryBase<UserScreen> userScreenRepository
            )
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.userScreenRepository = userScreenRepository ?? throw new ArgumentNullException(nameof(userScreenRepository));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public User? GetUser(string userName)
        {
            var user = userRepository.GetAllWhen(x => x.UserName == userName).FirstOrDefault();

            return user;
        }

        public LoginResponse? GetLoginResponse(User user, LoginRequest loginRequest)
        {
            GenerateHashService hashService = new GenerateHashService();
            if (!hashService.VerifyPassword(loginRequest.Password, user.Password))
            {
                return null;
            }
            else
            {
                UserDto userDto = new UserDto()
                {
                    UserName = user.UserName,
                    CallMeName = user.CallMeName,
                    CreatedAt = user.CreatedAt,
                    CreatedBy = user.CreatedBy,
                    Email = user.Email,
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    Telefone = user.Telefone,
                    UpdatedBy = user.UpdatedBy,
                    UpdatedDate = user.UpdatedDate,
                };
                return new LoginResponse()
                {
                    Token = this.tokenService.GenerateToken(userDto),
                    Views = GetListView(user),
                    User = userDto,
                };
            }
        }

        public List<ViewResponse> GetListView(User user)
        {
            List<ViewResponse> list = new List<ViewResponse>();
            var userViews = userScreenRepository.GetAllWhen(v => v.Users.Id == user.Id);
            foreach (var view in userViews)
            {
                list.Add(new ViewResponse()
                {
                    Name = view.Screens.ScreenName,
                    Route = view.Screens.ScreenUrl,
                    AcessLevel = AccessLevelToString(view.AccessLevel),
                });
            }
            return list;
        }
        public string AccessLevelToString(AccessLevel accessLevel)
        {
            List<string> accessStrings = new List<string>();

            if (accessLevel.HasFlag(AccessLevel.View))
            {
                accessStrings.Add("View");
            }
            if (accessLevel.HasFlag(AccessLevel.Create))
            {
                accessStrings.Add("Create");
            }
            if (accessLevel.HasFlag(AccessLevel.Update))
            {
                accessStrings.Add("Update");
            }
            if (accessLevel.HasFlag(AccessLevel.Delete))
            {
                accessStrings.Add("Delete");
            }

            return string.Join(",", accessStrings);
        }



    }
}
