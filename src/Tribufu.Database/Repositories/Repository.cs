// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore;
using Tribufu.Framework.Interfaces;

namespace Tribufu.Database.Repositories
{
    public class Repository<C, T> : IRepository<T> where C : DbContext where T : class
    {
        protected readonly C _context;

        protected readonly DbSet<T> _dbSet;

        public Repository(C context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual IList<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual IList<T> GetPage(uint page, uint limit)
        {
            return _dbSet.Skip((int)((page < 1 ? 0 : page - 1) * limit)).Take((int)limit).ToList();
        }

        public virtual async Task<IList<T>> GetPageAsync(uint page, uint limit)
        {
            return await _dbSet.Skip((int)((page < 1 ? 0 : page - 1) * limit)).Take((int)limit).ToListAsync();
        }

        public virtual T GetById(ulong id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetByIdAsync(ulong id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual T Create(T entity)
        {
            try
            {
                _dbSet.Add(entity);

                var result = _context.SaveChanges();
                if (result == 0)
                {
                    return null;
                }

                return entity;
            }
            catch (Exception)
            {
            }

            return null;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);

                var result = await _context.SaveChangesAsync();
                if (result == 0)
                {
                    return null;
                }

                return entity;
            }
            catch (Exception)
            {
            }

            return null;
        }

        public virtual T Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception)
            {
            }

            return null;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
            }

            return null;
        }

        public virtual void Delete(ulong id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual async Task DeleteAsync(ulong id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
