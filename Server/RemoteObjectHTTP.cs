﻿using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.Remoting.Lifetime;

namespace Server
{
    [Serializable]
    public class RemoteObjectHTTP : MarshalByRefObject, IRemoteObject
    {
        private string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=C:/My Files/Универ/3 курс/Технологии программирования/TP_Truckers/Truckers/data/TruckersDB.mdb";

        public RemoteObjectHTTP()
        {
            Console.WriteLine("Удаленный объект HTTP создан!");
        }
        ~RemoteObjectHTTP()
        {
            Console.WriteLine("Удаленный объект HTTP уничтожен!");
        }

        public string Autorization(string login, string password)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable dataTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter(string.Format("SELECT * FROM Users WHERE Login='{0}' AND Password='{1}'", login, password), connection);
                adapter.Fill(dataTable);

                connection.Close();

                return Convert.ToString(dataTable.Rows.Count);
            }
        }

        public string Registration(string name, string login, string password, string post)
        {
            Console.WriteLine("asdasdasdasdas");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                // проверка на повтор логина
                DataTable dataTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Users WHERE Login='" + login + "'", connection);
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    string sql = string.Format("INSERT INTO Users ([Username], [Post], [Login], [Password]) VALUES ('{0}', '{1}', '{2}', '{3}')", name, post, login, password);
                    OleDbCommand cmd = new OleDbCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return "0";
                }
                else
                {
                    connection.Close();
                    return "1";
                }
            }
        }

        public byte[] Upload(string path)
        {
            return File.ReadAllBytes("Sources\\" + path);
        }

		public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();

            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(3);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(10);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }

            return lease;
        }

    }
}