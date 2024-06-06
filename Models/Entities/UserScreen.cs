namespace AplicacaoWeb.Models.Entities
{
    public class UserScreen
    {
        public virtual int UserId { get; set; }
        public virtual User Users { get; set; }
        public virtual int ScreenId { get; set; }
        public virtual Screen Screens { get; set; }
    }
}
