using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceHit
{
    public partial class Factura : System.Web.UI.Page
    {
        //Rev5:01 Se define como unico objeto para la factura un oDsPreliquidaciones
        DsPreliquidaciones oDsPreliquidaciones;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                oDsPreliquidaciones = (DsPreliquidaciones)Session["oDsCarritoFact"];
                
                //Rev5:02 Cargar y Renderizar la factura


                //Rev5:03 Renderizar la cabedera de la factura;
                string BL = oDsPreliquidaciones.PreliquidacionNCF.Rows[0][1].ToString();

                //Rev:8 9.Determina la moneda en la que se preliquido el Almacenaje
                string moneda = oDsPreliquidaciones.Preliquidaciones.Rows[0][4].ToString();

                var qc =
                    from cli in oDsPreliquidaciones.PreliquidacionNCF.AsEnumerable()
                    select new
                    {
                        
                        Cliente = oDsPreliquidaciones.Cliente.Rows[0][2].ToString(),
                        Rnc = oDsPreliquidaciones.Cliente.Rows[0][0].ToString(),
                        Fecha = DateTime.Now.ToShortDateString(),
                        FacturaNo = 1000 + DateTime.Now.Millisecond,
                        NCF= oDsPreliquidaciones.PreliquidacionNCF.Rows[0]["NCF"].ToString()
                    };
 
                DlHeader.DataSource = qc.ToList();
                DlHeader.DataBind();


                //Renderiza la primera tabla

                var qt1 =
                    from alm in oDsPreliquidaciones.Det_fact_alm.AsEnumerable()
                    where alm.BL == BL
                    select new
                    {
                        Nombre_Buque = "SEABOR RANGER" ,
                        BL = alm.BL ,
                        Fecha_Llegada = alm.FECHA_LLEGADA, 
                        viaje = "",
                        Linea = ""

                    };
                Gd1.DataSource = qt1.Distinct().ToList();
                foreach (DataControlField  col in Gd1.Columns)
                {
                col.HeaderText = col.HeaderText.Replace("_"," ");
                }
                Gd1.DataBind();

                //Renderiza la segunda tabla

                var qt2 =
                    from alm in oDsPreliquidaciones.Det_fact_alm.AsEnumerable()
                    //Rev:8 2. Filtrar la cabecera de la factura almacenaje
                    where  alm.DESCRIPCION.Contains("Almacenaje") 
                    select new
                    {
                        CRE_N = alm.NUMERO_CRE, 
                        Automatiación_Aduanal = "",
                        Peso_Tonelada = alm.CANTIDAD ,
                        Peso_Facturado = alm.CANTIDAD ,
                        Semana=alm.SEMANAS 

                    };

                Gd2.DataSource = qt2.Distinct().ToList();
                foreach (DataControlField col in Gd2.Columns)
                {
                    col.HeaderText = col.HeaderText.Replace("_", " ");
                }
                Gd2.DataBind();

                
                //Renderiza la tercera tabla
                var qt3 =
                    from alm in oDsPreliquidaciones.Det_fact_alm.AsEnumerable()
                    select new
                    {
                        Código = "",
                        Descripción = alm.DESCRIPCION,
                        Cantidad = alm.CANTIDAD,
                        //Rev:8 9.1 Formatear los valores de ALmacenaje segun la moneda
                        ITBIS = moneda == "RD$" ? string.Format("RD{0:C}",0):string.Format("U{0:C}",0),
                        Tarifa = moneda == "RD$" ? string.Format("RD{0:C}",alm.TARIFA) : string.Format("U{0:C}",alm.TARIFA),
                        Monto = moneda == "RD$" ? string.Format("RD{0:C}", alm.MONTO) : string.Format("U{0:C}", alm.MONTO) 
                    };

                Gd3.DataSource = qt3.ToList();
                foreach (DataControlField col in Gd3.Columns)
                {
                    col.HeaderText = col.HeaderText.Replace("_", " ");
                }
                Gd3.DataBind();


                //Render Subtotal
                System.Data.DataRow[] r = oDsPreliquidaciones.Preliquidaciones.Select("BL = '" + Session["BLFact"].ToString() + "'");

                if(r.Count() >= 1)
                {
                    //Rev:8 9.2 Formatear los valores de ALmacenaje segun la moneda
                    LbSubtotal.Text = moneda == "RD$" ? string.Format("RD{0:C}", r[0][3]) : string.Format("U{0:C}", r[0][3]);
                }   

                //Render Itbis
                //Rev:8 9.3 Formatear los valores de ALmacenaje segun la moneda
                LbItbis.Text = moneda == "RD$" ? string.Format("RD{0:C}", 0) : string.Format("U{0:C}", 0);

                //Render Total
                LbTotal.Text = LbSubtotal.Text;

                
                //Rev:10 9.Render Unidades - Favor pasar todo el if y el else completos
                string sUnidades;
                //Obeteber las unidades desde servicio web
                WsExtraHit ws = new WsExtraHit();
                //oDsPreliquidaciones.Unidades.Load(ws.GetUnidades(BL).CreateDataReader());
                sUnidades=oDsPreliquidaciones.Unidades.Rows[0][0]==null?"":oDsPreliquidaciones.Unidades.Rows[0][0].ToString();

                //Determinar si se requere anexo unidades o se despliegan en la misma hoja de la factura
                if (sUnidades.Length > 500)
                { //Se requiere anexo de unidades
                    LbUnidades.Text = "Ver anexo.";
                    LbAnexo.Visible = true;
                    PnCuerpoAnexo.Visible = true;
                    LbAnexo.Text = "<div style='page-break-before: always;'></div>";
                    DlHeaderAnexo.DataSource = qc;
                    DlHeaderAnexo.DataBind();
                    LbUnidadesAnexo.Text = sUnidades;


                }
                else 
                { //No se requiere anexo de unidades
                    LbUnidades.Text = sUnidades;
                    LbAnexo.Visible = false;
                    PnCuerpoAnexo.Visible = false;
                }
                
                


                
                
                


              

            }

        }
    }
}