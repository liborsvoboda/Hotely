using System.Linq.Expressions;

namespace UbytkacBackend.Services {

    // Generic Repository implementing generic interface takes two generic paremeters DbContext and
    // model class type
    public class Repository<DbEntity, Tentity> : IRepository<DbEntity, Tentity> where DbEntity : hotelsContext where Tentity : class {
        private readonly DbContext _context;
        private readonly DbSet<Tentity> dbSet;

        public Repository(DbEntity context) {
            _context = context;
            dbSet = context.Set<Tentity>();
            Query = dbSet.AsQueryable();
        }

        internal IQueryable<Tentity> Query { get; set; }

        public virtual Tentity GetById(int id) {
            return dbSet.Find(id)!;
        }

        public virtual Tentity Get(Expression<Func<Tentity, bool>> predicate) {
            return Query.FirstOrDefault(predicate);
        }

        public virtual EntityEntry<Tentity> Add(Tentity entity) {
            EntityEntry<Tentity> eentity = dbSet.Add(entity);
            return eentity;
        }

        public virtual EntityEntry<Tentity> Delete(int id) {
            Tentity? entityToDelete = GetById(id)!;
            _context.Entry(entityToDelete).State = EntityState.Deleted;
            if (entityToDelete != null) {
                Delete(entityToDelete!);
            }
            EntityEntry<Tentity> eentity = Delete(entityToDelete!);
            return eentity;
        }

        public virtual EntityEntry<Tentity> Update(Tentity entityToUpdate) {
            EntityEntry<Tentity> eentity = dbSet.Update(entityToUpdate);
            return eentity;
        }

        public virtual EntityEntry<Tentity> Delete(Tentity entityToDelete) {
            EntityEntry<Tentity> eentity = dbSet.Remove(entityToDelete);
            return eentity;
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public virtual int Total(Expression<Func<Tentity, bool>> predicate) {
            return Query.Count(predicate);
        }

        public virtual bool Exist(Expression<Func<Tentity, bool>> predicate) {
            return Query.Any(predicate);
        }

        public virtual List<Tentity> GetList(
            Expression<Func<Tentity, bool>>? filter = null,
            Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
            string includeProperties = "",
            int PageNo = 0, int PageSize = 0) {
            var query = QueryEntity(filter, orderBy, includeProperties);
            if (PageSize + PageNo > 0) {
                return query.Skip(PageNo * PageSize).Take(PageSize).ToList();
            }
            return query.ToList();
        }

        public virtual List<Tentity> GetFromSql(string sql) {
            return dbSet.FromSqlRaw(sql).ToList();
        }

        private IQueryable<Tentity> QueryEntity(
            Expression<Func<Tentity, bool>>? filter = null,
            Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
            string includeProperties = "") {
            if (filter != null) {
                Query = Query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                Query = Query.Include(includeProperty);
            }

            if (orderBy != null) {
                return orderBy(Query);
            }
            else {
                return Query;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}