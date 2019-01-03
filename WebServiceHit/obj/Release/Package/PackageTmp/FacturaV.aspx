<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacturaV.aspx.cs" Inherits="WebServiceHit.FacturaV" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div class="cuerpo">
<div class="contenido">
<div class="logofactura"><img src="css/images/baucher.jpg" /></div>
<div class="tx_head">Registro Mercantil No. SC-30<br />
RNC: 1-24-00836-2
</div>
<div class="factura">Factura de Verificaci&oacute;n </div>

   <asp:DataList ID="DlHeader" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both"   >
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <ItemStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
    <ItemTemplate>
       
         <div class="cuadro1">
              <div class="empresa">Facturado a :<b><asp:Label ID="LbCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Cliente") %></asp:Label></b></div>
              <div class="rnc">RNC:<b><asp:Label ID="LbRncCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Rnc") %></asp:Label></b></div>
              <div class="rnc">Dir:<b><asp:Label ID="LbDireccion" runat="server"><%#DataBinder.Eval(Container.DataItem,"Direccion") %></asp:Label></b></div>
              <div class="rnc">Tel:<b><asp:Label ID="LbTelefono" runat="server"><%#DataBinder.Eval(Container.DataItem,"Telefono") %> </asp:Label> </b></div>
	          <div class="rnc">BL:<b><asp:Label ID="LbBL" runat="server"><%#DataBinder.Eval(Container.DataItem,"BL") %></asp:Label></b></div>
    
          <div class="rnc">RNC:<b></b></div>
        </div>
        <div class="cuadro2">
            <div class="empresa">Fecha facturación:<b><asp:Label ID="LbFecha" runat="server" ><%#DataBinder.Eval(Container.DataItem,"Fecha") %></asp:Label></b></div>
            <div class="rnc">Factura No. <b><asp:Label ID="LbFacturaNo" runat="server" ><%#DataBinder.Eval(Container.DataItem,"FacturaNo") %></asp:Label></b></div>
            <div class="empresa">Fecha de Verificación<b><asp:Label ID="LbFechaVerificacion" runat="server"><%#DataBinder.Eval(Container.DataItem,"FechaVerificacion") %></asp:Label> </b></div>
            <div class="rnc">Hora de facturación <b><asp:Label ID="LbHoraFacturacion" runat="server"></asp:Label><%#DataBinder.Eval(Container.DataItem,"HoraFacturacion") %></b></div>
            <div class="rnc">NCF <b><asp:Label ID="LbNCF" runat="server"><%#DataBinder.Eval(Container.DataItem,"NCF") %></asp:Label></b></div>

        </div>
  
     </ItemTemplate>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
    </asp:DataList>

    <div class="tabla_1">
  
    <asp:GridView ID="Gd1" runat="server">
    </asp:GridView>
    </div>

    <div class="lasfirmas"> 
  <div class="firmas">
  <div class="lineas2">
  <b>  Autorizado por</b>
  </div>
   <div class="lineas2">
  <b>  Revisado por</b>
  </div>
   
  </div>
  </div>
  <div class="eltotal">

<div class="titulos2">SubTotal</div>
<div class="total"><asp:Label ID="LbSubtotal" runat="server" Text="Label"></asp:Label></div>
<div class="titulos">ITBS</div>
<div class="total"><asp:Label ID="LbItbis" runat="server" Text="Label"></asp:Label></div>
<div class="titulos">Total</div>
<div class="total"><asp:Label ID="LbTotal" runat="server" Text="Label"></asp:Label></div>
</div>

  </div>
 <div class="datos_tex2">KM. 13 Carretera Sánchez, Edf. Naviero, Puerto de Rio Haina, Santo Domingo, Rep. Dom.<br />
Tel.: 809-740-1025
  </div>   

</div>
  
  
<div>
  

</div>
    </form>
</body>
</html>
