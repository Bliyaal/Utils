using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Utils
{
    public static class StringCompressor
    {
        /// <summary>
        /// Compresses a string
        /// </summary>
        /// <param name="value">String to compress</param>
        /// <returns></returns>
        public static string CompressString(string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);

            using (var output = new MemoryStream())
            using (var compressionStream = new DeflateStream(output, CompressionMode.Compress))
            {
                compressionStream.Write(buffer,
                                        0,
                                        buffer.Length);

                return Convert.ToBase64String(output.ToArray());
            }
        }

        /// <summary>
        /// Decompresses a string
        /// </summary>
        /// <param name="value">String to decompress</param>
        /// <returns></returns>
        public static string DecompressString(string value)
        {
            using (var input = new MemoryStream(Convert.FromBase64String(value)))
            using (var output = new MemoryStream())
            using (var decompressionStream = new DeflateStream(input, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(output);
                return Encoding.UTF8.GetString(output.ToArray());
            }
        }
    }
}