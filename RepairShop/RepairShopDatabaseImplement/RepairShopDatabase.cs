using System;
using System.Collections.Generic;
using System.Text;
using RepairShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace RepairShopDatabaseImplement
{
    public class RepairShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-AP669A3;Initial Catalog=RepairShopDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Component> Components { set; get; }
        public virtual DbSet<Repair> Repairs { set; get; }
        public virtual DbSet<RepairComponent> RepairComponents { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<Implementer> Implementers { set; get; }
    }
}
