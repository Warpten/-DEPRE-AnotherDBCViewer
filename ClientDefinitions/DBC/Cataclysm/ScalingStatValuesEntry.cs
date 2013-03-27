using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ScalingStatValuesEntry
    {
        public uint Id;
        public uint Level;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 6)]
        public uint[] DpsMod;
        public uint SpellPower;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] StatMultiplier;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 32)]
        public uint[] Armor;
        public uint CloakArmor;
    }
}
