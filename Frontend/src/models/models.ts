export class Project {
  ProjectId?: number;
  ProjectName?: string;
  Description?: string;
  CreatedBy?: string;
  CreatedAt?: Date;
  Email?: string;
}

export class Task {
  TaskId?: number;
  TaskName?: string;
  Description?: string;
  DueDate?: Date;
  Status?: string;
  CreatedBy?: string;
  ProjectId?: number;
  CreatedAt?: Date;
}
