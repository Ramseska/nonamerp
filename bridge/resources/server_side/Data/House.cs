using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using server_side.DBConnection;
using System.Threading.Tasks;

namespace server_side.Data
{
    class House : Script, IComparable<House>
    {
        public int CompareTo(House id)
        {
            if (id == null) return -1;
            else return this.HouseID.CompareTo(id.HouseID);
        }

        static List<House> HouseList = new List<House>();

        public int HouseID;
        public int HouseClass;
        public int HousePrice;
        public int HouseRent;
        public int HouseDays;
        public bool HouseDoors;
        public string HouseOwner;
        public Vector3 HouseEnterPosition;
        public float HouseEnterRotation;
        public bool HouseStatus;
        public int HouseInterior;

        public ColShape HouseColShape;
        public Marker HouseMarker;
        public Blip HouseBlip;
        public TextLabel HouseText;

        public void InitHouses()
        {
            using (MySqlConnection con = MySqlConnector.GetDBConnection())
            {
                con.Open();

                int cc = 0;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM house", con);

                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    if (!read.HasRows)
                        NAPI.Util.ConsoleOutput("[MySQL]: Дома не найдены.");

                    else
                    {
                        while (read.Read())
                        {
                            cc++;

                            House hd = new House();

                            for (int i = 0; i < read.FieldCount; i++)
                            {
                                hd.HouseID = (int)read["h_id"];
                                hd.HouseClass = (int)read["h_class"];
                                hd.HousePrice = (int)read["h_price"];
                                hd.HouseRent = (int)read["h_price"] * (1 / 100);
                                hd.HouseOwner = (string)read["h_owner"];
                                hd.HouseDays = (int)read["h_days"];
                                hd.HouseStatus = Convert.ToString(read["h_owner"]).Contains("None") ? false : true;
                                hd.HouseDoors = (bool)read["h_lock"];
                                hd.HouseInterior = (int)read["h_interior"];
                                hd.HouseEnterPosition = new Vector3((float)read["h_enterposX"], (float)read["h_enterposY"], (float)read["h_enterposZ"]);
                                hd.HouseEnterRotation = (float)read["h_enterposR"];
                            }

                            ColShape cs = NAPI.ColShape.CreateCylinderColShape(hd.HouseEnterPosition, 1.0f, 2f, dimension: 0);
                            cs.SetData("CSHouseID", hd.HouseID);
                            hd.HouseColShape = cs;
                            hd.HouseMarker = NAPI.Marker.CreateMarker(1, new Vector3(hd.HouseEnterPosition.X, hd.HouseEnterPosition.Y, hd.HouseEnterPosition.Z - 1f), new Vector3(), new Vector3(), 1.0f, new Color(139, 201, 131, 100));
                            hd.HouseBlip = NAPI.Blip.CreateBlip(hd.HouseStatus == true ? 375 : 374, hd.HouseEnterPosition, 1f, 2, drawDistance: 15.0f, dimension: 0, shortRange: true, name: $"Дом № {hd.HouseID}");
                            hd.HouseText = NAPI.TextLabel.CreateTextLabel($"House ID: {hd.HouseID}\nHouse Owner: None\nHouse Price: {hd.HousePrice}\nHouse Class: {hd.HouseClass}\nHouse Status: {hd.HouseStatus}", hd.HouseEnterPosition, 3f, 3f, 10, new Color(255, 255, 255));

                            HouseList.Add(hd);
                        }
                        foreach (House h in HouseList)
                            NAPI.Util.ConsoleOutput($"ID: {h.HouseID} - Price: {h.HousePrice}, Class: {h.HouseClass}, Owner: {h.HouseOwner}, Status: {h.HouseStatus} | {h.HouseColShape}");
                        
                        NAPI.Util.ConsoleOutput("[MySQL]: Домов загружено: {0} | Домов в списке: {1}", cc, HouseList.Count);
                    }
                }
            }
        }

