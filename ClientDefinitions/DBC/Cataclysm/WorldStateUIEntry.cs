using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class WorldStateUIEntry
    {
        // Automagic dbc2csv: long long long none long none str str str long long str str str long long None
        // Bullshit!
        public uint Id;
        public int MapId;
        public uint ZoneId;
        public uint PhaseMask;
        public uint IconId;
        public int Null1;
        public string Text2;
        public string Description;
        public int RealState;
        public uint WorldStateId;
        public uint Type;
        public string DynIcon;
        public string DynTooltip;
        public string Extended; // Point capture worldstates (towers in EoTS, etc)
        public uint ExtUiStateVar; // Capture percentage
        public uint ExtUiStateVarNeutral; // %age width of the neutral part
        public int Null2;
    }
}
