using System;
using System.Collections.Generic;
using System.Text;
using server_side.Jobs;
using GTANetworkAPI;
using System.Runtime.CompilerServices;
using server_side.Data;
using System.Text.Json;
using Microsoft.VisualBasic;
using server_side.Utils;

namespace server_side.Jobs
{
    class AppleCollector : Script
    {
        // for job
        public const string APPLECOL_DELIVERY_COLSHAPE = "APPLECOL_DELIVERY_COLSHAPE";

        // for player
        public const string APPLECOL_UNITS_HAVE = "APPLECOL_UNITS_HAVE";
        public const string APPLECOL_UNITS_DELIVED = "APPLECOL_UNITS_DELIVED";
        public const string APPLECOL_CURRENT_CP = "APPLECOL_CURRENT_CP";

        const int MaxApples = 20; // максимальное кол-во переносимых яблок
        private static double SalaryFactor = 0.2; // $ за 1 яблоко

        private ColShape StartJobColShape { get; set; }
        private ColShape AppleDeliveryColShape { get; set; }
        private TextLabel AppleLabel { get; set; }

        List<Vector3> ListApplePoints = new List<Vector3>()
        {
            new Vector3(377.3193, 6506.1440, 28.0053),
            new Vector3(370.7314, 6506.0933, 28.3905),
            new Vector3(363.8416, 6506.0181, 28.5467),
            new Vector3(355.9707, 6504.8516, 28.4529),
            new Vector3(348.2852, 6505.1514, 28.7895),
            new Vector3(339.6548, 6506.0200, 28.6744),
            new Vector3(331.3927, 6505.7583, 28.5092),
            new Vector3(322.2917, 6505.7466, 29.1749),
            new Vector3(321.7747, 6516.9214, 29.1287),
            new Vector3(329.7590, 6517.4858, 28.9766),
            new Vector3(338.5842, 6516.6948, 28.9474),
            new Vector3(347.0867, 6517.4756, 28.8212),
            new Vector3(354.8439, 6517.2476, 28.2308),
            new Vector3(362.8199, 6517.3423, 28.2722),
            new Vector3(369.6315, 6518.1514, 28.3766),
            new Vector3(377.6424, 6517.4170, 28.3777),
            new Vector3(369.8222, 6531.4707, 28.3874),
            new Vector3(361.7260, 6530.9502, 28.3633),
            new Vector3(354.2234, 6530.5322, 28.3792),
            new Vector3(345.1126, 6531.5684, 28.7428),
            new Vector3(339.1192, 6531.1201, 28.5678),
            new Vector3(330.0130, 6530.9126, 28.5784),
            new Vector3(322.2608, 6530.8955, 29.1381),
        };

        public void CreateJob()
        {
            NAPI.Marker.CreateMarker(30, new Vector3(413.0344, 6539.8838, 27.7248), new Vector3(), new Vector3(), 1f, new Color(209, 217, 126), dimension: Utils.Constants.SERVER_DEFAULT_DIMENSION);
            StartJobColShape = NAPI.ColShape.CreateCylinderColShape(new Vector3(413.0344, 6539.8838, 27.7248), 1f, 2f, 0);
            StartJobColShape.SetData(EntityData.JOB_ID, Job.eJobs.AppleCollector);

            NAPI.Marker.CreateMarker(2, new Vector3(385.0542, 6530.6968, 28.0458), new Vector3(), new Vector3(), 1f, new Color(209, 217, 126), dimension: Utils.Constants.SERVER_DEFAULT_DIMENSION);
            
            AppleDeliveryColShape = NAPI.ColShape.CreateCylinderColShape(new Vector3(385.0542, 6530.6968, 28.0458), 1f, 2f, 0);
            AppleDeliveryColShape.SetData<bool>(APPLECOL_DELIVERY_COLSHAPE, true);

            NAPI.Blip.CreateBlip(478, new Vector3(413.0344, 6539.8838, 27.7248), 1f, 0, name: "Яблочная ферма", drawDistance: Utils.Constants.SERVER_BLIP_DRAW_DISTANTION, shortRange: true, dimension: Utils.Constants.SERVER_DEFAULT_DIMENSION);

            AppleLabel = NAPI.TextLabel.CreateTextLabel(string.Empty, new Vector3(413.0344, 6539.8838, 27.7248), 3f, 3f, 10, new Color(255, 255, 255), dimension: Utils.Constants.SERVER_DEFAULT_DIMENSION);
            NAPI.TextLabel.SetTextLabelText(AppleLabel, $"~y~Яблочная ферма\n_______________\n\n~w~Яблок на складе: ~y~0~w~ ед.");
        }

        public void StartJob(Player player)
        {
            player.SetData(EntityData.PLAYER_JOB, player.GetData<Job.eJobs>(EntityData.PLAYER_TEMPJOB));
            player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}Вы начали работу сборщика яблок!");

            player.SetData<int>(APPLECOL_UNITS_HAVE, 0);
            player.SetData<int>(APPLECOL_UNITS_DELIVED, 0);
            player.SetData<Checkpoint>(APPLECOL_CURRENT_CP, null);

            CreatePoint(player);
        }

