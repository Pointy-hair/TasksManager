using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.DbImplementation.Tags
{
    public class CannotDeleteTagNotFound : Exception
    {
        public CannotDeleteTagNotFound() : base("Cannot delete tag: Tag not found in database") { }
    }
}
