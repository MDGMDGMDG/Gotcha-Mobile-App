using System;
using System.Collections.Generic;
using System.Text;

namespace Gotcha_Mobile_App.Model
{
    public class Game
    {
        public string NameOfGame { get; set; }
        public double CriminalLatitude { get; set; }
        public double CriminalLongitude { get; set; }
        public double PoliceLatitude { get; set; }
        public double PoliceLongitude { get; set; }
        public DateTime StartDate { get; set; }
        public string CriminalName { get; set; }
        public string PoliceName { get; set; }
        public int NumberOfPlayers { get; set; }

        public Game()
        {
            this.NumberOfPlayers = 1;
        }
    }
}
