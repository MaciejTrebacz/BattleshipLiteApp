using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BattleshipLiteLibrary.Models;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(model,letter,number );
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel();
            spot.SpotLetter = letter;
            spot.SpotNumber = number;
            spot.Status = GridSpotStatus.Empty;
            model.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            GridSpotModel spot = new GridSpotModel();
            spot.SpotLetter = location.Substring(0, 1);
            int.TryParse(location.Substring(1), out int result);
            Console.WriteLine(result);
            spot.SpotNumber = result;
            spot.Status = GridSpotStatus.Ship;
            model.ShipLocations.Add(spot);
            return true;
        }

        public static bool PlayerStillActive(PlayerInfoModel opponent)
        {
            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.Status == GridSpotStatus.Ship)
                {
                    return true;
                }
            }

            return false;
        }

        public static int GetShotCount(PlayerInfoModel winner)
        {
            return winner.ShotCount;
        }

        public static (string row, int column) SplitshotIntoRowAndColumn(string shot)
        {
            int newColumn = int.Parse(shot.Substring(1));
            string row = shot.Substring(0, 1);
            return (row, newColumn);
        }

        public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
        {
            foreach (var player in activePlayer.ShotGrid)
            {
                if (row.ToUpper() == player.SpotLetter && column == player.SpotNumber)
                {
                    activePlayer.ShotCount++;
                    return true;
                } 
            }
            return false;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            foreach (var ship in opponent.ShipLocations)
            {
                if (row.ToUpper() == ship.SpotLetter && column == ship.SpotNumber && ship.Status == GridSpotStatus.Ship)
                {
                    return true;
                }
            } return false;
        }

        public static void MarkShotResult(PlayerInfoModel activePlayer,PlayerInfoModel opponent, string row, int column, bool isAHit)
        {
        
            foreach (var ship in opponent.ShipLocations)
            {
                if (row.ToUpper() == ship.SpotLetter && column == ship.SpotNumber && isAHit == true) 
                {
                    ship.Status = GridSpotStatus.Sunk;
                    Console.WriteLine("SUNK IT ");
                } 
            }

            foreach (var spot in activePlayer.ShotGrid)
            {
                if (row.ToUpper() == spot.SpotLetter && column == spot.SpotNumber && isAHit == true) 
                {
                    spot.Status = GridSpotStatus.Hit;
                } else if ((row.ToUpper() == spot.SpotLetter && column == spot.SpotNumber && isAHit == false))
                {
                    spot.Status = GridSpotStatus.Miss;
                }
            }
            
        }
    }
}
