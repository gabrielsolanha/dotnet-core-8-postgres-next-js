using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Mapper.Interface
{
    public interface IMapper<T, S> where T : BaseDto
    {
        T MapperToDto(S entity);
        S MapperFromDto(T dto);
        S MapperFromDtoToUpdate(T dto, S currentValue);
    }
}
