using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    // generic constraint
    // T herşey olmamalı , entitiy yani db nesnesi olmalı :  IEntity :  IEntity ve ya IEntity implement olan bir nesne olabilir
    // new() :  IEntiy olamaz artık, ondan türeyen class yani new lenen bir şey olabilir artık
    // class : referans tip
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        // GetAll içindeki linq sayesinde buna gerek kalmadı
        //List<T> GetAllByCategory(int CategoryId);
    }
}
