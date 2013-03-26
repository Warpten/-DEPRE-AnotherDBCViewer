﻿using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class SpellTotemsEntry
    {
        public uint Id;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] TotemCategory;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] Totem;
    }
}
