using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    public static class FileWorker
    {
        public static bool DeleteFile(string deletePath)
        {
            // Was it possible to delete the file?
            bool isDeleted = false;

            if (Directory.Exists(deletePath)) // Deleting a directory.
            {
                Directory.Delete(deletePath, true);
                isDeleted = true;
            }
            else if (File.Exists(deletePath)) // Deleting a file.
            {
                File.Delete(deletePath);
                isDeleted = true;
            }

            return isDeleted;
        }

        public static string GenerateUniqueFileName(string sourcePath, string targetPath)
        {
            FileInfo sourcePathInfo = new(sourcePath);

            FileInfo targetPathInfo = new(targetPath);

            int duplicateNumber = 1;

            while (File.Exists(targetPath))
            {
                targetPath = targetPathInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(sourcePathInfo.Name) + $" ({duplicateNumber})" + sourcePathInfo.Extension;

                targetPathInfo = new FileInfo(targetPath);

                duplicateNumber++;
            }

            return targetPath;
        }

        public static string GenerateUniqueDirectoryName(string sourcePath, string targetPath)
        {
            FileInfo sourcePathInfo = new(sourcePath);

            FileInfo targetPathInfo = new(targetPath);

            int duplicateNumber = 1;

            while (Directory.Exists(targetPath))
            {
                targetPath = targetPathInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(sourcePathInfo.Name) + $" ({duplicateNumber})" + sourcePathInfo.Extension;

                targetPathInfo = new FileInfo(targetPath);

                duplicateNumber++;
            }

            return targetPath;
        }
    }
}
