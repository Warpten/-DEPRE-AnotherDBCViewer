using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LiquidTypeEntry
    {
        public uint Id;
        public string Name;
        public int Flags;
        public uint Type;
        public uint SoundId;
        public uint SpellId;
        public float MaxDarkenDepth;
        public float FogDarkenIntensity;
        public float AmbDarkenIntensity;
        public float DirDarkenIntensity;
        public uint LightId;
        public float ParticleScale;
        public uint ParticleTextureSlot;
        public uint ParticleMovement;
        public uint LiquidMaterialId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 6)]
        public string[] Texture;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] Color;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 18)]
        public float[] Unk0;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public uint[] Unk1;
    }
}
