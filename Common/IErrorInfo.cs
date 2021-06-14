namespace HideItBobby.Common
{
    internal interface IErrorInfo
    {
        bool IsError { get; set; }
        int ErrorCount { get; }
    }
}