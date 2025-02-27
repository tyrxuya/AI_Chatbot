using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace AIChatbot.Data.Models
{
    [Table("chatrooms")]
    public class Chatroom
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Message> Messages { set { Messages = value; } }
    }
}
