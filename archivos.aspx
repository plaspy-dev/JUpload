<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="archivos.aspx.cs" Inherits="archivos" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 
    <table>
            <tr>
                <td align="left">
                    Título</td>
                <td>
                    <asp:TextBox ID="Titulo" runat="server" Width="100%"></asp:TextBox></td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Titulo"
                        Display="Dynamic" ErrorMessage="Por favor ingrese un título" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="left" style="height: 94px" valign="top">
                    Comentarios</td>
                <td style="height: 94px">
                    <asp:TextBox ID="Comentarios" runat="server" Height="82px" TextMode="MultiLine" Width="100%"></asp:TextBox></td>
                <td align="left" style="height: 94px">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                        ErrorMessage="Por favor ingrese un comentario." ControlToValidate="Comentarios"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="left">
                </td>
                <td style="text-align: left; width: 400px;">
                    <asp:GridView ID="m_LCArchivos" runat="server" CellPadding="4" DataKeyNames="id" DataSourceID="ObjectDataSource1"
                        EmptyDataText="No hay archivos." ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnSelectedIndexChanged="m_LCArchivos_SelectedIndexChanged">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:CommandField SelectText="Eliminar" ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/imagenes/eliminar.gif"/>
                            <asp:BoundField DataField="nombre" HeaderText="Archivo" />
                            <asp:BoundField DataField="tamano" HeaderText="Tama&#241;o" />
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
                        TypeName="Archivos"></asp:ObjectDataSource>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Upload"
                        Display="Dynamic" ErrorMessage="Por favor seleccione un archivo." SetFocusOnError="True"
                        ValidationGroup="subir"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="left">
                    Archivo</td>
                <td>
                    <asp:FileUpload ID="Upload" runat="server" Width="100%" /></td>
                <td align="left">
                    <asp:Button ID="Subir" runat="server" OnClick="Subir_Click" Text="Subir" ValidationGroup="subir" /></td>
            </tr>
            <tr>
                <td align="left">
                </td>
                <td style="text-align: right">
                    <asp:Button ID="Enviar" runat="server" OnClick="Enviar_Click" Text="Enviar" />
                    <asp:Button ID="Cancelar" runat="server" CausesValidation="False" OnClick="Cancelar_Click"
                        Text="Cancelar" /></td>
                <td align="left">
                    <asp:Label ID="m_SMensaje" runat="server"></asp:Label></td>
            </tr>
        </table>
</asp:Content>

