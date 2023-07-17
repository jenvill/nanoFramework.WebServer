using System;
using System.Text;

namespace nanoFramework.WebServer.Results
{
    public class OkResult : BaseResult
    {
        public OkResult(string content) : base(content)
        {            
        }

        public override int StatusCode => 200;
    }
}
