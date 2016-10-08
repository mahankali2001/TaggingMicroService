using System;
using System.Data.Entity;
//using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data.Common;
using TaggingEntities = Persistence.Entities;
using CommonEntities = Persistence.Entities.Common;

namespace Persistence.Implementation.Context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbConnection connection): base(connection, false)
        {
            Database.SetInitializer<RepositoryContext>(null);
            Configuration.LazyLoadingEnabled = false;
            this.databaseContext = new DatabaseContext(this);
        }

        private IDatabaseContext databaseContext;
        public IDatabaseContext DatabaseContext
        {
            get 
            {
                return this.databaseContext; 
            }
        }


        #region Public methods
        public IQueryable<T> Read<T>() where T : class
        {
            return this.Set<T>().AsNoTracking();
        }

        public DbSet<T> Write<T>() where T : class
        {
            return this.Set<T>();
        }
        #endregion

        #region Protected methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Tagging");

            #region Register Entities

            //modelBuilder.Entity<TaggingEntities.TEMPLATEEntity>();
            modelBuilder.Entity<TaggingEntities.Tag>();
            modelBuilder.Entity<TaggingEntities.TaggedObject>();
            //modelBuilder.HasDefaultSchema("dbo").Entity<CommonEntities.EmailAttributes>();
            //modelBuilder.Entity<DBEntities.EmailAttributes>();
            #endregion
        }
        #endregion
    }
}