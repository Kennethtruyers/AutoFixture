using System;
using System.Globalization;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class IInvalidValueExtensions
    {
        public static void ReflectionAssert(this IInvalidValue invalid, Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            try
            {
                invalid.Assert(action);
                throw new ValueGuardConventionException(
                    string.Format(CultureInfo.CurrentCulture,
                         "The action did not throw the expected exception for the invalid value {0}", invalid.Description));
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException && invalid.IsSatisfiedBy(e.InnerException.GetType()))
                {
                    return;
                }

                throw new ValueGuardConventionException(
                    string.Format(CultureInfo.CurrentCulture,
                         "The action did not throw the expected exception for the invalid value {0}", invalid.Description),
                    e);
            } 
        }
    }
}