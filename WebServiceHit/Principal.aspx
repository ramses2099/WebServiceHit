<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="WebServiceHit.Principal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="WfTest.aspx">Ir a Preliquidaciones...</a>

        <asp:GridView ID="GridView1" runat="server" OnRowDataBound ="GridView1_RowDataBound"  ></asp:GridView>
        <br />
        <br />
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>
        <br />
        <br />
    </div>


    </form>
</body>
</html>
