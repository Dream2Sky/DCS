using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.Types;

namespace com.dcs.entity
{
    public class Entity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public MySqlDateTime InsertTime { get; set; }

        [Required]
        public MySqlDateTime UpdateTime { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
