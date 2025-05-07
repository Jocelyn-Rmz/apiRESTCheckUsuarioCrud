using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//-------------------------------
using apiRESTCheckUsuario.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
//-------------------------------

namespace apiRESTCheckUsuario.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("check/usuario/spinsusuario")]
        public clsApiStatus spInsUsuario([FromBody] clsUsuario modelo)
        {
            //Definicion de los objetos de modelos
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            //-------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario(modelo.nombre,
                                                        modelo.apellidoPaterno,
                                                        modelo.apellidoMaterno,
                                                        modelo.usuario,
                                                        modelo.contrasena,
                                                        modelo.ruta,
                                                        modelo.tipo);
                ds = objUsuario.spInsUsuario();
                //Configuracion del objeto de salida 
                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse
                                            (ds.Tables[0].Rows[0][0].ToString());

                //Validar el valor de ban
                if (objRespuesta.ban == 0)
                {
                    objRespuesta.msg = "Usuario registrado exitosamente";
                    jsonResp.Add("msgData", "Usuario registado exitosamente");

                }
                else
                {
                    objRespuesta.msg = "Usuario no registrado verificar..";
                    jsonResp.Add("msgData", "Usuario no registrado verificar..");
                }
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida 
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Fallo la inserción de usuario, verificar...";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            //Salida del objeto objRespuesta
            return objRespuesta;

        }

        // endpoint para validación de acceso spValidarAcceso
        [HttpPost]
        [Route("check/usuario/spvalidaracceso")]
        public clsApiStatus spValidarAcceso([FromBody] clsUsuario modelo)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                // Creación del objeto del modelo clsUsuario

                clsUsuario objUsuario = new clsUsuario(modelo.usuario,
                                                       modelo.contrasena);

                ds = objUsuario.spValidarAcceso();

                //Configuracion del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                //Validar el valor recibido en bandera
                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario validado exitosamente!";
                    jsonResp.Add("usu_nombre_completo", ds.Tables[0].Rows[0][1].ToString());
                    jsonResp.Add("usu_ruta", ds.Tables[0].Rows[0][2].ToString());
                    jsonResp.Add("usu_usuario", ds.Tables[0].Rows[0][3].ToString());
                    jsonResp.Add("tip_descripcion", ds.Tables[0].Rows[0][4].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Acceso denegado, verificar...";
                    jsonResp.Add("msgData", "Acceso denegado, verificar..");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;

            }
            //Retorno del obj de salida
            //
            return objRespuesta;

        }


        // Endpoint para validación de acceso vwRptUsuario
        [HttpGet]
        [Route("check/usuario/vwrptusuario")]
        public clsApiStatus vwRptUsuario([FromUri] string filtro)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario(filtro);  // Pasamos el filtro al modelo

                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }

        [HttpGet]
        [Route("check/usuario/vwrptusuario")]
        public IHttpActionResult GetUsuarios(string filtro = "")
        {
            clsUsuario objUsuario = new clsUsuario();
            var ds = objUsuario.vwRptUsuario(filtro);

            var datos = ds.Tables[0];

            return Ok(new
            {
                estatus = "ok",
                datos = new Dictionary<string, object>
        {
            { "vwRptUsuario", datos }
   }
            });
        }

        // endpoint para validación de acceso vwTipoUsuario
        [HttpGet]
        [Route("check/usuario/vwtipousuario")]
        public clsApiStatus vwTipoUsuario()
        {  // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwTipoUsuario();
                //Configuración del objeto de Salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte de TipoUsuario generado exitosamente";
                //Formatear los datos recibidos (DataSet) para 
                //enviarlos de salida (Json)
                // Migración del ds(DataSet) al objeto Json
                string jsonString =
                    JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp =
                    JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }



        //Busca Por clave´para saber si el usuario se encuentra
        [HttpGet]
        [Route("check/usuario/spbuscarusuarioclave")]
        public clsApiStatus GetUsuarioPorClave([FromUri] string clave)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                objUsuario.cve = clave;

                var ds = objUsuario.spBuscarUsuarioPorClave();

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("UsuarioNoEncontrado") == false)
                {
                    objRespuesta.statusExec = true;
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "Usuario encontrado";

                    string json = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    jsonResp = JObject.Parse($"{{\"usuario\": {json}}}");
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.statusExec = true;
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Usuario no encontrado";
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = ex.Message;
            }

            return objRespuesta;
        }


        //Endpoint para modificar usuarios 
        [HttpPut]
        [Route("check/usuario/spupdusuario")]
        public clsApiStatus spUpdUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                objUsuario.cve = modelo.cve;
                objUsuario.nombre = modelo.nombre;
                objUsuario.apellidoPaterno = modelo.apellidoPaterno;
                objUsuario.apellidoMaterno = modelo.apellidoMaterno;
                objUsuario.usuario = modelo.usuario;
                objUsuario.contrasena = modelo.contrasena;
                objUsuario.ruta = modelo.ruta;
                objUsuario.tipo = modelo.tipo;

                var ds = objUsuario.spUpdUsuario();
                string resultado = ds.Tables[0].Rows[0][0].ToString();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(resultado);

                switch (resultado)
                {
                    case "0":
                        objRespuesta.msg = "Usuario modificado correctamente";
                        break;
                    case "1":
                        objRespuesta.msg = "La clave del usuario no existe";
                        break;
                    case "2":
                        objRespuesta.msg = "Ya existe un usuario con ese nombre completo";
                        break;
                    case "3":
                        objRespuesta.msg = "Ya existe un nombre de usuario igual";
                        break;
                    case "4":
                        objRespuesta.msg = "El tipo de usuario no existe";
                        break;
                    default:
                        objRespuesta.msg = "Error desconocido";
                        break;
                }

                jsonResp.Add("msgData", objRespuesta.msg);
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al actualizar usuario";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        //ELIMINAR USUARIOS
        [HttpDelete]
        [Route("check/usuario/spdelusuario")]
        public clsApiStatus spDelUsuario([FromUri] string clave)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                objUsuario.cve = clave;

                ds = objUsuario.spDelUsuario();
                objRespuesta.statusExec = true;
                objRespuesta.ban = 1; // asumimos éxito si llega hasta aquí
                objRespuesta.msg = ds.Tables[0].Rows[0]["mensaje"].ToString();
                jsonResp.Add("msgData", ds.Tables[0].Rows[0]["mensaje"].ToString());
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al eliminar usuario";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }




    }
}
