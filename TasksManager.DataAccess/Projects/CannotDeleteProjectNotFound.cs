using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace TasksManager.DataAccess.Projects
{
    public class CannotDeleteProjectNotFound : Exception
    {
        public CannotDeleteProjectNotFound() 
            : base("Cannot delete project: Project not found in database") { }
    }
}
