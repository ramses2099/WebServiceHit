using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceHit
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //WsExtraHit ws = new WsExtraHit();
            //DsPreliquidaciones oDs = new DsPreliquidaciones();

            ////Recibe Almacenaje
            //oDs.Det_fact_alm.Load(ws.GetAlmacenaje("WS1501", 1, "123").CreateDataReader());

            ////Recibe Verificacion
            //oDs.Det_fact_VERI.Load(ws.GetVerificacion("SMLU3414851A", 1, "123").CreateDataReader());

            ////Agrega Preliquidaciones
            ////oDs.Preliquidaciones.AddPreliquidacionesRow(oDs.Det_fact_alm.Rows[0]["BL"].ToString(),1,"Almacenaje", 0,);
            ////oDs.Preliquidaciones.AddPreliquidacionesRow(oDs.Det_fact_VERI.Rows[0]["BL"].ToString(),2,"Verificación", 0);
            
            ////Calcula Totales
            //DsPreliquidaciones.PreliquidacionesRow rt;
            //rt = (DsPreliquidaciones.PreliquidacionesRow) oDs.Preliquidaciones.Rows[0];

            //foreach (DsPreliquidaciones.Det_fact_almRow  row in oDs.Det_fact_alm.Rows)
            //{
            //    rt.TOTAL += row.MONTO ;
            //}

            //rt = (DsPreliquidaciones.PreliquidacionesRow)oDs.Preliquidaciones.Rows[1];

            //foreach (DsPreliquidaciones.Det_fact_VERIRow row in oDs.Det_fact_VERI.Rows)
            //{
            //    rt.TOTAL += row.TOTAL;
            //}

            ////Retorna Dataset
            ////Response.Write(oDs.GetXml());

            //Label1.Text = oDs.GetXml();
        }
    }
}