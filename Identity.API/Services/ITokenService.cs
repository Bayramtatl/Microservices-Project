namespace Identity.API.Services
{
    public interface ITokenService
    {
        string CreateToken(string username);
    }
}
