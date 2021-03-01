using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Westwind.Utilities;

namespace Joho.Web.API.Filters
{
    /// <summary>
    /// Used for Exceptions that are not good for serialization because of the sensitive data and complex structure
    /// </summary>
    public class ApiError
    {
        #region Variables
        /// <summary>
        /// Gets or sets the API error message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string message { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether api error has been occurred or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public bool isError { get; set; }
        /// <summary>
        /// Gets or sets the error detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public string detail { get; set; }
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public ValidationErrorCollection errors { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of ApiError
        /// </summary>
        /// <param name="message">error message</param>
        public ApiError(string message)
        {
            this.message = message;
            isError = true;
        }

        /// <summary>
        /// Constructor for ApiError
        /// </summary>
        /// <param name="modelState">ModelStateDictionary</param>
        public ApiError(ModelStateDictionary modelState)
        {
            this.isError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                message = "Please correct the specified errors and try again.";
            }
        }
        #endregion
    }
}
