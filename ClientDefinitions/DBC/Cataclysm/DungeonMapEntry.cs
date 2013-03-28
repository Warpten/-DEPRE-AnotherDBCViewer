namespace FileStructures.DBC.Cataclysm
{
    public sealed class DungeonMapEntry
    {
        public uint Id;
        public uint MapId;
        public int Layer; // Typically, ICC
        public float MinX; //! TODO: Possibly swapped orders
        public float MinY;
        public float MaxX;
        public float MaxY;
        public uint Area;
    }
}
