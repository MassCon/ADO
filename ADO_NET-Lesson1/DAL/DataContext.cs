using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NET_Lesson1.DAL
{
    internal class DataContext
    {
        private SqlConnection _connection;
        internal DepartmentApi Departments { get; set; }
        public DataContext()
        {
            _connection = new SqlConnection(App.ConnectionString);
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;
                App.Logger.Log(msg, "SEVERE");
                throw new Exception("Context creation failed");
            }
            Departments = new(_connection);
            _connection.Close();
        }
    }
}