        public void EndJob(Player player)
        {
            if (player.GetSharedData<Checkpoint>(APPLECOL_CURRENT_CP) != null)
            {
                DestroyPoint(player);
            }

            player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}Вы закончили работу сборщика яблок!");
            player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}Всего собрано {Utils.Colors.YELLOW}{player.GetData<int>(APPLECOL_UNITS_DELIVED)}{Utils.Colors.WHITE} яблок.");
            player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}Ваша зарплата: {Utils.Colors.YELLOW}{Math.Round(player.GetData<double>(EntityData.PLAYER_JOB_SALARY), 2)}{Utils.Colors.WHITE}$");

            Job.GiveJobSalary(player, player.GetData<Job.eJobs>(EntityData.PLAYER_JOB));

            NAPI.ClientEvent.TriggerClientEvent(player, "destroyAppleCollectorApp");

            player.SetData<int>(EntityData.PLAYER_TEMPJOB, 0);
            player.SetData<int>(EntityData.PLAYER_JOB, 0);
            player.ResetData(APPLECOL_UNITS_HAVE);
            player.ResetData(APPLECOL_UNITS_DELIVED);
            player.ResetData(APPLECOL_CURRENT_CP);
        }

        public void GiveApples(Player player, int apples)
        {
            player.SetData<int>(APPLECOL_UNITS_HAVE, (player.GetData<int>(APPLECOL_UNITS_HAVE) + apples));

            if (player.GetData<int>(APPLECOL_UNITS_HAVE) > MaxApples)
            {
                player.SetData<int>(APPLECOL_UNITS_HAVE, MaxApples);
            }
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnDeliveryApples(ColShape shape, Player player)
        {
            if (shape.GetData<bool>(APPLECOL_DELIVERY_COLSHAPE))
            {
                if (player.GetData<Job.eJobs>(EntityData.PLAYER_JOB) != Job.eJobs.AppleCollector)
                    return;

                if (player.GetData<int>(APPLECOL_UNITS_HAVE) == 0)
                {
                    player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}У Вас нет яблок!");
                    return;
                }

                player.SetData<int>(APPLECOL_UNITS_DELIVED, player.GetData<int>(APPLECOL_UNITS_DELIVED) + player.GetData<int>(APPLECOL_UNITS_HAVE));
                player.SetData<int>(APPLECOL_UNITS_HAVE, 0);

                player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}Вы положили яблоки в ящик. Всего собрано {Utils.Colors.YELLOW}{player.GetData<int>(APPLECOL_UNITS_DELIVED)} {Utils.Colors.WHITE}яблок!");

                player.SetData<double>(EntityData.PLAYER_JOB_SALARY, GetSalary(player));

                CreatePoint(player);
            }
        }

        [RemoteEvent("InitCurrentAppleColCP")]
        public void InitCurrentAppleColCP(Player player, Checkpoint cp)
        {
            player.SendChatMessage($"InitCP: {cp}");

            player.SetData<Checkpoint>(APPLECOL_CURRENT_CP, cp);
        }

        [ServerEvent(Event.PlayerEnterCheckpoint)]
        public void PlayerEnterCheckpoint(Checkpoint checkpoint, Player player)
        {
            if (player.IsInVehicle) return;

            if (player.GetSharedData<Checkpoint>(APPLECOL_CURRENT_CP) == checkpoint)
            {
                StartCollectApples(player);
            }
        }

        [RemoteEvent("sEndCollectApples")]
        public void EndCollectApples(Player player)
        {
            if (player.GetData<Job.eJobs>(EntityData.PLAYER_JOB) != Job.eJobs.AppleCollector)
                return;

            DestroyPoint(player);

            GiveApples(player, new Random().Next(1, 5));

            player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}Вы собрали яблоки! Сейчас у Вас собрано {Utils.Colors.YELLOW}{player.GetData<int>(APPLECOL_UNITS_HAVE)}{Utils.Colors.WHITE} яблок.");

            CreatePoint(player);
        }

        public void StartCollectApples(Player player)
        {
            if (player.GetData<Job.eJobs>(EntityData.PLAYER_JOB) != Job.eJobs.AppleCollector)
                return;

            if (player.GetData<int>(APPLECOL_UNITS_HAVE) >= MaxApples)
            {
                player.SendChatMessage($"{Utils.Colors.YELLOW}[Яблочная ферма]: {Utils.Colors.WHITE}У Вас больше нет места для яблок!");
                return;
            }
            NAPI.ClientEvent.TriggerClientEvent(player, "createAppleCollectorApp");
        }

        private void CreatePoint(Player player)
        {
            if (player.GetSharedData<Checkpoint>(APPLECOL_CURRENT_CP) == null)
            {
                Vector3 point = ListApplePoints[new Random().Next(0, ListApplePoints.Count)];
                NAPI.ClientEvent.TriggerClientEvent(player, "cAppleJobCreateCheckpoint", point);
            }
        }

        private void DestroyPoint(Player player)
        {
            player.GetSharedData<Checkpoint>(APPLECOL_CURRENT_CP).Delete();
            player.SetData<Checkpoint>(APPLECOL_CURRENT_CP, null);
        }

        private double GetSalary(Player player)
        {
            return Math.Round(player.GetData<int>(APPLECOL_UNITS_DELIVED) * SalaryFactor, 2);
        }

        public void LeaveFromJobArea(Player player)
        {
            throw new NotImplementedException("КТТС");
        }
    }
}
