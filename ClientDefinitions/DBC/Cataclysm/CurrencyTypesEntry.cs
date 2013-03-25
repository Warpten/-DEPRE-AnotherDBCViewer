using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class CurrencyTypesEntry
    {
        public uint Id;
        public uint Category;
        public string Name;
        public string IconName;
        public uint Unk;
        public uint HasSubstitution;
        public uint SubstitutionId;
        public uint TotalCap;
        public uint WeekCap;
        public uint Flags;
        public string Description;
    }
}
