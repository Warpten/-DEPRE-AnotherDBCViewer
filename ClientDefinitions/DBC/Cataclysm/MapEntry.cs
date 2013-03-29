namespace FileStructures.DBC.Cataclysm
{
    public sealed class MapEntry
    {
        public uint MapId;
        public string InternalName;
        public uint MapType;
        public uint Unk_330;
        public uint Unk4;
        public uint IsPvP;
        public string Name;
        public uint LinkedZone;
        public string HordeIntro;
        public string AllianceIntro;
        public uint MultimapId;
        public float BattlefieldMapIconScale;
        public int Entrance_map;
        public float EntranceX;
        public float EntranceY;
        public int TimeOfDayOverride;
        public uint Addon;
        public uint ExpireTime;
        public uint MaxPlayers;
        public int RootPhaseMap;
    }
}
