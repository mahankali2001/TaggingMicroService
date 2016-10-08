using Persistence.Entities;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data;
using System;


namespace Persistence.Interface.Context
{
    public interface IContext : IDisposable
    {
        int SaveChanges();
        EntityState GetState(object entity);
        void SetState(object entity, EntityState state);
        void SetValue(object oldEntity, object newEntity);
        T Attach<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;

        IDatabaseContext DatabaseContext { get; }
    }
}
