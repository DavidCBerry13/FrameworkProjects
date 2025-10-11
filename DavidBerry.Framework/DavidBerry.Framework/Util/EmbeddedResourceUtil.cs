using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DavidBerry.Framework.Util
{
    public static class EmbeddedResourceUtil
    {

        /// <summary>
        /// Reads an embedded resource text file from the specified assembly and returns its contents as a string
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ReadEmbeddedResourceTextFile(this Assembly assembly, string filename)
        {
            var resourceName = $"{assembly.GetName().Name}.{filename}";

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                string contents = reader.ReadToEnd();
                return contents;
            }
        }



        public static byte[] ReadEmbeddedResourceBinaryFile(this Assembly assembly, string filename)
        {
            var resourceName = $"{assembly.GetName().Name}.{filename}";

            using (BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream(resourceName)))
            {
                byte[] bytes = reader.ReadBytes((int)reader.BaseStream.Length);
                return bytes;
            }
        }




    }
}
