using DBFilesClient.NET;

namespace FileStructures.DB2.Cataclysm
{
    public sealed class ItemCurrencyCostEntry : BaseDbcFormat
    {
        public uint Id;
        public uint ItemId;
    }
}
