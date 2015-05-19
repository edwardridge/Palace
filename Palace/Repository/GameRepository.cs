using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Repository
{
    using Raven.Client;
    using Raven.Client.Document;

    public class SavedGame
    {
        public SavedGame()
        {
            
        }
        public SavedGame(Game game)
        {
            this.RulesProcessesor = game.RulesProcessesor;
            //this.Players = game.Players;
        }

        public RulesProcessesor RulesProcessesor { get; set; }

        //public int NumberOfPlayers { get; set; }
        public List<Player> Players { get; set; }
    }

    public class GameRepository
    {

        private IDocumentSession documentSession;
        public GameRepository(IDocumentSession documentSession)
        {
            
            this.documentSession = documentSession;

        }

        public void Save(Game game)
        {
            documentSession.Store(game);
            documentSession.Store(new SavedGame(game));
            //documentSession.Store(Card.AceOfClubs);
            documentSession.SaveChanges();
        }


        public Game Open(int p)
        {
            //var savedGame = documentSession
            //    .Include<SavedGame>(sg => sg.RulesProcessesor)
            //    .Include<SavedGame>(sg => sg.Players)
            //    .Load<SavedGame>("SavedGames/" + p);
            //return new Game(savedGame);
            ////return documentSession.Load<Game>(p);
            /// 
            Game game = documentSession
                
                .Include<Game>(g => g.PlayPile)
                .Include<Game>(g => g.Players)
                .Include<Game>(g => g.RulesProcessesor)
                .Include<Game>(g => g.CardDealer)
                .Include<Game>(g => g.OrderOfPlay)
                .Include<Game>(g=>g.CurrentPlayer)
                .Load<Game>("games/" + p);
            return game;
        }
    }
}
