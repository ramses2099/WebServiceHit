using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceHit
{
    public partial class Voucher : System.Web.UI.Page
    {
        //Rev4:07 Se define como unico objeto para el voucher un oDsPreliquidaciones
           DsPreliquidaciones oDsPreliquidaciones;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Rev2:10 Cargar y Renderizar Voucher
               
                //Rev4:06 Renderizar Almacenaje usando Session["oDsCarritoImp"];
                oDsPreliquidaciones = (DsPreliquidaciones)Session["oDsCarritoImp"];

                //Rev:8 7.Determina la moneda en la que se preliquido
                string moneda = oDsPreliquidaciones.Preliquidaciones.Rows[0][4].ToString();

                //Rev:8 8.1 Formatear los valores del voucher segun la moneda
                var qa =
                    from alm in oDsPreliquidaciones.Det_fact_alm.AsEnumerable()
                    select new
                    {
                        BL = alm.BL,
                        Descripcion = alm.DESCRIPCION,
                        Fecha_Llegada = alm.FECHA_LLEGADA, 
                        Cantidad = alm.CANTIDAD, 
                        Valido_Hasta = alm.VALIDO_HASTA, 
                        Semanas = alm.SEMANAS, 
                        Tarifa= moneda=="RD$" ? string.Format("RD{0:C}", alm.TARIFA) : string.Format("U{0:C}", alm.TARIFA) ,
                        Monto = moneda == "RD$" ? string.Format("RD{0:C}", alm.MONTO) : string.Format("U{0:C}", alm.MONTO),
                         
                    };
                GdPreliquidaciones.DataSource = qa.ToList();
                GdPreliquidaciones.DataBind();

                //Rev2:10 Identificar la refencia de la preliquidacion
                string Ref = oDsPreliquidaciones.Preliquidaciones_Response.Rows[0]["Referencia"].ToString();

                //Rev2:10 Renderiza el total por almacenaje
                //Rev:8 8.2 Formatear los valores del voucher segun la moneda
                LbTotalAlm.Text = moneda=="RD$" ? string.Format("RD{0:C}", oDsPreliquidaciones.Det_fact_alm.Sum(w => w.MONTO)) : string.Format("U{0:C}", oDsPreliquidaciones.Det_fact_alm.Sum(w => w.MONTO));
              

                //Rev4:08 Renderizar Cliente desde oDsPreliquidaciones
                var qc =
                    from cli in oDsPreliquidaciones.Cliente.AsEnumerable()
                    select new
                    {
                        Fecha = DateTime.Now.ToShortDateString(),
                        Cliente = cli.name,
                        Rnc = cli.tax_id,
                        Referencia = Ref
                    };

                DlHeader.DataSource = qc.ToList();
                DlHeader.DataBind();

                //Rev3:04 Renderizar Verificacion
                
                var qv =
                    from veri in oDsPreliquidaciones.Det_fact_VERI.AsEnumerable()
                    select new
                    {
                        Contenedor = veri.EQUIPAMENTO, 
                        Tipo_Verificación = veri.EVENTO, 
                        //Despacho = veri.GL,
                        //Rev:8 8.3 Formatear los valores del voucher segun la moneda
                        Tarifa= moneda=="RD$" ? string.Format("RD{0:C}", veri.SUBTOTAL) : string.Format("U{0:C}",veri.SUBTOTAL),
                        Tasa = veri.TASA ,
                        ITBS = moneda == "RD$" ? string.Format("RD{0:C}", veri.ITBIS ) : string.Format("U{0:C}", veri.ITBIS),
                        Valor = moneda == "RD$" ? string.Format("RD{0:C}", veri.TOTAL) : string.Format("U{0:C}", veri.TOTAL) 
                    };
                GdPreliquidacionesVeri.DataSource = qv.ToList();
                GdPreliquidacionesVeri.DataBind();

                //Rev3:05 Renderiza el totales por verificacion
                //Rev:8 8.4 Formatear los valores del voucher segun la moneda
                LbSubtotalVeri.Text = moneda == "RD$" ? string.Format("RD{0:C}", oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.SUBTOTAL)) : string.Format("U{0:C}", oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.SUBTOTAL));
                LbItbisVeri.Text = moneda == "RD$" ? string.Format("RD{0:C}", oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.ITBIS)) : string.Format("U{0:C}", oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.ITBIS));
                LbTotalVeri.Text = moneda == "RD$" ? string.Format("RD{0:C}", oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.TOTAL)) : string.Format("U{0:C}", oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.TOTAL));

                decimal SubTotalVeri = oDsPreliquidaciones.Det_fact_VERI.Sum(w => w.SUBTOTAL);
                decimal SubTotalAlm = 0;

                //Rev3:06 Renderiza el Gran Total
                //Rev:8 8.5 Formatear los valores del voucher segun la moneda
                LbGranTotal.Text = moneda == "RD$" ? string.Format("RD{0:C}", oDsPreliquidaciones.Preliquidaciones.Sum(w => w.TOTAL)) : string.Format("U{0:C}", oDsPreliquidaciones.Preliquidaciones.Sum(w => w.TOTAL));
                LbGransubTotal.Text = moneda == "RD$" ? string.Format("RD{0:C}", Convert.ToDecimal(SubTotalAlm) + Convert.ToDecimal(SubTotalVeri)) : string.Format("U{0:C}", Convert.ToDecimal(SubTotalAlm) + Convert.ToDecimal(SubTotalVeri));

                //Rev4:13 Limpia la preliquidacion actual
                Session["oDsCarritoImp"] = null; 

            }

        }
    }
}