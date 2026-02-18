using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class FileHelper
    {
        /// <summary>
        /// Checks if the file 'file' is unlocked for reading.
        ///
        /// Source: https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use/937558#937558
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns>null if file is unlocked, an exception otherwise</returns>
        public static Exception IsFileLocked(string filepath)
        {
            FileStream stream = null;

            try
            {
                var file = new FileInfo(filepath);
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError("Unauthorized file access!");
                return ex;
            }
            catch (FileNotFoundException ex)
            {
                Debug.LogError("File not found! Filepath: " + filepath);
                return ex;
            }
            catch (IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return null;
        }

        public static bool LoadData(string path, out string data)
        {
            if (!File.Exists(path))
            {
                data = null;
                return false;
            }

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fileStream, Encoding.UTF8, true, 512))
                {
                    data = reader.ReadToEnd();
                    return true;
                }
            }
        }

        public static void SaveData(string path, string data)
        {
            var folderPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(data);
                }
            }
        }

        public static bool ArePathsEqual(string path1, string path2)
        {
            return string.Equals(Path.GetFullPath(path1), Path.GetFullPath(path2), StringComparison.OrdinalIgnoreCase);
        }
    }
}
