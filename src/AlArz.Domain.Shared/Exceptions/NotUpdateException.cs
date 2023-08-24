using Volo.Abp;

namespace Application.Exceptions
{
    public class NotUpdateException : BusinessException
    {
        public NotUpdateException() : base(SharedErrorCodes.Update)
        {

        }

    }
}