        public void CreateHouse(Vector3 HousePosition, float rotation, int HousePrice, int HouseClass)
        {
            int tempID = -1;

            HouseList.Sort();
            if (!HouseList.Exists(x => x.HouseID == 1))
                tempID = 1;
            else
            {
                for (int i = 0; i < HouseList.Count; i++)
                {
                    if (i != HouseList.Count - 1)
                    {
                        if ((HouseList[i].HouseID + 1) != HouseList[i + 1].HouseID)
                        {
                            tempID = HouseList[i].HouseID + 1;
                            break;
                        }
                    }
                    else tempID = HouseList[i].HouseID + 1;
                }
            }           

            HouseList.Sort();

            ColShape tempShape = NAPI.ColShape.CreateCylinderColShape(HousePosition, 1.0f, 2f, dimension: 0);
            tempShape.SetData("CSHouseID", tempID);

            House h = new House()
            {
                HouseID = tempID,
                HouseClass = HouseClass,
                HousePrice = HousePrice,
                HouseRent = HousePrice * (1 / 100),
                HouseOwner = "None",
                HouseEnterPosition = HousePosition,
                HouseEnterRotation = rotation,
                HouseDays = 0,
                HouseStatus = false,
                HouseDoors = false,
                HouseInterior = 0,

                HouseColShape = tempShape,
                HouseMarker = NAPI.Marker.CreateMarker(1, new Vector3(HousePosition.X, HousePosition.Y, HousePosition.Z - 1f), new Vector3(), new Vector3(), 1.0f, new Color(139, 201, 131, 100)),
                HouseBlip = NAPI.Blip.CreateBlip(374, HousePosition, 1f, 2, drawDistance: 15.0f, dimension: 0, shortRange: true, name: $"Дом №{tempID}"),
                HouseText = NAPI.TextLabel.CreateTextLabel($"House ID: {tempID}\nHouse Owner: None\nHouse Price: {HousePrice}\nHouse Class: {HouseClass}\nHouse Status: {HouseStatus}", HousePosition, 3f, 3f, 10, new Color(255, 255, 255))
            };
            
            using(MySqlConnection con = MySqlConnector.GetDBConnection())
            {
                con.Open();
                string query = 
                    $"INSERT INTO `house`(`h_id`, `h_owner`, `h_days`, `h_lock`, `h_price`, `h_class`, `h_interior`, `h_enterposX`, `h_enterposY`, `h_enterposZ`, `h_enterposR`) " +
                    $"VALUES ('{h.HouseID}', '{h.HouseOwner}', '{h.HouseDays}', '{Convert.ToInt32(h.HouseDoors)}', '{h.HousePrice}', '{h.HouseClass}', '{h.HouseInterior}'," +
                    $"'{Math.Round(h.HouseEnterPosition.X, 4).ToString().Replace(",",".")}', " +
                    $"'{Math.Round(h.HouseEnterPosition.Y, 4).ToString().Replace(",", ".")}', " +
                    $"'{Math.Round(h.HouseEnterPosition.Z, 4).ToString().Replace(",", ".")}', " +
                    $"'{Math.Round(h.HouseEnterRotation, 4).ToString().Replace(",", ".")}')";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            HouseList.Add(h);
        }

        public static void OnPlayerEnterColshape(ColShape shape, Client client)
        {
            if (!shape.HasData("CSHouseID")) return;

            if (client.HasData("HouseCreateKD"))
                if (client.GetData("HouseCreateKD") != 0)
                    return;

            House h = HouseList.Where(x => x.HouseID == (int)shape.GetData("CSHouseID")).First();
            NAPI.Util.ConsoleOutput("Data: {0} | HouseID: {1}", shape.GetData("CSHouseID"), h.HouseID);
            NAPI.ClientEvent.TriggerClientEvent(client, "CreateHouseInfoBar", h.HouseID, h.HouseClass, h.HousePrice, h.HouseOwner);
        }

        [Command("hcr")]
        public void CMD_hcr(Client client, int HousePrice, int HouseClass)
        {
            client.SetData("HouseCreateKD", 2);
            CreateHouse(client.Position, client.Rotation.Z, HousePrice, HouseClass);
            client.SendChatMessage("House has been created!");
        }
        [Command("hdl")]
        async public void CMD_hdl(Client client, int houseid)
        {
            if(!HouseList.Exists(x => x.HouseID == houseid))
            {
                client.SendChatMessage("Дом с таким ID не найден!");
                return;
            }
            House h = HouseList.Where(x => x.HouseID == houseid).First();

            h.HouseColShape.Delete();
            h.HouseMarker.Delete();
            h.HouseBlip.Delete();
            h.HouseText.Delete();

            HouseList.Remove(h);

            await Task.Run(() =>
            {
                using(MySqlConnection con = MySqlConnector.GetDBConnection())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"DELETE FROM house WHERE h_id = {houseid}", con);
                    cmd.ExecuteNonQuery();
                }
            });

            client.SendChatMessage($"Дом {houseid} успешно был удален!");
        }
        [Command("htp")]
        public void CMD_htp(Client client, int houseid)
        {
            if (HouseList.Exists(x => x.HouseID == houseid))
            {
                House h = HouseList.Where(x => x.HouseID == houseid).First();
                client.SetData("HouseCreateKD", 2);
                client.Position = h.HouseEnterPosition;
                client.SendChatMessage($"Вы успешно телепортировались к дому с {houseid} ID");
            }
            else client.SendChatMessage("Дом с таким ID не найден!");
        }
    }
}
