using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Shared.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dBSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dBSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            _dBSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dBSet.AddRange(entities);
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }
        public virtual IQueryable<T> GetAll()
        {
            return _dBSet.Where(x => x.IsDeleted == false);
        }

        public IQueryable<T> GetAllDeleted(Expression<Func<T, bool>> predicate)
        {
            return _dBSet.Where(t => t.IsDeleted == true).Where(predicate);
        }

        public IQueryable<T> GetAllDeleted()
        {
            return _dBSet.Where(t => t.IsDeleted == true);
        }

        

        public T GetById(Guid Id)
        {
            return _dBSet.Find(Id);
        }

        public virtual T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dBSet.Where(t => t.IsDeleted == false).AsNoTracking().FirstOrDefault(predicate);
        }

        public void Remove(Guid id)
        {
            T entity = _dBSet.Find(id);
            entity.IsDeleted = true;
            _dBSet.Update(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (T item in entities)
            {
                item.IsDeleted = true;
            }
            _dBSet.UpdateRange(entities);
        }

        public void Update(T entity)
        {
            _dBSet.Update(entity);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            _dBSet.UpdateRange(entities);
        }
    }

}
