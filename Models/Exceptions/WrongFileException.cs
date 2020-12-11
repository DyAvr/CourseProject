using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Models.Exceptions
{
    public class WrongFileException : Exception
    {
        public WrongFileException() : base("You have uploaded a file with an unsupported format.")
        {
        }
    }
}
