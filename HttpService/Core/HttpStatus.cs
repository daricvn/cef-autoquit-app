using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Core
{
    public enum HttpStatus
    {
        SUCCESS=200,
        NO_CONTENT=204,
        NOT_MODIFIED=304,
        BAD_REQUEST=400,
        UNAUTHORIZED=401,
        FORBIDDEN=403,
        NOT_FOUND=404
    }
}
