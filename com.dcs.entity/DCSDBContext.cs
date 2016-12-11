using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace com.dcs.entity
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DCSDBContext:DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<CustomItem> CustomItems { get; set; }
        public DbSet<CustomItemValue> CustomItemValues { get; set; }
    }
}
