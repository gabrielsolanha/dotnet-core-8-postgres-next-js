using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Data.UnitOfWork;
using AplicacaoWeb.Domain;
using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Dacpac
{
    public class DataInsert
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryBase<Screen> _screenRepository;
        private readonly IRepositoryBase<Category> _categoryRepository;
        private readonly IRepositoryBase<UserScreen> _userScreenRepository;

        public DataInsert(IUnitOfWork unitOfWork,
                          IUserRepository userRepository,
                          IRepositoryBase<Screen> screenRepository,
                          IRepositoryBase<Category> categoryRepository,
                          IRepositoryBase<UserScreen> userScreenRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _screenRepository = screenRepository;
            _categoryRepository = categoryRepository;
            _userScreenRepository = userScreenRepository;
        }

        public async Task InitializeAsync()
        {
            _unitOfWork.BeginTransaction();

            try
            {
                await AddScreensAsync();
                await AddUserWithAccessAsync();
                await AddCategoryAsync();
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        private async Task AddScreensAsync()
        {
            var screens = new List<Screen>
            {
                new Screen { ScreenName = "Filme", ScreenType = "public", ScreenUrl = "/filme" },
                new Screen { ScreenName = "Screens", ScreenType = "private", ScreenUrl = "/screens" },
                new Screen { ScreenName = "Users", ScreenType = "private", ScreenUrl = "/users" }
            };


            foreach (var screen in screens)
            {
                screen.CreatedBy = "Dacpac";
                screen.CreatedAt = DateTime.UtcNow;
                if (!(_screenRepository.GetAllWhen(s => s.ScreenName == screen.ScreenName)).Any())
                {
                    _screenRepository.Add(screen);
                }
            }

            await _unitOfWork.SaveAsync();
        }


        private async Task AddCategoryAsync()
        {
            var inserts = new List<Category>
            {
                new Category { CategoryName = "Popular" }
            };


            foreach (var insert in inserts)
            {
                insert.CreatedBy = "Dacpac";
                insert.CreatedAt = DateTime.UtcNow;
                if (!(_categoryRepository.GetAllWhen(i => i.CategoryName == insert.CategoryName)).Any())
                {
                    _categoryRepository.Add(insert);
                }
            }

            await _unitOfWork.SaveAsync();
        }

        private async Task AddUserWithAccessAsync()
        {
            var userName = Environment.GetEnvironmentVariable("GSUSER");
            var password = Environment.GetEnvironmentVariable("GSPASS");
            var phone = Environment.GetEnvironmentVariable("GSPHONE");
            var name = Environment.GetEnvironmentVariable("GSNAME");
            var mail = Environment.GetEnvironmentVariable("GSMAIL");

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mail))
            {
                throw new InvalidOperationException("Some of gsenvironment variables are not set.");
            }

            var user = _userRepository.GetAllWhen(u => u.UserName == userName);

            if (!user.Any())
            {
                var hashService = new GenerateHashService();
                var newUser = new User
                {
                    UserName = userName,
                    Telefone = phone,
                    CallMeName = name,
                    Email = mail,
                    IsDeleted = false,
                    Password = hashService.GenerateHashedPassword(password)
                };
                newUser.CreatedBy = "Dacpac";
                newUser.CreatedAt = DateTime.UtcNow;

                _userRepository.Add(newUser);
                await _unitOfWork.SaveAsync();
            }

            var existingUser = (_userRepository.GetAllWhen(u => u.UserName == userName)).FirstOrDefault();

            if (existingUser != null)
            {
                var screens = _screenRepository.GetAllWhen(s => true);

                foreach (var screen in screens)
                {
                    if (!(_userScreenRepository.GetAllWhen(us => us.UserId == existingUser.Id && us.ScreenId == screen.Id)).Any())
                    {
                        var userScreen = new UserScreen
                        {
                            UserId = existingUser.Id,
                            ScreenId = screen.Id
                        };

                        _userScreenRepository.Add(userScreen);
                    }
                }

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
