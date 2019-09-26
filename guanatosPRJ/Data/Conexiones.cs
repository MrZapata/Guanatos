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
    public sealed class Conexiones
    {
        private SqlConnection con;
        private List<Restaurant> list = new List<Restaurant>();
        String parametro = string.Empty;
        private static Conexiones instance = null;

        public static Conexiones GetInstance
        {
            get {
                if (instance == null)
                    instance = new Conexiones();
                return instance;
                }
        }

        private Conexiones()
        {
        }

        public void CrearConexion()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();

            cs.DataSource = @"localhost\SQLEXPRESS";
            cs.InitialCatalog = "guanatosDB";
            cs.UserID = "user";
            cs.Password = "user123456";

            con = new SqlConnection(cs.ConnectionString);
        }

        public int Guardar(Restaurant restaurant)
        {
            try
            {
                byte[] data = File.ReadAllBytes(restaurant.Path);
                string qry = "insert into tblRestaurant (Giro, Descripcion, Imagen, Path) values (@prGiro, @prDescripcion, @prImagen, @prPath)";

                // Inicializa el objeto SqlCommand
                SqlCommand SqlCom = new SqlCommand(qry, con);

                // Se agrega la información como parámetros
                SqlCom.Parameters.Add(new SqlParameter("@prGiro", restaurant.Giro));
                SqlCom.Parameters.Add(new SqlParameter("@prDescripcion", restaurant.Descripcion));
                SqlCom.Parameters.Add(new SqlParameter("@prImagen", data));
                SqlCom.Parameters.Add(new SqlParameter("@prPath", restaurant.Path));

                // Abrir la conexión y ejecutar el query
                con.Open();
                SqlCom.ExecuteNonQuery();
                // Copy picture files.
                // Remove path from the file name.
                //for(int i = ;)
                File.Copy(Path.Combine("C://Users/webjo/Documents/", "babyshower - copia.jpg"), Path.Combine("/NewFolder", "babyshower - copia.jpg"), true);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                // Cerrar la conexión si esta se encuentra abierta
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return 1;
        }

        public int Guardar(String correo)
        {
            try
            {
                string qry = "insert into tblInscripcion (Correo, Estado) values (@prCorreo, @prEstado)";

                // Inicializa el objeto SqlCommand
                SqlCommand SqlCom = new SqlCommand(qry, con);

                // Se agrega la información como parámetros
                SqlCom.Parameters.Add(new SqlParameter("@prCorreo", correo));
                SqlCom.Parameters.Add(new SqlParameter("@prEstado", 1));

                // Abrir la conexión y ejecutar el query
                con.Open();
                SqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                // Cerrar la conexión si esta se encuentra abierta
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return 1;
        }

        public List<Restaurant> Consulta(String parametro)
        {
            string qry = "select Giro, Descripcion, Imagen, Path from tblRestaurant";
            qry += (parametro.Equals(string.Empty)) ? ";" : " where Giro = @prParametro;";
            Restaurant res;
            SqlDataReader rdr;
            list.Clear();

            try
            {
                // Inicializa el objeto SqlCommand
                SqlCommand SqlCom = new SqlCommand(qry, con);
                SqlCom.Parameters.Add(new SqlParameter("@prParametro", parametro));

                // Abre la conexión y ejecutar el query
                con.Open();
                rdr = SqlCom.ExecuteReader(CommandBehavior.CloseConnection);

                if(rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        res = new Restaurant();
                        // Obtiene los resultados de la búsqueda

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
                    //MessageBox.Show("No existe registro con ese Id", "Búsqueda Nota", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                // Cierra la conexión si esta se encuentra abierta
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return list;
        }

        public List<String> Consulta()
        {
            string qry = "select Correo from tblInscribir;";
            SqlDataReader rdr;
            List<String> list = new List<string>();
            list.Clear();

            try
            {
                // Inicializa el objeto SqlCommand
                SqlCommand SqlCom = new SqlCommand(qry, con);

                // Abre la conexión y ejecutar el query
                con.Open();
                rdr = SqlCom.ExecuteReader(CommandBehavior.CloseConnection);

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        // Obtiene los resultados de la búsqueda

                        list.Add(rdr.GetString(0));
                    }
                }
                else
                {
                    //MessageBox.Show("No existe registro con ese Id", "Búsqueda Nota", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                // Cierra la conexión si esta se encuentra abierta
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return list;
        }

    }

}
