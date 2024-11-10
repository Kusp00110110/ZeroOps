using System;
using System.Collections.Generic;

namespace ProductLoader.DataContracts.SupplierPriceLists
{
    public class FontosaPriceList
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, int> Quantities { get; set; }
        public string Status { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Invoice
    {
        public byte[] FileContent { get; set; }
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Invoice { get; set; }
        public string Type { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    public class OrderRequest
    {
        public string Token { get; set; }
        public string Reference { get; set; }
        public int Method { get; set; }
        public int Build { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string Code { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
