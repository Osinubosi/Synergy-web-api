using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.Repository.Interfaces
{
  public  class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly IDbContext _context;
        private DbSet<TEntity> DbEntities { set; get; }
        public IQueryable<TEntity> Table => Entities;
        public GenericRepository(IDbContext context)
        {
            _context = context;
        }



        public async Task<TEntity> GetEnitityById(object id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            await Entities.AddAsync(entity);
            return entity;
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            await Entities.AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (_context.Entry(entity).State.Equals(EntityState.Detached))
                _context.Set<TEntity>().Attach(entity);

            _context.Set<TEntity>().Remove(entity);
        }

       

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            IEnumerable<TEntity> enumerable = entities as TEntity[] ?? entities.ToArray();
            if (_context.Entry(entities).State.Equals(EntityState.Detached))
                _context.Set<IEnumerable<TEntity>>().Attach(enumerable);

            _context.Set<TEntity>().RemoveRange(enumerable);
        }

       

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.SaveChanges();
        }

       

        public void UpdateAll(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (_context.Entry(entity).State.Equals(EntityState.Detached))
                _context.Set<TEntity>().Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
        }

        public TEntity Create(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);

            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            await Entities.AddAsync(entity);

            return entity;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

      

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(orderBy: orderBy, includeProperties: includeProperties, skip: skip, take: take);
        }

        public IAsyncEnumerable<TEntity> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryableAsync(orderBy: orderBy, includeProperties: includeProperties, skip: skip, take: take);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        {
            return GetQueryable(filter: filter, orderBy: orderBy, includeProperties: includeProperties).FirstOrDefault();
        }

        //public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        //{
        //    var query = await GetQueryableAsync(filter: filter, orderBy: orderBy, includeProperties: includeProperties);
        //}

        private DbSet<TEntity> Entities
        {
            get
            {
                if (DbEntities == null)
                {
                    DbEntities = _context.Set<TEntity>();
                }
                return DbEntities;
            }
        }
        protected virtual IEnumerable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.AsEnumerable();
        }

        protected virtual IAsyncEnumerable<TEntity> GetQueryableAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.AsAsyncEnumerable();
        }
    }
}
