<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Voucher.aspx.cs" Inherits="WebServiceHit.Voucher" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
        <!--Rev2:09 Cabecera de Voucher -->
    <div class="cuerpo">
    <div class="contenido">
    <asp:DataList ID="DlHeader" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both"   >
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <ItemStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
    <ItemTemplate>
                
        <div class="logofactura"><img src="css/images/baucher.jpg" /></div>
        <div class="fecha"><b>FECHA:</b> <asp:Label ID="LbFechaVoucher" runat="server" ><%# DataBinder.Eval(Container.DataItem,"Fecha") %></asp:Label></div>
        <div class="tx_head">Registro Mercantil No. SC-30<br />
        RNC: 1-24-00836-2
        </div>
        <!--<div class="secuencia">00001</div> --> <!--Rev2:  -->
        <div style="width:550px; height:1px; background:#000099; margin-left:10px;"></div>
        <div class="cuadro1">
          <div class="empresa">Empresa:<b><asp:Label ID="LbCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Cliente") %></asp:Label></b></div>
          <div class="rnc">RNC:<b><asp:Label ID="LbRncCliente" runat="server"><%#DataBinder.Eval(Container.DataItem,"Rnc") %></asp:Label></b></div>
        </div>
        <div class="cuadro2">
        <div class="empresa"><b>No. Referencia</b></div>
          <div class="referencia">
              <asp:Label ID="LbReferencia" runat="server"><%#DataBinder.Eval(Container.DataItem,"Referencia") %></asp:Label></div>
        </div>
     </ItemTemplate>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
    </asp:DataList>

           <!--Rev2: Fin Cabecera de Voucher -->

        <!--Rev2:09 Preliquidacion Almacenaje -->
<h1>Preliquidación Almacenaje</h1>
<div class="tabla1">
    
            <asp:GridView ID="GdPreliquidaciones" runat="server">
            </asp:GridView>
</div>
<div class="nota">Nota</div>
<div class="montototal">
<div class="titulos3">SubTotal</div>
<div class="total2">
    <asp:Label ID="LbsubTotalAlm" runat="server" Text=""></asp:Label></div>
    <div class="titulos">ITBS</div>
<div class="total"><asp:Label ID="LbItbisAlm" runat="server" ></asp:Label></div>
<div class="titulos">Almacenaje</div>
<div class="total"><asp:Label ID="LbTotalAlm" runat="server" ></asp:Label></div>
</div>
        <!--Rev2: Preliquidacion Almacenaje -->

<div>

<!--Rev3:01 Preliquidacion Verificacion -->
<h1>Preliquidación Verificación</h1>
<div class="tabla1">
 <asp:GridView ID="GdPreliquidacionesVeri" runat="server">

            </asp:GridView>
</div>
<div class="montototal">

 <!--Rev3:02 Preliquidacion Verificacion -->
<div class="titulos2">SubTotal</div>
<div class="total"><asp:Label ID="LbSubtotalVeri" runat="server" Text=""></asp:Label></div>
<div class="titulos">ITBIS</div>
<div class="total"><asp:Label ID="LbItbisVeri" runat="server" ></asp:Label></div>
<div class="titulos">Total</div>
<div class="total"><asp:Label ID="LbTotalVeri" runat="server" ></asp:Label></div>
<div class="nota2">Nota</div>
</div>


</div>
<!--Rev3:03 Preliquidacion Verificacion -->
<div class="titulo4">GRAN SUBTOTAL:</div>
<div class="grantotal"><asp:Label ID="LbGransubTotal" runat="server" ></asp:Label></div>
        <div class="titulo4">ITBIS:</div>
<div class="grantotal"><asp:Label ID="LbGranItbis" runat="server" Text="0.00" ></asp:Label></div>
        <div class="titulo4">GRAN TOTAL:</div>
<div class="grantotal"><asp:Label ID="LbGranTotal" runat="server" ></asp:Label></div>
</div>


</div>


    </form>
</body>
</html>
