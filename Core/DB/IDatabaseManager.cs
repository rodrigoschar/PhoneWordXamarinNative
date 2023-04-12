using System;
using System.Collections.Generic;

namespace Core.DB
{
	public interface IDatabaseManager
	{
        bool checkTableExists(string tableName);
        void StoreData<T>(T newItem, string tableName) where T : new();
        void UpdateData<T>(T updateItem, string tableName) where T : new();
        void DeleteDataById<T>(T item, string tableName, int id) where T : new();
        Result<List<T>> GetAllData<T>() where T : new();
    }
}

