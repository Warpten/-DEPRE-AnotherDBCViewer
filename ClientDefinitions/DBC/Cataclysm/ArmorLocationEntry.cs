using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ArmorLocationEntry
    {
        public uint InventoryType;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public float[] Value;
    }
}
