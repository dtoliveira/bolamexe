﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopSunday.Models;
using TopSunday.SK;
using TopSunday.SpecificObjects;
using TopSunday.ViewModels;

namespace TopSunday.Controllers
{
    public class GameController : SKController
    {
        string seasonValue = "2018/2019";

        public ActionResult Classification(string gameType)
        {
            GamesViewModel GamesViewModel = new GamesViewModel();

            List<Classification> classification = GetClassification(seasonValue, gameType);
            GamesViewModel.Classification = BuildClassification(classification);
            return View(GamesViewModel);
        }


        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult AddPlayer(GamesViewModel viewModel)
        {
            if (viewModel.NewPlayer != null)
            {
                InsertPlayer(viewModel.NewPlayer);
            }

            return RedirectToAction("Resume", new { @season = viewModel.Season, @gameType = viewModel.GameType });
        }


        public List<PlayersToGame> GetFinalTeams(string gameType)
        {
            List<PlayersToGame> lastTeams = new List<PlayersToGame>();
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                var currentGame = ctx.CurrentGame.Include("GameType")
                    .Where(c => c.WasOpen.Equals(true))
                    .Where(c => c.GameType.Description.Equals(gameType));

                if (currentGame != null && currentGame.Count() > 0)
                {
                    List<int> lastIdPlayersTeams = GetlastPlayersPreMatch(gameType, ctx);

                    GetPreMatchTeamsWithClassification(gameType, lastTeams, ctx, lastIdPlayersTeams);

                }
                ctx.Dispose();
            }

            return (lastTeams != null && lastTeams.Count() > 0) ? lastTeams : null;
        }

        private static void GetPreMatchTeamsWithClassification(string gameType, List<PlayersToGame> lastTeams, ApplicationDbContext ctx, List<int> lastIdPlayersTeams)
        {
            if (lastIdPlayersTeams != null && lastIdPlayersTeams.Count() > 0)
            {

                List<Player> players = ctx.Player.Include("Classification")
                    .Where(c => lastIdPlayersTeams.Contains(c.ID)).ToList<Player>();

                List<Classification> playersClassification = new List<Classification>();
                List<Classification> auxClassifications = new List<Classification>();

                if (players != null && players.Count() > 9)
                {
                    foreach (Player p in players)
                    {
                        auxClassifications = p.Classification.Where(c => c.GameType.Description.Equals(gameType)).ToList<Classification>();

                        if (auxClassifications != null)
                        {
                            playersClassification.Add(auxClassifications.FirstOrDefault());

                        }
                    }
                }

                if (playersClassification != null && playersClassification.Count() > 0)
                {
                    foreach (Classification c in playersClassification)
                    {
                        if (c!=null)
                        {

                        var isPlayerSubstitute = ctx.Players_GameType.Include("GameType")
                            .Where(a => a.GameType.Description.Equals(gameType))
                            .Where(a => lastIdPlayersTeams.Contains(a.PlayerID)).FirstOrDefault().IsSubstitute;


                        lastTeams.Add(new PlayersToGame
                        {
                            PlayerID = c.PlayerID,
                            PlayerName = c.Player.Name,
                            Points = c.TotalPoints,
                            Goals = c.Goals,
                            NumGames = c.NumGames,
                            IsSubstitute = isPlayerSubstitute
                        });
                        }

                    }
                }
            }
        }

        private static List<int> GetlastPlayersPreMatch(string gameType, ApplicationDbContext ctx)
        {
            var gameTeamsByPlayer = ctx.Player.Include("Classification").Include("GameTeams").ToList<Player>();

            List<int> lastIdPlayersTeams = new List<int>();

            List<GameTeams> gt = ctx.GameTeams.Include("GameType")
                                  .Where(p => p.SeasonID == 1)
                                  .Where(p => p.GameType.Description.Equals(gameType))
                                  .OrderByDescending(p => p.GameDate).Take(10).ToList<GameTeams>();

            if (gt != null)
            {
                gt.ForEach(
                    x => lastIdPlayersTeams.Add(x.PlayerID)
                    );

            }

            return lastIdPlayersTeams;
        }

