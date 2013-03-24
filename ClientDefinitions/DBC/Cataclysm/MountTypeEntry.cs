using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class MountTypeEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 24)]
        public uint[] MountCapability;
    }
}
