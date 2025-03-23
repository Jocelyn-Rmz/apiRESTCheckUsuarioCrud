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
        public clsApiStatus vwRptUsuario([FromUri]string filtro)
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



    }
}
