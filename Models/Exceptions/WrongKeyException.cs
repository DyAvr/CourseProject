using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Models.Exceptions
{
    public class WrongKeyException : Exception
    {
        public WrongKeyException() : base("Incorrect key.")
        {
        }
    }
}
