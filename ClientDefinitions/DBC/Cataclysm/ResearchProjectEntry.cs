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
        public int Unk0;
        public int Unk1;
        public uint ItemId;
        public int Unk2;
        public string IconName;
        public int Unk3; // Chance ?
    }
}
