public class CmdResult
{
    public bool Success { get; }
    public string Error { get; }

    private CmdResult(bool success, string error)
    {
        Success = success;
        Error = error;
    }

    public static CmdResult Ok() {
         return new(true, null);
    }
    public static CmdResult Fail(string error)
    {
        return new(false, error);
    }
}
