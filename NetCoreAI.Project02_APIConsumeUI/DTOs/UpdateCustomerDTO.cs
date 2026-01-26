namespace NetCoreAI.Project02_APIConsumeUI.DTOs
{
    public class UpdateCustomerDTO
    {
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string customerSurname { get; set; }
        public decimal customerBalance { get; set; }
    }
}
