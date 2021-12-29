using System;
using System.Collections.Generic;
using System.Text;

namespace JSONXML
{
    class FileAtribuitesCompare : IComparer<FileAtribuiteModel>
    {
        public int Compare(FileAtribuiteModel p1, FileAtribuiteModel p2)
        {
            if (p1.Count < p2.Count)
                return 1;
            else if (p1.Count > p2.Count)   
                return -1;
            else
                return 0;
        }
    }
}
