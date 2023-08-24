using Volo.Abp;

namespace Application.Exceptions
{
    public class NotDeleteException : BusinessException
    {
        public NotDeleteException() : base(SharedErrorCodes.Delete)
        {

        }
    }
}
