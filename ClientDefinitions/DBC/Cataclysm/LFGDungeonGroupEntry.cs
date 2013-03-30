using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LFGDungeonGroupEntry
    {
        public uint Id;
        public string Name;
        public uint OrderIndex;
        public uint ParentGroupId;
        public uint TypeId;
    }
}
