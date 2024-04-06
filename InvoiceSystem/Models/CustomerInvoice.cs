namespace InvoiceSystem.Models
{
    public class CustomerInvoice
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public IList<InvoiceItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
