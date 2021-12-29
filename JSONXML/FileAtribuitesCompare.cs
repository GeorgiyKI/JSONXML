using System;
using System.Collections.Generic;
using System.Text;

namespace JSONXML
{
    class FileAtribuitesCompare : IComparer<AFileAtribuites>
    {
        public int Compare(AFileAtribuites p1, AFileAtribuites p2)
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
