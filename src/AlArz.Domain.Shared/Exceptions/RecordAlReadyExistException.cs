using Volo.Abp;

namespace Application.Exceptions
{
    public class RecordAlReadyExistException : BusinessException
    {
        public RecordAlReadyExistException() : base(SharedErrorCodes.RecordAlReadyExist)
        {

        }
    }
}
