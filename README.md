[![Challonge.Net on nuget.org](https://img.shields.io/nuget/vpre/Challonge.Net)](https://www.nuget.org/packages/Challonge.Net)
[![Challonge.Net on fuget.org](https://www.fuget.org/packages/Challonge.Net/badge.svg)](https://www.fuget.org/packages/Challonge.Net)

**THIS PACKAGE IS NO LONGER ACTIVELY MAINTAINED. YOU SHOULD USE [Challonge-DotNet](https://github.com/jacobhood/Challonge-DotNet) INSTEAD.**

# Challonge.Net
Challonge.Net is an unofficial wrapper for the [Challonge!](https://challonge.com/) [API](https://api.challonge.com/v1).

It uses modern async/await non blocking code.

The documentation of this project is available [here](https://erwantlg.github.io/Challonge.Net/index.html) 

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

# State of the project
Currently, the project is available as a pre-release. It is not suited for production, because it may contain bugs and it is currently not well documented.

So what's left to do before the first stable release is to:
- Extensively test the entire project (using a testing framework)
- Document the project, at least the public methods and properties.
