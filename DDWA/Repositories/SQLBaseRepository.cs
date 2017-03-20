using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Models;
using System.Data.Entity;

namespace DDWA.Repositories
{
    public abstract class SQLBaseRepository<TEntity> where TEntity: class
    {
        protected DrugsContext db;
        protected DbSet<TEntity> entity;

        public SQLBaseRepository(DrugsContext context)
        {
            this.db = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return entity;
        }

        public void Create(TEntity item)
        {
            entity.Add(item);
        }

        public void Update(TEntity item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}