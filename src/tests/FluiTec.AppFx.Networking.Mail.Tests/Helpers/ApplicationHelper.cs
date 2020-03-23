using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FluiTec.AppFx.Networking.Mail.Tests.Helpers
{
    public static class ApplicationHelper
    {
        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}
