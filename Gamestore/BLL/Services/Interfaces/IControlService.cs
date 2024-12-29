using BLL.Models;

namespace BLL.Services.Interfaces;
public interface IControlService<TDTO>
    where TDTO : BaseDto
{
    Task<IEnumerable<TDTO>?> GetAllAsync();

    Task<TDTO?> GetByIdAsync(Guid id);

    Task<TDTO?> CreateAsync(TDTO dto);

    Task<TDTO?> UpdateAsync(TDTO dto);

    Task<bool> RemoveAsync(Guid id);
}