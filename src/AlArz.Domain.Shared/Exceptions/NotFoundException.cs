
using Volo.Abp;

namespace Application.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException() : base(SharedErrorCodes.NotFount)
        {

        }
    }
}
