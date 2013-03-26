using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class DurabilityCostsEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 29)]
        public uint[] Multiplier;
    }
}
