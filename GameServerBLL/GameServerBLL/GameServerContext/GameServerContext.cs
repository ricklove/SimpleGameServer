using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GameServerBLL.Entities;

namespace GameServerBLL.GameServerContext
{
    public class GameServerContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserClient> UserClients { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map one-to-zero relationship 
            modelBuilder.Entity<UserClient>()
                .HasRequired(t => t.User);

            // Map one-to-zero relationship 
            modelBuilder.Entity<UserSession>()
                .HasRequired(t => t.UserClient);
        } 
    }
}
