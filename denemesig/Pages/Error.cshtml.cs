using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace denemesig.Pages
{
    /// <summary>
    /// Error model.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>The request identifier.</value>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:denemesig.Pages.ErrorModel"/> show request identifier.
        /// </summary>
        /// <value><c>true</c> if show request identifier; otherwise, <c>false</c>.</value>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Ons the get.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
