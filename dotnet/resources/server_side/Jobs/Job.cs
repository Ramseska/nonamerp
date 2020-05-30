using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using server_side.Jobs;
using server_side.Data;
using System.Security.Cryptography;

namespace server_side.Jobs
{
    public class Job : Script
    {
        public enum eJobs : int
        {
            None = 0,
            AppleCollector = 1,
            Miner = 2
        }

        static List<string[]> jobsInfo = new List<string[]>()
        {
           new string[] { "None", "None" },
           new string[] { "Яблочная ферма", "Работа сборщиком яблок.<br><br>Вы уверенны, что хотите начать работу?" },
           new string[] { "Карьер", "Пока что нихуя нема.." }
        };

        public static void ShowJobDialog(Player player, eJobs job)
        {
            if(player.GetData<eJobs>(EntityData.PLAYER_JOB) == job)
            {
                // uval
                NAPI.ClientEvent.TriggerClientEvent(player, "createWorkDialog", jobsInfo[(int)job][0], "Вы уверенны, что хотите завершить работу?");
            }
            else if(player.GetData<eJobs>(EntityData.PLAYER_JOB) == 0)
            {
                // prival
                player.SetData<eJobs>(EntityData.PLAYER_TEMPJOB, job);
                NAPI.ClientEvent.TriggerClientEvent(player, "createWorkDialog", jobsInfo[(int)job][0], jobsInfo[(int)job][1]);
            }
            else
            {
                player.SendChatMessage("Вы уже работаете на другой работе!");
            }
        }

        [RemoteEvent("sAcceptJob")]
        public static void OnAcceptJob(Player player)
        {
            switch(player.GetData<eJobs>(EntityData.PLAYER_TEMPJOB))
            {
                case eJobs.None: break;
                case eJobs.AppleCollector:
                {
                    if (player.GetData<eJobs>(EntityData.PLAYER_TEMPJOB) != player.GetData<eJobs>(EntityData.PLAYER_JOB))
                    {
                        new AppleCollector().StartJob(player);
                    }
                    else
                    {
                        new AppleCollector().EndJob(player);
                    }
                    break;
                }
                case eJobs.Miner:
                {
                    break;
                }
            }
        }

        public static void GiveJobSalary(Player player, eJobs job)
        {
            new PlayerInfo(player).GiveMoney(player.GetData<double>(EntityData.PLAYER_JOB_SALARY), jobsInfo[(int)job][0]);

            Utilities.UtilityFuncs.SendPlayerNotify(player, 0, $"+{player.GetData<double>(EntityData.PLAYER_JOB_SALARY)}$");

            player.SetData<double>(EntityData.PLAYER_JOB_SALARY, 0.0);
        }

        public static void OnEnterJobPickUp(ColShape shape, Player player)
        {
            if (player.IsInVehicle) return;

            if (shape.HasData(EntityData.JOB_ID))
            {
                ShowJobDialog(player, shape.GetData<eJobs>(EntityData.JOB_ID));
            }
        }

        public static void IsLeaveOnJob(Player player)
        {
            if (player.GetData<double>(EntityData.PLAYER_JOB_SALARY) > 0)
            {
                PlayerInfo pInfo = new PlayerInfo(player);
                pInfo.AddToPayCheck(player.GetData<double>(EntityData.PLAYER_JOB_SALARY), "from job salary");
                player.SetData<double>(EntityData.PLAYER_JOB_SALARY, 0.00);
            }
        }

        public static void InitJobs()
        {
            new AppleCollector().CreateJob();
        }
    }
}
