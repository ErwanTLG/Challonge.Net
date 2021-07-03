---
uid: articles_first_steps
---

# First Steps

Every interaction with the Challonge! API happens through the [`ChallongeClient`](xref:Challonge.ChallongeClient).
You have to initialize it using the api key you obtained in the [previous section](xref:articles_project_setup).
You can find it under the [`Challonge`](xref:Challonge) namespace
```c#
using Challonge;

class Program
{
    static void Main(string[] args)
    {
        ChallongeClient client = new ChallongeClient("YOUR API KEY HERE");
    }
}
```

> [!WARNING]
> As explained previously, your api key is like your password. You shouldn't expose it like this in your
> code, your should rather use an external configuration file or environment variables.

The code above creates a new [`ChallongeClient`](xref:Challonge.ChallongeClient). By default, it is instantiated with a new
[`HttpClient`](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0) class.

## Using the Api
The api is divided into 4 parts:
- [Tournaments](xref:Challonge.ChallongeClient.TournamentHandler)
- [Participants](xref:Challonge.ChallongeClient.ParticipantsHandler)
- [Matches](xref:Challonge.ChallongeClient.MatchesHandler)
- [Attachments](xref:Challonge.ChallongeClient.AttachmentsHandler)

> [!NOTE]
> The Attachments api is incomplete due to server-side issues
