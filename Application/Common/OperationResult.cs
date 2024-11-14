using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
  public class OperationResult
  {
    public bool Success { get; }

    public string Message { get; }

    public OperationResult(bool success, string message)
    {
      Success = success;
      Message = message;
    }

    public static OperationResult SuccessResult(string message = "Operation completed successfully.")
    {
      return new OperationResult(true, message);
    }

    public static OperationResult ErrorResult(string message)
    {
      return new OperationResult(false, message);
    }
  }
}
