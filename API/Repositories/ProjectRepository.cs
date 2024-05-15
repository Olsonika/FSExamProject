﻿using API.Model;
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

    public IEnumerable<Project> GetAllProjectsForUser(int user_Id)
    {
        var sql = $@"
    SELECT
    p.project_id as {nameof(Project.ProjectId)},
    p.projectname as {nameof(Project.ProjectName)},
    p.description as {nameof(Project.Description)},
    p.created_by as {nameof(Project.CreatedBy)},
    p.created_at as {nameof(Project.CreatedAt)}
    FROM collabapp.projects p 
    JOIN collabapp.users_in_project up ON p.project_id = up.project_id
    WHERE up.user_id = @user_Id;";
    
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Project>(sql, new { user_Id }) ??
                   throw new KeyNotFoundException("No projects found for user with id " + user_Id);
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
}