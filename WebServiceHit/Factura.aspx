<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Factura.aspx.cs" Inherits="WebServiceHit.Factura" %>

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
<div class="factura">Factura</div>

    <asp:DataList ID="DlHeader" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both"   >
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <ItemStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
    <ItemTemplate>
       
         <div class="cuadro1">
          <div class="empresa">Facturado a :<b><asp:Label ID="LbCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Cliente") %></asp:Label></b></div>
          <div class="rnc">RNC:<b><asp:Label ID="LbRncCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Rnc") %></asp:Label></b></div>
        </div>
        <div class="cuadro2">
        <div class="empresa">Fecha:<b><asp:Label ID="LbFecha" runat="server" ><%#DataBinder.Eval(Container.DataItem,"Fecha") %></asp:Label></b></div>
          <div class="rnc">Factura No. <b><asp:Label ID="LbFacturaNo" runat="server" ><%#DataBinder.Eval(Container.DataItem,"FacturaNo") %></asp:Label></b></div>
          <div class="rnc">NCF. <b><asp:Label ID="LbNCF" runat="server"><%#DataBinder.Eval(Container.DataItem,"NCF") %></asp:Label></b></div>
        </div>
  
     </ItemTemplate>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
    </asp:DataList>



<div class="tabla_1">
  
    <asp:GridView ID="Gd1" runat="server">
    </asp:GridView>
 
</div>
<div class="tabla_1">

      <asp:GridView ID="Gd2" runat="server">
    </asp:GridView>

</div>
<div class="tabla_1">

        <asp:GridView ID="Gd3" runat="server">
    </asp:GridView>
</div>
</div>

<div class="observaciones">
<div class="titulo5">Observaciones:</div>
 <div class="empresa">Autorización</div>
 <div class="montototal2">

<div class="titulos2">SubTotal</div>
<div class="total">
    <asp:Label ID="LbSubtotal" runat="server" Text="Label"></asp:Label></div>
<div class="titulos">ITBS</div>
<div class="total">
    <asp:Label ID="LbItbis" runat="server" Text="Label"></asp:Label></div>
<div class="titulos">Total</div>
<div class="total">
    <asp:Label ID="LbTotal" runat="server" Text="Label"></asp:Label></div>
</div>

  <div class="unidades">
   <div class="titulo5">Unidades</div>
   <div class="empresa">
       <asp:Label ID="LbUnidades" runat="server" Text="Label"></asp:Label></div>
  </div>
  <div class="firmas">
  <div class="lineas">
  <b>  Autorizado por</b>  </div>
   <div class="lineas">
  <b>  Revisado por</b>  </div>
     <!-- <a href="WfTest.aspx">WfTest.aspx</a> -->
   <div class="lineas">
  <b>  Recibido por</b>
  <div class="datos_tex">KM. 13 Carretera Sánchez, Edf. Naviero, Puerto de Rio Haina, Santo Domingo, Rep. Dom.
Tel.: 809-740-1025</div>
  </div>
  </div>
  </div>
  
   
</div>

<asp:label ID="LbAnexo" runat="server" text=""></asp:label>

 <asp:Panel class="cuerpo" runat="server" ID="PnCuerpoAnexo" Visible="false" >

<div class="contenido">
<div class="logofactura"><img src="css/images/baucher.jpg" /></div>
<div class="tx_head">Registro Mercantil No. SC-30<br />
RNC: 1-24-00836-2
</div>
<div class="factura">Anexo Factura</div>

    <asp:DataList ID="DlHeaderAnexo" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both"   >
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <ItemStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
    <ItemTemplate>
       
         <div class="cuadro1">
          <div class="empresa">Facturado a :<b><asp:Label ID="LbCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Cliente") %></asp:Label></b></div>
          <div class="rnc">RNC:<b><asp:Label ID="LbRncCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Rnc") %></asp:Label></b></div>
        </div>
        <div class="cuadro2">
        <div class="empresa">Fecha:<b><asp:Label ID="LbFecha" runat="server" ><%#DataBinder.Eval(Container.DataItem,"Fecha") %></asp:Label></b></div>
          <div class="rnc">Factura No. <b><asp:Label ID="LbFacturaNo" runat="server" ><%#DataBinder.Eval(Container.DataItem,"FacturaNo") %></asp:Label></b></div>
          <div class="rnc">NCF. <b><asp:Label ID="LbNCF" runat="server"><%#DataBinder.Eval(Container.DataItem,"NCF") %></asp:Label></b></div>
        </div>
  
     </ItemTemplate>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
    </asp:DataList>

</div>

<div class="observaciones"> 
<!--<div class="titulo5">Observaciones:</div>
 <div class="empresa">Autorización</div> -->

  <div class="unidades">
   <div class="titulo5">Unidades</div>
   <div class="empresa">
       <asp:Label ID="LbUnidadesAnexo" runat="server" Text="Label"></asp:Label></div>
  </div>
  </div>
  
   
</asp:Panel>

    </form>
</body>
</html>

