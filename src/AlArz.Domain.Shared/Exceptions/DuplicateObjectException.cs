using Volo.Abp;

namespace Application.Exceptions
{
    public class DuplicateObjectException : BusinessException
    {
        public DuplicateObjectException() : base(SharedErrorCodes.DuplicateObjectException)
        {

        }
    }
}
