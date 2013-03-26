using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class DungeonEncounterEntry
    {
        public uint Id;
        public uint MapId;
        public int Difficulty;
        public int Unk0;
        public uint EncounterIndex;
        public string Name;
        public int NameFlags;
        public int Unk1;
    }
}
