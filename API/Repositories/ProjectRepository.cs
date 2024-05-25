using API.Model;
using API.Model.ParameterModels;
using Dapper;
using Npgsql;

namespace API.Repositories;

public class ProjectRepository
{
    private NpgsqlDataSource _dataSource;

    public ProjectRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }

    public IEnumerable<Project> GetAllProjects()
    {
        var sql = $@"
    SELECT
    p.project_id as {nameof(Project.ProjectId)},
    p.projectname as {nameof(Project.ProjectName)},
    p.description as {nameof(Project.Description)},
    p.created_by as {nameof(Project.CreatedBy)},
    p.created_at as {nameof(Project.CreatedAt)},
    u.email as {nameof(Project.Email)}
    FROM collabapp.projects p 
    JOIN collabapp.endusers u ON p.created_by = u.user_id;
    /*JOIN collabapp.users_in_project up ON p.project_id = up.project_id
    WHERE up.user_id = @user_Id;*/";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Project>(sql) ??
                   throw new KeyNotFoundException("No projects found");
        }
    }

    public Project InsertProject(InsertProjectParams insertProjectParams)
    {
        var sql = $@"
            INSERT INTO collabapp.projects (projectname, description, created_by)
            VALUES (@{nameof(InsertProjectParams.ProjectName)}, @{nameof(InsertProjectParams.Description)}, @{nameof(InsertProjectParams.CreatedBy)})
            RETURNING project_id as {nameof(Project.ProjectId)},
            projectname as {nameof(Project.ProjectName)},
            description as {nameof(Project.Description)},
            created_by as {nameof(Project.CreatedBy)},
            created_at as {nameof(Project.CreatedAt)};";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<Project>(sql, insertProjectParams)
                   ?? throw new InvalidOperationException("Insertion and retrieval failed");
        }
    }

    public bool DeleteProject(int projectId)
    {
        var sql = $@"
            DELETE FROM collabapp.projects WHERE project_id = @projectId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { projectId }) == 1;
        }
    }

    public Project GetProjectById(int projectId)
    {
        var sql = $@"
             SELECT
    p.project_id as {nameof(Project.ProjectId)},
    p.projectname as {nameof(Project.ProjectName)},
    p.description as {nameof(Project.Description)},
    p.created_by as {nameof(Project.CreatedBy)},
    p.created_at as {nameof(Project.CreatedAt)},
    u.email as {nameof(Project.Email)}
    FROM collabapp.projects p 
    JOIN collabapp.endusers u ON p.created_by = u.user_id
    WHERE p.project_id = @projectId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Project>(sql, new { projectId });
        }
    }
}