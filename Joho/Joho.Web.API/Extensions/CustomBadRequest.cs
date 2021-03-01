using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace Joho.Web.API.Extensions
{
    /// <summary>
    /// A custom class for handling bad requests
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ValidationProblemDetails" />
    public class CustomBadRequest : ValidationProblemDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBadRequest"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CustomBadRequest(ActionContext context)
        {
            Title = "Invalid arguments to the API";
            Detail = "The inputs supplied to the API are invalid";
            Status = 400;
            ConstructErrorMessages(context);
            Type = context.HttpContext.TraceIdentifier;
        }
        /// <summary>
        /// Constructs the error messages.
        /// </summary>
        /// <param name="context">The context.</param>
        private void ConstructErrorMessages(ActionContext context)
        {
            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = GetErrorMessage(errors[0]);
                        Errors.Add(key, new[] { errorMessage });
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = GetErrorMessage(errors[i]);
                        }
                        Errors.Add(key, errorMessages);
                    }
                }
            }
        }
        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        string GetErrorMessage(ModelError error)
        {
            try
            {
                bool showCustomMessage = false;
                var match = Regex.Match(error.ErrorMessage, @"^[ A-Za-z0-9_'@.#&+-]*$", RegexOptions.IgnoreCase);
                if (!string.IsNullOrEmpty(error.ErrorMessage) && !match.Success)
                    showCustomMessage = true;
                return (string.IsNullOrEmpty(error.ErrorMessage) || (showCustomMessage)) ?
                    "The input was not valid." :
                error.ErrorMessage;
            }
            catch
            { return error.ErrorMessage; }
        }
    }
}
