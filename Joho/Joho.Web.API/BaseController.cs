
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Joho.Web.API.Filters;
using Joho.Web.API.Filters.JwtAuthentication;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Joho.Web.API
{
    /// <summary>
    /// Base controller for all controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        #region constructor
        /// <summary>
        /// Constructor of BaseController
        /// </summary>
        public BaseController() { }
        #endregion

      /* public virtual new int UserId
        {

            get
            {
                int userid = 0;
                var loggedInUser = HttpContext.User;
                if (loggedInUser.Identity.IsAuthenticated)
                {
                    string userxid = loggedInUser.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Name).Value; //Another way to get the name or any other claim we set. 
                    if (!string.IsNullOrEmpty(userxid))
                    {
                        userid = Convert.ToInt32(userxid);
                    }
                }
                return userid;
            }
        }*/
        /// <summary>
        /// Get logined user token
        /// </summary>
        public virtual new string AuthToken
        {

            get
            {
                string authtoken = string.Empty;
                var loggedInUser = HttpContext.User;
                if (loggedInUser.Identity.IsAuthenticated)
                {
                    string token = loggedInUser.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Authentication).Value; //Another way to get the name or any other claim we set. 
                    if (!string.IsNullOrEmpty(token))
                    {
                        authtoken = token;
                    }
                }
                return authtoken;
            }
        }
        /// <summary>
        /// Formats the results.
        /// </summary>
        /// <param name="Obj">The object.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="isError">if set to <c>true</c> [is error].</param>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected ActionResult FormatResults(object Obj, int statusCode, bool isError = false, string error = "", string message = "")
        {
            FormatResult formatResult = new FormatResult();
            formatResult.Result = Obj;
            formatResult.IsError = isError;
            formatResult.Error = error;
            formatResult.Detail = message;
            if (Obj.GetType().ToString().ToLower().Contains("exception"))
            {
                Exception exp = (Exception)Obj;
                Log.Logger.Error(exp, exp.Message);
                Exception ex = new CustomException(exp.Message);
                formatResult.Result = ex;
                if (!isError)
                    formatResult.IsError = true;
            }

            return StatusCode(statusCode, formatResult);
        }


        /// <summary>
        /// Format object for OK response.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected ObjectResult ObjectOk(object result)
        {
            SuccessResponse response = new SuccessResponse("Ok");
            response.result = result;
            return Ok(response);
        }
        /// <summary>
        /// Format object for OK response with collections result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected ObjectResult CollectionResultOk<T>(IList<T> result)
        {
            SuccessListResponse response = new SuccessListResponse("Ok");
            response.result = result;
            response.resultCount = result.Count();
            return Ok(response);
        }
        /// <summary>
        ///  Format object for Created ok response.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected ObjectResult ObjectCreatedOk(object result)
        {
            SuccessResponse createdResponse = new SuccessResponse("Created");
            createdResponse.result = result;
            return StatusCode(StatusCodes.Status201Created, createdResponse);
        }

        /// <summary>
        /// Objects the update ok.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected ObjectResult ObjectUpdateOk(object result)
        {
            SuccessResponse createdResponse = new SuccessResponse("Updated")
            {
                result = result
            };
            return StatusCode((int)result > 0 ? StatusCodes.Status200OK : StatusCodes.Status204NoContent, createdResponse);
        }
        /// <summary>
        /// Format object for Deleted ok response.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected ObjectResult ObjectDeletedOk(object result)
        {
            SuccessResponse createdResponse = new SuccessResponse("Deleted");
            createdResponse.result = result;
            return Ok(createdResponse);
        }
        /// <summary>
        /// Format object for Not found response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected ObjectResult ObjectNotFound(string message)
        {
            ErrorResponse createdResponse = new ErrorResponse("NotFound");
            createdResponse.message = message;
            return StatusCode(StatusCodes.Status404NotFound, createdResponse);
        }
        /// <summary>
        /// Format object for paginated collections response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        //protected ObjectResult CollectionPaginatedResultOk<T>(PagedList<T> result)
        //{

        //    SuccessPaginatedResponse response = new SuccessPaginatedResponse("Ok");
        //    response.result = result;
        //    response.resultCount = result != null ? result.Count() : 0;

        //    PaginationParameters Pagination = new PaginationParameters();
        //    Pagination.TotalCount = result.TotalCount;
        //    Pagination.PageSize = result.PageSize;
        //    Pagination.CurrentPage = result.CurrentPage;
        //    Pagination.TotalPages = result.TotalPages;
        //    Pagination.HasPrevious = result.HasPrevious;
        //    Pagination.HasNext = result.HasNext;
        //    response.Pagination = Pagination;


        //    return Ok(response);
        //}
    }

    #region Classes to format response
    /// <summary>
    /// Class for generating custom response
    /// </summary>
    public abstract class CustomResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public CustomResponse(string statusCode)
        {
            this.statusCode = statusCode;
        }
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public string statusCode { get; set; }
    }

    /// <summary>
    /// Class for generating custom success response
    /// </summary>
    /// <seealso cref="JohoWebApi.CustomResponse" />
    public class SuccessResponse : CustomResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public SuccessResponse(string statusCode) : base(statusCode)
        {

        }
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public object result { get; set; }
    }
    /// <summary>
    /// Class for generating custom error response
    /// </summary>
    /// <seealso cref="JohoWebApi.CustomResponse" />
    public class ErrorResponse : CustomResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public ErrorResponse(string statusCode) : base(statusCode)
        {

        }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public object message { get; set; }
    }

    /// <summary>
    /// Class for generating custom success list response
    /// </summary>
    /// <seealso cref="JohoWebApi.SuccessResponse" />
    public class SuccessListResponse : SuccessResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessListResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public SuccessListResponse(string statusCode) : base(statusCode)
        {

        }
        /// <summary>
        /// Gets or sets the result count.
        /// </summary>
        /// <value>
        /// The result count.
        /// </value>
        public int resultCount { get; set; }
    }
    /// <summary>
    /// Class for generating custom success paginated response
    /// </summary>
    /// <seealso cref="JohoWebApi.SuccessListResponse" />
    public class SuccessPaginatedResponse : SuccessListResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessPaginatedResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public SuccessPaginatedResponse(string statusCode) : base(statusCode)
        {

        }
        /// <summary>
        /// Gets or sets the pagination.
        /// </summary>
        /// <value>
        /// The pagination.
        /// </value>
        public PaginationParameters Pagination { get; set; }
    }
    #endregion  Classes to format response

    /// <summary>
    /// Class for paging implementation
    /// </summary>
    public class PaginationParameters
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has previous.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has previous; otherwise, <c>false</c>.
        /// </value>
        public bool HasPrevious { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has next.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has next; otherwise, <c>false</c>.
        /// </value>
        public bool HasNext { get; set; }

    }

    /// <summary>
    /// A custom class for handling Exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class CustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        public CustomException()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CustomException(string message)
            : base(message)
        {

        }
    }


}
