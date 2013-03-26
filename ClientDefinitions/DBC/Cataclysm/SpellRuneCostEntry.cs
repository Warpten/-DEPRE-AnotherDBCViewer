using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class SpellRuneCostEntry
    {
        public uint Id;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public uint[] RuneCost;

        public uint RunicPowerGain;
    }
}
