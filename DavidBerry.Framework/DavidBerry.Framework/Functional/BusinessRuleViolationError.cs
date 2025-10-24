using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Functional
{
    /// <summary>
    /// Represents an error where a business rule has been violated
    /// </summary>
    public class BusinessRuleViolationError : Error
    {

        public BusinessRuleViolationError(string message) : base(message)
        {
        }
    }
}
