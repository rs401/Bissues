// Need to DI this and add it to controller constructor
namespace Bissues.Services
{
    public interface ISanitizer
    {
        public string Sanitize(string str);
    }
}