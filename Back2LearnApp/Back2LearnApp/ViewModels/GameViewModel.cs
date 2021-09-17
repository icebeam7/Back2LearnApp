using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using Back2LearnApp.Models;
using Back2LearnApp.Services;

namespace Back2LearnApp.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private List<Country> countries;

        private Country currentCountry;

        public Country CurrentCountry
        {
            get => currentCountry;
            set { SetProperty(ref currentCountry, value); }
        }

        private string flag;

        public string Flag
        {
            get => flag;
            set { SetProperty(ref flag, value); }
        }

        private string resultMessage;

        public string ResultMessage
        {
            get => resultMessage;
            set { SetProperty(ref resultMessage, value); }
        }

        private int score;

        public int Score
        {
            get => score;
            set { SetProperty(ref score, value); }
        }

        private ObservableCollection<Country> options;

        public ObservableCollection<Country> Options
        {
            get => options;
            set { SetProperty(ref options, value); }
        }

        private Country selectedCountry;

        public Country SelectedCountry
        {
            get => selectedCountry;
            set { selectedCountry = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { private set; get; }
        public ICommand EvaluateChoiceCommand { private set; get; }

        private readonly Random generator;

        public GameViewModel()
        {
            generator = new Random();
            CurrentCountry = new Country() { CountryCode = string.Empty };

            LoadDataCommand = new Command(async () => await LoadData());
            EvaluateChoiceCommand = new Command(async () => await EvaluateChoice());
        }

        async Task LoadData()
        {
            countries = await CountriesService.GetCountries();
            await GenerateNewQuestion();
        }

        async Task GenerateNewQuestion()
        {
            IsBusy = true;

            var numOfCountries = countries.Count;

            if (numOfCountries > 0)
            {
                await Task.Delay(3000);

                var countryList = new List<Country>();
                var correctAnswer = generator.Next(0, 4);

                for (int i = 0; i < 4; i++)
                {
                    var index = generator.Next(0, numOfCountries);
                    var country = countries[index];

                    if (!countryList.Any(x => x.GeonameId == country.GeonameId))
                        countryList.Add(country);
                    else
                        i--;
                }

                CurrentCountry = countryList[correctAnswer];
                Flag = $"https://raw.githubusercontent.com/hjnilsson/country-flags/master/png250px/{CurrentCountry.CountryCode.ToLower()}.png";

                Options = new ObservableCollection<Country>(countryList);
                ResultMessage = string.Empty;
            }

            IsBusy = false;
        }

        async Task EvaluateChoice()
        {
            if (!IsBusy && SelectedCountry != null)
            {
                var points = 10;

                if (CurrentCountry.GeonameId == SelectedCountry.GeonameId)
                {
                    Score += points;
                    ResultMessage = "¡Correct! =)";
                }
                else
                {
                    ResultMessage = "¡Incorrect! =(";
                }

                await GenerateNewQuestion();
            }
        }
    }
}
