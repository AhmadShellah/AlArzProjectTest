
using Volo.Abp;

namespace Application.Exceptions
{
    public class InProgressExceptions : BusinessException
    {
        public InProgressExceptions()
         : base(SharedErrorCodes.InProgressExceptions)
        {
        }

    }
}
