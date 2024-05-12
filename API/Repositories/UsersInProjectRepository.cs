using Dapper;
using Npgsql;

namespace API.Repositories;

public class UsersInProjectRepository
{
    private NpgsqlDataSource _dataSource;
    
    public UsersInProjectRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }

    public void InsertUsersInProject(int userId, int projectId)
    {
        var sql = @"
        INSERT INTO collabapp.users_in_project (user_id, project_id)
        VALUES (@userId, @projectId);";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sql, new { userId, projectId });
        }
    }
}