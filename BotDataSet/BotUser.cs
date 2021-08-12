using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace BotDataSet
{
    public class BotUser
    {
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }
        public bool NextGame { get; set; }
        public uint Points { get; set; } = 0;
        public bool IsBanned { get; set; } = false;
        public DateTime? UnBanDate { get; set; }
        public bool IsMuted { get; set; } = false;
        public DateTime? UnMutedDate { get; set; }
        public string MuteReason { get; set; }
        public string BanReason { get; set; }
        //[ForeignKey("UserId")]
        public ICollection<Warn> Warns{ get; set; }
    }
    public class Warn
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Reason { get; set; }
        public long UserId { get; set;}
        //[ForeignKey("UserId")]
        public BotUser User { get; set; }

    }
    public class Friends
    {
        [Key]
        public int RSId { get; set; }
        public long UserId { get; set; }
        public long FriendId { get; set; }

        [ForeignKey("UserId")]
        public virtual BotUser User { get; set; }
        [ForeignKey("FriendId")]
        public virtual BotUser Friend {  get; set; }
    }
    public class Payment
    {
        [Key]
        public uint RId { get; set; }
        public uint Sum { get; set; }
        public long UserId { get; set; }
    }
}
