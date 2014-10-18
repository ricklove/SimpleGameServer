using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GameServerDAL.Entities;

namespace GameServerDAL
{

    public class GameServerContextInitializer : DropCreateDatabaseAlways<GameServerContext>
    {
        protected override void Seed(GameServerContext context)
        {
            //base.Seed(context);
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Values', RESEED, 100);");
        }
    }

    public class GameServerContext : DbContext 
    {
        private static GameServerContext instance;
        public GameServerContext()
        {
            Database.SetInitializer<GameServerContext>(new GameServerContextInitializer());
        }

        public static GameServerContext Instance
        {
            get
            {
                if (instance == null)
                {
                    //System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<GameServerContext, GameServerDAL.Migrations.Configuration>());
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
            //modelBuilder.Entity<Value>()
            //    .HasRequired(t => t.Key);
        } 
    }
}
