using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LFGDungeonExpansionEntry
    {
        public uint Id;
        public uint LFGId;
        public uint ExpansionLevel;
        public uint RandomId;
        public uint HardLevelMin;
        public uint HardLevelMax;
        public uint TargetLevelMin;
        public uint TargetLevelMax;
    }
}
