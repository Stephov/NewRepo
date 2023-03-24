using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Entities;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IMainRepository<T> where T : BaseDbEntity, new()
    {
        Task<List<T>> GetAllAsync(params string[] includes);
        Task<T> GetAsync(int id, params string[] includes);
        Task<T> GetAsNoTrackingAsync(int id, params string[] includes);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
