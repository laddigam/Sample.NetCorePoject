using System;
using Westwind.Utilities;

namespace Joho.Web.API.Filters
{
    /// <summary>
    /// Used for validation errors or common operations that can have known negative responses such as a failed login attempt
    /// </summary>    
    public class ApiException : Exception
    {
        #region Variables
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public ValidationErrorCollection Errors { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of ApiException
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="statusCode">status code</param>
        /// <param name="errors">ValidationErrorCollection</param>
        public ApiException(string message,
                            int statusCode = 500,
                            ValidationErrorCollection errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        /// <summary>
        /// Constructor of ApiException
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="statusCode">status code</param>
        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor of ApiException
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="message">error message</param>
        /// <param name="statusCode">status code</param>
        /// <param name="errors">ValidationErrorCollection</param>
        public ApiException(Exception ex, string message,
                            int statusCode = 500,
                            ValidationErrorCollection errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        #endregion
    }
}
