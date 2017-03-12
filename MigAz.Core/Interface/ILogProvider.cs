namespace MigAz.Core.Interface
{
    public interface ILogProvider
    {
        void WriteLog(string function, string message);
    }
}
