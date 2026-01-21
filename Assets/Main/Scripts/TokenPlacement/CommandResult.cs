public class CommandResult
{
    public bool Success { get; }
    public string Error { get; }

    private CommandResult(bool success, string error)
    {
        Success = success;
        Error = error;
    }

    public static CommandResult Ok() {
         return new(true, null);
    }
    public static CommandResult Fail(string error)
    {
        return new(false, error);
    }
}
