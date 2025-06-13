using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApPac256
{
    public static class ObjectExtensions
    {
        public static void InvokeInternal(this object obj, string methodName) 
        {
            MethodInfo methodInfo = obj.GetType().GetMethod("ChangeLogoSize", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(obj, null);
        }
    }
}
