using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Tasks
{
    public class CannotCreateTaskProjectNotFound : Exception
    {
        public CannotCreateTaskProjectNotFound() 
            : base("Cannot create task: Project not found in database") { }
    }
}
