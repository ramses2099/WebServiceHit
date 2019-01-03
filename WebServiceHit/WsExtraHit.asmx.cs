using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml;

namespace WebServiceHit
{
    public enum Tipo_Referencia : int
    {
        BL = 1,
        Contenedor = 2
     
    }

    public enum Tipo_Carga : int
    {
        Contenedores = 1,
        CargaSuelta = 2

    }

    public enum Tipo_Preliquidacion
    { 
    Almacenaje = 1, Verificacion=2
    }

    /// <summary>
    /// Summary description for WsExtraHit
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WsExtraHit : System.Web.Services.WebService
    {



        [WebMethod]
        public DsExtraHit GetContenedores(string Referencia, Tipo_Referencia Treferencia, string Rnc)
        {
            DsExtraHit oDs = new DsExtraHit();

            //Llenar Ds llamando a SP

            try
            {

                oDs = new DsExtraHit();
                //Instancia Adaptador para llenar tabla
                DsExtraHitTableAdapters.Consulta_ContenedoresTableAdapter Ta = new DsExtraHitTableAdapters.Consulta_ContenedoresTableAdapter();
                //Llena tabla con metodo fill
                Ta.Fill(oDs.Consulta_Contenedores, Referencia, (int)Treferencia, Rnc);

                //Rev:06 02 Enmascarar  
                //Que salga en rojo la columna impedimentos en consulta contenedores: 
                //Facturación permiso = pago verificación, almacenaje permiso = pago almacenaje, todo los que diga permiso = impedimento.
                if (oDs.Consulta_Contenedores.Rows.Count > 0)
                {
                    foreach (DsExtraHit.Consulta_ContenedoresRow r in oDs.Consulta_Contenedores.Rows)
                    {
                        if (Treferencia == Tipo_Referencia.BL)
                        {
                            if (!r.IsIMPEDIMENTOSNull())
                            {
                                if (r.IMPEDIMENTOS == "!DPH_PERMISO,FACTURACION_PERMISO,!ALMACENAJE_PERMISO,!MSC_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "DPH_Impedimento,Pago Verificación,Pago Almacenaje,MSC_Impedimento";
                                }
                                else if (r.IMPEDIMENTOS == "FACTURACION_PERMISO,!ALMACENAJE_PERMISO,!MSC_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "Pago Verificación,Pago Almacenaje,MSC_Impedimento";
                                }
                                else if (r.IMPEDIMENTOS == "!DPH_PERMISO,FACTURACION_PERMISO,!ALMACENAJE_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "DPH_Impedimento,Pago Verificación,Pago Almacenaje";
                                }
                                else if (r.IMPEDIMENTOS == "!ALMACENAJE_PERMISO,!MSC_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "Pago Almacenaje,MSC_Impedimento";
                                }

                                else if (r.IMPEDIMENTOS == "!DPH_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "DPH_Impedimento";
                                }
                                else if (r.IMPEDIMENTOS == "FACTURACION_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "Pago Verificación";
                                }
                                else if (r.IMPEDIMENTOS == "!ALMACENAJE_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "Pago Almacenaje";
                                }
                                else if (r.IMPEDIMENTOS == "!MSC_PERMISO")
                                {
                                    r.IMPEDIMENTOS = "MSC_Impedimento";
                                }
                            }
                            else
                            {
                                r.IMPEDIMENTOS = "";
                            }
                        }
                        else
                        {
                           // r.IMPEDIMENTOS = "N/A";
                        }
                        if (r.IsVGMNull())
                        {
                            r.VGM = 0;
                        }
                        if (r.IsVGM_ENtityNull())
                        {
                            r.VGM_ENtity = "";
                        }

                    }
                    oDs.AcceptChanges();
                }
            }
            catch
            {
                //Cuando hay excepcion, instancia un nuevo dataset
                oDs = new DsExtraHit();
            }
            //Retorna el dataset
            return oDs;


        }

      
        [WebMethod]
        public DsExtraHit GetItinerarios ()
        {
            //Define dataset para retornar
            DsExtraHit oDs ;

            //Llenar Ds llamando a SP
            try
            {
                //Instancia un dataset
                oDs = new DsExtraHit();
                //Instancia Adaptador para llenar tabla
                DsExtraHitTableAdapters.Itinerario_BuqueTableAdapter Ta = new DsExtraHitTableAdapters.Itinerario_BuqueTableAdapter();
                //Llena tabla con metodo fill
                Ta.Fill(oDs.Itinerario_Buque);

                //Rev5:01 Enmascarar el Estado de itinerario
                if (oDs.Itinerario_Buque.Rows.Count > 0)
                {
                    foreach(DsExtraHit.Itinerario_BuqueRow r in oDs.Itinerario_Buque)
                    {
                        if(r.ESTADO == "20INBOUND") 
                        {
                            r.ESTADO = "Entrando";
                        }
                        else if  (r.ESTADO == "40WORKING")
                        {
                            r.ESTADO = "Trabajando";
                        }
  
                        else if(r.ESTADO=="30ARRIVED")
                        {
                            r.ESTADO = "Llegó";
                        }

                     }

                    oDs.AcceptChanges(); 
                 }
             }

            catch 
            {
                //Cuando hay excepcion, instancia un nuevo dataset
                oDs = new DsExtraHit();
            }
                //Retorna el dataset
                return oDs;

            
          }

       
        [WebMethod]
        public DsExtraHit GetCalculoAlmacenaje(Tipo_Carga TipoCarga, double Peso, int Semanas)
   
        {
            DsExtraHit oDs = new DsExtraHit();
            try
            {

                oDs = new DsExtraHit();
                //Instancia Adaptador para llenar tabla
                DsExtraHitTableAdapters.USP_GETCalculoAlmTableAdapter Ta = new DsExtraHitTableAdapters.USP_GETCalculoAlmTableAdapter();
                
                //Llena tabla con metodo fill
                Ta.Fill(oDs.USP_GETCalculoAlm, (int)TipoCarga, Peso, Semanas);

            }

            catch
            {
                //Cuando hay excepcion, instancia un nuevo dataset
                oDs = new DsExtraHit();
            }
            //Retorna el dataset
            return oDs;
        }


        [WebMethod]
        public DsPreliquidaciones.Det_fact_almDataTable  GetAlmacenaje(string Bl, int TipoMoneda, string Rnc)
        {
            DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter Ta = new DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter();

            return Ta.GetData(Bl); 
        }

        [WebMethod]
        public DsPreliquidaciones.Det_fact_VERIDataTable  GetVerificacion(string Bl, int TipoMoneda, string Rnc)
        {
            

            DsPreliquidacionesTableAdapters.Det_fact_VERITableAdapter Ta = new DsPreliquidacionesTableAdapters.Det_fact_VERITableAdapter();

            return Ta.GetData(Bl);
        }

         [WebMethod]
        public DsPreliquidaciones FillDsPReliquidacionesTest()
        {
            DsPreliquidaciones oDs;
            oDs = new DsPreliquidaciones();



            return oDs;
        
        }

        //Rev2:01 GetHistorico 
        [WebMethod]
        public DsHistorico.HistoricoDataTable  GetHistorico (string Rnc)
       {
             
             DsHistoricoTableAdapters.HistoricoTableAdapter Ta = new DsHistoricoTableAdapters.HistoricoTableAdapter();
             return Ta.GetData(Rnc);
         }

        //Rev2:02 GetCliente
        [WebMethod]
        public DsCliente.ClienteDataTable GetCliente(string Rnc)
        {
            DsClienteTableAdapters.ClienteTableAdapter Ta = new DsClienteTableAdapters.ClienteTableAdapter();
            return Ta.GetData(Rnc);
        
        }

        //Rev2:07 SetPreliquidaciones
        [WebMethod]
        public DsPreliquidaciones.Preliquidaciones_ResponseDataTable SetPreliquidacion(string Rnc, DsPreliquidaciones oInDsPreliquidaciones)
        {
            DsPreliquidacionesTableAdapters.Preliquidaciones_ResponseTableAdapter Ta = new DsPreliquidacionesTableAdapters.Preliquidaciones_ResponseTableAdapter();
            return Ta.GetData(Rnc, oInDsPreliquidaciones.GetXml().ToString());
        }

        //Rev4:10 Metodo para obtener una preliquidacion guardada previamente
        [WebMethod]
        public DsPreliquidaciones GetPreliquidacion(string Referencia)
        {
            DsPreliquidaciones oDsPreliquidacionImp = new DsPreliquidaciones();
            
            DsPreliquidaciones oDsPreliquidacion = new DsPreliquidaciones();
            DsPreliquidacionesTableAdapters.XmlPreliquidacionTableAdapter Ta = new DsPreliquidacionesTableAdapters.XmlPreliquidacionTableAdapter();
            Ta.Fill(oDsPreliquidacion.XmlPreliquidacion, Referencia);

            System.IO.StringReader sr = new System.IO.StringReader(oDsPreliquidacion.XmlPreliquidacion.Rows[0]["Ar_Xml"].ToString()); 
            XmlTextReader xr = new XmlTextReader(sr);
            oDsPreliquidacionImp.ReadXml(xr);
            return oDsPreliquidacionImp; 
        }

        //Rev:6 3.Metodo para obtene la tasa del dollar a la fecha
        [WebMethod]
        public double GetTasaDollar()
        {
            DsGeneralTableAdapters.QueriesTableAdapter Ta = new DsGeneralTableAdapters.QueriesTableAdapter();
            double tasar;
            try
            {
                tasar = (Double) Ta.USP_Gettasa(DateTime.Now.Date); 
            }
            catch(Exception ex)
            {
                tasar = -1;
            
            }

            return tasar; 
        }


        //Rev:10 8.Metodo para obtener las unidades
        [WebMethod]
        public DsPreliquidaciones.UnidadesDataTable GetUnidades(string BL)
        {
            DsPreliquidacionesTableAdapters.UnidadesTableAdapter Ta = new DsPreliquidacionesTableAdapters.UnidadesTableAdapter();
            return Ta.GetData(BL);
        }

        [WebMethod]
        public DsExtraHit.LoginExtraDataTable LoginExtra(string Usr, string Pass)
        {
            DsExtraHitTableAdapters.LoginExtraTableAdapter Ta = new DsExtraHitTableAdapters.LoginExtraTableAdapter();
            return Ta.GetData(Usr,Pass);
        
        }

        public struct _getcalculoalm
        {
            public string Codigo;
            public int semana;
            public string bl;
            public string descripcion;
            public double cantidad;
            public double tarifa;
            public double monto;
            public DateTime ValidoHasta;
            public DateTime FechaLlegada;
            public string visita;
            public string buque;
            public string linea;
            public string CodigoRespuesta;
            public string DescripcionRespuesta;
            public string Unidades;
            public string Anexo;
           

        }

        [WebMethod]
        public  _getcalculoalm[] /*DsPreliquidaciones.Det_fact_almDataTable*/ GetCalculoALM(string Bl, int TipoMoneda, string Rnc,string Fecha,string fecha_reliquidacion)
        {
            //DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter dt = new DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter();
            DsPreliquidacionesTableAdapters.GetcalculoALMTableAdapter dt = new DsPreliquidacionesTableAdapters.GetcalculoALMTableAdapter();
            string reliqui;
            DateTime fechafac = Convert.ToDateTime(Fecha);
           

            if (fecha_reliquidacion==""){  
           //if (fecha_reliquidacion.HasValue){
            reliqui="NO";
               //fechafac = Convert.ToDateTime(fecha_reliquidacion);
                // querty = dt.GetData(Bl, fecha_reliquidacion, "SI", Rnc);
            }
            else
            {
               reliqui = "SI";
               fechafac = Convert.ToDateTime(fecha_reliquidacion);
               // querty = dt.GetData(Bl, Fecha, "NO", Rnc);
            }

            var querty = dt.GetData(Bl, fechafac, reliqui, Rnc);

            _getcalculoalm[] estructura = new _getcalculoalm[0]; 
            int n = 0;

            if (querty.LongCount() > 0)
            {
                estructura = new _getcalculoalm[querty.LongCount()];
                foreach (DsPreliquidaciones.GetcalculoALMRow r in querty)
                {
                    estructura[n].Codigo = r.Codigo;
                    estructura[n].semana = r.Semana;
                    estructura[n].bl = r.BL;
                    estructura[n].descripcion = r.Descripcion;
                    estructura[n].cantidad = (double)r.Cantidad;
                    estructura[n].tarifa = r.Tarifa;
                    estructura[n].monto = r.Monto;
                    estructura[n].FechaLlegada = r.FechaLlegada;
                    estructura[n].ValidoHasta = r.ValidoHasta;
                    estructura[n].linea = r.Linea;
                    estructura[n].buque = r.Buque;
                    estructura[n].visita = r.Visita;
                    estructura[n].Unidades = r.Unidades;
                    estructura[n].Anexo = r.Anexo;
                    estructura[n].CodigoRespuesta = "01";
                    estructura[n].DescripcionRespuesta = "Consulta Exitosa";
                    n++;

                }



            }


            return estructura;



        }



        
        public struct _GetalamcenajeEstr
        {
            public DateTime FEC_LLEGA;
            public DateTime FECHA;
            public int cant_semana;
            public string BARCO;
            public string CON_EMBAR;
            public string con_embar_padre;
            public string CONSOLIDADO;
            public string MANIFIESTO;
            public string DESCRIPCION;
            public double cantidad;
            public double tarifa;
            public double Monto;
            public int idfamilia;
            public int Numerocre;
            public DateTime Valido_hasta;
            public string CodigoRespuseta;
            public string DescripcionRespuesta;
        }

        public struct _GetalamcenajeEstrString
        {
            public string FEC_LLEGA;
            public string FECHA;
            public string cant_semana;
            public string BARCO;
            public string CON_EMBAR;
            public string con_embar_padre;
            public string CONSOLIDADO;
            public string MANIFIESTO;
            public string DESCRIPCION;
            public string cantidad;
            public string tarifa;
            public string Monto;
            public string idfamilia;
            public string Numerocre;
            public string Valido_hasta;
            public string CodigoRespuseta;
            public string DescripcionRespuesta;
        }

        [WebMethod]
        public _GetalamcenajeEstr[] /*DsPreliquidaciones.Det_fact_almDataTable*/ GetAlmacenaje_Estr(string Bl, int TipoMoneda, string Rnc)
        {
            DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter dt = new DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter();

            var querty = dt.GetData(Bl);

            _GetalamcenajeEstr[] estructura=  new _GetalamcenajeEstr[0]; ;
            int n=0;

            if (querty.LongCount() > 0) {
                estructura = new _GetalamcenajeEstr[querty.LongCount()];
                foreach (DsPreliquidaciones.Det_fact_almRow r in querty)
                {
                    estructura[n].FEC_LLEGA = r.FECHA_LLEGADA;
                    estructura[n].cant_semana = (int)r.SEMANAS;
                    estructura[n].BARCO = r.BARCO;
                    estructura[n].CON_EMBAR = r.BL;
                    estructura[n].con_embar_padre = r.BL_PADRE;
                    estructura[n].CONSOLIDADO= r.CONSOLIDADO;
                    estructura[n].MANIFIESTO=r.MANIFIESTO;
                    estructura[n].DESCRIPCION = r.DESCRIPCION;
                    estructura[n].cantidad=(double)r.CANTIDAD;
                    estructura[n].tarifa=(double)r.TARIFA;
                    estructura[n].Monto = (double)r.MONTO;
                    estructura[n].idfamilia = r.FAMILIA;
                    estructura[n].Numerocre = r.NUMERO_CRE;
                    estructura[n].CodigoRespuseta = "01";
                    estructura[n].DescripcionRespuesta = "Consulta Exitosa";
                    n++;

                }



            }
         

            return estructura;
         

                      
        }

        [WebMethod]
        public _GetalamcenajeEstrString[] /*DsPreliquidaciones.Det_fact_almDataTable*/ GetAlmacenaje_EstrString(string Bl, int TipoMoneda, string Rnc)
        {
            DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter dt = new DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter();

            var querty = dt.GetData(Bl);

            _GetalamcenajeEstrString[] estructura = new _GetalamcenajeEstrString[0]; ;
            int n = 0;

            if (querty.LongCount() > 0)
            {
                estructura = new _GetalamcenajeEstrString[querty.LongCount()];
                foreach (DsPreliquidaciones.Det_fact_almRow r in querty)
                {
                    estructura[n].FEC_LLEGA = r.FECHA_LLEGADA.ToString() + "|";
                    estructura[n].cant_semana = r.SEMANAS.ToString() + "|";
                    estructura[n].BARCO = r.BARCO.ToString() + "|";
                    estructura[n].CON_EMBAR = r.BL.ToString() + "|";
                    estructura[n].con_embar_padre = string.Format("{0}|", r.BL_PADRE);
                    estructura[n].CONSOLIDADO = string.Format("{0}|", r.CONSOLIDADO);
                    estructura[n].MANIFIESTO = r.MANIFIESTO.ToString() + "|";
                    estructura[n].DESCRIPCION = r.DESCRIPCION.ToString() + "|";
                    estructura[n].cantidad = r.CANTIDAD.ToString() + "|";
                    estructura[n].tarifa = r.TARIFA.ToString() + "|";
                    estructura[n].Monto = r.MONTO.ToString() + "|";
                    estructura[n].idfamilia = r.FAMILIA.ToString() + "|";
                    estructura[n].Numerocre = r.NUMERO_CRE.ToString() + "|";
                    estructura[n].CodigoRespuseta = "01".ToString() + "|";
                    estructura[n].DescripcionRespuesta = "Consulta Exitosa|";
                    n++;

                }



            }


            return estructura;



        }

        //Rev2:07 SetPreliquidaciones
        [WebMethod]
        public DsPreliquidaciones.Preliquidaciones_ResponseDataTable SetPreliquidacionxml(string Rnc, string oInDsPreliquidaciones)
        {
            DsPreliquidacionesTableAdapters.Preliquidaciones_ResponseTableAdapter Ta = new DsPreliquidacionesTableAdapters.Preliquidaciones_ResponseTableAdapter();
            return Ta.GetData(Rnc, oInDsPreliquidaciones);
        }
        
        
        //[WebMethod]
        //public string /*DsPreliquidaciones.Det_fact_almDataTable*/ GetAlmacenaje_js(string Bl, int TipoMoneda, string Rnc)
        //{
        //    DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter Ta = new DsPreliquidacionesTableAdapters.Det_fact_almTableAdapter();

        //    var result = Ta.GetData(Bl);
        //    var items = result.Select(r => new
        //    {
        //        FEC_LLEGA = r.IsFECHA_LLEGADANull() ? default(DateTime) : r.FECHA_LLEGADA,
        //        cant_semana = (int)r.SEMANAS,
        //        BARCO = r.BARCO,
        //        CON_EMBAR = r.BL,
        //        con_embar_padre = r.BL_PADRE,
        //        CONSOLIDADO =  r.IsCONSOLIDADONull() ? ,
        //        MANIFIESTO = r.MANIFIESTO,
        //        DESCRIPCION = r.DESCRIPCION,
        //        cantidad = (double)r.CANTIDAD,
        //        tarifa = (double)r.TARIFA,
        //        Monto = (double)r.MONTO,
        //        idfamilia = r.FAMILIA,
        //        Numerocre = r.NUMERO_CRE,
        //        CodigoRespuseta = "01",
        //        DescripcionRespuesta = "Consulta Exitosa",
        //    });
        //    var js = new JavaScriptSerializer();
        //    var strJSON = js.Serialize(items);

        //    return strJSON;
        //}


    }
}
