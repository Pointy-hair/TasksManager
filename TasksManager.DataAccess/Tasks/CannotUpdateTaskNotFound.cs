using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Tasks
{
    public class CannotUpdateTaskNotFound : Exception
    {
        public CannotUpdateTaskNotFound() 
            : base("Cannot update task: Task not found in database") { }
    }
}
