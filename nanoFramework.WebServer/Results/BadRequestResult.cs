using System;
using System.Text;

namespace nanoFramework.WebServer.Results
{
    public class BadRequestResult : BaseResult
    {
        public BadRequestResult(string content) : base(content)
        {            
        }

        public override int StatusCode => 400;
    }
}
