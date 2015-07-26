namespace GnomeServer
{
    public interface IWebServer
    {
        void Start();

        void Stop();

        /// <summary>
        /// Gets an array containing all currently registered request handlers.
        /// </summary>
        IRequestHandler[] RequestHandlers { get; }
    }
}
