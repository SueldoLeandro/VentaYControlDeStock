using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VentaYControlDeStock
{
    class Program
    {
        static void Main(string[] args)
        {
            // LLENO LOS OBJETOS CON LA INFORMACION DEL ARCHIVO JSON

            string json = File.ReadAllText(@"F:\PRACTICA C#\VentaYControlDeStock\Datos.json");// PONER LA DIRECCION DEL ARCHIVO JSON
            List<Productos> ListaDeProductos = (List<Productos>)JsonConvert.DeserializeObject(json,typeof (List<Productos>));

            
            // ELECCION DE COMPRAS
            double TotalAPagar=0;

            for(int i =0; i < ListaDeProductos.Count; i++) {

                Console.WriteLine("\n\n ¿ Desea comprar {0} al precio de $ {1}  ?. " , ListaDeProductos[i].Nombre , ListaDeProductos[i].Precio);

                // MENU DE OPCIONES
                Console.WriteLine("\n Opciones:");
                Console.WriteLine("Si y ver el siguiente producto, ingrese: 1");
                Console.WriteLine("No y ver el siguiente producto, ingrese: 2");
                int respuesta = Convert.ToInt32(Console.ReadLine());


                // EN CASO DE COMPRAR EJECUTA ESTE CODIGO
                if(respuesta == 1)
                {   
                    Console.WriteLine("\n Cuantos {0} quiere comprar." , ListaDeProductos[i].Nombre);
                    respuesta = Convert.ToInt32(Console.ReadLine());

                    // EN CASO DE QUE ELIJA COMPRAR MAS DE CERO PRODUCTOS EJECUTA ESTE CODIGO
                    if(respuesta > 0)
                    {
                        Console.WriteLine("\n Se agrego {0} {1} a su carrito: " , respuesta , ListaDeProductos[i].Nombre);

                        // SE LE SUMA AL CARRITO LO QUE VA COMPRANDO
                        TotalAPagar += (ListaDeProductos[i].Precio * respuesta);

                        // SE CONTROLA STOCK DEL PRODUCTO
                        ListaDeProductos[i].Stock -= respuesta;
                        BaseDeDatos NuevaBaseDeDatos1 = new BaseDeDatos();
                        NuevaBaseDeDatos1.ModificarTablaDeProductos(ListaDeProductos[i].Stock, i);

                        // SE MUESTRA EL VALOR DEL CARRITO TOTAL
                        Console.WriteLine("Total a pagar es de $ : " + TotalAPagar);
                    }
                    

                }

                

            }

            // PASO LA LISTA A FORMATO JSON
            string jsonModificado = JsonConvert.SerializeObject(ListaDeProductos);

            // LIMPIO LA LISTA
            ListaDeProductos.Clear();

            // ESCRIBIR EL NUEVO CONTENIDO EN EL ARCHIVO JSON
            File.WriteAllText(@"F:\PRACTICA C#\VentaYControlDeStock\Datos.json", jsonModificado);



            Console.WriteLine("\n\n Compra finalizada, vuelva pronto.");



            // VERIFICO SI LOS CAMBIOS EN BASE DE DATOS SE HICIERON
            Console.WriteLine("\n\n Control de stock.");

            List<Productos> productosDeBaseDeDatos = new List<Productos>();
            BaseDeDatos NuevaBaseDeDatos = new BaseDeDatos();

            productosDeBaseDeDatos = NuevaBaseDeDatos.ObtenerTablaDeProductos();

            Console.WriteLine("\nBase de datos modificado.");
            for (int i = 0; i < productosDeBaseDeDatos.Count; i++)
            {
                Console.WriteLine(productosDeBaseDeDatos[i].Nombre);
                Console.WriteLine(productosDeBaseDeDatos[i].Precio);
                Console.WriteLine(productosDeBaseDeDatos[i].Stock + "\n");
            }


            // VERIFICO SI LOS CAMBIOS EN JSON SE HICIERON
            string nuevaLectura = File.ReadAllText(@"F:\PRACTICA C#\VentaYControlDeStock\Datos.json");
            ListaDeProductos = (List<Productos>)JsonConvert.DeserializeObject(nuevaLectura, typeof(List<Productos>));

            Console.WriteLine("\n\nJson modificado.");
            for (int i=0; i < ListaDeProductos.Count; i++)
            {
                
                Console.WriteLine("El nombre del producto es: " + ListaDeProductos[i].Nombre);
                Console.WriteLine("El precio del producto es: " + ListaDeProductos[i].Precio);
                Console.WriteLine("El sotck del producto es: " + ListaDeProductos[i].Stock + "\n");
            }


            NuevaBaseDeDatos.Desconectar(NuevaBaseDeDatos.Conectar());
            Console.ReadKey();
        }
    }
}
