<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="usuarios.aspx.cs" Inherits="usuarios" Title="Usuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
        <tr>
            <td>
                Nombre</td>
            <td>
                <asp:TextBox ID="m_ENombre" runat="server"></asp:TextBox></td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="m_ENombre"
                    Display="Dynamic" ErrorMessage="Por favor ingrese un nombre" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td>
                Usuario</td>
            <td>
                <asp:TextBox ID="m_EUsuario" runat="server"></asp:TextBox></td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="m_EUsuario"
                    Display="Dynamic" ErrorMessage="Por favor ingrese un usuario" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
        </tr>
    <tr>
        <td>
            E-Mail</td>
        <td>
            <asp:TextBox ID="m_EEMail" runat="server"></asp:TextBox></td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="m_EEMail"
                Display="Dynamic" ErrorMessage="Por favor ingrese una dirección de correo." SetFocusOnError="True"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>
            Grupo</td>
        <td>
            <asp:DropDownList ID="m_CBGrupo" runat="server">
                <asp:ListItem Value="2">Cliente</asp:ListItem>
                <asp:ListItem Value="1">Administradores</asp:ListItem>
            </asp:DropDownList></td>
        <td>
        </td>
    </tr>
        <tr>
            <td>
                Contraseña</td>
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
                <asp:HiddenField ID="m_HId" runat="server" />
            </td>
            <td>
            </td>
            <td>
                <asp:Button ID="m_BInsertar" runat="server" OnClick="m_BInsertar_Click" Text="Nuevo" />
                <asp:Button ID="m_BCancelar" runat="server" OnClick="m_BCancelar_Click" Text="Cancelar" CausesValidation="False" />
                <asp:Button ID="m_BEliminar" runat="server" CausesValidation="False" OnClick="m_BEliminar_Click"
                    Text="Eliminar" Visible="False" /></td>
        </tr>
    </table>
    <asp:Label ID="m_SMensaje" runat="server"></asp:Label><br />
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="ObjectDataSource1" ForeColor="#333333" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" EmptyDataText="No hay usuarios" OnRowDataBound="GridView1_RowDataBound">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:CommandField SelectText="Seleccionar" ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/imagenes/edit.gif"/>
        </Columns>
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
        TypeName="CTablaSQL"></asp:ObjectDataSource>
</asp:Content>

