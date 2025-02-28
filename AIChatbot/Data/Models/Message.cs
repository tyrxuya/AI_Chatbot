﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIChatbot.Data.Models
{
    [Table("messages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("chatroom_id")]
        public int ChatroomId { get; set; }
        public Chatroom Chatroom { get; set; }

        [Required]
        [Column("content")]
        [MaxLength(300)]
        public string Content { get; set; }

        [Required]
        [Column("message_type")]
        public string Type { get; set; }
    }
}
