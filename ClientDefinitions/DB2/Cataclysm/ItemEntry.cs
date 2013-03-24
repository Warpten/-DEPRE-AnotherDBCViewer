namespace FileStructures.DB2.Cataclysm
{
    public sealed class ItemEntry : BaseDbcFormat
    {
        public uint ID;
        public uint Class;
        public uint SubClass;
        public int SoundOverrideSubclass;
        public int Material;
        public uint DisplayId;
        public uint InventoryType;
        public uint Sheath;
    }
}
