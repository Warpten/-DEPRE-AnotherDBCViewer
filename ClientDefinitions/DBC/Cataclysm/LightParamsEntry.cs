using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LightParamsEntry
    {
        public uint Id;
        public uint HighlightSky;
        public uint LightSkyboxId;
        public uint CloudTypeId;
        public float Glow;
        public float WaterShallowAlpha;
        public float WaterDeepAlpha;
        public float OceanShallowAlpha;
        public float OceanDeepAlpha;
		public uint Flags;
    }
}
