using simple_crud.Api.Models;

using System.Data.Common;

using Dapper;
using simple_crud.Library;

namespace simple_crud.Api.Infrastructure;

public class MariaDbRepository : IDatabaseRepository
{
    readonly DbConnection dbConnection;

    public MariaDbRepository(DbConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public async Task<OperationResult<Usuario?>> GetUsuarioById(uint id)
    {
        try
        {
            await dbConnection.OpenAsync();

            var query = "SELECT Id, Username FROM usuario WHERE Id = @id LIMIT 1";
            var usuario = await dbConnection.QuerySingleOrDefaultAsync<Usuario>(query, new { id });

            return new OperationResult<Usuario?>(true, value: usuario);
        }
        catch (Exception ex)
        {
            return new OperationResult<Usuario?>(false, $"Error al obtener el usuario con Id {id}.", exception: ex);
        }
        finally
        {
            dbConnection.Close();
        }
    }
}
