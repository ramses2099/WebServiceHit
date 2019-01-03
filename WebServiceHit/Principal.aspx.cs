using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceHit
{
    public partial class Principal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //WsExtraHit srv = new WsExtraHit();
            //DsExtraHit ds = new DsExtraHit();
            //ds = srv.GetItinerarios();
            //GridView1.DataSource = ds.Itinerario_Buque;
            //GridView1.DataBind();
            Tipo_Referencia tr;
            tr = (Tipo_Referencia)int.Parse("1");

            //DsExtraHit ds1 = new DsExtraHit();
            //ds1 = srv.GetCalculoAlmacenaje(tr,1234, 1);
            //ds1.USP_GETCalculoAlm.Columns.Remove ("TOTAL");
            //GridView2.DataSource = ds1.USP_GETCalculoAlm;
            //GridView2.DataBind();


            WsExtraHit srv = new WsExtraHit();
            DsExtraHit ds = new DsExtraHit();
            ds = srv.GetContenedores("MSCUDS766113", tr, "1");


            GridView1.DataSource = ds.Consulta_Contenedores;

            GridView1.DataBind();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
     {

         //string t = "";
         //if (e.Row.RowType == DataControlRowType.DataRow)
         //{
         //    t = DataBinder.Eval(e.Row.DataItem, "IMPEDIMENTOS").ToString();
         //    if (t.Equals("DPH_Impedimento,Pago Verificación,Pago Almacenaje,MSC_Impedimento"))
         //    {
         //        e.Row.Cells[14].ForeColor = System.Drawing.Color.Red;
             
         //    }
         //}


         if (e.Row.RowType == DataControlRowType.DataRow)
         {
            DataBinder.Eval(e.Row.DataItem, "IMPEDIMENTOS").ToString();
            e.Row.Cells[14].ForeColor = System.Drawing.Color.Red;
             
         }
        
        }


    }
}