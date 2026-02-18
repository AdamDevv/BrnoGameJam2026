namespace APX.Extra.Misc
{
    public static class FileUtils
    {
        /// <summary>
        /// Returns the human-readable file size for an arbitrary, 64-bit file size
        /// The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
        /// </summary>
        /// <param name="i"></param>
        /// <remarks>http://stackoverflow.com/a/281684/147003</remarks>
        /// <returns></returns>
        public static string GetReadableBytesSize(long i)
        {
            var sign = (i < 0 ? "-" : "");
            double readable = (i < 0 ? -i : i);
            string suffix;
            switch (i)
            {
                // Exabyte
                case >= 0x1000000000000000:
                    suffix = "EB";
                    readable = i >> 50;
                    break;
                
                // Petabyte
                case >= 0x4000000000000:
                    suffix = "PB";
                    readable = i >> 40;
                    break;
                
                // Terabyte
                case >= 0x10000000000:
                    suffix = "TB";
                    readable = i >> 30;
                    break;
                
                // Gigabyte
                case >= 0x40000000:
                    suffix = "GB";
                    readable = i >> 20;
                    break;
                
                // Megabyte
                case >= 0x100000:
                    suffix = "MB";
                    readable = i >> 10;
                    break;
                
                // Kilobyte
                case >= 0x400:
                    suffix = "KB";
                    readable = i;
                    break;
                
                default: 
                    return i.ToString(sign + "0 B"); // Byte
            }

            readable /= 1024;

            return sign + readable.ToString("0.### ") + suffix;
        }
    }
}
