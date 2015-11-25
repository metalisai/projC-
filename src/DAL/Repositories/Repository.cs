using System;
using System.Collections.Generic;
using DAL.Interfaces;
using Microsoft.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity.ChangeTracking;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; }

        //Constructor, requires dbContext as dependency
        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            DbContext = dbContext as DbContext;
            //get the dbset from context
            if (DbContext != null) DbSet = DbContext.Set<T>();
        }

        public List<T> All
        {
            get { return DbSet.ToList(); }
        }

        // TODO: replace with DbSet.Find once its added to EF7
        public T GetById(object id)
        {
            var context = DbContext;

            var entityType = context.Model.FindEntityType(typeof(T));
            var key = entityType.FindPrimaryKey();

            var entries = context.ChangeTracker.Entries<T>();

            var i = 0;
            foreach (var property in key.Properties)
            {
                entries = entries.Where(e => e.Property(property.Name).CurrentValue == id);
                i++;
            }

            var entry = entries.FirstOrDefault();
            if (entry != null)
            {
                // Return the local object if it exists.
                return entry.Entity;
            }

            // TODO: Build the real LINQ Expression
            // set.Where(x => x.Id == keyValues[0]);
            var parameter = Expression.Parameter(typeof(T), "x");
            var query = DbSet.Where((Expression<Func<T, bool>>)
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "Id"),
                        Expression.Constant(id))));

            // Look in the database
            return query.FirstOrDefault();
        }

        public void Add(T entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public void Update(T entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public void Delete(object id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);
        }

        public void Dispose()
        {
        }
    }
}
