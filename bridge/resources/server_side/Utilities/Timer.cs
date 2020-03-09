using System;
using System.Threading.Tasks;
using System.Timers;
using GTANetworkAPI;
using server_side.Utilities;

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
            secTimer.Elapsed += OnSecondTimer;
            secTimer.AutoReset = secTimer.Enabled = true;
            // one minute timer
            minuteTimer = new System.Timers.Timer(60 * 1000);
            minuteTimer.Elapsed += OnMinuteTimer;
            minuteTimer.AutoReset = minuteTimer.Enabled = true;
        }

        private async static void OnMinuteTimer(object source, ElapsedEventArgs e)
        {
            /*
            await Task.Run(() =>
            {
                NAPI.World.SetTime(time.Hour, time.Minute, time.Second);
            });
            */
        }

        private async static void OnSecondTimer(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                if (time.Minute == 0 && time.Second == 0) // every new hour
                {
                    UtilityFuncs.SetCurrentWeatherInLA();
                }
            });

            NAPI.Pools.GetAllPlayers().ForEach(p =>
            {
                if (p.HasData("PickupKD"))
                    if (p.GetData("PickupKD") != 0) p.SetData("PickupKD", p.GetData("PickupKD") - 1);

                if (p.HasData("HouseCreateKD"))
                    if (p.GetData("HouseCreateKD") != 0) p.SetData("HouseCreateKD", p.GetData("HouseCreateKD") - 1);
            });
        }
    }
}
