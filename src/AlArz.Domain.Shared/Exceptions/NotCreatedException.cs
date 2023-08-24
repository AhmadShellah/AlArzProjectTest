using Volo.Abp;

namespace Application.Exceptions
{
    public class NotCreatedException : BusinessException
    {
        public NotCreatedException() : base(SharedErrorCodes.Create)
        {

        }
    }

}
