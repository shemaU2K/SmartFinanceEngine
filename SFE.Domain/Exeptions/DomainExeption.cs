using System;
using System.Collections.Generic;
using System.Text;

namespace SFE.Domain.Exeptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
