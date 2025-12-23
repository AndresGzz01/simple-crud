namespace simple_crud.Library.Models;

/// <summary>
/// Clase que representa el resultado de una operación con un resultado genérico.
/// </summary>
public readonly struct OperationResult<T>
{
    /// <summary>
    /// Obtiene o establece el estado exitoso de la operación.
    /// </summary>
    public bool Success { get; }
    public string? Message { get; }
    public T? Value { get; }
    public Exception? Exception { get; }

    public OperationResult(bool success, string? message = null, T? value = default, Exception? exception = null)
    {
        Success = success;
        Message = message;
        Value = value;
        Exception = exception;
    }
}

/// <summary>
/// Clase que representa el resultado de una operación sin valor específico.
/// </summary>
public readonly struct OperationResult
{
    /// <summary>
    /// Obtiene o establece el estado exitoso de la operación.
    /// </summary>
    public bool Success { get; }
    public string? Message { get; }
    public Exception? Exception { get; }

    public OperationResult(bool success, string? message = null, Exception? exception = null)
    {
        Success = success;
        Message = message;
        Exception = exception;
    }
}