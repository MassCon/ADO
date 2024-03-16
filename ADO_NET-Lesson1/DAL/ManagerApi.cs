using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ADO_NET_Lesson1.DAL
{
    internal class ManagerApi
    {
        private readonly SqlConnection _connection;
        private readonly DataContext _dataContext;
        private List<Manager> list;
        public ManagerApi(SqlConnection connection, DataContext dataContext)
        {
            _connection = connection;
            _dataContext = dataContext;
            list = null;
        }
        public List<Manager> GetAll(bool includeDeleted = false)
        {
            if (list is not null) return list;

            list = new List<Manager>();

            try
            {
                string query = "SELECT * FROM Managers";
                if (!includeDeleted)
                    query += " WHERE DeleteDt IS NULL";

                using SqlCommand cmd = new(query, _connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    list.Add(new Entities.Manager(reader) { dataContext = _dataContext });

                reader.Close();
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;

                App.Logger.Log(msg, "SEVERE");
            }
            return list;
        }
    }
}