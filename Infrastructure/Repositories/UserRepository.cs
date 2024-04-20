using Dapper;
using Infrastructure.Model;
using Infrastructure.Model.ParameterModels;
using Npgsql;

public class UserRepository
{
    private NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }
    
    public EndUser GetUser(FindByEmailParams findByEmailParams)
    {
        var sql = $@"
        SELECT
        email as {nameof(EndUser.email)},
        isbanned as {nameof(EndUser.isbanned)}, 
        id as {nameof(EndUser.id)},
        hash as {nameof(EndUser.hash)},
        salt as {nameof(EndUser.salt)},
        isadmin as {nameof(EndUser.isadmin)}
        from collabapp.enduser where email = @{nameof(FindByEmailParams.email)};";
        

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<EndUser>(sql);
        }
    }
}