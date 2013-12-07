using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SmartElk.Antler.Core.Common.Extensions;

namespace SmartElk.Antler.Core.Common.Reflection
{
    public static class From
    {
        public static IEnumerable<Assembly> ThisAssembly
        {
            get
            {
                return Assembly.GetCallingAssembly().AsEnumerable();
            }
        }

        public static IEnumerable<Assembly> ExecutingAssembly
        {
            get
            {
                return Assembly.GetExecutingAssembly().AsEnumerable();
            }
        }

        public static IEnumerable<Assembly> AllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
       
        public static IEnumerable<Assembly> AssemblyWithType<T>()
        {
            return typeof (T).Assembly.AsEnumerable();
        }

        public static IEnumerable<FileInfo> AllFilesIn(string path, bool recursively = false)
        {
            return new DirectoryInfo(path)
                .EnumerateFiles("*.*", recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<FileInfo> AllFilesInApplicationFolder()
        {
            var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            return AllFilesIn(Path.GetDirectoryName(uri.LocalPath));
        }
    }
}