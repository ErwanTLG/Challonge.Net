using Challonge;
using Challonge.Tournaments;
using NUnit.Framework;

namespace Tests
{
    [SetUpFixture]
    public class Main
    {
        public static  ChallongeClient Client;
        
        [OneTimeSetUp]
        public void Init()
        {
            Client = new ChallongeClient("dj7kvUuSMSWeDl6R6tMuNtux0CXFmdEdQwMg5oOg");
        }

        [OneTimeTearDown]
        public void Clean()
        {
            Tournament[] tournaments = Client.Tournaments.GetTournamentsAsync().Result;
            foreach (Tournament tournament in tournaments)
            {
                if (tournament.Name.StartsWith("chalnet-tests-"))
                    Client.Tournaments.DeleteTournamentAsync(tournament.Url).Wait();
            }
        }
    }
}