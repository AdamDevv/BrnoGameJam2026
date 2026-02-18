using System.IO;
using System.IO.Compression;
using System.Text;

namespace APX.Extra.Misc
{
    public static class CompressionExtensions
    {
        public static string CompressToBase64(this string data) { return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(data).Compress()); }

        public static string DecompressFromBase64(this string data) { return Encoding.UTF8.GetString(System.Convert.FromBase64String(data).Decompress()); }

        public static byte[] CompressToByteArray(this string data) { return Encoding.UTF8.GetBytes(data).Compress(); }

        public static string DecompressFromByteArray(this byte[] data) { return Encoding.UTF8.GetString(data.Decompress()); }

        public static byte[] Compress(this byte[] data)
        {
            using (var sourceStream = new MemoryStream(data))
            using (var destinationStream = new MemoryStream())
            {
                sourceStream.CompressTo(destinationStream);
                return destinationStream.ToArray();
            }
        }

        public static byte[] Decompress(this byte[] data)
        {
            using (var sourceStream = new MemoryStream(data))
            using (var destinationStream = new MemoryStream())
            {
                sourceStream.DecompressTo(destinationStream);
                return destinationStream.ToArray();
            }
        }

        public static void CompressTo(this Stream inputStream, Stream outputStream, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
            // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
            // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
            using var compressorStream = new DeflateStream(outputStream, compressionLevel, true);
            inputStream.Position = 0;
            inputStream.CopyTo(compressorStream);
        }

        public static void DecompressTo(this Stream inputStream, Stream outputStream)
        {
            using var decompressorStream = new DeflateStream(inputStream, CompressionMode.Decompress);
            decompressorStream.CopyTo(outputStream);
        }


        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        public static byte[] CompressFromStream(this MemoryStream inputStream, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            using var compressedStream = new MemoryStream();

            inputStream.CompressTo(compressedStream, compressionLevel);

            // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
            return compressedStream.ToArray();
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        public static MemoryStream DecompressToStream(this byte[] data)
        {
            using var compressedStream = new MemoryStream(data);
            var decompressedStream = new MemoryStream();

            compressedStream.DecompressTo(decompressedStream);

            return decompressedStream;
        }
    }
}
