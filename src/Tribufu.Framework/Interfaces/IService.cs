// Copyright (c) Tribufu. All Rights Reserved.
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tribufu.Framework.Interfaces
{
    public interface IService<T, K> where T : class
    {
        public IList<T> GetAll();

        public Task<IList<T>> GetAllAsync();

        public IList<T> GetPage(uint page, uint limit = DatabaseLimits.PAGINATION);

        public Task<IList<T>> GetPageAsync(uint page, uint limit = DatabaseLimits.PAGINATION);

        public T GetOne(K id);

        public Task<T> GetOneAsync(K id);

        public T Create(T entity);

        public Task<T> CreateAsync(T entity);

        public T Update(T entity);

        public Task<T> UpdateAsync(T entity);

        public void Delete(K id);

        public Task DeleteAsync(K id);

        public void Delete(T entity);

        public Task DeleteAsync(T entity);
    }
}
