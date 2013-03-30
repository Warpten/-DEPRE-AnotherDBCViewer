using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LFGDungeonsEntry
    {
        public uint Id;
        public string Name;
        public uint MinLevel;
        public uint MaxLevel;
        public uint RecLevel;
        public uint RecMinLevel;
        public uint RecMaxLevel;
        public int Map;
        public int Difficulty;
        public int Flags;
        public uint Type;
        public uint Unk0;
        public string IconName;
        public uint Expansion;
        public uint Unk1;
        public uint GroupType;
        public string Description;
        public uint RandomCategoryId;
        public uint UnkCataclysm1;
        public uint UnkCataclysm2;
        public uint UnkCataclysm3;
    }
}
