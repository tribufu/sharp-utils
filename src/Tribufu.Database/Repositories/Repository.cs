// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore;
using Tribufu.Framework.Interfaces;

namespace Tribufu.Database.Repositories
{
    public class Repository<C, T, K> : IRepository<T, K> where C : DbContext where T : class
    {
        protected readonly C context;

        private readonly DbSet<T> dbSet;

        public Repository(C context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = context.Set<T>();
        }

        public IList<T> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public IList<T> GetPage(uint page, uint limit)
        {
            return dbSet.Skip((int)((page < 1 ? 0 : page - 1) * limit)).Take((int)limit).ToList();
        }

        public async Task<IList<T>> GetPageAsync(uint page, uint limit)
        {
            return await dbSet.Skip((int)((page < 1 ? 0 : page - 1) * limit)).Take((int)limit).ToListAsync();
        }

        public T GetOne(K key)
        {
            var entity = dbSet.Find(key) ?? throw new KeyNotFoundException($"Entity with key {key} was not found.");
            return entity;
        }

        public async Task<T> GetOneAsync(K key)
        {
            var entity = await dbSet.FindAsync(key) ?? throw new KeyNotFoundException($"Entity with key {key} was not found.");
            return entity;
        }

        public T Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            dbSet.Add(entity);

            var result = context.SaveChanges();
            if (result == 0)
            {
                throw new DbUpdateException("Entity creation failed. No changes were saved.");
            }

            return entity;
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await dbSet.AddAsync(entity);

            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                throw new DbUpdateException("Entity creation failed. No changes were saved.");
            }

            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            dbSet.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            dbSet.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public void Delete(K key)
        {
            var entity = dbSet.Find(key) ?? throw new KeyNotFoundException($"Entity with key {key} was not found.");
            Delete(entity);
        }

        public async Task DeleteAsync(K key)
        {
            var entity = await dbSet.FindAsync(key) ?? throw new KeyNotFoundException($"Entity with key {key} was not found.");
            await DeleteAsync(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
