using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Models
{
    public class ConversationLog
    {
        [Key]
        public Guid Id { get; set; }
        [Key]
        public Guid ConversationId { get; set; } 
        [Key]
        public Guid BotId { get; set; }
        public string Message { get; set; }
        public DateTime MessageAt { get; set; }
        public string ConversationContext { get; set; }
    }
}
