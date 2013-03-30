using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LightIntBandEntry
    {
        public uint Id;
        public uint EntriesAmount;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 16)]
        public uint[] TimeValues;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 16)]
        public uint[] ColorValues;
    }
}
