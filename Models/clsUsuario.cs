using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//----------------------------
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
//----------------------------


namespace apiRESTCheckUsuario.Models
{
    public class clsUsuario
    {

        // Definición de atributos
        public string cve { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string ruta { get; set; }
        public string tipo { get; set; }

        //Métodos y atributos de funcionalidad y seguridad 
        string cadConn = ConfigurationManager.ConnectionStrings["bd_control_acceso"].ConnectionString;

        //Constructores 
        //1. Solo para listas
        //2. Para el endpoint de acceso
        //3. Registro de toda la información 
        public clsUsuario()
        {
            //Código pendiente...
        }
        public clsUsuario(string usuario, string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }
        public clsUsuario(string nombre,
            string apellidoPaterno,
            string apellidoMaterno,
            string usuario,
            string contrasena,
            string ruta,
            string tipo)
        {
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.ruta = ruta;
            this.tipo = tipo;
        }

        //Método para la ejecucion de spInsUsuario
        public DataSet spInsUsuario()
        {

            string cadSql = "CALL spInsUsuario('" + this.nombre +
                                            "', '" + this.apellidoPaterno +
                                            "', '" + this.apellidoMaterno +
                                            "', '" + this.usuario +
                                            "', '" + this.contrasena +
                                            "', '" + this.ruta +
                                            "'," + this.tipo + ")";

            // Configuración de los objetos de conexion a datos 

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();

            //Ejecucion del adaptador de dtaos (reto9rna un DataSet)
            da.Fill(ds, "spInsUsuario");

            //Retorna los datos recibidos 
            return ds;
        }
        // Proceso de validación de usuarios (spValidarAcceso)
        public DataSet spValidarAcceso()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spValidarAcceso('" + this.usuario + "','"
                                              + this.contrasena + "');";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spValidarAcceso");
            return ds;
        }
        // Proceso de validación de usuarios (svwRptUsuario)




        public DataSet vwRptUsuario(string filtro = "", string tipoUsuario = "")
        {
            string cadSQL = "SELECT Clave, Nombre, Usuario, Foto, SUBSTRING_INDEX(Rol, '-', -1) AS Rol FROM vwRptUsuario WHERE 1=1";

            if (!string.IsNullOrEmpty(filtro))
            {
                cadSQL += " AND (Nombre LIKE '%" + filtro + "%' OR Usuario LIKE '%" + filtro + "%')";
            }

            if (!string.IsNullOrEmpty(tipoUsuario))
            {
                cadSQL += " AND Clave = '" + tipoUsuario + "'";
            }

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "vwRptUsuario");
            return ds;
        }




        // Proceso de validación de usuarios (vwTipoUsuario)
        public DataSet vwTipoUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "SELECT * FROM vwTipoUsuario";

            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwTipoUsuario");
            return ds;
        }


        //Busca al usuario por nombre
        public DataSet spBuscarUsuarioPorClave()
        {
            string cadSQL = $"CALL spBuscarUsuarioPorClave({this.cve})";

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "spBuscarUsuarioPorClave");
            return ds;
        }

        //Modificara los datos del usuario 
        public DataSet spUpdUsuario()
        {
            string cadSQL = $"CALL spUpdUsuario({this.cve}, " +
                            $"'{this.nombre}', " +
                            $"'{this.apellidoPaterno}', " +
                            $"'{this.apellidoMaterno}', " +
                            $"'{this.usuario}', " +
                            $"'{this.contrasena}', " +
                            $"'{this.ruta}', " +
                            $"{this.tipo})";

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "spUpdUsuario");
            return ds;
        }

        //Eliminar Usuarios
        public DataSet spDelUsuario()
        {
            string cadSQL = $"CALL spDelUsuario({this.cve})";

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "spDelUsuario");

            return ds;
        }





    }

}
