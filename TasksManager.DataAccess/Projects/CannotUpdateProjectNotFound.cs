using System;

namespace TasksManager.DataAccess.Projects
{
    public class CannotUpdateProjectNotFound : Exception
    {
        public CannotUpdateProjectNotFound() 
            : base("Cannot update project: Project not found in database") { }
    }
}
