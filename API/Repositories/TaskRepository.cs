using Npgsql;
using API.Model;
using Dapper;
using Task = API.Model.Task;

namespace API.Repositories;

public class TaskRepository
{
    private NpgsqlDataSource _dataSource;

    public TaskRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }

    public IEnumerable<Task> GetAllTasksForProject(int projectId)
    {
        var sql = $@"
            SELECT
            t.task_id as {nameof(Task.TaskId)},
            t.task_name as {nameof(Task.TaskName)},
            t.description as {nameof(Task.Description)},
            t.due_date as {nameof(Task.DueDate)},
            t.status as {nameof(Task.Status)},
            t.created_by as {nameof(Task.CreatedBy)},
            t.project_id as {nameof(Task.ProjectId)},
            t.created_at as {nameof(Task.CreatedAt)}
            FROM collabapp.tasks t
            JOIN collabapp.endusers u ON t.created_by = u.user_id;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Task>(sql, new {projectId}) ??
                   throw new KeyNotFoundException("No tasks found");

        }
    }
    
}