using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    //! This one is kinda fucked up.
    public sealed class AchievementCriteriaEntry
    {
        public uint Id;
        public uint AchievementId;
        public uint Type;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] Values;
        public uint ExtraReqType_0;
        public uint ExtraReqValue_0;
        public uint ExtraReqType_1;
        public uint ExtraReqValue_1;
        public uint Unk_0;
        public string Name;
        public uint CompletionFlags;
        public uint TimedCriteriaStartType;
        public uint TimedCriteriaMiscId;
        public uint TimerLimit;
        public uint ShowOrder;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] Unk_1;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public uint[] ExtraConditionType;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public uint[] ExtraConditionValue;
    }
}
