using Ganss.XSS;

namespace Bissues.Services
{
    public class Sanitizer : ISanitizer
    {
        public string Sanitize(string str)
        {
            var sanitizer = new HtmlSanitizer();
            return sanitizer.Sanitize(str);
        }
    }
}