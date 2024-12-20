﻿using Dapper;
using MauiContactManager.Interfaces;
using MauiContactManager.Models;
using Microsoft.Data.Sqlite;

namespace MauiContactManager.Data
{
    public class ContactDatabase : IContactDatabase
    {
        private string _connectionString;

        public ContactDatabase(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            string createContactsTableSql = @"
                CREATE TABLE IF NOT EXISTS Contact (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    PhoneNumbers TEXT,
                    Emails TEXT,
                    BirthDate TEXT
                )
            ";

            string createSettingsTableSql = @"
                CREATE TABLE IF NOT EXISTS Settings (
                    Key TEXT PRIMARY KEY,
                    Value TEXT
                )
            ";

            using var conncetion = GetConnection();
            conncetion.Open();
            conncetion.Execute(createContactsTableSql);
            conncetion.Execute(createSettingsTableSql);
        }

        public List<ContactModel> GetContacts()
        {
            string sql = "SELECT * FROM Contact";
            using var conncetion = GetConnection();
            return conncetion.Query<ContactModel>(sql).ToList();
        }

        public ContactModel GetContact(int id)
        {
            string sql = "SELECT * FROM Contact WHERE Id = @Id";
            using var conncetion = GetConnection();
            return conncetion.QueryFirstOrDefault<ContactModel>(sql, new { Id = id });
        }

        public void SaveContact(ContactModel contact)
        {
            string sql = string.Empty;
            if (contact.Id != 0)
            {
                sql = @"UPDATE Contact SET Name = @Name, PhoneNumbers = @PhoneNumbers, Emails = @Emails, BirthDate = @BirthDate WHERE Id = @Id";
            }
            else
            {
                sql = @"INSERT INTO Contact (Name, PhoneNumbers, Emails, BirthDate) VALUES (@Name, @PhoneNumbers, @Emails, @BirthDate)";
            }

            using var connection = GetConnection();
            connection.Execute(sql, contact);
        }

        public void DeleteContact(int id)
        {
            var connection = GetConnection();
            connection.Execute("DELETE FROM Contact WHERE Id = @Id", new { Id = id });
        }

        public string GetSetting(string key)
        {
            string sql = "SELECT Value FROM Settings WHERE Key = @Key";
            using var connection = GetConnection();
            return connection.QueryFirstOrDefault<string>(sql, new { Key = key });
        }

        public void SaveSetting(string key, string value)
        {
            string sql = @"INSERT INTO Settings (Key, Value) VALUES (@Key, @Value) ON CONFLICT(Key) DO UPDATE SET Value = @Value";
            using var connection = GetConnection();
            connection.Execute(sql, new { Key = key, Value = value });
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
