using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IService<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddItemAsync(T item);
        Task DeleteItemAsync(int id);       
        Task UpdateItemAsync(int id, T item);

    }
}
