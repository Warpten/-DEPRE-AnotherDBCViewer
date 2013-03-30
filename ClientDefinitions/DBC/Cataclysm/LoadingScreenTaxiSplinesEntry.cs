using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LoadingScreenTaxiSplinesEntry
    {
        public uint Id;
        public uint TaxiPathId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public float[] LocationX;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public float[] LocationY;
        public uint LegIndex;
    }
}
