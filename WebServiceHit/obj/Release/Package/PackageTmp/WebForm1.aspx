<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace = "System.Data" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title> DataList Select Ejemplo </title>
<Script runat = "server">

    ICollection CreateDataSource()
    {

        // Create sample data for the DataList control.
        DataTable dt = new DataTable();
        DataRow dr;

        // Define the columns of the table.
        dt.Columns.Add(new DataColumn("Item", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Qty", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Price", typeof(double)));

        // Populate the table with sample values.
        for (int i = 0; i < 9; i++)
        {
            dr = dt.NewRow();

            dr[0] = i;
            dr[1] = i * 2;
            dr[2] = 1.23 * (i + 1);

            dt.Rows.Add(dr);
        }

        DataView dv = new DataView(dt);
        return dv;

    }

      void Page_Load(object sender, EventArgs e)
      {

         // Cargar datos de ejemplo sólo una vez, cuando la página se carga por primera vez. 
         if (!IsPostBack)
         {
            ItemsList.DataSource = CreateDataSource();
            ItemsList.DataBind();
         }

      }

      void Item_Command(Object sender, DataListCommandEventArgs e)
      {

         // Establecer la propiedad SelectedIndex para seleccionar un elemento de la DataList.
         //ItemsList.SelectedIndex = e.Item.ItemIndex;
         // ItemsList.SelectedItem.
          string qs = ((Label)e.Item.FindControl("QtyLabel")).Text;
          Response.Redirect("Principal.aspx?qr="+qs); 

         // Vuelva a enlazar el origen de datos del control DataList para actualizar el control.
         //ItemsList.DataSource = CreateDataSource();
         //ItemsList.DataBind();

      }

   </Script>

</head>

<body>

   <form id="form1" runat="server">

      <h3>DataList Select Example</h3>

      Click <b>Select</b> to select an item.

      <br /><br />

      <asp:DataList id="ItemsList"
           GridLines="Both"
           CellPadding="3"
           CellSpacing="0"           
           OnItemCommand="Item_Command"
           runat="server">

         <HeaderStyle BackColor="#aaaadd">
         </HeaderStyle>

         <AlternatingItemStyle BackColor="Gainsboro">
         </AlternatingItemStyle>

         <SelectedItemStyle BackColor="Yellow">
         </SelectedItemStyle>

         <HeaderTemplate>

            Items

         </HeaderTemplate>

         <ItemTemplate>

            <%--<asp:LinkButton id="SelectButton" 
                 Text="Select" 
                 CommandName="Select"
                 runat="server"/>--%>
             <asp:ImageButton id="SelectButton" 
                 Text="Select" 
                 CommandName="Select"
                 runat="server"/>
             Item:
            <asp:Label id="ItemLabel" 
                 Text='<%# DataBinder.Eval(Container.DataItem, "Item") %>' 
                 runat="server"/>

            <br />

            Quantity:
            <asp:Label id="QtyLabel" 
                 Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' 
                 runat="server"/>

            <br />

            Price:
            <asp:Label id="PriceLabel" 
                 Text='<%# DataBinder.Eval(Container.DataItem, "Price", "{0:c}") %>' 
                 runat="server"/>

            Item <%# DataBinder.Eval(Container.DataItem, "Item") %>

         </ItemTemplate>

         <SelectedItemTemplate>

            

         </SelectedItemTemplate>

      </asp:DataList>

   </form>

</body>
</html>

