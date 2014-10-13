using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GameServerDAL.Entities;

namespace GameServerDAL
{
    public class GameServerContext : DbContext 
    {
        private static GameServerContext instance;
        private GameServerContext() { }

        public static GameServerContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameServerContext();
                }
                return instance;
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClient> UserClients { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        public DbSet<Key> Keys { get; set; }
        public DbSet<Value> Values { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map one-to-zero relationship 
            modelBuilder.Entity<UserClient>()
                .HasRequired(t => t.User);

            // Map one-to-zero relationship 
            modelBuilder.Entity<UserSession>()
                .HasRequired(t => t.UserClient);

            // Map one-to-zero relationship 
            modelBuilder.Entity<Value>()
                .HasRequired(t => t.Key);
        } 
    }
}
