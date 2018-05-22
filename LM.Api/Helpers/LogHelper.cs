using System.Reflection;
using log4net;

namespace LM.Api.Helpers
{
    public static class LogHelper
    {
        public static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
