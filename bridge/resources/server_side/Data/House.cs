using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;

namespace server_side.Data
{
    class House : Script
    {
        static List<House> HouseList = new List<House>();

        public int HouseID;
        public int HouseClass;
        public int HousePrice;
        private int HouseRent;
        public int HouseDays;
        public string HouseOwner;
        public Vector3 HouseEnterPosition;
        private bool HouseStatus;

        private ColShape HouseColShape;
        private Checkpoint HouseCheckpoint;
        private Blip HouseBlip;
        private TextLabel HouseText;


        public void InitHouses()
        {

        }

        public void CreateHouse(Vector3 HousePosition, int HousePrice, int HouseClass = 1)
        {
            int freeID = HouseList.Any() ? (HouseList.Max(x => x.HouseID) + 1) : 0;

            ColShape tempShape = NAPI.ColShape.CreatCircleColShape(HousePosition.X, HousePosition.Y, 1.5f);
            tempShape.SetData("CSHouseID", freeID);

            HouseList.Add(new House()
            {
                HouseID = freeID,
                HouseClass = HouseClass,
                HousePrice = HousePrice,
                HouseRent = HousePrice * (1 / 100),
                HouseOwner = "None",
                HouseEnterPosition = HousePosition,
                HouseDays = 0,
                HouseStatus = false,

                HouseColShape = tempShape,
                HouseCheckpoint = NAPI.Checkpoint.CreateCheckpoint(CheckpointType.Cyclinder, new Vector3(HousePosition.X, HousePosition.Y, HousePosition.Z - 5f), new Vector3(0, 0, 0), 1.5f, new Color(255, 0, 0)),
                HouseBlip = NAPI.Blip.CreateBlip(374, HousePosition, 1f, 2),
                HouseText = NAPI.TextLabel.CreateTextLabel($"House ID: {freeID}\nHouse Owner: None\nHouse Price: {HousePrice}\nHouse Class: {HouseClass}\nHouse Status: {HouseStatus}", HousePosition, 3f, 3f, 10, new Color(255, 255, 255))
            });
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnPlayerEnterColshape(ColShape shape, Client client)
        {
            if(shape.HasData("CSHouseID"))
                client.SendChatMessage($"Y enter in {shape.GetData("CSHouseID")} colshape");
        }

        [Command("crh")]
        public void CMD_crh(Client client, int HousePrice)
        {
            CreateHouse(client.Position, HousePrice);
            client.SendChatMessage("House has been created!");
        }
    }
}
