using System;
using System.Text;

namespace nanoFramework.WebServer.Results
{
    public abstract class BaseResult
    {
        public BaseResult(string content) => Content = content;
        public abstract int StatusCode { get; }
        public string Content { get; }
    }
}
