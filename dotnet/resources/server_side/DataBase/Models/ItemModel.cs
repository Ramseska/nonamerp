using server_side.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace server_side.DataBase.Models
{
    class ItemModel
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; } = -1;
        public string Type { get; set; }
        public int Amount { get; set; }
        public int Slot { get; set; } = -1;
    }
}
