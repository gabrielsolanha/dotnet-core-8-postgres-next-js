using AplicacaoWeb.Mapper.Interface;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Mapper
{
    public class UserMapper : IMapper<UserDto, User>
    {
        public User MapperFromDto(UserDto dto)
        {
            User user = new User() { 
                Id = dto.Id ?? 0,
                IsDeleted = dto.IsDeleted,
                UserName = dto.UserName,
                CallMeName = dto.CallMeName,
                Email = dto.Email,
                Telefone = dto.Telefone,                
            };
            return user;
        }

        public User MapperFromDtoToUpdate(UserDto dto, User currentValue)
        {
            User user = new User()
            {
                Id = currentValue.Id,
                IsDeleted = dto.IsDeleted,
                CreatedAt = currentValue.CreatedAt,
                CreatedBy = currentValue.CreatedBy,
                UserName = dto.UserName,
                CallMeName = dto.CallMeName,
                Email = dto.Email,
                Telefone = dto.Telefone,
            };
            return user;
        }

        public UserDto MapperToDto(User entity)
        {
            UserDto user = new UserDto()
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,
                IsDeleted = entity.IsDeleted,
                UserName = entity.UserName,
                CallMeName = entity.CallMeName,
                Email = entity.Email,
                Telefone = entity.Telefone,
            };
            return user;
        }
    }
}
