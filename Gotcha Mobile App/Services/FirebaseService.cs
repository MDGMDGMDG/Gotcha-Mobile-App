using Firebase.Database;
using Gotcha_Mobile_App.Model;
using Firebase.Database.Query;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Gotcha_Mobile_App
{
    public static class FirebaseService
    {
        private static FirebaseClient _firebase;
        private static string _firebaseConnectionString = "https://gotcha-1ecf9-default-rtdb.europe-west1.firebasedatabase.app/";

        /// <summary>
        /// Adds an User
        /// </summary>
        public static async Task Create(Game game)
        {
            if (_firebase == null)
                _firebase = new FirebaseClient(_firebaseConnectionString);

            await _firebase.Child(nameof(Game)).PostAsync(game);
        }

        public static async Task<Game> Get(Game game)
        {
            if (_firebase == null)
                _firebase = new FirebaseClient(_firebaseConnectionString);

            var retrievedGame = (await _firebase
              .Child(nameof(Game))
              .OnceAsync<Game>()).Where(a => a.Object.NameOfGame == game.NameOfGame).FirstOrDefault();

            var latestGame = retrievedGame.Object;

            if (latestGame == null)
                await App.Current.MainPage.DisplayAlert("Oops!", "There's an issue with retrieving your data.", "OK");

            return latestGame;
        }

        public static async Task<List<Game>> GetGames()
        {
            if (_firebase == null)
                _firebase = new FirebaseClient(_firebaseConnectionString);

            return (await _firebase.Child(nameof(Game)).OnceAsync<Game>()).Select(item => new Game
            {
                NameOfGame = item.Object.NameOfGame,
                CriminalLatitude = item.Object.CriminalLatitude,
                CriminalLongitude = item.Object.CriminalLongitude,
                PoliceLatitude = item.Object.PoliceLatitude,
                PoliceLongitude = item.Object.PoliceLongitude,
                CriminalName = item.Object.CriminalName,
                NumberOfPlayers = item.Object.NumberOfPlayers,
                PoliceName = item.Object.PoliceName,
                StartDate = item.Object.StartDate
            }).ToList();
        }

        public static async Task AddToNumPlayers(Game game)
        {
            if (_firebase == null)
                _firebase = new FirebaseClient(_firebaseConnectionString);

            var storage = (await _firebase
              .Child(nameof(Game))
              .OnceAsync<Game>()).Where(a => a.Object.NameOfGame == game.NameOfGame).FirstOrDefault();

            storage.Object.NumberOfPlayers++;
            storage.Object.PoliceName = game.PoliceName;
            storage.Object.CriminalName = game.CriminalName;

            await _firebase.Child(nameof(Game)).Child(storage.Key)
                .PutAsync(storage.Object);
        }

        public static async Task UpdateLocation(Location location, Game game)
        {
            if (_firebase == null)
                _firebase = new FirebaseClient(_firebaseConnectionString);

            var storage = (await _firebase
              .Child(nameof(Game))
              .OnceAsync<Game>()).Where(a => a.Object.NameOfGame == game.NameOfGame).FirstOrDefault();

            //update location for criminal or police
            if (App.OnTheRun)
            {
                storage.Object.CriminalLatitude = location.Latitude;
                storage.Object.CriminalLongitude = location.Longitude;
            }
            else
            {
                storage.Object.PoliceLatitude = location.Latitude;
                storage.Object.PoliceLongitude = location.Longitude;
            }

            await _firebase.Child(nameof(Game)).Child(storage.Key)
                .PutAsync(storage.Object);
        }


        //public static async Task Add(Occupation occupation)
        //{
        //    if (_firebase == null)
        //        _firebase = new FirebaseClient(_firebaseConnectionString);

        //    await _firebase.Child(nameof(Occupation)).PostAsync(occupation);
        //}

        //public static async Task Add(Measurement measurement)
        //{
        //    if (_firebase == null)
        //        _firebase = new FirebaseClient(_firebaseConnectionString);

        //    //get the iscurrent measurement from day before (there should only be one current)
        //    DateTime measureday = measurement.ScoreDate.Date;
        //    DateTime referenceday = measureday.AddDays(-1);

        //    var referenceMeasurement = (await _firebase
        //      .Child(nameof(Measurement))
        //      .OnceAsync<Measurement>()).Where(a => a.Object.IsCurrent == true && a.Object.ScoreDate >= referenceday && a.Object.ScoreDate < measureday && a.Object.UserEmail == App.User.EmailAddress).FirstOrDefault();

        //    //calculate startscore = endscore + sleep - penalty
        //    double startscore = 80;
        //    if (referenceMeasurement != null && referenceMeasurement.Object != null)
        //    {
        //        startscore = referenceMeasurement.Object.EndScore + referenceMeasurement.Object.Sleep - referenceMeasurement.Object.Penalty;
        //        if (startscore > 100)
        //            startscore = 100;
        //        else if (startscore < 0)
        //            startscore = 0;
        //    }
        //    measurement.StartScore = startscore;

        //    //set endscore based on startscore
        //    measurement.EndScore = startscore - measurement.TotalEffortScore;
        //    if (measurement.EndScore < 0) measurement.EndScore = 0;

        //    //update iscurrent measurement from today and set iscurrent false
        //    var endOfMeasureDay = measureday.AddDays(1).AddSeconds(-1);
        //    var measurementDayItems = (await _firebase.Child(nameof(Measurement))
        //      .OnceAsync<Measurement>()).Where(a => a.Object.IsCurrent == true && a.Object.ScoreDate >= measureday && a.Object.ScoreDate < endOfMeasureDay && a.Object.UserEmail == App.User.EmailAddress);

        //    foreach (var item in measurementDayItems)
        //    {
        //        item.Object.IsCurrent = false;
        //        await _firebase.Child(nameof(Measurement)).Child(item.Key).PutAsync(item.Object);
        //    }

        //    await _firebase.Child(nameof(Measurement)).PostAsync(measurement);
        //}

        //public static async Task Add(Debrief debrief)
        //{
        //    if (_firebase == null)
        //        _firebase = new FirebaseClient("_firebaseConnectionString");

        //    await _firebase.Child(nameof(Debrief)).PostAsync(debrief);
        //}

        //public static async Task<Debrief> GetDebrief(MeasurementBO measurement)
        //{
        //    if (_firebase == null)
        //        _firebase = new FirebaseClient("_firebaseConnectionString");

        //    var combination = $"{ConvertScoreToCharacter(measurement.MorningScore)}-{ConvertScoreToCharacter(measurement.MiddayScore)}-{ConvertScoreToCharacter(measurement.EveningScore)}";

        //    var retrievedPerson = (await _firebase
        //      .Child(nameof(Debrief))
        //      .OnceAsync<Debrief>()).Where(a => a.Object.Combination == combination).FirstOrDefault();

        //    var debrief = retrievedPerson.Object;

        //    if (debrief == null)
        //        await App.Current.MainPage.DisplayAlert("Oops!", "There's an issue with retrieving your data.", "OK");

        //    return debrief;
        //}

        //public static async Task<List<Measurement>> GetMeasurements(string userEmail)
        //{
        //    if (_firebase == null)
        //        _firebase = new FirebaseClient("_firebaseConnectionString");

        //    return (await _firebase.Child(nameof(Measurement)).OnceAsync<Measurement>()).Where(x => x.Object.UserEmail == userEmail && x.Object.IsCurrent).Select(x => x.Object).ToList();
        //}
    }
}
