﻿using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Service.Pattern
{
    public interface IService<TEntity> where TEntity : IObjectState
    {
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertOrUpdateGraph(TEntity entity);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        IQueryFluent<TEntity> Query();
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryable<TEntity> Queryable();
    }
}
