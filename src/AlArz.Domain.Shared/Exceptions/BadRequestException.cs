using Volo.Abp;

namespace Application.Exceptions
{
    public class BadRequestException : BusinessException
    {

        public BadRequestException() : base(SharedErrorCodes.BadRequest)
        {

        }
    }
}
