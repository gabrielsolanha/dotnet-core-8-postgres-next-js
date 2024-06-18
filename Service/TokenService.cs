using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AplicacaoWeb.Domain;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private static string Secret = (new Key()).GetSecret();

    public string GenerateToken(UserDto user)
    {
        var key = Encoding.ASCII.GetBytes(Secret);
        var tokenConfig = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public string GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.First(claim => claim.Type == "UserId");
            return userIdClaim.Value;
        }
        catch
        {
            return null;
        }
    }
}
