namespace Application.Exceptions
{
    public class SharedErrorCodes
    {
        private const string Prefix = "shared:";
        public const string RecordNotFound = Prefix + "RecordNotFound";
        public const string RecordAlReadyExist = Prefix + "RecordAlReadyExist";
        public const string Create = Prefix + "NotCreated";
        public const string Delete = Prefix + "NotDelete";
        public const string Update = Prefix + "NotUpdated";
        public const string NotFount = Prefix + "NotFound";
        public const string BadRequest = Prefix + "BadRequest";
        public const string CustomException = Prefix + "CustomException";
        public const string DuplicateObjectException = Prefix + "DuplicateObjectException";
        public const string InProgressExceptions = Prefix + "InProgressExceptions";
        public const string NullValue = Prefix + "NullValue";
    }
}
