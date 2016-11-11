using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace com.dcs.entity
{
    public class SuperAdmin:Entity
    {
        [Required]
        [MaxLength(20)]
        public string Account { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
