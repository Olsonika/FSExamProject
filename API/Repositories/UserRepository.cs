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
        email as {nameof(EndUser.Email)},
        id as {nameof(EndUser.UserId)},
        hash as {nameof(EndUser.Hash)},
        salt as {nameof(EndUser.Salt)},
        isadmin as {nameof(EndUser.Isadmin)}
        from collabapp.enduser where email = @{nameof(FindByEmailParams.email)};";
        

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<EndUser>(sql, findByEmailParams) ??
                   throw new KeyNotFoundException("There is no user with email:" + findByEmailParams.email);;
        }
    }
    
    public bool DoesUserAlreadyExist(FindByEmailParams findByEmailParams)
    {
        var sql = $@"
            SELECT COUNT(*) FROM collabapp.enduser WHERE email = @{nameof(findByEmailParams.email)}";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.ExecuteScalar<int>(sql, findByEmailParams) == 1;
        }
    }
    
    public EndUser InsertUser(InsertUserParams insertUserParams)
    {

        var sql = $@"
            INSERT INTO collabapp.enduser (email, hash, salt, isadmin)
            VALUES (@{nameof(InsertUserParams.Email)}, @{nameof(InsertUserParams.Hash)}, @{nameof(InsertUserParams.Salt)}, false)
            RETURNING email as {nameof(EndUser.Email)}, 
            isadmin as {nameof(EndUser.Isadmin)},
            id as {nameof(EndUser.UserId)};";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<EndUser>(sql, insertUserParams)
                   ?? throw new InvalidOperationException("Insertion and retrieval failed");
        }
    }
}