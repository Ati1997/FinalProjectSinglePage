using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResponseFramework
{
    public interface IResponse<TResult>
    {
        bool IsSuccessful { get; set; }
        string? Message { get; set; }
        string? ErrorMessage { get; set; }
        TResult? Result { get; set; }
        HttpStatusCode HttpStatusCode { get; set; }
    }
}
