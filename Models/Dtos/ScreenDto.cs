namespace AplicacaoWeb.Models.Dtos
{
    public class ScreenDto : BaseDto
    {
        public virtual string? ScreenName { get; set; }
        public virtual string? ScreenType { get; set; }
        public virtual string? ScreenUrl { get; set; }
        public virtual IEnumerable<int> IdsUsersAuthorized { get; set; }
    }
}
