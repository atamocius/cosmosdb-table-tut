using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace cosmosdb_table_tut
{
    using static Console;
    using Models;

    public class Demo
    {
        public async Task Run(TableDB db)
        {
            WriteLine("Azure Cosmos DB Table - Basic Samples");
            WriteLine();
            WriteLine();

            var tableName = "demo" + Guid.NewGuid().ToString().Substring(0, 5);

            // Create or reference an existing table
            var table = await db.CreateTable(tableName);

            try
            {
                // Demonstrate basic CRUD functionality
                await BasicDataOperations(db, table);
            }
            finally
            {
                // Delete the table
                await table.DeleteIfExistsAsync();
            }
        }

        private static async Task BasicDataOperations(
            TableDB db,
            CloudTable table)
        {
            // Create an instance of a customer entity. See the
            // Model\CustomerEntity.cs for a description of the entity.
            var customer = new CustomerEntity("Harp", "Walter")
            {
                Email = "Walter@contoso.com",
                PhoneNumber = "425-555-0101"
            };

            // Demonstrate how to insert the entity
            WriteLine("Insert an Entity.");
            customer = await db.InsertOrMergeEntity(table, customer);

            // Demonstrate how to Update the entity by changing the phone number
            WriteLine(
                "Update an existing Entity using the InsertOrMerge Upsert Operation.");
            customer.PhoneNumber = "425-555-0105";
            await db.InsertOrMergeEntity(table, customer);
            WriteLine();

            // Demonstrate how to Read the updated entity using a point query
            WriteLine("Reading the updated Entity.");
            customer = await db.RetrieveEntityUsingPointQuery<CustomerEntity>(
                table,
                "Harp",
                "Walter");
            WriteLine();

            // Demonstrate how to Delete an entity
            WriteLine("Delete the entity. ");
            await db.DeleteEntity(table, customer);
            WriteLine();
        }
    }
}
