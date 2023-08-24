using Volo.Abp;

namespace Application.Exceptions
{
    public class NullValue : BusinessException
    {
        public NullValue() : base(SharedErrorCodes.NullValue)
        {

        }
    }
}
