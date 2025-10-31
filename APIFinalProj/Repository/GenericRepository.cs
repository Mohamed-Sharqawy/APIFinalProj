using APIFinalProj.Context;

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
    }
}
