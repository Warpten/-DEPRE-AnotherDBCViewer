using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LockEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] Type;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] Index;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] Skill;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] Action;
    }
}
