namespace TALib
{
    public static partial class Core
    {
        public enum MAType
        {
            Sma,
            Ema,
            Wma,
            Dema,
            Tema,
            Trima,
            Kama,
            Mama,
            T3
        }

        public enum RetCode : ushort
        {
            Success,
            LibNotInitialize,
            BadParam,
            GroupNotFound,
            FuncNotFound,
            InvalidHandle,
            InvalidParamHolder,
            InvalidParamHolderType,
            InvalidParamFunction,
            InputNotAllInitialize,
            OutputNotAllInitialize,
            OutOfRangeStartIndex,
            OutOfRangeEndIndex,
            InvalidListType,
            BadObject,
            NotSupported,
            InternalError = 5000,
            UnknownErr = UInt16.MaxValue
        }
    }
}
