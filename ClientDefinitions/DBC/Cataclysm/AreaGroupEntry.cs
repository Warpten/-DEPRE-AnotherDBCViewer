using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class AreaGroupEntry
    {
        public uint Id;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 6)]
        public uint[] AreaId;

        public uint NextGroup;
    }
}
