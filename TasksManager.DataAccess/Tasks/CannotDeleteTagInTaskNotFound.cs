using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Tasks
{
    public class CannotDeleteTagInTaskNotFound : Exception
    {
        public CannotDeleteTagInTaskNotFound() 
            : base("Cannot delete tag in task: Tag not found in Task") { }
    }
}
