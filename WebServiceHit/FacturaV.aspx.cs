using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceHit
{
    public partial class FacturaV : System.Web.UI.Page
    {
        //Rev6:01 Se define como unico objeto para la factura un oDsPreliquidaciones
        DsPreliquidaciones oDsPreliquidaciones;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                oDsPreliquidaciones = (DsPreliquidaciones)Session["oDsCarritoFact"];

                //Rev6:02 Cargar y Renderizar la factura C&R

                //Rev6:02a C&R Header
                string BL = oDsPreliquidaciones.PreliquidacionNCF.Rows[0][1].ToString();

                //Rev:8 10.Determina la moneda en la que se preliquido la verificacion
                string moneda = oDsPreliquidaciones.Preliquidaciones.Rows[0][4].ToString();

                var qh =
                    from cli in oDsPreliquidaciones.PreliquidacionNCF.AsEnumerable()
                    select new
                    {
                        
                        Cliente = oDsPreliquidaciones.Cliente.Rows[0][2].ToString(),
                        Rnc = oDsPreliquidaciones.Cliente.Rows[0][0].ToString(),
                        Direccion = "",
                        Telefono = oDsPreliquidaciones.Cliente.Rows[0][6].ToString(),
                        BL = BL,
                        Fecha = DateTime.Now.ToShortDateString(),
                        FacturaNo = 1000 + DateTime.Now.Millisecond,
                        FechaVerificacion = "",
                        HoraFacturacion = "",
                        NCF = oDsPreliquidaciones.PreliquidacionNCF.Rows[0]["NCF"].ToString()

                    };

                DlHeader.DataSource = qh.ToList();
                DlHeader.DataBind();

                //Rev6:02b C&R Grid
                var qt1 =
                    from veri in oDsPreliquidaciones.Det_fact_VERI.AsEnumerable()
                    where veri.BL == BL
                    select new
                    {
                        Nombre_Buque = veri.IsBARCONull() ? "NO ESPECIFICADO" : veri.BARCO  ,
                        BL = veri.BL,
                        Fecha_Llegada = "",
                        Viaje = "",
                        Linea = ""

                    };
                Gd1.AutoGenerateColumns = true;
                Gd1.DataSource = qt1.Distinct().ToList();
               
                Gd1.DataBind();

                foreach (TableCell col in Gd1.HeaderRow.Cells)
                {
                    col.Text = col.Text.Replace("_", " ");
                }

                //Rev6:02c C&R Totales
                DsPreliquidaciones.Det_fact_VERIRow[] veris = (DsPreliquidaciones.Det_fact_VERIRow[]) oDsPreliquidaciones.Det_fact_VERI.Select("BL = '" + BL + "'");

                //Rev:8 10.1 Formatear los valores segun la moneda
                LbSubtotal.Text = moneda == "RD$" ? string.Format("RD{0:C}", veris.Sum(w => w.SUBTOTAL)) : string.Format("U{0:C}", veris.Sum(w => w.SUBTOTAL));
                LbItbis.Text = moneda == "RD$" ? string.Format("RD{0:C}", veris.Sum(w => w.ITBIS)) : string.Format("U{0:C}", veris.Sum(w => w.ITBIS));
                LbTotal.Text = moneda == "RD$" ? string.Format("RD{0:C}", veris.Sum(w => w.TOTAL)) : string.Format("U{0:C}", veris.Sum(w => w.TOTAL));

             
            }

        }
    }
}