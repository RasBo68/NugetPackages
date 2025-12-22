using Coeo.FileSystem.Repositories.Database.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coeo.FileSystem.Repositories.Files
{
    public abstract class FileRepositoryBase
    {
        protected const string FILE_PROCESS_ERROR_MESSAGE = "File Read operation failed!";
        protected const string DIRECTORY_DOES_NOT_EXIST_ERROR = "{0} is not a path to a valid directory.";
        protected const string FILE_DOES_NOT_EXIST_ERROR = "{0} is not a path to a valid file.";
        protected async Task<TOut> ExecuteWithHandlingAsync<TOut>(Func<Task<TOut>> action, object? entity = default)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentNullException || ex is NotSupportedException || ex is InvalidCastException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (entity == null)
                    throw new FileException(FILE_PROCESS_ERROR_MESSAGE, ex.InnerException!);
                else
                    throw new FileException(FILE_PROCESS_ERROR_MESSAGE, ex.InnerException!, entity);
            }
        }
        protected TOut ExecuteWithHandling<TOut>(Func<TOut> action, object? entity = default)
        {
            try
            {
                return action();
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentNullException || ex is NotSupportedException || ex is InvalidCastException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (entity == null)
                    throw new FileException(FILE_PROCESS_ERROR_MESSAGE, ex.InnerException!);
                else
                    throw new FileException(FILE_PROCESS_ERROR_MESSAGE, ex.InnerException!, entity);
            }
        }
    }
}
