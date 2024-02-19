using System.Linq.Expressions;

namespace UbytkacBackend.Services {

    // Generic Repository interface takes two generic paremeters DbContext and model class type
    public interface IRepository<DbEntity, Tentity> : IDisposable where DbEntity : hotelsContext where Tentity : class {

        Tentity GetById(int id);

        Tentity Get(Expression<Func<Tentity, bool>> predicate);

        EntityEntry<Tentity> Add(Tentity entity);

        EntityEntry<Tentity> Update(Tentity entityToUpdate);

        EntityEntry<Tentity> Delete(int id);

        EntityEntry<Tentity> Delete(Tentity entityToDelete);

        void SaveChanges();

        int Total(Expression<Func<Tentity, bool>> predicate);

        bool Exist(Expression<Func<Tentity, bool>> predicate);

        List<Tentity> GetList(
           Expression<Func<Tentity, bool>>? filter = null,
           Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
           string includeProperties = "", int PageNo = 0, int PageSize = 0);

        List<Tentity> GetFromSql(string Sql);
    }
}