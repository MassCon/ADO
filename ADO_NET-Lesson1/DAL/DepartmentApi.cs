using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace ADO_NET_Lesson1.DAL
{
    internal class DepartmentApi
    {
        private readonly SqlConnection _connection;

        public DepartmentApi(SqlConnection connection)
        {
            _connection = connection;
        }
        public bool Add(Department department)
        {
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = $"INSERT INTO Departments (Id, Name) VALUES( @id, @name)";
                cmd.Parameters.AddWithValue("@id", department.Id);
                cmd.Parameters.AddWithValue("@name", department.Name);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;

                App.Logger.Log(msg, "SEVERE");
                return false;
            }
        }
        public List<Entities.Department> GetAll()
        {
            var list = new List<Entities.Department>();
            try
            {
                _connection.Open();

                using SqlCommand cmd = new("SELECT * FROM Departments", _connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                    list.Add(new Department(reader));

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