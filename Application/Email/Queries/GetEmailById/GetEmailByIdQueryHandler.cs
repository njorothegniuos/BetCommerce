using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exception;
using System.Data;

namespace Application.Email.Queries.GetEmailById
{
    internal sealed class GetEmailByIdQueryHandler : IQueryHandler<GetEmailByIdQuery, EmailResponse>
    {
        private readonly IDbConnection _dbConnection;

        public GetEmailByIdQueryHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<EmailResponse> Handle(
            GetEmailByIdQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"SELECT * FROM ""EmailAlerts"" WHERE ""Id"" = @emailId";

            var email = await _dbConnection.QueryFirstOrDefaultAsync<EmailResponse>(
                sql,
                new { request.emailId });

            if (email is null)
            {
                throw new EmailAlertNotFoundException(request.emailId);
            }

            return email;
        }
    }
}
