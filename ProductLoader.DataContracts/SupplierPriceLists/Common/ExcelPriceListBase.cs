namespace ProductLoader.DataContracts.SupplierPriceLists.Common
{
    public interface IExcelModel
    {
        public bool HasHeader { get; set; }

        public ExcelRowEnums HeaderRowNumber { get; set; }

        public string SheetName { get; set; }

        public ExcelColumnEnums HeaderStartColumn { get; set; }

        public ExcelColumnEnums HeaderEndColumn { get; set; }

        public string SupplierCode { get; set; }

        void MapColumn(string columnName, string columnValue);

        PriceListRow MapToPriceListRow();
    }
}