        public ActionResult SetFinalteams(GamesViewModel viewModel)
        {
            List<PlayerConfirmation> tenPlayers = viewModel.PlayerConfirmations.Where(p => p.GoToGame.Equals(true)).ToList<PlayerConfirmation>();
            List<PlayersToGame> classificationPlayers = AddTeams(tenPlayers, viewModel.GameType);
            List<PlayersToGame> goalsTeamA = new List<PlayersToGame>();
            List<PlayersToGame> goalsTeamB = new List<PlayersToGame>();


            if (classificationPlayers != null && classificationPlayers.Count() > 0)
            {
                classificationPlayers = classificationPlayers
                    .OrderByDescending(p => p.Points)
                    .ThenByDescending(p => p.Goals)
                    //.ThenBy(p => p.IsSubstitute)
                    .ThenByDescending(p => p.NumGames)
                    .ThenBy(p => p.PlayerName)
                    .ToList<PlayersToGame>();

                goalsTeamA = BuildTeamGoals(classificationPlayers, 'A');
                goalsTeamB = BuildTeamGoals(classificationPlayers, 'B');

                SaveTeams(classificationPlayers, 1, viewModel.GameType);
                viewModel.LinkTeamA = BuildTeam(classificationPlayers, 'A');
                viewModel.GoalsTeamA = goalsTeamA;
                viewModel.GoalsTeamB = goalsTeamB;

                viewModel.LinkTeamB = BuildTeam(classificationPlayers, 'B');
                viewModel.HasTeams = true;


                ClosePresencesInGame(viewModel.GameType);

                TempData["Game"] = viewModel;

            }

            return RedirectToAction("Resume", new { @season = viewModel.Season, @gameType = viewModel.GameType });
        }

        private List<PlayersToGame> BuildTeamGoals(List<PlayersToGame> classificationPlayers, char typeTeam)
        {
            //Set goals to 0

            foreach (PlayersToGame item in classificationPlayers)
            {
                item.Goals = 0;
            }

            List<PlayersToGame> team = new List<PlayersToGame>();
            switch (typeTeam.ToString())
            {
                case "A":

                    team.Add(classificationPlayers[0]);
                    team.Add(classificationPlayers[3]);
                    team.Add(classificationPlayers[5]);
                    team.Add(classificationPlayers[6]);
                    team.Add(classificationPlayers[9]);
                    break;
                case "B":
                    team.Add(classificationPlayers[1]);
                    team.Add(classificationPlayers[2]);
                    team.Add(classificationPlayers[4]);
                    team.Add(classificationPlayers[7]);
                    team.Add(classificationPlayers[8]);
                    break;
            }

            return team;
        }

        private void SaveTeams(List<PlayersToGame> classificationPlayers, int season, string gameType)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                int gameTypeID = ctx.GameType.Where(c => c.Description.Equals(gameType)).FirstOrDefault().GameTypeID;
                foreach (PlayersToGame player in classificationPlayers)
                {

                    ctx.GameTeams.Add(new GameTeams
                    {
                        PlayerID = player.PlayerID,
                        SeasonID = season,
                        GameTypeID = gameTypeID,
                        GameDate = DateTime.Now

                    });
                }

