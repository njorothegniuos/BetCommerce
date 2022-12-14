namespace Application.Client.Queries.AuthenticateClient
{
    public sealed record TokenRequest(Guid ApiKey, string AppSecret);
}
