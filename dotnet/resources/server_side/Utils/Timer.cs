using System;
using GTANetworkAPI;
using server_side.Systems;
using server_side.Data;
using System.Timers;

namespace server_side.Timers
{ 
    class Timer : Script
    {
        private static System.Timers.Timer minuteTimer;
        private static System.Timers.Timer secTimer;

        private static DateTime time = DateTime.Now;

        [ServerEvent(Event.ResourceStart)]
        public void Event_OnResourceStart()
        {
            CreateGlobalTimers();
        }

        private void CreateGlobalTimers()
        {
            // sec timer
            secTimer = new System.Timers.Timer(1000);
            secTimer.Elapsed += UpdateEverySec;
            secTimer.AutoReset = secTimer.Enabled = true;

            // minute timer
            minuteTimer = new System.Timers.Timer(60 * 1000);
            minuteTimer.Elapsed += UpdateEveryMinute;
            minuteTimer.AutoReset = minuteTimer.Enabled = true;
        }

        private void _ResetPickUpKD()
        {
            NAPI.Pools.GetAllPlayers().ForEach(p =>
            {
                if (p.HasData(EntityData.PLAYER_PICKUPKD))
                    if (p.GetData<int>(EntityData.PLAYER_PICKUPKD) != 0) p.SetData(EntityData.PLAYER_PICKUPKD, p.GetData<int>(EntityData.PLAYER_PICKUPKD) - 1);

                if (p.HasData("HouseCreateKD"))
                    if (p.GetData<int>("HouseCreateKD") != 0) p.SetData("HouseCreateKD", p.GetData<int>("HouseCreateKD") - 1);
            });
        }

        private void UpdateEveryMinute(object s, EventArgs e)
        {
            // NAPI.World.SetTime(time.Hour, time.Minute, time.Second);
        }

        private void UpdateEverySec(object s, EventArgs e)
        {
            /*
            // current weather from LA
            if (time.Minute == 0 && time.Second == 0)
                new RealWeather().SetCurrentWeatherInLA();
            */

            _ResetPickUpKD();
        }
    }
}
