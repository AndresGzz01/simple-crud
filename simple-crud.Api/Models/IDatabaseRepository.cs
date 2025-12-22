using simple_crud.Library;

namespace simple_crud.Api.Models;

public interface IDatabaseRepository
{
    Task<OperationResult<Usuario?>> GetUsuarioById(uint id);
}
