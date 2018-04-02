using SQLite.Net;
using SQLite.Net.Async;

namespace eBooksDT.Interfaces
{
    public interface ISQLite
    {
        void CloseConnection();
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
        void DeleteDatabase();
    }
}