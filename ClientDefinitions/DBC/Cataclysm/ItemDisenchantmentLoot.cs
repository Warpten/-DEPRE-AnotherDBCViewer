using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ItemDisenchantLoot
    {
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 6)]
        public uint Unk;
    }
}
