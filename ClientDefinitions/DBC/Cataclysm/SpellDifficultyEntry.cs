using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class SpellDifficultyEntry
    {
        public uint Id;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public uint[] SpellId;
    }
}
