using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class Cfg_CategoriesEntry
    {
        public uint Id;
        public uint LocaleMask;
        public uint CharsetMask;
        public uint CharsetMask2;
        public uint Flags;
        public string Name;
    }
}
