using Crud.Models.Entities;


namespace Crud.Contracts
{
    public interface ISP_EmployeeService
    {
        Task<List<SP_Employee>> GetAllAsync();
        Task<SP_Employee?> GetByIdAsync(int id);
        Task InsertAsync(SP_Employee emp);
        Task UpdateAsync(SP_Employee emp);
        Task DeleteAsync(int id);

    }

}
