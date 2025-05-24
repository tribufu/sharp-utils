// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tribufu.Framework.Interfaces;

namespace Tribufu.Framework.Services
{
    public class Service<T, K> : IService<T, K> where T : class
    {
        protected readonly IRepository<T, K> repository;

        public Service(IRepository<T, K> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual IList<T> GetAll()
        {
            return repository.GetAll();
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual IList<T> GetPage(uint page, uint limit)
        {
            return repository.GetPage(page, limit);
        }

        public virtual async Task<IList<T>> GetPageAsync(uint page, uint limit)
        {
            return await repository.GetPageAsync(page, limit);
        }

        public virtual T? GetOne(K id)
        {
            return repository.GetOne(id);
        }

        public virtual async Task<T?> GetOneAsync(K id)
        {
            return await repository.GetOneAsync(id);
        }

        public virtual T? Create(T entity)
        {
            return repository.Create(entity);
        }

        public virtual async Task<T?> CreateAsync(T entity)
        {
            return await repository.CreateAsync(entity);
        }

        public virtual T? Update(T entity)
        {
            return repository.Update(entity);
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            return await repository.UpdateAsync(entity);
        }

        public virtual void Delete(K id)
        {
            repository.Delete(id);
        }

        public virtual async Task DeleteAsync(K id)
        {
            await repository.DeleteAsync(id);
        }

        public virtual void Delete(T entity)
        {
            repository.Delete(entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await repository.DeleteAsync(entity);
        }
    }
}
