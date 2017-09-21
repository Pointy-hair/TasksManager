using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Tasks
{
    public class CannotDeleteTaskNotFound : Exception
    {
        public CannotDeleteTaskNotFound() 
            : base("Cannot delete task: Task not found in database") { }
    }
}
