using System;
using SQLite;
using System.Collections.Generic;

namespace Core.DB
{
	public class DatabaseManager
	{
        IPathManager path;
		public DatabaseManager(IPathManager pathManager)
		{
            this.path = pathManager;
		}

        public void CreatePhoneNumber(Core.Models.PhoneNumber data)
        {
            var db = new SQLiteConnection(path.GetPath());
            db.CreateTable<Core.Models.PhoneNumber>();
            db.Insert(data);
        }

        public void DeletePhoneNumber(int id)
        {
            var db = new SQLiteConnection(path.GetPath());
            var deleteNumber = db.Delete<Core.Models.PhoneNumber>(id);
        }

        public List<Core.Models.PhoneNumber> GetAllPhoneNumbers()
        {
            List<Core.Models.PhoneNumber> phoneNumbers = new List<Core.Models.PhoneNumber>(); ;
            var db = new SQLiteConnection(path.GetPath());
            var table = db.Table<Core.Models.PhoneNumber>();
            foreach (var s in table)
            {
                phoneNumbers.Add(new Core.Models.PhoneNumber() { Id = s.Id, phoneNumber = s.phoneNumber });
            }
            return phoneNumbers;
        }
    }
}

