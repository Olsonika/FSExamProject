using API.Model;
using API.Model.ParameterModels;
using Dapper;
using Infrastructure.Model;
using Npgsql;

namespace API.Repositories;

public class ProjectRepository
{
    private NpgsqlDataSource _dataSource;
    
    public ProjectRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }

    public IEnumerable<Project> GetAllProjectsForUser(int userId)
    {
        var sql = $@"
        SELECT
        project_id as {nameof(Project.ProjectId)},
        project_name as {nameof(Project.Name)},
        description as {nameof(Project.Description)},
        created_by as {nameof(Project.CreatedBy)}
        created_at as {nameof(Project.CreatedAt)}
        FROM collabapp.projects p 
        JOIN collabapp.users_in_project up ON p.project_id = up.project_id
        WHERE up.user_id = @userId;";
        
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Project>(sql) ??
                   throw new KeyNotFoundException("No projects found for user with it" + userId);
        }
    }

    public Project InsertProject(InsertProjectParams insertProjectParams)
    {
        var sql = $@"
            INSERT INTO collabapp.projects (project_name, description, created_by)
            VALUES (@{nameof(InsertProjectParams.ProjectName)}, @{nameof(InsertProjectParams.Description)}, @{nameof(InsertProjectParams.CreatedBy)})
            RETURNING project_id as {nameof(Project.ProjectId)},
            project_name as {nameof(Project.Name)},
            description as {nameof(Project.Description)},
            created_by as {nameof(Project.CreatedBy)},
            created_at as {nameof(Project.CreatedAt)};";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<Project>(sql)
                   ?? throw new InvalidOperationException("Insertion and retrieval failed");
        }
    }
}