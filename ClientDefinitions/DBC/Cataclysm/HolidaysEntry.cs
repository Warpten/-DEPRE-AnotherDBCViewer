using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class HolidaysEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public uint[] Duration;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 26)]
        public uint[] Date;
        public uint Region;
        public uint Looping;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public int[] CalendarFlags;
        public uint HolidayNameId;
        public uint HolidayDescriptionId;
        public string TextureName;
        public uint Priority;
        public int CalendarFilterType;
        public int Flags;
    }
}
