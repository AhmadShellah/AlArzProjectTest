using Volo.Abp;

namespace Application.Exceptions
{
    public class RecordNotExistsException : BusinessException
    {
        public RecordNotExistsException()
            : base(SharedErrorCodes.RecordNotFound)
        {
        }



    }
}