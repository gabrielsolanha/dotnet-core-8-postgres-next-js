using AplicacaoWeb.Models.Dtos.Banner;

namespace AplicacaoWeb.Aplication.Interfaces
{
    public interface IBannersApp : IApp<BannerDto>
    {
        Task<BannerDto> Add(BannerWithArquiveDto obj, string changeMaker);
        Task<BannerDto> Update(int id, BannerWithArquiveDto obj, string changeMaker);
        BannerWithArquiveDto GetComplete(int id);

        IEnumerable<BannerWithArquiveDto> ListAllCompleteToFront(bool isMobile);

    }
}
