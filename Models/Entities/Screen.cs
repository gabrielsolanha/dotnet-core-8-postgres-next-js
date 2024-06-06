namespace AplicacaoWeb.Models.Entities
{
    public class Screen:BaseEntity
    {
        public virtual string ScreenName { get; set; }
        public virtual string ScreenType { get; set; }
        public virtual string ScreenUrl { get; set; }
        public virtual IEnumerable<User> UsersAuthorized { get; set; }
    }
}
