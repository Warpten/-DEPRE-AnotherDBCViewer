namespace FileStructures.DBC.Cataclysm
{
    public sealed class TerrainTypeEntry
    {
        public uint Id;
        public uint UnkCataclsym;
        public string Description;
        public uint FootstepSprayRun; // SpellVisualEffectName.dbc
        public uint FootstepSprayWalk; // SpellVisualEffectName.dbc
        public uint TerrainTypeSoundsId;
        public uint Flags;
    }
}
