using System;
using GTANetworkAPI;
using server_side.Systems;
using server_side.Data;

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

        private static void CreateGlobalTimers()
        {
            // sec timer
            secTimer = new System.Timers.Timer(1000);
            secTimer.Elapsed += (s, e) =>
            {
                // update weather every new hour
                if (time.Minute == 0 && time.Second == 0)
                {
                    RealWeather.SetCurrentWeatherInLA();
                }

                // reset pickups kd for all players
                NAPI.Pools.GetAllPlayers().ForEach(p =>
                {
                    if (p.HasData(EntityData.PLAYER_PICKUPKD))
                        if (p.GetData<int>(EntityData.PLAYER_PICKUPKD) != 0) p.SetData(EntityData.PLAYER_PICKUPKD, p.GetData<int>(EntityData.PLAYER_PICKUPKD) - 1);

                    if (p.HasData("HouseCreateKD"))
                        if (p.GetData<int>("HouseCreateKD") != 0) p.SetData("HouseCreateKD", p.GetData<int>("HouseCreateKD") - 1);
                });
            };
            secTimer.AutoReset = secTimer.Enabled = true;
            // --

            // minute timer
            minuteTimer = new System.Timers.Timer(60 * 1000);
            minuteTimer.Elapsed += (s, e) =>
            {
                // NAPI.World.SetTime(time.Hour, time.Minute, time.Second);
            };
            minuteTimer.AutoReset = minuteTimer.Enabled = true;
            // --
        }
        [ServerEvent(Event.Update)]
        public void Event_Update()
        {
            NAPI.Pools.GetAllPlayers().ForEach(p =>
            {
                if(p.GetData<bool>("temp_gm"))
                    p.Health = 100;
            });
        }
    }
}
