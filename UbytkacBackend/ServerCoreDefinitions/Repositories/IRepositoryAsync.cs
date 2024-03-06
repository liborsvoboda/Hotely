using System.Linq.Expressions;

namespace UbytkacBackend.Services {

    // Generic Repository interface takes two generic paremeters DbContext and model class type
    public interface IRepositoryAsync<DbEntity, Tentity> : IDisposable where DbEntity : hotelsContext where Tentity : class {

        Task<Tentity> GetByIdAsync(int id);

        Task<Tentity> GetAsync(Expression<Func<Tentity, bool>> predicate);

        Task<EntityEntry<Tentity>> AddAsync(Tentity entity);

        Task<EntityEntry<Tentity>> UpdateAsync(Tentity entityToUpdate);

        Task<EntityEntry<Tentity>> DeleteAsync(int id);

        Task<EntityEntry<Tentity>> DeleteAsync(Tentity entityToDelete);

        Task<int> SaveChangesAsync();

        Task<int> TotalAsync(Expression<Func<Tentity, bool>> predicate);

        Task<bool> ExistAsync(Expression<Func<Tentity, bool>> predicate);

        Task<List<Tentity>> GetListAsync(
            Expression<Func<Tentity, bool>>? filter = null,
            Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
            string includeProperties = "", int PageNo = 0, int PageSize = 0);

        Task<List<Tentity>> GetFromSqlAsync(string Sql);
    }
}