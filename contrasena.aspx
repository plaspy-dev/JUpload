<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="contrasena.aspx.cs" Inherits="contrasena" Title="Cambio de Contraseña" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td>
            Actual</td>
        <td>
            <asp:TextBox ID="m_EActual" runat="server" TextMode="Password"></asp:TextBox></td>
        <td>
        </td>
    </tr>
        <tr>
            <td>
                Nueva</td>
            <td>
                <asp:TextBox ID="m_EContrasena" runat="server" TextMode="Password"></asp:TextBox></td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="m_EContrasena"
                    Display="Dynamic" ErrorMessage="Por favor ingrese una contraseña" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td>
                Confirmar</td>
            <td>
                <asp:TextBox ID="m_EConfirmar" runat="server" TextMode="Password"></asp:TextBox></td>
            <td>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="m_EContrasena"
                    ControlToValidate="m_EConfirmar" Display="Dynamic" ErrorMessage="Las contraseñas no coinciden"
                    SetFocusOnError="True"></asp:CompareValidator></td>
        </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td>
            <asp:Button ID="m_BCambiar" runat="server" OnClick="m_BCambiar_Click" Text="Cambiar" /></td>
    </tr>
</table>
    <asp:Label ID="m_SMensaje" runat="server"></asp:Label>
</asp:Content>

