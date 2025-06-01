namespace Eko.Auth.Jwt;

public class JwtOptions
{
    public const string Issuer = "https://localhost:5237";
    public const string Audience = "https://localhost:5237";
    public const string SecurityKey = "this_is_my_private_secret_key_fo_the_jwt_token";
    public const int ExpirationTime = 5;
}