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
        public static List<string> FindAllAncestorFiles(string childePath, string ancestorPath)
        {
            if (childePath == ancestorPath)
                return new List<string>();
            else if (ancestorPath == null || childePath == null)
                return new List<string>();

            List<string> allAncestors = new List<string>();

            string currentAncestor = Directory.GetParent(childePath).FullName;

            allAncestors.Add(currentAncestor);

            while (currentAncestor != ancestorPath) 
            {
                currentAncestor = Directory.GetParent(currentAncestor).FullName;

                allAncestors.Add(currentAncestor);
            }

            return allAncestors;
        }
    }
}
