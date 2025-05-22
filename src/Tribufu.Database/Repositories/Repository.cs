// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore;
using Tribufu.Framework.Interfaces;

namespace Tribufu.Database.Repositories
{
    public class Repository<C, T, K> : IRepository<T, K> where C : DbContext where T : class
    {
        protected readonly C context;

        protected readonly DbSet<T> dbSet;

        public Repository(C context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = context.Set<T>();
        }

        public virtual IList<T> GetAll()
        {
            return [.. dbSet];
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual IList<T> GetPage(uint page, uint limit)
        {
            return dbSet.Skip((int)((page < 1 ? 0 : page - 1) * limit)).Take((int)limit).ToList();
        }

        public virtual async Task<IList<T>> GetPageAsync(uint page, uint limit)
        {
            return await dbSet.Skip((int)((page < 1 ? 0 : page - 1) * limit)).Take((int)limit).ToListAsync();
        }

        public virtual T? GetOne(K key)
        {
            return dbSet.Find(key);
        }

        public virtual async Task<T?> GetOneAsync(K key)
        {
            return await dbSet.FindAsync(key);
        }

        public virtual T? Create(T entity)
        {
            dbSet.Add(entity);

            var result = context.SaveChanges();
            return result > 0 ? entity : null;
        }

        public virtual async Task<T?> CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            var result = await context.SaveChangesAsync();
            return result > 0 ? entity : null;
        }

        public virtual T? Update(T entity)
        {
            dbSet.Update(entity);
            var result = context.SaveChanges();
            return result > 0 ? entity : null;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            var result = await context.SaveChangesAsync();
            return result > 0 ? entity : null;
        }

        public virtual void Delete(K key)
        {
            var entity = dbSet.Find(key);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual async Task DeleteAsync(K key)
        {
            var entity = await dbSet.FindAsync(key);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
