using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace cosmosdb_table_tut
{
    using static Console;
    using Models;

    public class TableDB
    {
        private readonly CloudStorageAccount storageAccount;

        public TableDB(string connectionString)
        {
            this.storageAccount = this.CreateStorageAccount(connectionString);
        }

        private CloudStorageAccount CreateStorageAccount(
            string connectionString)
        {
            try
            {
                var sa = CloudStorageAccount.Parse(connectionString);
                return sa;
            }
            catch (FormatException)
            {
                WriteLine(string.Join(" ",
                    "Invalid storage account information provided.",
                    "Please confirm the AccountName and AccountKey are valid",
                    "in the app.config file - then restart the application."));
                throw;
            }
            catch (ArgumentException)
            {
                WriteLine(string.Join(" ",
                    "Invalid storage account information provided.",
                    "Please confirm the AccountName and AccountKey are valid",
                    "in the app.config file - then restart the sample."));
                throw;
            }
        }

        public async Task<CloudTable> CreateTable(string tableName)
        {
            // Create a table client for interacting with the table service
            var tableClient = this.storageAccount
                .CreateCloudTableClient(new TableClientConfiguration());

            WriteLine("Create a Table for the demo");

            // Create a table client for interacting with the table service
            var table = tableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                WriteLine("Table {0} already exists", tableName);
            }

            WriteLine();

            return table;
        }

        public async Task<T> InsertOrMergeEntity<T>(CloudTable table, T entity)
            where T : ITableEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                // Create the InsertOrMerge table operation
                var insertOrMergeOperation =
                    TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                var result = await table.ExecuteAsync(insertOrMergeOperation);
                var insertedCustomer = (T)result.Result;

                if (result.RequestCharge.HasValue)
                {
                    WriteLine(
                        "Request Charge of InsertOrMerge Operation: {0}",
                        result.RequestCharge);
                }

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                WriteLine(e.Message);
                throw;
            }
        }

        public async Task<T> RetrieveEntityUsingPointQuery<T>(
            CloudTable table,
            string partitionKey,
            string rowKey)
            where T : ITableEntity
        {
            try
            {
                var retrieveOperation =
                    TableOperation.Retrieve<T>(
                        partitionKey,
                        rowKey);

                var result = await table.ExecuteAsync(retrieveOperation);

                var customer = (T)result.Result;
                if (customer != null)
                {
                    WriteLine(customer);
                }

                if (result.RequestCharge.HasValue)
                {
                    WriteLine(
                        "Request Charge of Retrieve Operation: {0}",
                        result.RequestCharge);
                }

                return customer;
            }
            catch (StorageException e)
            {
                WriteLine(e.Message);
                throw;
            }
        }

        public async Task DeleteEntity(
            CloudTable table,
            CustomerEntity deleteEntity)
        {
            try
            {
                if (deleteEntity == null)
                {
                    throw new ArgumentNullException("deleteEntity");
                }

                var deleteOperation = TableOperation.Delete(deleteEntity);
                var result = await table.ExecuteAsync(deleteOperation);

                if (result.RequestCharge.HasValue)
                {
                    WriteLine(
                        "Request Charge of Delete Operation: {0}",
                        result.RequestCharge);
                }

            }
            catch (StorageException e)
            {
                WriteLine(e.Message);
                throw;
            }
        }
    }
}