                ctx.SaveChanges();
                ctx.Dispose();
            }
        }

        private string BuildTeam(List<PlayersToGame> classificationPlayers, char typeTeam)
        {
            string principalURL_teamA = "http://lineupbuilder.com/2014/custom/354x526.php?p=5&amp;a=1&amp;t=Equipa%20A&amp;c=dc0000&amp;";
            string principalURL_teamB = "http://lineupbuilder.com/2014/custom/354x526.php?p=5&amp;a=1&amp;t=Equipa%20B&amp;c=0f00db&amp;";

            string formatTeamA = "1=GK_{0}%201_1_388_174&amp;2=ML_{1}%207_7_189_64&amp;3=DM_{2}%2010_10_267_180&amp;4=FC_{3}%209_9_75_173&amp;5=MR_{4}%2020_20_175_296&amp;6=MLA___204_64&amp;7=MCL___222_138&amp;8=MCR___222_211&amp;9=MRA___204_284&amp;10=FCL___98_138&amp;11=FCR___98_211&amp;c2=ffffff&amp;c3=ffffff&amp;output=embed";
            string formatTeamB = "1=GK_{0}%201_1_388_174&amp;2=ML_{1}%207_7_189_64&amp;3=DM_{2}%204_4_267_180&amp;4=FC_{3}%2010_10_75_173&amp;5=MR_{4}%206_6_175_296&amp;6=MLA___204_64&amp;7=MCL___222_138&amp;8=MCR___222_211&amp;9=MRA___204_284&amp;10=FCL___98_138&amp;11=FCR___98_211&amp;c2=ffffff&amp;c3=ffffff&amp;output=embed";

            string team = string.Empty;

            switch (typeTeam.ToString())
            {
                case "A":
                    team = string.Format(principalURL_teamA + formatTeamA,
                classificationPlayers[0].PlayerName,
                classificationPlayers[3].PlayerName,
                classificationPlayers[5].PlayerName,
                classificationPlayers[6].PlayerName,
                classificationPlayers[9].PlayerName);
                    break;
                case "B":
                    team = string.Format(principalURL_teamB + formatTeamB,
                  classificationPlayers[1].PlayerName,
                  classificationPlayers[2].PlayerName,
                  classificationPlayers[4].PlayerName,
                  classificationPlayers[7].PlayerName,
                  classificationPlayers[8].PlayerName);
                    break;
            }

            return team;

        }

        public ActionResult InsertGoals(GamesViewModel playerGoals)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                BuildGameResult(playerGoals.GoalsTeamA, playerGoals.GoalsTeamB, playerGoals.GameType, ctx);

                UpdatePlayerData(playerGoals.GoalsTeamA, playerGoals.GameType, ctx);
                UpdatePlayerData(playerGoals.GoalsTeamB, playerGoals.GameType, ctx);
                SaveResults(playerGoals.GoalsTeamA, playerGoals.GameType, ctx);
                SaveResults(playerGoals.GoalsTeamB, playerGoals.GameType, ctx);

                CloseGame(ctx, playerGoals.GameType);


                ctx.SaveChanges();
            }

            return RedirectToAction("Resume", new { @season = playerGoals.Season, @gameType = playerGoals.GameType });
        }

        private void BuildGameResult(List<PlayersToGame> goalsTeamA, List<PlayersToGame> goalsTeamB, string gameType, ApplicationDbContext ctx)
        {
            int goalsA = goalsTeamA.Sum(g => g.Goals);
            int goalsB = goalsTeamB.Sum(g => g.Goals);

            if (goalsA > goalsB) { foreach (PlayersToGame p in goalsTeamA) { p.Win = true; } foreach (PlayersToGame p in goalsTeamB) { p.Lose = true; } }
            if (goalsB > goalsA) { foreach (PlayersToGame p in goalsTeamB) { p.Win = true; } foreach (PlayersToGame p in goalsTeamA) { p.Lose = true; } }
            if (goalsB == goalsA) { foreach (PlayersToGame p in goalsTeamB) { p.Draw = true; } foreach (PlayersToGame p in goalsTeamA) { p.Draw = true; } }
        }

        private void CloseGame(ApplicationDbContext ctx, string gameType)
        {
            CurrentGame cg = ctx.CurrentGame
                .Where(c => c.GameType.Description.Equals(gameType))
                .Where(c => c.WasOpen.Equals(true)).FirstOrDefault();

            if (cg != null)
            {
                ctx.CurrentGame.Remove(cg);
            }

        }

        private void SaveResults(List<PlayersToGame> playersInfo, string gameType, ApplicationDbContext ctx)
        {
            int gameTypeID = ctx.GameType.Where(g => g.Description.Equals(gameType)).FirstOrDefault().GameTypeID;
            DateTime gameDate = ctx.CurrentGame.Where(c => c.GameTypeID.Equals(gameTypeID)).FirstOrDefault().GameDate;

            foreach (PlayersToGame item in playersInfo)
            {
                GameTeams gt = ctx.GameTeams
                    .Where(g => g.GameType.Description.Equals(gameType))
                    .Where(g => g.PlayerID.Equals(item.PlayerID)).OrderByDescending(g => g.GameDate).FirstOrDefault();

                if (gt != null)
                {
                    if (item.Draw) gt.FinalResult = "E";
                    if (item.Win) gt.FinalResult = "V";
                    if (item.Lose) gt.FinalResult = "D";

                    gt.Goals = item.Goals;
                    gt.GameDate = new DateTime(gameDate.Year, gameDate.Month, gameDate.Day, 23, 25, 00);
                    // gt.OwnGoals = item.OwnGoals;
                }
            }
        }

        private void UpdatePlayerData(List<PlayersToGame> playersInfo, string gameType, ApplicationDbContext ctx)
        {
            //inserir jogos golos vitorias derrotas empates na classificacao
            foreach (PlayersToGame item in playersInfo)
            {
                Classification cls = ctx.Classification
                        .Where(c => c.GameType.Description.Equals(gameType))
                        .Where(c => c.PlayerID.Equals(item.PlayerID)).FirstOrDefault();

                if (cls != null)
                {
                    cls.NumGames += 1;
                    cls.Goals += item.Goals;

                    if (item.Draw)
                    {
                        cls.Draws += 1;
                        cls.TotalPoints += 2;
                    }

                    if (item.Win)
                    {
                        cls.Wins += 1;
                        cls.TotalPoints += 4;
                    }

                    if (item.Lose)
                    {
                        cls.Loses += 1;
                        cls.TotalPoints += 1;

                    }

                }
                //update na tabela gameteams com golos data do jogo e vitoria/empate/derrota
            }
        }

        public List<PlayersToGame> AddTeams(List<PlayerConfirmation> tenPlayers, string gameType)
        {
            List<PlayersToGame> classificationPlayers = new List<PlayersToGame>();
            Player player = null;
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                foreach (PlayerConfirmation item in tenPlayers)
                {
                    player = ctx.Player.Where(p => p.ID.Equals(item.PlayerID)).FirstOrDefault();

                    if (item.IsSubstitute.Equals(true))
                    {

                        if (player != null)
                        {
                            player.Debit += 1;
                            ctx.SaveChanges();
                        }
                    }

                    Classification cls = ctx.Classification.Include("GameType").Include("Season").Include("Player")
                        .Where(p => p.SeasonID.Equals(1))
                        .Where(p => p.GameType.Description.Equals(gameType))
                        .Where(p => p.PlayerID.Equals(item.PlayerID)).FirstOrDefault();

                    if (cls != null)
                    {
                        classificationPlayers.Add(new PlayersToGame
                        {
                            PlayerID = cls.PlayerID,
                            PlayerName = cls.Player.Name,
                            Points = cls.TotalPoints,
                            Goals = cls.Goals,
                            NumGames = cls.NumGames,
                            IsSubstitute = ctx.Players_GameType.Where(p => p.PlayerID.Equals(cls.PlayerID)).FirstOrDefault().IsSubstitute
                        });
                    }
                    else
                    {
                        classificationPlayers.Add(new PlayersToGame
                        {
                            PlayerID = player.ID,
                            PlayerName = player.Name,
                            Points = 0,
                            Goals = 0,
                            NumGames = 0,
                            IsSubstitute = ctx.Players_GameType.Where(p => p.PlayerID.Equals(player.ID)).FirstOrDefault().IsSubstitute
                        });
                    }


                }

                ctx.Dispose();
            }

            return classificationPlayers;
        }

        public void InsertPlayer(PlayerViewModel player)
        {
            int playerID;

            if (player != null)
            {
                using (ApplicationDbContext ctx = new ApplicationDbContext())
                {
                    Player addPlayer = InsertNewPlayer(player, ctx);
                    playerID = addPlayer.ID;
                    InsertPlayerInClassification(player, playerID, ctx);
                    ctx.Dispose();
                }
            }


        }

        private static void InsertPlayerInClassification(PlayerViewModel player, int playerID, ApplicationDbContext ctx)
        {
            if (player.Wednesday.Equals(true))
            {
                AddPlayerToClassifications(player, playerID, ctx, 2);
            }

            if (player.Sunday.Equals(true))
            {
                AddPlayerToClassifications(player, playerID, ctx, 1);
            }
        }

        private static void AddPlayerToClassifications(PlayerViewModel player, int playerID, ApplicationDbContext ctx, int gameTypeID)
        {
            ctx.Players_GameType.Add(new Players_GameType
            {
                PlayerID = playerID,
                GameTypeID = gameTypeID,
                IsSubstitute = true
            });

            ctx.SaveChanges();

            var playerExistsInClassification = ctx.Classification.Include("GameType")
                .Where(c => c.GameTypeID.Equals(gameTypeID))
                .Where(c => c.PlayerID.Equals(player.ID));

            if (playerExistsInClassification.Equals(null) || playerExistsInClassification.Count().Equals(0))
            {
                ctx.Classification.Add(new Classification
                {
                    PlayerID = playerID,
                    SettingsId = 1,
                    SeasonID = 1,
                    GameTypeID = gameTypeID

                });


            }

            ctx.SaveChanges();
        }

        private static Player InsertNewPlayer(PlayerViewModel player, ApplicationDbContext ctx)
        {
            Player addPlayer = new Player
            {
                Name = player.Name,
                PhoneNumber = player.PhoneNumber,
                Email = player.Email

            };

            ctx.Player.Add(addPlayer);
            ctx.SaveChanges();
            return addPlayer;
        }

        // GET: Sunday
        public ActionResult Resume(string season, string gameType)
        {
            season = seasonValue;
            if (string.IsNullOrEmpty(season) && string.IsNullOrEmpty(gameType))
            {
                return RedirectToAction("HomePage");
            }


            GamesViewModel sundayVM = new GamesViewModel();


            if (TempData["Game"] != null)
            {
                sundayVM = TempData["Game"] as GamesViewModel;
            }
            else
            {
                List<PlayersToGame> lastTeams = GetFinalTeams(gameType);

                lastTeams = CreateTeams(season, gameType, sundayVM, lastTeams);

            }

            return View(sundayVM);
        }

        private List<PlayersToGame> CreateTeams(string season, string gameType, GamesViewModel sundayVM, List<PlayersToGame> lastTeams)
        {
            if (lastTeams != null && lastTeams.Count() > 0)
            {
                sundayVM.hasOpengames = true;
                sundayVM.HasTeams = true;
                sundayVM.Season = season;
                sundayVM.GameType = gameType;
                lastTeams = lastTeams
               .OrderByDescending(p => p.Points)
               .ThenByDescending(p => p.Goals)
           .ThenBy(p => p.PlayerName)
            .ToList<PlayersToGame>();

                sundayVM.LinkTeamA = BuildTeam(lastTeams, 'A');
                sundayVM.LinkTeamB = BuildTeam(lastTeams, 'B');
                sundayVM.GoalsTeamA = BuildTeamGoals(lastTeams, 'A');
                sundayVM.GoalsTeamB = BuildTeamGoals(lastTeams, 'B');
            }
            else
            {
                sundayVM.hasOpengames = false;
                sundayVM.HasTeams = false;
                sundayVM.Season = season;
                sundayVM.GameType = gameType;
                sundayVM.Players = GetPlayers(season, gameType);
                sundayVM.hasOpengames = HasOpenGames(gameType);
                sundayVM.PlayerConfirmations = new List<PlayerConfirmation>();
                List<Players_GameType> substitutesList = GetPLayersSubstitues(season, gameType);
                List<Classification> sundayClassificationFromDB = GetClassification(season, gameType);


                foreach (Player player in sundayVM.Players)
                {
                    sundayVM.PlayerConfirmations.Add(new PlayerConfirmation
                    {
                        PlayerID = player.ID,
                        PlayerName = player.Name,
                        IsSubstitute = substitutesList!=null && substitutesList.Where(p => p.PlayerID.Equals(player.ID)).ToList<Players_GameType>().Count > 0 ? true : false

                    });

                };

                var orderedList = sundayVM.PlayerConfirmations.OrderBy(p => p.IsSubstitute).ThenBy(p => p.PlayerName);
                sundayVM.PlayerConfirmations = orderedList.ToList<PlayerConfirmation>();
                sundayVM.Classification = BuildClassification(sundayClassificationFromDB);

            }

            return lastTeams;
        }

        private List<Player> GetPlayers(string season, string gameType)
        {
            List<Player> playersList = new List<Player>();
            List<Player> playerSubstitutesList = new List<Player>();

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {

                playersList = (from p in ctx.Players_GameType.Include("GameType").Include("Player").Include("Classification")
                               where p.GameType.Description.Equals(gameType)
                               select p.Player).ToList<Player>();



                ctx.Dispose();
            }



            return playersList;
        }

        private void ClosePresencesInGame(string gameType)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                ctx.CurrentGame.Add(new CurrentGame
                {
                    GameDate = DateTime.Now,
                    WasOpen = true,
                    GameTypeID = ctx.GameType.Where(g => g.Description.Equals(gameType)).FirstOrDefault().GameTypeID
                });
                ctx.SaveChanges();
                ctx.Dispose();
            }
        }

        private bool HasOpenGames(string gameType)
        {
            var opengameByType = new List<CurrentGame>();
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                opengameByType = ctx.CurrentGame.Include("GameType")
                    .Where(g => g.GameType.Description.Equals(gameType))
                    .Where(g => g.WasOpen.Equals(true)).ToList<CurrentGame>();

                ctx.Dispose();

            }

            return (opengameByType != null && opengameByType.Count() > 0) ? true : false;
        }

        private List<ClassificationList> BuildClassification(List<Classification> sundayClassificationFromDB)
        {
            if (sundayClassificationFromDB != null && sundayClassificationFromDB.Count() > 0)
            {
                List<ClassificationList> classificationList = new List<ClassificationList>();

                foreach (Classification fromDB in sundayClassificationFromDB)
                {
                    ClassificationList classification = new ClassificationList();
                    PlayerResume playerResume = new PlayerResume();

                    playerResume.PlayerID = fromDB.PlayerID;
                    playerResume.PlayerName = fromDB.Player.Name;
                    playerResume.Games = fromDB.NumGames;
                    playerResume.Goals = fromDB.Goals;
                    playerResume.Wins = fromDB.Wins;
                    playerResume.Draws = fromDB.Draws;
                    playerResume.Loses = fromDB.Loses;

                    classification.Player = playerResume;
                    classification.PlayerName = fromDB.Player.Name;
                    classification.NumGames = fromDB.NumGames;
                    classification.Goals = fromDB.Goals;
                    classification.Wins = fromDB.Wins;
                    classification.Draws = fromDB.Draws;
                    classification.Loses = fromDB.Loses;
                    classification.TotalPoints = fromDB.TotalPoints;

                    classificationList.Add(classification);
                }

                if (classificationList.Count() > 0)
                {
                    classificationList = classificationList.OrderByDescending(c => c.TotalPoints).ThenByDescending(c => c.Goals).ThenBy(c => c.PlayerName).
                        ToList<ClassificationList>();

                }
                return classificationList;

            }

            return null;

        }

        private List<Player> GetPlayersConfirmations(string season, string gameType)
        {
            List<Player> confirmatedPlayersList = new List<Player>();

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                var hasGameOpened = ctx.CurrentGame.Where(g => g.WasOpen.Equals(true)).FirstOrDefault();

                if (hasGameOpened != null)
                {
                    DateTime gameDate = hasGameOpened.GameDate;
                    confirmatedPlayersList = ctx.PlayerConfirmationGames.Include("Player")
                        .Where(p => p.GameDate.Equals(gameDate))
                        .Select(p => p.Player).ToList<Player>();

                    ctx.Dispose();
                    return confirmatedPlayersList;
                }


            }
            return null;
        }


        private List<Players_GameType> GetPLayersSubstitues(string season, string gameType)
        {
            List<Players_GameType> playersList = new List<Players_GameType>();

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {

                int gameTypeID = ctx.GameType.Where(g => g.Description.Equals(gameType)).FirstOrDefault().GameTypeID;

                playersList = ctx.Players_GameType
                    .Where(p => p.IsSubstitute.Equals(true))
                    .Where(p => p.GameTypeID.Equals(gameTypeID))
                    .GroupBy(p => p.Player.Name).Select(p => p.FirstOrDefault()).ToList<Players_GameType>();


                ctx.Dispose();
            }

            return playersList != null && playersList.Count() > 0 ? playersList : null;

        }

        private List<Classification> GetClassification(string season, string gameType)
        {
            List<Classification> sundayClassification = new List<Classification>();

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                var entities = ctx.Classification.Include("Season").Include("GameType").Include("Player");
                sundayClassification =
                    entities.
                    Where(
                    c => c.Season.SeasonDesc.Equals(season)).
                    Where(
                            c => c.GameType.Description.Equals(gameType)).
                    Where(
                            c => c.NumGames > 0)
                            .ToList<Classification>();




                ctx.Dispose();
            }

            return sundayClassification.Count() > 0 ? sundayClassification : null;
        }
    }
}