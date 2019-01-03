using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;

namespace WebServiceHit
{ 
    //Rev4:15 Enumeracion para determinar el tipo de Render Historico
        public enum RenderHistorico : short 
        { 
            Pendientes=1, Pagadas=2, PendientesPagadas=3
        }

    public partial class WfTest : System.Web.UI.Page
    {
        DsPreliquidaciones oDsConsulta;
        DsHistorico oDsHistorico;

       

        //Rev4:01 El tipo de la variable cliente es DsPreliquidaciones, se cambio para incluir todos los datos del cliente y facilitar la reimpresion
        DsPreliquidaciones  oDsCliente;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
               
                WsExtraHit ws = new WsExtraHit();

                
                //Rev4:02 Obtener el rnc del cliente autenticado. FAVOR REEMPLAZAR DATO FIJO 101017831 CON EL User.Identity.Name
                Session.Add("Rnc", "101017831");
                //Rev4:03 Obtener datos del cliente autenticado y mantenerlo en sesion
                oDsCliente = new DsPreliquidaciones();
                oDsCliente.Cliente.Load(ws.GetCliente(Session["Rnc"].ToString()).CreateDataReader());
                Session.Add("oDsCliente", oDsCliente);

                
            }

            //Rev4:15 Renderizar Historico 
            RenderHistoricos(RenderHistorico.PendientesPagadas);

        }

        //Rev4:15 Renderizar Historico Pendientes de mas reciente a mas antiguo
        protected  void RenderHistoricos(RenderHistorico R)
        {
            WsExtraHit ws = new WsExtraHit(); 
            oDsHistorico = new DsHistorico();
            oDsHistorico.Historico.Load(ws.GetHistorico(Session["Rnc"].ToString()).CreateDataReader());

         switch (R)
         {
             case RenderHistorico.Pendientes :
                var q =
                from cli in oDsHistorico.Historico.AsEnumerable()
                where cli.Estatus.ToLower() == "pendiente"
                orderby cli.Referencia descending
                select new
                {

                    Servicio = cli.Servicio,
                    Referencia = cli.Referencia,
                    Monto = string.Format("{0:C}", cli.Monto) 
               
                };

                 //DdMoneda.Text == "RD$" ? string.Format("RD{0:C}", ) : string.Format("U{0:C}", )

                GdPendientes.DataSource = q.ToList();
                GdPendientes.DataBind();

                 break;

             case RenderHistorico.Pagadas:
               
                 //Evita null en NCF
                 foreach (DsHistorico.HistoricoRow r in oDsHistorico.Historico)
                 {
                     if (r.IsNCFNull())
                     {
                         r.NCF = "";
                     }
                 }

                 var p =
               from cli in oDsHistorico.Historico.AsEnumerable()
               where cli.Estatus.ToLower() == "pagada"
               orderby cli.NCF descending 
               select new
               {

                   Servicio = cli.Servicio,
                   Referencia = cli.Referencia, 
                   BL = cli.BL,
                   NCF = cli.NCF,
                  
                   Monto = string.Format("{0:C}", cli.Monto)
               };


                GdPagadas.DataSource = p.ToList();
                GdPagadas.DataBind();
                 break;

             case RenderHistorico.PendientesPagadas:
                 RenderHistoricos(RenderHistorico.Pendientes);
                 RenderHistoricos(RenderHistorico.Pagadas); 
                 break;
     
         }
         


            
            
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            PnAlma.Visible = true;
            PnVeri.Visible = false;
            CambioTipoPreliquidacion();



        }

        void CambioTipoPreliquidacion()
        {
            Session["oDsConsulta"] = null;
            GridVeri.DataSource = "";
            GridVeri.DataBind();
            GridAlma.DataSource = "";
            GridAlma.DataBind();
            //Rev:10 5.Limpriar ChekList
            LbCostoPre.Text = "";
            TxReferencia.Text = "";
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            PnAlma.Visible = false;
            PnVeri.Visible = true;
            CambioTipoPreliquidacion();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Rev4:17 Proteccion de ejeccion de excepciones
            try
            {
                //Instancia un nuevo Dataset y servicio en cada consulta
                oDsConsulta = new DsPreliquidaciones();
                WsExtraHit ws = new WsExtraHit();


                if (RbAlma.Checked)
                {//Preliquidacion es de Almacenaje
                    //Rev4:16 Carga en Dsconsulta lo que trae GetAlmacenaje del servicio ExtraHit para el Rnc del usurio
                    oDsConsulta.Det_fact_alm.Load(ws.GetAlmacenaje(TxReferencia.Text, int.Parse(DdMoneda.SelectedValue), Session["Rnc"].ToString()).CreateDataReader());

                    //Rev4:17 Mostrar mensaje que  bl invalido
                    if (oDsConsulta.Det_fact_alm.Rows.Count < 1)
                    {
                        lblError.Text = "BL Incorrecto";

                        return;
                        
                    }

                    //Rev:10 2.Verificacion de campos en null
                    if(ChkHaveNull(ref oDsConsulta, oDsConsulta.Det_fact_alm) == true)
                    {
                        lblError.Text = "BL No disponible para preliquidaciones.";

                        return;
                    }


                    lblError.Text = "";
                    lblError1.Text = "";

                    //Rev:06 d. Realiza las conversiones según la moneda seleccionada
                    if (DdMoneda.SelectedValue == "2")
                    {
                        //Conversión RD$ a USD
                        foreach (DsPreliquidaciones.Det_fact_almRow r in oDsConsulta.Det_fact_alm.Rows)
                        {
                            //Rev:8 3.Conversion a Dolares controlando tasa en null
                            //Controla excepcion cuando TASA es null para el registro consultar al servicio por una tasa para poder convertir los montos
                            //Esto se puede evitar desde la base de datos asegurando que siempre hay una tasa.
                            //if(r.IsTASANull())
                            //{
                            //    r.TASA =  ws.GetTasaDollar();
                            //}
                            //if (r.IsMONTONull())
                            //{
                            //    r.MONTO = 0;
                            //}
                            //if (r.IsTARIFANull())
                            //{
                            //    r.TARIFA = 0;
                            
                            //}
                            r.TARIFA /= Convert.ToDecimal(r.TASA);
                            r.MONTO /= Convert.ToDecimal(r.TASA); 
                        }

                    }

                    //Rev:8 4.Formatear la moneda RD$ U$ segun Ddlmoneda
                     LbCostoPre.Text = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", oDsConsulta.Det_fact_alm.Sum(w => w.MONTO)) : string.Format("U{0:C}", oDsConsulta.Det_fact_alm.Sum(w => w.MONTO));

                     //Enlaza el grid
                    //Rev:06 f. Enlazar Grid de busqueda
                    var ql = from fila in oDsConsulta.Det_fact_alm.AsEnumerable()
                            select new 
                            { 
                                TRANSACCION = fila.ID_TRANSACCION,
                                FECHA_LLEGADA = fila.FECHA_LLEGADA ,
                                FECHA = fila.FECHA ,
                                SEMANAS = fila.SEMANAS,
                                BL= fila.BL,
                                BL_PADRE=fila.BL_PADRE, 
                                CONSOLIDADO=fila.CONSOLIDADO,
                                MANIFIESTO=fila.MANIFIESTO, 
                                DESCRIPCION=fila.DESCRIPCION,
                                CANTIDAD=fila.CANTIDAD,

                                ////Rev:7 2.Conversion RD US segun Ddlmoneda
                                TARIFA = DdMoneda.SelectedValue  == "1" ? string.Format("RD{0:C}", fila.TARIFA) : string.Format("U{0:C}", fila.TARIFA),
                                MONTO = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", fila.MONTO) : string.Format("U{0:C}", fila.MONTO),

                                FAMILIA=fila.FAMILIA,
                                NUMERO_CRE=fila.NUMERO_CRE,
                                VALIDO_HASTA=fila.VALIDO_HASTA,
                                BARCO=fila.BARCO,
                                TASA=fila.TASA 
                            
                            };
                    //GridAlma.DataSource = oDsConsulta.Det_fact_alm;
                    GridAlma.DataSource = ql.ToList();
                    //GridAlma.DataMember = "Det_fact_alm";
                    GridAlma.DataBind();
                }
                else
                {//Preliquidacion es de Verificacion 
                    //Rev4:16 Carga en Dsconsulta lo que trae GetVerificacion del servicio ExtraHit para el Rnc del usurio
                    oDsConsulta.Det_fact_VERI.Load(ws.GetVerificacion(TxReferencia.Text,  int.Parse(DdMoneda.SelectedValue), Session["Rnc"].ToString()).CreateDataReader());
                    
                    //Rev4:17 Mostrar mensaje que  bl inválido
                    if (oDsConsulta.Det_fact_VERI.Rows.Count < 1)
                    {
                        lblError1.Text = "BL Incorrecto";


                        return;

                    }

                    //Rev:10 3.Verificacion de campos en null
                    if (ChkHaveNull(ref oDsConsulta, oDsConsulta.Det_fact_VERI) == true)
                    {
                        lblError1.Text = "BL No disponible para preliquidaciones.";

                        return;
                    }


                    lblError1.Text = "";
                    lblError.Text = "";


                    //Rev:06 e. Realiza las conversiones según la moneda seleccionada
                    if (DdMoneda.SelectedValue == "2")
                    {
                        //Conversión RD$ a USD
                        foreach (DsPreliquidaciones.Det_fact_VERIRow r in oDsConsulta.Det_fact_VERI.Rows)
                        {
                            //Rev:8 3.Conversion a Dolares controlando tasa en null
                            //Controla excepcion cuando TASA es null para el registro consultar al servicio por una tasa para poder convertir los montos
                            //Esto se puede evitar desde la base de datos asegurando que siempre hay una tasa.
                            //if (r.IsTASANull())
                            //{
                            //    r.TASA = ws.GetTasaDollar();
                            //}
                            //if (r.IsSUBTOTALNull())
                            //{
                            //    r.SUBTOTAL = 0;
                            //}
                            //if (r.IsTOTALNull())
                            //{
                            //    r.TOTAL = 0;
                            //}
                            //if (r.IsITBISNull())
                            //{
                            //    r.ITBIS = 0;
                            //}
                            //if (r.IsGLNull())
                            //{
                            //    r.GL = "";
                            //}
                            //if (r.IsPRECIO_USNull())
                            //{
                            //    r.PRECIO_US = 0;
                            //}

                            r.SUBTOTAL /= Convert.ToDecimal(r.TASA);
                            r.ITBIS /= Convert.ToDecimal(r.TASA);
                            r.TOTAL /= Convert.ToDecimal(r.TASA);
                        }

                    }

                    //Rev:9 2.Carga contenedores para la referencia dada en la lista de incluir
                    var VerificacionesIncluir = (from vf in oDsConsulta.Det_fact_VERI select new {BL = vf.BL, EQUIPAMENTO = vf.EQUIPAMENTO, TOTAL = oDsConsulta.Det_fact_VERI.Select("EQUIPAMENTO='" + vf.EQUIPAMENTO +  "'").Sum(w=> Convert.ToDecimal(w["TOTAL"])) }).Distinct();
                    var resumen = (from fila in VerificacionesIncluir select new { CADENA = fila.EQUIPAMENTO + ": " + (DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", fila.TOTAL) : string.Format("U{0:C}", fila.TOTAL)), EQUIPAMENTO = fila.EQUIPAMENTO });
                    CkLIncluir.DataTextField = "CADENA";
                    CkLIncluir.DataValueField = "EQUIPAMENTO";
                    CkLIncluir.DataSource = resumen;
                    CkLIncluir.DataBind();
                    //Rev:10 7. Por defecto, Todos los contenedores de la verificacion cargan seleccionados
                    CkTodos.Checked = true;
                    Session.Add("oDsConsulta", oDsConsulta);
                    CkTodos_CheckedChanged(CkTodos, new EventArgs());

                    //Rev:9 2a. Mover el enlace del grid detalle al evento SelectedIndexChaged en al seccion Rev:9 3.
                  ////  //Rev:8 4.Formatear la moneda RD$ U$ segun Ddlmoneda
                  ////  LbCostoPre.Text = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", oDsConsulta.Det_fact_VERI.Sum(w => w.TOTAL)) : string.Format("U{0:C}", oDsConsulta.Det_fact_VERI.Sum(w => w.TOTAL));

                  ////   //Enlaza el grid
                  ////  //Rev:06 g. Enlazar Grid de busqueda
                  ////  var qr = from filav in oDsConsulta.Det_fact_VERI.AsEnumerable()
                  ////          select new
                  ////          {
                               
                  ////              BL=filav.BL ,
                  ////              EQUIPAMENTO=filav.EQUIPAMENTO,
                  ////              CANTIDAD=filav.CANTIDAD,
                  ////              EVENTO=filav.EVENTO,
                  ////              GL=filav.GL,
                  ////              TASA=filav.TASA,
                  ////              PRECIO_US = string.Format("{0:C}", filav.PRECIO_US),
                  ////              SUBTOTAL = string.Format("{0:C}", filav.SUBTOTAL),
                  ////              ITBIS = string.Format("{0:C}", filav.ITBIS),
                  ////              TOTAL = string.Format("{0:C}", filav.TOTAL)
                  ////          };

                  ////GridVeri.DataSource = qr.ToList();
                  ////  //GridVeri.DataSource = oDsConsulta.Det_fact_VERI;
                  ////  //GridVeri.DataMember = "Det_fact_alm";
                  ////  GridVeri.DataBind();
                }

                //Almacena en sesion la consulta
                Session.Add("oDsConsulta", oDsConsulta);
            }
            catch (DataException)
            { 


            }

           
           
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            DsPreliquidaciones oDsCarrito=null;

            //El usuario ha buscado algun BL
            if (Session["oDsConsulta"] != null)
            {
                //Descarga de sesion la consulta a preliquidar y el carrito
                DsPreliquidaciones oDsConsulta = (DsPreliquidaciones)Session["oDsConsulta"];
                
                //Rev4:14 Verifica si debe crear una nueva instancia del carrito o usar una ya existente
                if (Session["oDsCarrito"] != null)
                {
                     oDsCarrito = (DsPreliquidaciones)Session["oDsCarrito"];
                }
                else 
                {
                    oDsCarrito = new DsPreliquidaciones();
                }
                    //Determina el tipo de preliquidacion
                if (RbAlma.Checked)
                {//Preliquidacion es de Almacenaje

                    //Entra al carrito el detalle
                    oDsCarrito.Det_fact_alm.Load(oDsConsulta.Det_fact_alm.CreateDataReader());

                    //Entra al carrito el Resumen
                    oDsCarrito.Preliquidaciones.AddPreliquidacionesRow(oDsConsulta.Det_fact_alm.Rows[0]["BL"].ToString(), 1,"Almacenaje", oDsConsulta.Det_fact_alm.Sum(w => w.MONTO),DdMoneda.SelectedItem.Text  );

                }
                else 
                {//Preliquidacion es de Verificacion 
                    //Entra al carrito el detalle
                    if (Session["oDsVeri"] == null || ((DsPreliquidaciones)Session["oDsVeri"]).Det_fact_VERI.Rows.Count <= 0)
                    {
                        return;
                    }
                    oDsConsulta = (DsPreliquidaciones)Session["oDsVeri"];
                    oDsCarrito.Det_fact_VERI.Load(oDsConsulta.Det_fact_VERI.CreateDataReader());

                    //Entra al carrito el Resumen
                    oDsCarrito.Preliquidaciones.AddPreliquidacionesRow(oDsConsulta.Det_fact_VERI.Rows[0]["BL"].ToString(), 2, "Verificación", oDsConsulta.Det_fact_VERI.Sum(w => w.TOTAL), DdMoneda.SelectedItem.Text);

                }

                //Rev:8 4.Formatear la moneda RD$ U$ segun Ddlmoneda
                LbTotal.Text =  DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", oDsCarrito.Preliquidaciones.Sum(w=>w.TOTAL)) :  string.Format("U{0:C}",oDsCarrito.Preliquidaciones.Sum(w=>w.TOTAL)) ;

                //Almacena el carrito en sesion
                Session["oDsCarrito"] = oDsCarrito; 

                //Rev:8 5.Formatear la moneda de las preliquidaciones agregadas cuando se enlaza el DataList
                var qpre = from pre in oDsCarrito.Preliquidaciones.AsEnumerable()
                           select new
                           {
                               PRELIQUIDACION = pre.PRELIQUIDACION,
                               BL = pre.BL,
                               TOTAL =  DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", pre.TOTAL) :  string.Format("U{0:C}",pre.TOTAL) 
                              };

                DataList1.DataSource = qpre;
                DataList1.DataBind();

                LbCostoPre.Text = "";
                TxReferencia.Text = "";
                lblError.Text = "";
                lblError1.Text = "";
                GridAlma.DataSource = "";
                GridAlma.DataBind();
                GridVeri.DataSource = "";
                GridVeri.DataBind();

                //Rev:10 4.Limpiar CheckList al agregar verificacion
                CkLIncluir.DataSource = "";
                CkLIncluir.DataBind();
                Session.Remove("oDsVeri");

                
            }

          
 
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //Rev4:13 Verifica se ha realizado una pre liquidacion
            if (Session["oDsCarrito"] != null)
            {
                //Rev2:08 Boton Guardar del formulario Preliquidaciones
                WsExtraHit ws = new WsExtraHit();

                //Rev2:08 Baja de memoria el carrito para enviarlo a SetPreliquidaciones, Esto almacena y entrega Numero referencia
                DsPreliquidaciones oDsCarrito = (DsPreliquidaciones)Session["oDsCarrito"];

                //Rev4:04 Baja de memoria el cliente para adicionar sus datos en la pre liquidacion actual
                oDsCliente = (DsPreliquidaciones)Session["oDsCliente"];
                oDsCarrito.Cliente.Load(oDsCliente.Cliente.CreateDataReader());

                //Rev4:05 Calcula los totales de la preliquidacion y otros campos que son necesarios para la factura
                string sServiciosPre = "";

                if (oDsCarrito.Det_fact_alm.Rows.Count > 0 && oDsCarrito.Det_fact_VERI.Count > 0)
                {
                    sServiciosPre = "Servicios pre liquidados de verificación y almacenaje";
                }
                else if (oDsCarrito.Det_fact_alm.Rows.Count > 0)
                {
                    sServiciosPre = "Servicios pre liquidados de almacenaje";
                }
                else if (oDsCarrito.Det_fact_VERI.Count > 0)
                {
                    sServiciosPre = "Servicios pre liquidados de verificación";
                }

                oDsCarrito.PreliquidacionTotales.AddPreliquidacionTotalesRow(oDsCarrito.Preliquidaciones.Sum(w => w.TOTAL) - oDsCarrito.Det_fact_VERI.Sum(w => w.ITBIS), oDsCarrito.Preliquidaciones.Sum(w => w.TOTAL), oDsCarrito.Det_fact_VERI.Sum(w => w.ITBIS), DdMoneda.SelectedItem.Text, sServiciosPre);

                //Rev2:08 Somete la preliquidacion
                oDsCarrito.Preliquidaciones_Response.Load(ws.SetPreliquidacion((string)Session["Rnc"], oDsCarrito).CreateDataReader());

                //Rev2:08 Almacena el carrito en memoria
                Session.Add("oDsCarritoImp", oDsCarrito);

                //Rev4:11 Limpiar Preliquidacion
                ClearPage();
                RenderHistoricos(RenderHistorico.Pendientes);

                //Rev2:08 Abre el voucher
                OpenNewWindow("Voucher.aspx");
            }
       }

        //Rev4:11 Limpiar los controles de la preliquidacion actual
        private void ClearPage()
        {
            GridAlma.DataSource = "";
            GridAlma.DataBind();

            GridVeri.DataSource = "";
            GridVeri.DataBind();

            TxReferencia.Text = "";

            DdTipoReferencia.SelectedIndex = -1;
            DdMoneda.SelectedIndex = -1;

            LbTotal.Text = "";
            LbCostoPre.Text = "";

            DataList1.DataSource = "";
            DataList1.DataBind();

            Session["oDsCarrito"] = null;


        }

        //Rev2:08 Para abrir una nueva ventana
        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "HIT-Voucher", String.Format("<script>window.open('{0}');</script>", url));
        }

        protected void GdPendientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Rev4:09 Obtener la pre liquidacion desde servicio web con la referencia seleccionada 
            WsExtraHit ws = new WsExtraHit();
            DsPreliquidaciones oDsCarrito = ws.GetPreliquidacion(GdPendientes.SelectedRow.Cells[1].Text);
            oDsCarrito.Preliquidaciones_Response.AddPreliquidaciones_ResponseRow(GdPendientes.SelectedRow.Cells[1].Text, "");

            //Rev4:09 Subir a memoria pre liquidacion y abrir el Boucher
            Session.Add("oDsCarritoImp", oDsCarrito);
            OpenNewWindow("Voucher.aspx");
        }

        //Rev4:12 Habiliar la paginacion del grid de historico pendientes
        protected void GdPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GdPendientes.PageIndex = e.NewPageIndex;
            GdPendientes.DataBind();
        }

        //Rev4:12 Habiliar la paginacion del grid de historico pagadas
        protected void GdPagadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GdPagadas.PageIndex = e.NewPageIndex;
            GdPagadas.DataBind();
        }

        protected void GdPagadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Rev4:18 Obtener la factura desde servicio web con la Referencia seleccionada 
            WsExtraHit ws = new WsExtraHit();
            DsPreliquidaciones oDsCarrito = ws.GetPreliquidacion(GdPagadas.SelectedRow.Cells[1].Text);
            oDsCarrito.Preliquidaciones_Response.AddPreliquidaciones_ResponseRow(GdPagadas.SelectedRow.Cells[1].Text, "");

            //Rev4:18 Especifica Referencia, BL y NCF Seleccionados 
            oDsCarrito.PreliquidacionNCF.AddPreliquidacionNCFRow(GdPagadas.SelectedRow.Cells[1].Text, GdPagadas.SelectedRow.Cells[2].Text, GdPagadas.SelectedRow.Cells[3].Text);

            //Rev4:18 Subir a memoria pre liquidacion y abrir la factura
           
            Session.Add("oDsCarritoFact", oDsCarrito);
            if (GdPagadas.SelectedRow.Cells[0].Text == "Almacenaje")
            {
                Session["BLFact"] = GdPagadas.SelectedRow.Cells[2].Text; 
                OpenNewWindow("Factura.aspx");
                

            }
            else
            {
                OpenNewWindow("FacturaV.aspx");
            }
            
        }

        protected void DdMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Refresca las preliquidaciones mostradas en pantalla
            TxReferencia.Text = "";
            LbCostoPre.Text = "";
            GridAlma.DataSource = "";
            GridAlma.DataBind();
            GridVeri.DataSource = "";
            GridVeri.DataBind();
            CkLIncluir.DataSource = "";
            CkLIncluir.DataBind();
            
            
            //Rev:06-01 b.Si hay preliquidaciones se deben recalcular

            // verifica se ha realizado alguna preliquidacion
            if (Session["oDsCarrito"] != null)
            { 

                 //Baja de memoria el carrito para realizar las conversiones
                DsPreliquidaciones oDsCarrito = (DsPreliquidaciones)Session["oDsCarrito"];


                //verifica si la moneda seleccionada es USD
                if (DdMoneda.SelectedValue == "2")
                {
                    //conversión de RD$ a USD

                    //Determina si hay almacenaje y realiza la conversión
                    if (oDsCarrito.Det_fact_alm.Rows.Count > 0)
                    {
                        foreach (DsPreliquidaciones.Det_fact_almRow r in oDsCarrito.Det_fact_alm.Rows)
                        {
                                                     
                
                            
                            r.TARIFA /= Convert.ToDecimal(r.TASA);
                            r.MONTO /= Convert.ToDecimal(r.TASA);

                        }
                    }

                    //Determina si hay verificación y realiza la conversión
                    if (oDsCarrito.Det_fact_VERI.Rows.Count > 0)
                    {
                        foreach (DsPreliquidaciones.Det_fact_VERIRow r in oDsCarrito.Det_fact_VERI.Rows)
                        {
                            r.SUBTOTAL /= Convert.ToDecimal(r.TASA);
                            r.ITBIS /= Convert.ToDecimal(r.TASA) ;
                            r.TOTAL /= Convert.ToDecimal(r.TASA);
                        }

                    }

                   

                }
                else 
                { 
                    //conversión de USD a RD$

                    //Determina si hay almacenaje y realiza la conversión
                    if (oDsCarrito.Det_fact_alm.Rows.Count > 0)
                    {
                        foreach (DsPreliquidaciones.Det_fact_almRow r in oDsCarrito.Det_fact_alm.Rows)
                        {

                            r.TARIFA *= Convert.ToDecimal(r.TASA);
                            r.MONTO *= Convert.ToDecimal(r.TASA);


                        }
                    }

                    //Determina si hay verificación y realiza la conversión
                    if (oDsCarrito.Det_fact_VERI.Rows.Count > 0)
                    {
                        foreach (DsPreliquidaciones.Det_fact_VERIRow r in oDsCarrito.Det_fact_VERI.Rows)
                        {
                            r.SUBTOTAL *= Convert.ToDecimal(r.TASA);
                            r.ITBIS *= Convert.ToDecimal(r.TASA);
                            r.TOTAL *= Convert.ToDecimal(r.TASA);
                        }

                    }

                    
                }

                //Determina si hay registros en preliquidaciones y realiza la conversión

                if (oDsCarrito.Preliquidaciones.Rows.Count > 0)
                {
                    foreach (DsPreliquidaciones.PreliquidacionesRow r in oDsCarrito.Preliquidaciones.Rows)
                    {


                        //identifica el tipo de preliquidacion
                        if (r.PRELIQUIDACION == "Almacenaje")
                        {
                            DsPreliquidaciones.Det_fact_almRow[] filas = (DsPreliquidaciones.Det_fact_almRow[])oDsCarrito.Det_fact_alm.Select("BL = '" + r.BL + "'");
                            r.TOTAL = filas.Sum(w => w.MONTO);
                        }
                        else if (r.PRELIQUIDACION == "Verificación")
                        {
                            DsPreliquidaciones.Det_fact_VERIRow[] filas = (DsPreliquidaciones.Det_fact_VERIRow[])oDsCarrito.Det_fact_VERI.Select("BL = '" + r.BL + "'");
                            r.TOTAL = filas.Sum(w => w.TOTAL);

                        }

                    }

                    //Rev:8 6.Formatear la moneda RD$ U$ segun Ddlmoneda
                    LbTotal.Text = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", oDsCarrito.Preliquidaciones.Sum(w => w.TOTAL)) : string.Format("U{0:C}", oDsCarrito.Preliquidaciones.Sum(w => w.TOTAL));

                    //Rev:8 6.Formatear la moneda de las preliquidaciones agregadas cuando se enlaza el DataList
                    var qpre = from pre in oDsCarrito.Preliquidaciones.AsEnumerable()
                               select new
                               {
                                   PRELIQUIDACION = pre.PRELIQUIDACION,
                                   BL = pre.BL,
                                   TOTAL = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", pre.TOTAL) : string.Format("U{0:C}", pre.TOTAL)
                               };

                    DataList1.DataSource = qpre;
                    DataList1.DataBind();

                }


                //Determina si hay preliquidaciones totales y realiza la conversión
                if (oDsCarrito.PreliquidacionTotales.Rows.Count > 0)
                {
                    foreach (DsPreliquidaciones.PreliquidacionTotalesRow r in oDsCarrito.PreliquidacionTotales.Rows)
                    {
                        r.Moneda = DdMoneda.SelectedItem.Text;
                        r.Total = oDsCarrito.Preliquidaciones.Sum(w => w.TOTAL);
                        r.ITBIS = oDsCarrito.Det_fact_VERI.Sum(w => w.ITBIS);
                        r.SubTotal = r.Total - r.SubTotal;

                    }

                }
            
            
            }

        }
        
        //Rev:8 1.Eliminar elemento DataList
        protected void DataList1_DeleteCommand(object source, DataListCommandEventArgs e)
      {
            DsPreliquidaciones oDsCarrito = null;
            string BlBorrar;
            string TipoPreliquidacion;
          
            //Verifica si hay una instancia del carrito
          if (Session["oDsCarrito"] != null)
          {
              oDsCarrito = (DsPreliquidaciones)Session["oDsCarrito"];
            
              //Identifica el BLBorrar y el Tipo de Preliquidacion
              BlBorrar=oDsCarrito.Preliquidaciones.Rows[e.Item.ItemIndex]["BL"].ToString();
              


              //Procede a Borrar detalle y preliquidacion segun el tipo de preliquidacion

              if (oDsCarrito.Preliquidaciones.Rows[e.Item.ItemIndex]["TIPO PRELIQUIDACION"].ToString()=="1")
              {//Preliquidacion es de Almacenaje

                  //Borra del carrito el detalle

                  foreach (DsPreliquidaciones.Det_fact_almRow  Fila in oDsCarrito.Det_fact_alm.Select("BL = '" + BlBorrar + "'"))
                  {
                      oDsCarrito.Det_fact_alm.RemoveDet_fact_almRow(Fila); 
                  }


              }
              else
              {//Preliquidacion es de Verificacion 
                  //Borra del carrito el detalle

                  foreach (DsPreliquidaciones.Det_fact_VERIRow Fila in oDsCarrito.Det_fact_VERI.Select("BL = '" + BlBorrar + "'"))
                  {
                      oDsCarrito.Det_fact_VERI.RemoveDet_fact_VERIRow(Fila);
                  }

              }

              //Borra del carrito el Resumen
              foreach (DsPreliquidaciones.PreliquidacionesRow Fila in oDsCarrito.Preliquidaciones.Select("BL = '" + BlBorrar + "'"))
              {
                  oDsCarrito.Preliquidaciones.RemovePreliquidacionesRow(Fila);
              }


              LbTotal.Text = oDsCarrito.Preliquidaciones.Sum(w => w.TOTAL).ToString("C");

              //Almacena el carrito en sesion
              Session["oDsCarrito"] = oDsCarrito;

              DataList1.DataSource = oDsCarrito.Preliquidaciones;
              DataList1.DataBind();

          }
          else
          {
              return; 
          }
          

      }


        //Rev:9 3.Detallar los contenedores seleccionados
        protected void CkLIncluir_SelectedIndexChanged(object sender, EventArgs e)
        {
           System.Text.StringBuilder  seleccion = new System.Text.StringBuilder();
            
            //Obtiene todos los datos del BL consultado
            oDsConsulta = (DsPreliquidaciones)Session["oDsConsulta"];
            DsPreliquidaciones oDsVeri = new DsPreliquidaciones();

            //Recorre los elementos seleccionados
            //Por cada elemento seleccionado
            //Contruye una cadena con que filtrar los datos de los BL seleccionados

            foreach (ListItem item in CkLIncluir.Items)
            {
                if (item.Selected)
                {
                    foreach (DsPreliquidaciones.Det_fact_VERIRow r in oDsConsulta.Det_fact_VERI.Select("EQUIPAMENTO='" + item.Value + "'"))
                    {
                        oDsVeri.Det_fact_VERI.LoadDataRow(r.ItemArray,true);
                    }
                }
            }


            //Enlazar Grid de busqueda
            var qr = from filav in oDsVeri.Det_fact_VERI.AsEnumerable()
                     select new
                     {

                         BL = filav.BL,
                         EQUIPAMENTO = filav.EQUIPAMENTO,
                         CANTIDAD = filav.CANTIDAD,
                         EVENTO = filav.EVENTO,
                         GL = filav.GL,
                         TASA = filav.TASA,
                         PRECIO_US = string.Format("{0:C}", filav.PRECIO_US),
                         SUBTOTAL = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", filav.SUBTOTAL) : string.Format("U{0:C}", filav.SUBTOTAL),
                         ITBIS = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", filav.ITBIS) : string.Format("U{0:C}", filav.ITBIS),
                         TOTAL = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", filav.TOTAL) : string.Format("U{0:C}", filav.TOTAL)


                     };

            GridVeri.DataSource = qr.ToList();
            GridVeri.DataBind();

            //Calcula el total de lo seleccionado
            LbCostoPre.Text = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", oDsVeri.Det_fact_VERI.Sum(w => w.TOTAL)) : string.Format("U{0:C}", oDsVeri.Det_fact_VERI.Sum(w => w.TOTAL));
            Session.Add("oDsVeri", oDsVeri);

            
            ////foreach(ListItem item in CkLIncluir.Items)
            ////{
            ////    if (item.Selected)
            ////    {
            ////        seleccion.Append (item.Value + ",");                
            ////    }
            ////}


            ////if (seleccion.Length == 0)
            ////{
            ////    GridVeri.DataSource = "";
            ////    GridVeri.DataBind();
            ////    LbCostoPre.Text="";
            ////}
            ////else
            ////{
            ////StringArrayConverter cs = new StringArrayConverter();
            //// string[] filtro = (string[])cs.ConvertFromString( seleccion.ToString(0, seleccion.Length - 1));



            //////Incluye los detalles de los BL Seleccionados

             
            //////Enlazar Grid de busqueda
            ////var qr = from filav in oDsConsulta.Det_fact_VERI.AsEnumerable()
            ////         where filtro.Contains(filav.EQUIPAMENTO) 
            ////         select new
            ////         {

            ////             BL = filav.BL,
            ////             EQUIPAMENTO = filav.EQUIPAMENTO,
            ////             CANTIDAD = filav.CANTIDAD,
            ////             EVENTO = filav.EVENTO,
            ////             GL = filav.GL,
            ////             TASA = filav.TASA,
            ////             PRECIO_US = string.Format("{0:C}", filav.PRECIO_US),
            ////             //SUBTOTAL = string.Format("{0:C}", filav.SUBTOTAL),
            ////             //ITBIS = string.Format("{0:C}", filav.ITBIS),
            ////             //TOTAL = string.Format("{0:C}", filav.TOTAL)
            ////             SUBTOTAL = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", filav.SUBTOTAL) : string.Format("U{0:C}", filav.SUBTOTAL),
            ////             ITBIS = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", filav.ITBIS) : string.Format("U{0:C}", filav.ITBIS),
            ////             TOTAL = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", filav.TOTAL) : string.Format("U{0:C}", filav.TOTAL)


            ////         };

            ////GridVeri.DataSource = qr.ToList();
            ////GridVeri.DataBind();


            //////Calcula el total de lo seleccionado
            //////Formatear la moneda RD$ U$ segun Ddlmoneda
            ////decimal dTotal = oDsConsulta.Det_fact_VERI.Where(x => filtro.Contains(x.EQUIPAMENTO)).AsEnumerable().Sum(xy => xy.TOTAL);
            ////LbCostoPre.Text = DdMoneda.SelectedValue == "1" ? string.Format("RD{0:C}", dTotal ): string.Format("U{0:C}", dTotal);

            ////DsPreliquidaciones oDsConsultaVeri = new DsPreliquidaciones();
                

            ////}
        }

        //Rev:10 1.Identificar si el BL se puede preliquidar
        bool ChkHaveNull(ref DsPreliquidaciones oDs, DataTable Tb)
        {

            foreach (DataRow r in oDs.Tables[Tb.TableName].Rows)
            {
                foreach (DataColumn c in Tb.Columns)
                {
                    if ( DBNull.Value.Equals(r[c.ColumnName]))
                    {
                        if (c.ColumnName == "ITBIS")
                        {
                            r[c.ColumnName] = 0;
                            continue;

                        }

                        if (c.ColumnName == "BL PADRE")
                        {
                            r[c.ColumnName] = "";
                            continue;
                        }
                        
                        if (c.ColumnName == "CONSOLIDADO")
                        {
                            r[c.ColumnName] = "";
                            continue;

                        }

                        if (c.ColumnName == "NUMERO CRE")
                        {
                            r[c.ColumnName] = 0;
                            continue;

                        }

                            
                        return true;
                    }
                
                }
            
            }
            return false;
        }

        //Rev:10 6.Seleccionar y deseleccionar todos
        protected void CkTodos_CheckedChanged(object sender, EventArgs e)
        {
            
            foreach (ListItem Li in CkLIncluir.Items)
            {
                Li.Selected = CkTodos.Checked;
               
                
            }
            CkLIncluir_SelectedIndexChanged(CkLIncluir, new EventArgs());

        }
           
    }
}