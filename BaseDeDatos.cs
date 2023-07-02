using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentaYControlDeStock
{
     class BaseDeDatos
    {
        public MySqlConnection Conectar()
        {
            string servidor = "localhost";
            string usuario = "root";
            string contraseña = "";
            string baseDeDatos = "control_de_stock";

            string cadenaConexion = "Database =" + baseDeDatos + "; Data Source = " + servidor + "; User Id = " + usuario + "; Password = " + contraseña;

            MySqlConnection conexionDb = new MySqlConnection(cadenaConexion);

            conexionDb.Open();

            return conexionDb;
        }

        
        public List<Productos> ObtenerTablaDeProductos()
        {
            List<Productos> ListaDeProductos = new List<Productos>();

            // SELECCIONO TODAS LAS COLUMNAS Y FILAS DE LA TABLA PRODUCTOS
            string consulta = "SELECT * FROM productos";
            MySqlCommand comando = new MySqlCommand(consulta);
            comando.Connection = Conectar();

            // GUARDO LOS DATOS DE LA CONSULTA EN LECTURA
            MySqlDataReader lectura = comando.ExecuteReader();
            
                while (lectura.Read())
                {
                    // GUARDO LOS DATOS EN OBJETOS Y A ESTOS EN LA LISTA
                    Productos NuevoProducto = new Productos();
                    NuevoProducto.Nombre = lectura.GetString("nombre");
                    NuevoProducto.Precio = Convert.ToDouble(lectura.GetString("precio"));
                    NuevoProducto.Stock = Convert.ToInt32(lectura.GetString("stock"));

                    ListaDeProductos.Add(NuevoProducto);

                }
            lectura.Close();
            

            return ListaDeProductos;

        }


        public void ModificarTablaDeProductos(int stockProducto,int indice)
        {   
            // MODIFICO EL STOCK EN EL INDICE INDICADO
            string consulta = "UPDATE productos SET stock = " + stockProducto + " WHERE id= " + indice ;
            MySqlCommand comando = new MySqlCommand(consulta);
            comando.Connection = Conectar();
            comando.ExecuteNonQuery();
        }


        public void  Desconectar(MySqlConnection con)
        {
            con.Close();
        }
    }
}
