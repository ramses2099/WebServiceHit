<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WfTest.aspx.cs" Inherits="WebServiceHit.WfTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link href="ItinerarioBuques/css/tablaitinerarios.css" rel="stylesheet" />
    <script src="ItinerarioBuques/js/tablaitinerarios.js"></script>
    <link href="ItinerarioBuques/css/cont.css" rel="stylesheet" />--%>
</head>
<body>
    <form id="form1" runat="server">
   <div style="">
       Historicos
       <table>
           <tr>
               <td>
                <div>
                    Preliquidaciones Pendientes
                    <br />
                     <asp:GridView ID="GdPendientes" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GdPendientes_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="GdPendientes_PageIndexChanging" PageSize="5"  >
                      
                          <Columns>
                            <asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                            <%--<asp:ButtonField ButtonType="Image" Text="Button" />--%>
                            <asp:CommandField ButtonType="Image" ShowSelectButton="True" />
                        </Columns>


                    </asp:GridView>
                 
                </div>

               </td>
               <td>
                     <div>
                         Preliquidaciones Pagadas
                         <br />

                          <asp:GridView ID="GdPagadas" runat="server" AutoGenerateColumns="False" AllowPaging="True"  PageSize="5" OnPageIndexChanging="GdPagadas_PageIndexChanging" OnSelectedIndexChanged="GdPagadas_SelectedIndexChanged"  >
                      
                          <Columns>
                            <asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
                             <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                            <asp:BoundField DataField="BL" HeaderText="BL" SortExpression="BL" />
                            <asp:BoundField DataField="NCF" HeaderText="NCF" SortExpression="NCF" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                            <%--<asp:ButtonField ButtonType="Image" Text="Button" />--%>
                            <asp:CommandField ButtonType="Image" ShowSelectButton="True"  />
                        </Columns>


                    </asp:GridView>
                </div>

               </td>

           </tr>

       </table>

   </div>
    <div>
        <br />
        <asp:RadioButton ID="RbAlma" runat="server" Text="Preliquidar Almacenaje" Checked="True" AutoPostBack="True" GroupName="G1" OnCheckedChanged="RadioButton1_CheckedChanged" />
&nbsp;<asp:RadioButton ID="RbVeri" runat="server" Text="Preliquidar Verificacion" AutoPostBack="True" GroupName="G1" OnCheckedChanged="RadioButton2_CheckedChanged" />
       
         <%--<asp:ButtonField ButtonType="Image" Text="Button" />--%>  
        <br />
                    
        Moneda: <asp:DropDownList ID="DdMoneda" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdMoneda_SelectedIndexChanged">
            <asp:ListItem Value="1">RD$</asp:ListItem>
            <asp:ListItem Value="2">US</asp:ListItem>
        </asp:DropDownList>

        <br />
        <asp:TextBox ID="TxReferencia" runat="server"></asp:TextBox>
&nbsp;&nbsp;
        <asp:DropDownList ID="DdTipoReferencia" runat="server" AutoPostBack="True">
            <asp:ListItem Value="1">BL</asp:ListItem>
        </asp:DropDownList>
&nbsp;
        &nbsp;
        <asp:Button ID="Button1" runat="server" Text="Buscar" OnClick="Button1_Click" />
&nbsp;<asp:Panel ID="PnAlma" runat="server">
            &nbsp;Almacenaje
            <br />

      <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            <asp:GridView ID="GridAlma" runat="server" >
            </asp:GridView>
            <br />
            
        </asp:Panel>
        <asp:Panel ID="PnVeri" runat="server" Visible="False">
            Verificacion
            <asp:Label ID="lblError1" runat="server" Text=""></asp:Label>
            <br />
            <asp:CheckBox ID="CkTodos" runat="server" AutoPostBack="True" OnCheckedChanged="CkTodos_CheckedChanged" Text="Todos" />
            <br />
            <!--Rev:9 1.Lista para seleccion de verificaciones a incluir-->
<%--            <asp:DataList ID="DLIncluir" runat="server">
                <ItemTemplate>
                <asp:CheckBox runat="server" ID="CkIncluirveri" Checked='<%# DataBinder.Eval(Container.DataItem, "INCLUIR") %>' OnCheckedChanged="CkIncluirveri_CheckedChanged" AutoPostBack="True"/>
              &nbsp;BL
                <asp:Label ID="LbIncluirReferencia" runat="server"><%# DataBinder.Eval(Container.DataItem,"BL") %></asp:Label> 
             &nbsp;EQUIPAMENTO
                <asp:Label ID="LbIncluirEquipamento" runat="server"><%# DataBinder.Eval(Container.DataItem,"EQUIPAMENTO") %></asp:Label> 
                &nbsp;<asp:Label ID="LbIncluirMonto" runat="server" Font-Bold="True"><%# DataBinder.Eval(Container.DataItem,"TOTAL") %></asp:Label>
                <%--<imagebutton  ID="imgBtnDelete" runat="server" CommandName=""></imagebutton>
                &nbsp;--%>
                
            <%--</ItemTemplate>
            </asp:DataList>--%>
            <asp:CheckBoxList ID="CkLIncluir" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CkLIncluir_SelectedIndexChanged">
            </asp:CheckBoxList>
            <br />
            <asp:GridView ID="GridVeri" runat="server">
            </asp:GridView><%-- OnRowEditing="GridVeri_RowEditing" OnSelectedIndexChanged="GridVeri_SelectedIndexChanged"--%>
        </asp:Panel>
    </div>
        Costo de&nbsp; preliquidacion:
        <asp:Label ID="LbCostoPre" runat="server" Text="0.00"></asp:Label>
&nbsp;<asp:Button ID="Button2" runat="server" Text="Agregar preliquidacion" OnClick="Button2_Click"  EnableViewState="true"/>
        <br />
        <br />
        <asp:DataList ID="DataList1" runat="server" OnDeleteCommand="DataList1_DeleteCommand" >
            <ItemTemplate>
                
                <asp:Label ID="LbPreliquidacion" runat="server"><%# DataBinder.Eval(Container.DataItem,  "PRELIQUIDACION") %> </asp:Label>
               
                &nbsp;BL
                <asp:Label ID="LbReferencia" runat="server"><%# DataBinder.Eval(Container.DataItem,"BL") %></asp:Label> 
                &nbsp;<asp:Label ID="LbMonto" runat="server" Font-Bold="True"><%# DataBinder.Eval(Container.DataItem,"TOTAL") %></asp:Label>
                <%--<imagebutton  ID="imgBtnDelete" runat="server" CommandName=""></imagebutton>--%>
                <asp:ImageButton ID="Delete" Runat="server"  Text="Delete" CommandName="delete" />

                &nbsp;
                
            </ItemTemplate>
        </asp:DataList>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Total:"></asp:Label>
&nbsp;<asp:Label ID="LbTotal" runat="server" Font-Bold="True"></asp:Label>
&nbsp;<asp:Button ID="Button3" runat="server" Text="Preliquidar" OnClick="Button3_Click" />
        <br />

        <br />
        <br />

    </form>
</body>
</html>
