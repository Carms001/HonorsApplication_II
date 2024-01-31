using HonorsApplication.ProgramClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace HonorsApplication.Data
{
    public class DatabaseContext : IAsyncDisposable
    {
        private const string dbName = "TaskManagerDB.db3"; //Name of the Database

        private static string dbPath => Path.Combine(FileSystem.AppDataDirectory, dbName); //Path of the Database

        private SQLiteAsyncConnection _connection;

        private SQLiteAsyncConnection Database => (_connection ??= new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));


        private async Task<bool> CreateTableIfNull<TTable>() where TTable : class, new() //Creates a new table if no table exists
        {
            return await Database.CreateTableAsync<TTable>() > 0;
            
        }

        private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new() //Gets Table
        {
            await CreateTableIfNull<TTable>();
            return Database.Table<TTable>();
        }

        public async Task <IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new() //Gets all 
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }

        public async Task<IEnumerable<TTable>> GetFilterAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new() //Gets filtered tables
        {

            var table = await GetTableAsync<TTable>();
            return await table.Where(predicate).ToListAsync();
        }

        public async Task<TTable> GetItemByKeyAsync<TTable>(object pKey) where TTable : class, new() //Gets an Item by its ID
        {
            await CreateTableIfNull<TTable>();
            return await Database.GetAsync<TTable>(pKey);
        }


        public async Task<bool>AddItemAsync<TTable>(TTable item) where TTable : class, new() //Adds a new item to the table
        {
            await CreateTableIfNull<TTable>();
            return await Database.InsertAsync(item) > 0;
        }

        public async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new() //Deletes Item
        {
            await CreateTableIfNull<TTable>();
            return await Database.DeleteAsync(item) > 0;
        }

        public async Task<bool> DeleteItemByKeyAsync<TTable>(object pKey) where TTable : class, new() //Deletes Itemm by Key
        {
            await CreateTableIfNull<TTable>();
            return await Database.DeleteAsync<TTable>(pKey) > 0;
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
    
}
