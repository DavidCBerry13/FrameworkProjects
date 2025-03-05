using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.ApiUtil.Models
{

    /// <summary>
    /// Model class to send error messages back to the client
    /// </summary>
    /// <remarks>
    /// This class expands on the ApiMessageModel object to also include an error code string in the model
    /// that can help the client differentiate the type of error that occurred.  This class can either be
    /// used directly or more likely, you will want to extend this class with your own custom error model
    /// classes that contain additional information about the error
    /// </remarks>
    public class ApiErrorMessageModel : ApiMessageModel
    {

        public string ErrorCode { get; set; }


    }
}
