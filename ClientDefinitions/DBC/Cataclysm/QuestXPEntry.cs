using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class QuestXPEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public uint[] Exp;
    }
}
