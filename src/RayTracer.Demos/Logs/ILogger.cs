namespace RayTracer.Demos.Logs
{
    public interface ILogger
    {
        void Write(string value);
        void WriteLine(string value);
        void WriteLine();
    }
}
