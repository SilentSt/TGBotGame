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
        [Required]
        public string UserName { get; set; }
        public uint Points { get; set; } = 0;
        public bool IsBanned { get; set; } = false;
        public bool IsMuted { get; set; } = false;
        public byte WarnCount { get; set; } = 0;
        public string MuteReason { get; set; }
        public string BanReason { get; set; }

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
}
