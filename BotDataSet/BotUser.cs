using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace BotDataSet
{
    public class BotUser
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public uint Points { get; set; } = 0;
        public bool IsBanned { get; set; } = false;
        public bool IsMuted { get; set; } = false;
        public byte WarnCount { get; set; } = 0;
    }
}
