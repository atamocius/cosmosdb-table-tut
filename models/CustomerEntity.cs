using Microsoft.Azure.Cosmos.Table;

namespace cosmosdb_table_tut.Models
{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {
        }

        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString() =>
            string.Format(
                "\t{0}\t{1}\t{2}\t{3}",
                this.PartitionKey,
                this.RowKey,
                this.Email,
                this.PhoneNumber);
    }
}
