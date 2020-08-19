using GTANetworkAPI;
using server_side.Data;
using server_side.DataBase.Models;
using server_side.DBConnection;
using server_side.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using server_side.Items;



namespace server_side.InventorySystem
{
    class LootBag : Script
    {
        public int Id { get; private set; }
        public int Owner { get; private set; }
        private ColShape Colshape { get; set; }
        private TextLabel Label { get; set; }
        private GTANetworkAPI.Object Prop { get; set; }
        public ItemsStorage Items { get; private set; }

        public override string ToString() 
        {
            return $"ID: {this.Id}, Owner: {this.Owner}, Label: {this.Label}";
        }

        public static readonly List<LootBag> LootBagsList = new List<LootBag>();

        public LootBag() {}

        [RemoteEvent("sCreateLootBag")]
        public void Create(Player player)
        {
            LootBag bag = new LootBag();

            LootBagsList.Add(bag);

            Vector3 pos = Utils.UtilityFuncs.GetPosFrontOfPlayer(player, 0.5);

            bag.Owner = new PlayerInfo(player).GetDbID();
            bag.Prop = NAPI.Object.CreateObject(1585260068, new Vector3(pos.X, pos.Y, pos.Z-0.95), new Vector3(90, player.Heading, 0), 255, player.Dimension);
            bag.Colshape = NAPI.ColShape.CreateSphereColShape(player.Position, 1.5f, player.Dimension);
            bag.Label = NAPI.TextLabel.CreateTextLabel("Press: \"E\"", new Vector3(pos.X, pos.Y, pos.Z-0.8), 4f, 3f, 8, new Color(255, 255, 255), dimension: player.Dimension);

            bag.Id = LootBagsList.IndexOf(bag);

            bag.Colshape.SetData("LootBagColShapeId", bag.Id);

            Console.WriteLine($"Created loot bag: " + bag.ToString());
        }

        public static void EnterLootBagColShape(ColShape shape, Player player)
        {
            if(!shape.HasData("LootBagColShapeId")) return;

            player.SetSharedData("InLootBagColShape", shape.GetData<int>("LootBagColShapeId"));
        }

        public static void ExitLootBagColShape(ColShape shape, Player player)
        {
            if(!shape.HasData("LootBagColShapeId")) return;

            player.ResetSharedData("InLootBagColShape");
        }

        private void Destroy(int id)
        {
            LootBag bag = LootBagsList.Where(x => x.Id == id).FirstOrDefault();

            if(bag == null) return;

            bag.Prop.Delete();
            bag.Colshape.Delete();
            bag.Label.Delete();

            LootBagsList.Remove(bag);
        }

        [RemoteEvent("sOpenLootBag")]
        public void Open(Player player, int id)
        {
            Console.WriteLine($"Player: {player} | id: {id}");
        }

        [RemoteEvent("sCloseLootBag")]
        public void Close(Player player)
        {

        }

        [RemoteEvent("sLootBagAddItem")]
        public void AddItem(Player player, int id)
        {
            
        }

        [RemoteEvent("sLootBagTaleItem")]
        public void TakeItem(Player player, int id)
        {
            LootBag bag = LootBagsList.Where(x => x.Id == id).FirstOrDefault();
            
            if(bag == null) return;

            // other
            
            if(bag.Items.ItemsList.Count == 0)
            {
                Close(player);
                Destroy(id);
            }
        }
    }
}