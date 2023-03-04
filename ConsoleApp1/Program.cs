using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLiteLibrary;

namespace BattleshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            // user input, convert user input, validate it, show it
            WelcomeMessage();
            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;
            
            do
            {
                // Display grid from activePlayer on where they fired
                DisplayShotGrid(activePlayer);

                // Ask player1 for a shot
                // Determine if its a valid shot
                // Determine shot results
                RecordPlayerShot(activePlayer, opponent);


                // Determine if game is continue
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
                // if over, set activePlayer as winner
                // else, go to opponent 
                if (doesGameContinue == true)  
                {
                    // swap positions
                    (activePlayer, opponent) = (opponent, activePlayer);

                }
                else
                {
                    winner = activePlayer;
                }



            } while (winner == null);

            IdentifyWinner(winner);


            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulation {winner.UserName}");
            Console.WriteLine($"Player {winner.UserName} took {GameLogic.GetShotCount(winner)}");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                string shot = AskForShot();
                (row, column) = GameLogic.SplitshotIntoRowAndColumn(shot);
                Console.WriteLine(row,column);
                isValidShot = GameLogic.ValidateShot(activePlayer,row, column);

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location");
                }


            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            Console.WriteLine();
            GameLogic.MarkShotResult(activePlayer, opponent,row,column,isAHit);

        }

        private static string AskForShot()
        {
            Console.Write("Please enter your shot: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (currentRow != gridSpot.SpotLetter)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }
                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                } else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ? ");
                }
            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("created by Maciej Trebacz");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTtile)
        {
            PlayerInfoModel output = new PlayerInfoModel();
            // Ask user for name
            Console.WriteLine($"{playerTtile} _________");
            output.UserName = AskForUsersName();
            // load up shot grid
            GameLogic.InitializeGrid(output);
            // ask user to place their ships
            PlaceShips(output);
            
            Console.Clear();
            return output;
        }

        private static string AskForUsersName()
        {
            Console.Write("What is your name: ");
            string output = Console.ReadLine();
            
            return output;
        }

        public static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Place your ship number {model.ShipLocations.Count +1}: ");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model,location);
                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location, try again");
                } 


            } while (model.ShipLocations.Count < 5);
        }
    }
}
