// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using System.Threading.Tasks;
using Tribufu.Framework.Interfaces;

namespace Tribufu.Framework.Services
{
    public class Service<T, K> : IService<T, K> where T : class
    {
        protected readonly IRepository<T, K> _repository;

        public Service(IRepository<T, K> repository)
        {
            _repository = repository;
        }

        public virtual IList<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual IList<T> GetPage(uint page, uint limit)
        {
            return _repository.GetAll();
        }

        public virtual async Task<IList<T>> GetPageAsync(uint page, uint limit)
        {
            return await _repository.GetAllAsync();
        }

        public virtual T GetOne(K id)
        {
            return _repository.GetOne(id);
        }

        public virtual async Task<T> GetOneAsync(K id)
        {
            return await _repository.GetOneAsync(id);
        }

        public virtual T Create(T entity)
        {
            return _repository.Create(entity);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public virtual T Update(T entity)
        {
            return _repository.Update(entity);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public virtual void Delete(K id)
        {
            _repository.Delete(id);
        }

        public virtual async Task DeleteAsync(K id)
        {
            await _repository.DeleteAsync(id);
        }

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
        }
    }
}
