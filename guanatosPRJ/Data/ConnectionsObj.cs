using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using guanatosSvc.Models;
using System.IO;

namespace guanatosSvc.Data
{
    public sealed class ConnectionsObj
    {
        private SqlConnection con;
        private List<Restaurant> list = new List<Restaurant>();
        private static ConnectionsObj instance = null;

        public static ConnectionsObj GetInstance
        {
            get {
                if (instance == null)
                    instance = new ConnectionsObj();
                return instance;
                }
        }

        private ConnectionsObj()
        {
        }

        public void CreateConnection()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();

            cs.DataSource = @"localhost\SQLEXPRESS";
            cs.InitialCatalog = "guanatosDB";
            cs.UserID = "user";
            cs.Password = "user123456";

            con = new SqlConnection(cs.ConnectionString);
        }

        public int Save(Restaurant restaurant)
        {
            try
            {
                byte[] data = File.ReadAllBytes(restaurant.Path);
                string qry = "insert into tblRestaurant (Giro, Descripcion, Imagen, Path) values (@prGiro, @prDescripcion, @prImagen, @prPath)";

                SqlCommand SqlCom = new SqlCommand(qry, con);
                SqlCom.Parameters.Add(new SqlParameter("@prGiro", restaurant.Giro));
                SqlCom.Parameters.Add(new SqlParameter("@prDescripcion", restaurant.Descripcion));
                SqlCom.Parameters.Add(new SqlParameter("@prImagen", data));
                SqlCom.Parameters.Add(new SqlParameter("@prPath", restaurant.Path));

                con.Open();
                SqlCom.ExecuteNonQuery();
                File.Copy(Path.Combine("C://Users/webjo/Documents/", "babyshower - copia.jpg"), Path.Combine("/NewFolder", "babyshower - copia.jpg"), true);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return 1;
        }

        public int Save(String email)
        {
            try
            {
                string qry = "insert into tblInscripcion (Correo, Estado) values (@prCorreo, @prEstado)";

                SqlCommand SqlCom = new SqlCommand(qry, con);

                SqlCom.Parameters.Add(new SqlParameter("@prCorreo", email));
                SqlCom.Parameters.Add(new SqlParameter("@prEstado", 1));

                con.Open();
                SqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return 1;
        }

        public List<Restaurant> Search(String parameter)
        {
            string qry = "select Giro, Descripcion, Imagen, Path from tblRestaurant";
            qry += (parameter.Equals(string.Empty)) ? ";" : " where Giro = @prParametro;";
            Restaurant res;
            SqlDataReader rdr;
            list.Clear();

            try
            {
                SqlCommand SqlCom = new SqlCommand(qry, con);
                SqlCom.Parameters.Add(new SqlParameter("@prParametro", parameter));

                con.Open();
                rdr = SqlCom.ExecuteReader(CommandBehavior.CloseConnection);

                if(rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        res = new Restaurant();
                        byte[] img = (byte[])rdr.GetValue(2);
                        res.Giro = rdr.GetString(0);
                        res.Descripcion = rdr.GetString(1);
                        res.Path = rdr.GetString(3);
                        res.Imagen = System.Text.Encoding.UTF8.GetString(img, 0, img.Length);

                        list.Add(res);
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return list;
        }

        public List<String> Search()
        {
            string qry = "select Correo from tblInscribir;";
            SqlDataReader rdr;
            List<String> list = new List<string>();
            list.Clear();

            try
            {
                SqlCommand SqlCom = new SqlCommand(qry, con);

                con.Open();
                rdr = SqlCom.ExecuteReader(CommandBehavior.CloseConnection);

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr.GetString(0));
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return list;
        }

    }

}
