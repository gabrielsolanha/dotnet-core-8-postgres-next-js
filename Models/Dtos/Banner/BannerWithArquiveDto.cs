namespace AplicacaoWeb.Models.Dtos.Banner
{
    public class BannerWithArquiveDto : BaseDto
    {
        public virtual string? Title { get; set; }
        public virtual string? Description { get; set; }
        public virtual string? RedirectLink { get; set; }
        public virtual bool IsMobile { get; set; }
        public virtual IFormFile? ImageFile { get; set; }
        public virtual FileStream? ImageFileStrem { get; set; }
    }


}
