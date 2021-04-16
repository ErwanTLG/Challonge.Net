# Challonge.Net
Challonge.Net is an unofficial wrapper for the [Challonge!](https://challonge.com/) [API](https://api.challonge.com/v1).

It uses modern async/await non blocking code.

# Quick start
To start, you will need to create a ChallongeClient class. This class will allow you to make the api calls.
```csharp
using Challonge;

class Program
{
    static void Main(string[] args)
    {
        ChallongeClient client = new ChallongeClient("YOUR API KEY HERE");
    }
}
```

By default, the ChallongeClient class is instantiated with a new [HttpClient class](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0).
You can pass an HttpClient as an argument to the ChallongeClient constructor to avoid the creation of a new HttpClient.
