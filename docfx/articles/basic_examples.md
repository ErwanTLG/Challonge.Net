---
uid: articles_basic_examples
---

# Basic examples

## Setting up the client
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

***

## Creating a tournament
Add the following using statement to the previous code:
```c#
using Challonge.Tournaments;
```

Now inside the `Main` method, add the following lines:
# [using TournamentBuilder](#tab/tournamentBuilder)
```c#
// We set the properties of the tournament we want to create
TournamentBuilder builder = new TournamentBuilder { Name = "MyTournament" };
// We then create the tournament using the properties we previously set
client.Tournaments.CreateTournamentAsync(builder).ConfigureAwait(false);
```

# [using optional method params](#tab/optionalParams)
```c#
// We create the tournament
client.Tournaments.CreateTournamentAsync("MyTournament").ConfigureAwait(false);
```

***  
  
> [!TIP]
> The methods provided by the`ChallongeClient` are all `async`.
> You can take advantage of this by using the `await` keyword before the method call.
> More information about this is available [here](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/).

This will create a tournament named _MyTournament_ on your Challonge! account.
You can further customize the created tournament using the 
[`TournamentBuilder`'s properties](xref:Challonge.Tournaments.TournamentBuilder)
or the optional parameters of the `CreateTournamentAsync` method.

Let's update some of the tournament's properties.

***  
  
## Updating a tournament
First of all, we need to retrieve the tournament we previously created. Change the last line of 
the code you added in the previous step : 
# [using TournamentBuilder](#tab/tournamentBuilder)
```c#
// Replace the last line of the previous step by this one
Tournament tournament = client.Tournaments.CreateTournamentAsync(builder).Result;
```

# [using optional method params](#tab/optionalParams)
```c#
// Replace the last line of the previous step by this one
Tournament tournament = client.Tournaments.CreateTournamentAsync("MyTournament").Result;
```

***  
  
Now we can update the created tournament. Let's change its name, type, and give it
a custom url:

Add the following lines to your code:
# [using TournamentBuilder](#tab/tournamentBuilder)
```c#
// We create a new builder based on the created tournament, and update its properties.
TournamentBuilder updater = new TournamentBuilder(tournament)
{ Name = "Updated tournament", Url = "new_url", TournamentType = TournamentType.DoubleElimination };
// Finally we update the tournament
client.Tournaments.UpdateTournamentAsync(tournament.Url, updater).ConfigureAwait(false);
```

# [using optional method params](#tab/optionalParams)
```c#
// We update the tournament
client.Tournaments.UpdateTournamentAsync(tournament.Url, name: "Updated tournament", url: "new_url", type: TournamentType.DoubleElimination).ConfigureAwait(false); 
```

***  
> [!TIP]
> As you can see, using the optional parameters results in fewer lines of code.
> However, we recommend using the TournamentBuilder instead, because it makes your
> easier to understand when using lots of parameters, and it might be more bug proof
> (both should have the same behaviour, but you never know).

## Working with matches and participants

The Matches and the Participants api work the same way as described here.
If you have some questions about anything related to Challonge.Net, please
visit [our discussions tab on GitHub](https://github.com/ErwanTLG/Challonge.Net/discussions).
