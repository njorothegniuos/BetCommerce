namespace Application.Token.Commands.CreateToken
{
    public sealed record TokenResponse(string accessToken, long expires, string tokenType);
}
