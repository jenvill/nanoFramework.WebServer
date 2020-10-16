# nanoFrmaework WebServer

This is a simple nanoFrmaework WebServer. Features:

- Handle multithread requests
- Serve static files on any storage
- Handle parameter in URL
- Possible to have multiple WebServer running at the same time
- supports GET/PUT and any other word
- Supports any type of header
- Supports content in POST

Limitations:
- Does not support any zip way
- No URL decode implemented yet
- No helper yet to build a proper HTML answer easilly

## Usage

You just need to specify a port and a timeout for the querries and add an event handler when a request is incoming.

```csharp
using (WebServer server = new WebServer(80, TimeSpan.FromSeconds(2)))
{
    // Add a handler for commands that are received by the server.
    server.CommandReceived += ServerCommandReceived;

    // Start the server.
    server.Start();

    Thread.Sleep(Timeout.Infinite);
}
```

## Managing incoming querries

Very basic usage is the following:

```csharp
private static void ServerCommandReceived(object source, WebServer.WebServerEventArgs e)
{
    var url = e.RawURL;
    Debug.WriteLine("Command received:" + e.RawURL);

    if (url.ToLower() == "sayhello")
    {
        // This is simple raw text returned
        WebServer.OutPutStream(e.Response, "It's working, url is empty, this is just raw text, /sayhello is just returning a raw text");
    }
    else
    {
        WebServer.OutPutStream(e.Response, $"HTTP/1.1 404");
    }
}
```

You can do more advance scenario like returning a full HTML page:

```csharp
WebServer.OutPutStream(e.Response, "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nCache-Control: no-cache\r\nConnection: close\r\n\r\n<html><head>" +
    "<title>Hi from nanoFramework Server</title></head><body>You want me to say hello in a real HTML page!<br/><a href='/useinternal'>Generate an internal text.txt file</a><br />" +
    "<a href='/Text.txt'>Download the Text.txt file</a><br>" +
    "Try this url with parameters: <a href='/param.htm?param1=42&second=24&NAme=Ellerbach'>/param.htm?param1=42&second=24&NAme=Ellerbach</a></body></html>");
```

And can get parameters from a URL a an example from the previous link on the param.html page:

```csharp
if (url.ToLower().IndexOf("param.htm") == 0)
{
    // Test with parameters
    var parameters = WebServer.decryptParam(url);
    string toOutput = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nCache-Control: no-cache\r\nConnection: close\r\n\r\n<html><head>" +
        "<title>Hi from nanoFramework Server</title></head><body>Here are the parameters of this URL: <br />";
    foreach (var par in parameters)
    {
        toOutput += $"Parameter name: {par.Name}, Value: {par.Value}<br />";
    }
    toOutput += "</body></html>";
    WebServer.OutPutStream(e.Response, toOutput);
}
```

And server static files:

```csharp
var files = storage.GetFiles();
foreach (var file in files)
{
    if (file.Name == url)
    {
        WebServer.SendFileOverHTTP(e.Response, file);
        return;
    }
}

WebServer.OutPutStream(e.Response, $"HTTP/1.1 404");
```

And also **REST API** is supported, here is a comprehensive example:

```csharp
if (url.ToLower().IndexOf("api/") == 0)
{
    string ret = $"HTTP/1.1 200 OK\r\nContent-Type: text/plain; charset=UTF-8\r\nCache-Control: no-cache\r\nConnection: close\r\n\r\n";
    ret += $"Your request type is: {e.Method}\r\n";
    ret += $"The request URL is: {e.RawURL}\r\n";
    var parameters = WebServer.DecryptParam(e.RawURL);
    if (parameters != null)
    {
        ret += "List of url parameters:\r\n";
        foreach (var param in parameters)
        {
            ret += $"  Parameter name: {param.Name}, value: {param.Value}\r\n";
        }
    }

    if (e.Headers != null)
    {
        ret += $"Number of headers: {e.Headers.Length}\r\n";
    }
    else
    {
        ret += "There is no header in this request\r\n";
    }

    foreach (var head in e.Headers)
    {
        ret += $"  Header name: {head.Name}, header value: {head.Value}\r\n";
    }

    ret = WebServer.OutPutStream(e.Response, ret);

    if (e.Content != null)
    {
        ret += $"Size of content: {e.Content.Length}\r\n";
        if (e.Content.Length > 0)
        {
            ret += $"Hex string representation:\r\n";
            for (int i = 0; i < e.Content.Length; i++)
            {
                ret += e.Content[i].ToString("X") + " ";
            }
        }
    }

    WebServer.OutPutStream(e.Response, ret);
}
```

This API example is basic but as you get the method, you can choose what to do.

As you get the url, you can check for a specific controller called. And you have the parameters and the content payload!

Example of a result with call:

![result](./doc/POSTcapture.jpg)

And more! Check the complete example for more about this WebServer!