using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class BattlemasterListEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public int[] Mapid;
        public uint Type;
        public uint CanJoinAsGroup;
        public string Name;
        public uint MaxGroupSize;
        public uint HolidayWorldStateId;
        public uint MinLevel;
        public uint MaxLevel;
        public uint MaxGroupSizeRated;
        public uint UnkCataclysm1;
        public uint MaxPlayers;
        public uint UnkCataclysm2;
    }
}
