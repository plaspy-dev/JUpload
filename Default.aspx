<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Ingreso a JUpload</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table>
            <tr>
                <td align="center">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/logo.jpg" /></td>
            </tr>
            <tr>
                <td>
     <table>
        <tr>
            <td align="left">
                Usuario&nbsp;</td>
            <td>
                <asp:TextBox ID="m_EUsuario" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ErrorMessage="Por favor ingrese un nombre de usuario." SetFocusOnError="True" ControlToValidate="m_EUsuario"></asp:RequiredFieldValidator>&nbsp;</td>
        </tr>
        <tr>
            <td align="left">
                Contraseña</td>
            <td style="text-align: right">
                <asp:TextBox ID="m_EContrasena" runat="server" TextMode="Password"></asp:TextBox></td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left">
            </td>
            <td style="text-align: right">
                <asp:Button ID="m_BIngresar" runat="server" OnClick="m_BIngresar_Click" Text="Ingresar" /></td>
            <td>
            </td>
        </tr>
    </table>
                </td>
            </tr>
            <tr>
                <td>
    <asp:Label ID="m_SMensaje" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
