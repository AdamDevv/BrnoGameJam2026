using System.Collections.Generic;

namespace APX.Extra.Misc
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<System.Exception> InnerExceptionsAndSelf(this System.Exception ex)
        {
            while (ex != null)
            {
                yield return ex;
                ex = ex.InnerException;
            }
        }
    }
}
