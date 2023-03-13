using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string message) : base(message)
        {

        }
    }
}
