using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class FactionEntry
    {
        public uint Id;
        public int ReputationListId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] BaseRepRaceMask;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] BaseRepClassMask;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] BaseRepValue;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] ReputationFlags;
        public int Team;
        public float SpillOverRateIn;
        public float SpillOverRateOut;
        public uint SpillOverMaxRankIn;
        public uint SpillOverMaxRankOut; // It does not seem to be the max standing at which a faction outputs spillover ...so no idea
        public string Name;
        public string Description;
        public uint GroupExpansion;
    }
}
