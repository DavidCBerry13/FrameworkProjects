using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Util
{
    public static class ChecksumExtensions
    {

        /// <summary>
        /// Calculate the chacksum for the specified data using the specified algorithm
        /// </summary>
        /// <param name="data">A byte array of the data to calculate the checksum for</param>
        /// <param name="algorithm">The Chcecksum Algorithm to use</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static byte[] GetChecksum(this byte[] data, ChecksumAlgorithm algorithm)
        {
            ArgumentNullException.ThrowIfNull(data);
            return algorithm switch
            {
                ChecksumAlgorithm.MD5 => System.Security.Cryptography.MD5.HashData(data),
                ChecksumAlgorithm.SHA1 => System.Security.Cryptography.SHA1.HashData(data),
                ChecksumAlgorithm.SHA256 => System.Security.Cryptography.SHA256.HashData(data),
                ChecksumAlgorithm.SHA384 => System.Security.Cryptography.SHA384.HashData(data),
                ChecksumAlgorithm.SHA512 => System.Security.Cryptography.SHA512.HashData(data),
                ChecksumAlgorithm.SHA3_256 => System.Security.Cryptography.SHA3_256.HashData(data),
                ChecksumAlgorithm.SHA3_384 => System.Security.Cryptography.SHA3_384.HashData(data),
                ChecksumAlgorithm.SHA3_512 => System.Security.Cryptography.SHA3_512.HashData(data),
                _ => throw new NotSupportedException($"The specified checksum algorithm '{algorithm}' is not supported."),
            };
        }


        public static byte[] GetChecksum(this string data, ChecksumAlgorithm algorithm)
        {
            ArgumentNullException.ThrowIfNull(data);
            return GetChecksum(Encoding.UTF8.GetBytes(data), algorithm);
        }


        public static byte[] GetChecksum(this Stream stream, ChecksumAlgorithm algorithm)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Copy the data from the source stream to the memory stream
                stream.CopyTo(memoryStream);

                // Convert the memory stream to a byte array
                byte[] byteArray = memoryStream.ToArray();

                return GetChecksum(byteArray, algorithm);
            }
        }

        public static byte[] GetChecksum(this FileInfo file, ChecksumAlgorithm algorithm)
        {
            return File.ReadAllBytes(file.FullName).GetChecksum(algorithm);
        }

    }
}
