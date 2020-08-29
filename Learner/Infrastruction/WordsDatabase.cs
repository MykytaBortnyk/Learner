using System;
using Learner.Models;
using SQLite;

namespace Learner.Infrastruction
{
    public class WordsDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public WordsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Word>().Wait();
        }
    }
}
