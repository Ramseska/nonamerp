using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;

namespace server_side.Data
{
    class House : Script, IComparable<House>
    {
        public int CompareTo(House id)
        {
            if (id == null) return -1;
            else return this.HouseID.CompareTo(id.HouseID);
        }
        public override string ToString() => "ID: " + HouseID + " | " + "Class: " + HouseClass + " | " + "Owner: " + HouseOwner + " | " + "Status: " + HouseStatus.ToString();
        public override int GetHashCode() => HouseID;

        static List<House> HouseList = new List<House>();

        public int HouseID;
        public int HouseClass;
        public int HousePrice;
        private int HouseRent;
        public int HouseDays;
        public bool HouseDoors;
        public string HouseOwner;
        public Vector3 HouseEnterPosition;
        private bool HouseStatus;
        private int HouseInterior;

        private ColShape HouseColShape;
        private Marker HouseMarker;
        private Blip HouseBlip;
        private TextLabel HouseText;


        public void InitHouses()
        {
            throw new NotImplementedException();
        }

        public void CreateHouse(Vector3 HousePosition, int HousePrice, int HouseClass = 1)
        {
            int tempID = -1;

            HouseClass = new Random().Next(0, 5);

            ColShape tempShape = NAPI.ColShape.CreatCircleColShape(HousePosition.X, HousePosition.Y, 1.5f);

            HouseList.Sort();

            if (HouseList.Count == 0) tempID = 0;
            else if(HouseList.Count == 1)
            {
                if (HouseList[0].HouseID == 0) tempID = 1;
                else tempID = 0;
            }
            else
            {
                for(int i = 0; i < HouseList.Count; i++)
                {
                    if (i != HouseList.Count - 1)
                    {
                        if ((HouseList[i].HouseID + 1) != HouseList[i + 1].HouseID)
                        {
                            tempID = HouseList[i].HouseID + 1;
                            break;
                        }
                    }
                    else if (HouseList[i].HouseID == HouseList.Count - 1) tempID = HouseList[i].HouseID + 1;
                }
            }

            HouseList.Sort();

            tempShape.SetData("CSHouseID", tempID);

            HouseList.Add(new House()
            {
                HouseID = tempID,
                HouseClass = HouseClass,
                HousePrice = HousePrice,
                HouseRent = HousePrice * (1 / 100),
                HouseOwner = "None",
                HouseEnterPosition = HousePosition,
                HouseDays = 0,
                HouseStatus = false,
                HouseDoors = false,
                HouseInterior = 0,

                HouseColShape = tempShape,
                HouseMarker = NAPI.Marker.CreateMarker(1, new Vector3(HousePosition.X, HousePosition.Y, HousePosition.Z-1f), new Vector3(), new Vector3(), 1.0f, new Color(139,201,131,100)),
                HouseBlip = NAPI.Blip.CreateBlip(374, HousePosition, 1f, 2, drawDistance: 100.0f, dimension: 0),
                HouseText = NAPI.TextLabel.CreateTextLabel($"House ID: {tempID}\nHouse Owner: None\nHouse Price: {HousePrice}\nHouse Class: {HouseClass}\nHouse Status: {HouseStatus}", HousePosition, 3f, 3f, 10, new Color(255, 255, 255))
            });
        }

        public static void OnPlayerEnterColshape(ColShape shape, Client client)
        {
            if (!shape.HasData("CSHouseID")) return;

            House h = HouseList.Where(x => x.HouseID == (int)shape.GetData("CSHouseID")).First();
            Console.WriteLine("Data: {0} | HouseID: {1}", shape.GetData("CSHouseID"), h.HouseID);
            NAPI.ClientEvent.TriggerClientEvent(client, "CreateHouseInfoBar", h.HouseID, h.HouseClass, h.HousePrice, h.HouseOwner);
        }

        [Command("crh")]
        public void CMD_crh(Client client, int HousePrice)
        {
            CreateHouse(Utilities.UtilityFuncs.GetPosFrontOfPlayer(client, 3.0), HousePrice);
            client.SendChatMessage("House has been created!");
        }
    }
}
