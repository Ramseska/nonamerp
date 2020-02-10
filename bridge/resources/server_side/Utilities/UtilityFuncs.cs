﻿using System;
using GTANetworkAPI;
using AngleSharp;
using System.Linq;

namespace server_side.Utilities
{
    class UtilityFuncs : Script
    {
        // Weather in the game from real LA
        public async static void SetCurrentWeatherInLA()
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
                    NAPI.World.SetWeather(gameWeather[i]);
                    Console.WriteLine($"Realworld Weather: Temperature: {temperature[0]} | Weather: {weather[0]}");
                    Console.WriteLine($"Current game weather: {gameWeather[i]}");
                    break;
                }
                else if (i == dataWeather.Length - 1) Console.WriteLine("Not finded rl weather");
            }
        }
        static public Vector3 GetPosFrontOfPlayer(Client client, double distantion)
        {
            double heading = client.Rotation.Z * Math.PI / 180;
            double x = client.Position.X + (distantion * Math.Sin(-heading));
            double y = client.Position.Y + (distantion * Math.Cos(-heading));
            return new Vector3(x,y,client.Position.Z);
        }
    }
}