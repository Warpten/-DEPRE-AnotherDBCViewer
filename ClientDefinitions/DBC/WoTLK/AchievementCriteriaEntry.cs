using DBFilesClient.NET;

namespace FileStructures.DBC.WoTLK
{
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
        public uint Unk0;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 16)]
        public string[] Name;
        public uint NameFlags;
        public uint TimedType;
        public uint TimedStartEvent;
        public uint TimedLimit;
        public uint ShowOrder;
    }
}
