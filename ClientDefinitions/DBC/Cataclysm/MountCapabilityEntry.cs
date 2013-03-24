using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class MountCapabilityEntry
    {
        public uint Id;
        public uint Flags;
        public uint RequiredRidingSkill;
        public uint RequiredArea;
        public uint RequiredAura;
        public uint RequiredSpell;
        public uint SpeedModSpell;
        public int RequiredMap;
    }
}
