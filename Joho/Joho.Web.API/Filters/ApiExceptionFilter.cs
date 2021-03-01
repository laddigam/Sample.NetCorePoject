using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Serilog;


namespace Joho.Web.API.Filters
{
    /// <summary>
    /// Class to log and handle exceptions
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute" />
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        #region variables
        private ILogger<ApiExceptionFilter> _Logger;
     //   private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="stringLocalizer">The string localizer.</param>
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _Logger = logger;
        
        }
        #endregion

        #region public methods        
        /// <summary>
        /// Method to handle exception and log.
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;
            string Message = "";
          
            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message);//send customized message here for api result.
                Message = ex.Message;
                apiError.errors = ex.Errors;
                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;

                // handle logging here   
                Message = "Unauthorized Access!";
            }
            else
            {
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
                apiError = new ApiError(msg);//send customized message here for api result.
                apiError.detail = stack;
                context.HttpContext.Response.StatusCode = 500;

                // handle logging here
                Message = msg;
            }


            FormatResult formatResult = new FormatResult();
            formatResult.Result = null;
            formatResult.IsError = apiError.isError;
            formatResult.Error = apiError.message;
            formatResult.Detail = apiError.detail;

            // always return a JSON result


            ErrorResponse errorResponse = new ErrorResponse("InternalServerError");
            errorResponse.message = apiError.message;
            context.Result = new JsonResult(errorResponse);

            // logging to file

            Log.Logger.Error(context.Exception, Message);

            base.OnException(context);
        }
        #endregion
    }
}
