using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class RandPropPointsEntry
    {
        // public uint Id;
        public uint ItemLevel;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] EpicPropertiesPoints;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] RarePropertiesPoints;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] UncommonPropertiesPoints;
    }
}
