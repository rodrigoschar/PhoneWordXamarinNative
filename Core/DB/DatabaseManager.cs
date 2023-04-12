using System;
using SQLite;
using System.Collections.Generic;

namespace Core.DB
{
	public class DatabaseManager : IDatabaseManager
	{
        private IPathManager path;
        private SQLiteConnection db;

        public DatabaseManager(IPathManager pathManager)
        {
            this.path = pathManager;
            this.db = new SQLiteConnection(pathManager.GetPath());
        }

        public bool checkTableExists(string tableName)
        {
            try
            {
                var tableInfo = db.GetTableInfo(tableName);
                if (tableInfo.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void StoreData<T>(T newItem, string tableName) where T : new()
        {
            try
            {
                if (checkTableExists(tableName))
                {
                    db.Insert(newItem);
                }
                else
                {
                    db.CreateTable<T>();
                    db.Insert(newItem);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e);
            }
        }

        public void UpdateData<T>(T updateItem, string tableName) where T : new()
        {
            try
            {
                if (checkTableExists(tableName))
                {
                    db.Update(updateItem);
                }
                else
                {
                    db.CreateTable<T>();
                    db.Insert(updateItem);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e);
            }
        }

        public void DeleteDataById<T>(T item, string tableName, int id) where T : new()
        {
            try
            {
                if (checkTableExists(tableName))
                    db.Delete<T>(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e);
            }
        }


        public Result<List<T>> GetAllData<T>() where T : new()
        {
            List<T> typeList = new List<T>();
            try
            {
                var data = db.Table<T>();
                foreach (var res in data)
                {
                    typeList.Add(res);
                }
                return Result.Ok<List<T>>(typeList);
            }
            catch
            {
                return Result.Fail<List<T>>("Could not get all data");
            }
        }
    }
}

