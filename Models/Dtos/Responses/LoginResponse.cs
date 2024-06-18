namespace AplicacaoWeb.Models.Dtos.Responses
{
    public class LoginResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public List<ViewResponse>? Views { get; set; }
    }

}
