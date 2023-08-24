using Volo.Abp;

namespace Application.Exceptions
{
    public class CustomException : BusinessException
    {
        public CustomException(string code, string Details) : base(code, Details)
        {
            this.Code = code;
            this.Details = Details;
        }
    }
}
