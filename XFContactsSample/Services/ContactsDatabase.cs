using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XFContactsSample.Models;
using XFContactsSample.Utils;

namespace XFContactsSample.Services
{
    public class ContactsDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });
        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ContactsDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Contact).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Contact)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<ObservableCollection<Contact>> GetItemsAsync()
        {
            return Task.Run(async () =>
            {
                var contacts = await Database.Table<Contact>().ToListAsync();
                return new ObservableCollection<Contact>(contacts);
            });
        }

        public Task<Contact> GetItemAsync(int id)
        {
            return Database.Table<Contact>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Contact item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {

                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Contact item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
