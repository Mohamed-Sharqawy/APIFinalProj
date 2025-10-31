using APIFinalProj.Context;
using System.Linq.Expressions;

namespace APIFinalProj.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        AppDbContext db;

        public GenericRepository(AppDbContext db)
        {
            this.db = db;
        }

        public List<TEntity> GetAll()
        {
            return db.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public TEntity GetByName(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            db.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
