using System;
using System.Reflection;

namespace Models
{
    public class ClassSingleton<T> where T : class
    {
        private static T _instance;
        public static T Instance => _instance ??= CreateInstance();

        protected ClassSingleton() { }

        private static T CreateInstance()
        {
            var cInfo = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[0], new ParameterModifier[0]);
            return (T) cInfo?.Invoke(null);
        }
    }
}