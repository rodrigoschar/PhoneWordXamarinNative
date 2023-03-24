using System;
using SQLite;

namespace Core.Models
{
    [Table("Items")]
    public class PhoneNumber
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(16)]
        public string phoneNumber { get; set; }
    }
}

