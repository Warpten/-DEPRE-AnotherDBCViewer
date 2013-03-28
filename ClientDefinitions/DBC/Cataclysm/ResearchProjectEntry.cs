using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ResearchProjectEntry
    {
        public uint Id;
        public string Name;
        public string Description;
        public int Type; // 0 Common - 1 Rare
        public int BranchId; // ResearchBranch.dbc
        public uint SpellId;
        public int MaxKeystones; // Amount that can be used in project
        public string IconPath;
        public int Fragments; // Amount needed
    }
}
