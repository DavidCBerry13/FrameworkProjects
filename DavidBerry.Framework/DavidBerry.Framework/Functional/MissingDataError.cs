using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{

    /// <summary>
    /// Error representing the case when a piece of data should exist but doesn't, typically indicating some sort
    /// of data load failure
    /// </summary>
    /// <remarks>
    /// The inspiration for this class is in the Portfolio Manager Securities API.  A service has checked that the
    /// trade date exists, the ticker exists and the trade date is in range the ticker has data for.  But what if there
    /// is no data?  What error should you return.  Having this error let's you pattern match and write an appropriate
    /// error to the log which is different than a simple Object Not Found because in this case, you have information
    /// that the data really should be there but isn't, so you want to respond differently
    /// </remarks>
    public class MissingDataError : Error
    {

        public MissingDataError(String message) : base(message)
        {

        }

    }
}
