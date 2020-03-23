using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FluiTec.AppFx.Networking.Mail.Tests.Helpers
{
    public static class ApplicationHelper
    {
        public static string GetApplicationPath()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
                .Replace("file:/", "/")
                .Replace("file:\\", "");
            return exePath;
        }

        public static string GetMailViewPath()
        {
            var path = Path.Combine(GetApplicationPath(), "MailViews");
            return path;
        }

        public static string GetApplicationCodebase()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}
