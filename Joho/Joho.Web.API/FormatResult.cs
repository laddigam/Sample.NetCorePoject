using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Joho.Web.API
{
    /// <summary>
    /// Class to format result of API
    /// </summary>
    public class FormatResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatResult"/> class.
        /// </summary>
        public FormatResult()
        {

        }
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public object Result { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether error occurred or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public bool IsError { get; set; } = false;
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public string Detail { get; set; }
    }
}
