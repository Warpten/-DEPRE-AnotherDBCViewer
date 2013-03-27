using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class QuestFactionRewardEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public int[] RewardAmount;
    }
}
