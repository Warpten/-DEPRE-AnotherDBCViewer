using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class CinematicSequencesEntry
    {
        public uint Id;
        public uint SoundId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] CinematicCamera;
    }
}
