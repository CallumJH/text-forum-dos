namespace Whispers.Chat.Core.Generated.Services;

/// <summary>
/// Higher-order function utility for standardizing error handling across domain services.
/// Provides consistent try/catch wrapping, logging, and error result handling.
/// </summary>
public static class ServiceErrorHandler
{
  /// <summary>
  /// Executes an async operation with standardized error handling for Result return types
  /// </summary>
  /// <param name="operation">The async operation to execute</param>
  /// <param name="logger">Logger instance for error logging</param>
  /// <param name="operationName">Name of the operation for logging context</param>
  /// <param name="fallbackErrorMessage">Default error message if operation fails</param>
  /// <param name="logParameters">Additional parameters to include in log messages</param>
  /// <returns>Result of the operation or error result</returns>
  public static async Task<Result> ExecuteAsync(
      Func<Task<Result>> operation,
      ILogger logger,
      string operationName,
      string fallbackErrorMessage,
      params object[] logParameters)
  {
    logger.LogInformation("Starting {OperationName} with parameters: {Parameters}",
        operationName, logParameters);

    try
    {
      var result = await operation();

      if (result.IsSuccess)
      {
        logger.LogInformation("Successfully completed {OperationName}", operationName);
      }
      else
      {
        logger.LogWarning("{OperationName} failed with error: {Error}", operationName, PrintErrors(result));
      }

      return result;
    }
    catch (DomainException ex)
    {
      logger.LogWarning("Domain validation failed during {OperationName}: {Message}",
          operationName, ex.Message);
      return Result.Error(ex.Message);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Unexpected error during {OperationName}", operationName);
      return Result.Error(fallbackErrorMessage);
    }
  }

  /// <summary>
  /// Executes an async operation with standardized error handling for Result&lt;T&gt; return types
  /// </summary>
  /// <typeparam name="T">Type of the result value</typeparam>
  /// <param name="operation">The async operation to execute</param>
  /// <param name="logger">Logger instance for error logging</param>
  /// <param name="operationName">Name of the operation for logging context</param>
  /// <param name="fallbackErrorMessage">Default error message if operation fails</param>
  /// <param name="logParameters">Additional parameters to include in log messages</param>
  /// <returns>Result&lt;T&gt; of the operation or error result</returns>
  public static async Task<Result<T>> ExecuteAsync<T>(
      Func<Task<Result<T>>> operation,
      ILogger logger,
      string operationName,
      string fallbackErrorMessage,
      params object[] logParameters)
  {
    logger.LogInformation("Starting {OperationName} with parameters: {Parameters}",
        operationName, logParameters);

    try
    {
      var result = await operation();

      if (result.IsSuccess)
      {
        logger.LogInformation("Successfully completed {OperationName}", operationName);
      }
      else
      {
        logger.LogWarning("{OperationName} failed with error: {Error}", operationName, PrintErrors(result));
      }

      return result;
    }
    catch (DomainException ex)
    {
      logger.LogWarning("Domain validation failed during {OperationName}: {Message}",
          operationName, ex.Message);
      return Result<T>.Error(ex.Message);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Unexpected error during {OperationName}", operationName);
      return Result<T>.Error(fallbackErrorMessage);
    }
  }

  /// <summary>
  /// Executes an async operation with standardized error handling that doesn't return a Result type
  /// </summary>
  /// <typeparam name="T">Type of the return value</typeparam>
  /// <param name="operation">The async operation to execute</param>
  /// <param name="logger">Logger instance for error logging</param>
  /// <param name="operationName">Name of the operation for logging context</param>
  /// <param name="logParameters">Additional parameters to include in log messages</param>
  /// <returns>The result of the operation</returns>
  /// <exception cref="Exception">Re-throws the original exception after logging</exception>
  public static async Task<T> ExecuteWithLoggingAsync<T>(
      Func<Task<T>> operation,
      ILogger logger,
      string operationName,
      params object[] logParameters)
  {
    logger.LogInformation("Starting {OperationName} with parameters: {Parameters}",
        operationName, logParameters);

    try
    {
      var result = await operation();
      logger.LogInformation("Successfully completed {OperationName}", operationName);
      return result;
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error during {OperationName}", operationName);
      throw; // Re-throw to maintain original stack trace
    }
  }

  /// <summary>
  /// Executes a synchronous operation with standardized error handling for Result return types
  /// </summary>
  /// <param name="operation">The operation to execute</param>
  /// <param name="logger">Logger instance for error logging</param>
  /// <param name="operationName">Name of the operation for logging context</param>
  /// <param name="fallbackErrorMessage">Default error message if operation fails</param>
  /// <param name="logParameters">Additional parameters to include in log messages</param>
  /// <returns>Result of the operation or error result</returns>
  public static Result Execute(
      Func<Result> operation,
      ILogger logger,
      string operationName,
      string fallbackErrorMessage,
      params object[] logParameters)
  {
    logger.LogInformation("Starting {OperationName} with parameters: {Parameters}",
        operationName, logParameters);

    try
    {
      var result = operation();

      if (result.IsSuccess)
      {
        logger.LogInformation("Successfully completed {OperationName}", operationName);
      }
      else
      {
        logger.LogWarning("{OperationName} failed with error: {Error}", operationName, PrintErrors(result));
      }

      return result;
    }
    catch (DomainException ex)
    {
      logger.LogWarning("Domain validation failed during {OperationName}: {Message}",
          operationName, ex.Message);
      return Result.Error(ex.Message);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Unexpected error during {OperationName}", operationName);
      return Result.Error(fallbackErrorMessage);
    }
  }

  /// <summary>
  /// Collect all result errors and present them in a vertical list
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="errorList"></param>
  /// <returns></returns>
  private static string PrintErrors<T>(Result<T> errorList)
  {
    return string.Join("\n", errorList.Errors);
  }
}
