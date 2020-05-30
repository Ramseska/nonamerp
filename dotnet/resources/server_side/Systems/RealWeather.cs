using System;
using System.Linq;
using GTANetworkAPI;
using AngleSharp;

namespace server_side.Systems
{
    // Нужно переписать, т.к. есть пара багов (или вовсе отказаться от нее и сделать какой-то рандом с вероятностью)

    class RealWeather : Script
    {
        // Weather in the game from real LA
        public async void SetCurrentWeatherInLA()
        {
            var config = new Configuration().WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync("https://www.bbc.com/weather/5368361");

            var temperature = document.GetElementsByClassName("wr-value--temperature--c").Select(x => x.TextContent.Trim()).ToArray();
            var weather = document.GetElementsByClassName("wr-day__weather-type-description wr-js-day-content-weather-type-description wr-day__content__weather-type-description--opaque").Select(x => x.TextContent.Trim()).ToArray();

            string[] dataWeather = new string[] { "clear", "Sunny", "Partly cloudy", "cloud", "rain", "Thundery", "Thick" };
            string[] gameWeather = new string[] { "CLEAR", "EXTRASUNNY", "OVERCAST", "CLOUDS", "RAIN", "THUNDER", "SMOG" };

            for (int i = 0; i < dataWeather.Length; i++)
            {
                if (weather[0].Contains(dataWeather[i]))
                {
                    NAPI.Task.Run(() =>
                    {
                        NAPI.World.SetWeather(gameWeather[i]);
                        NAPI.Util.ConsoleOutput($"Realworld Weather: Temperature: {temperature[0]} | Weather: {weather[0]}");
                        NAPI.Util.ConsoleOutput($"Current game weather: {gameWeather[i]}");
                    });
                    break;
                }
                else if (i == dataWeather.Length - 1)
                {
                    NAPI.Task.Run(() => { NAPI.Util.ConsoleOutput("Not finded rl weather"); });
                }
            }
        }
    }
}
