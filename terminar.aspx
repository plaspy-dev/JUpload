<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="terminar.aspx.cs" Inherits="terminar" Title="Archivos subidos con exito" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="text-align: center">
    Sus archivos han sido subidos al servidor.<br />
    <asp:Label ID="m_SMensaje" runat="server"></asp:Label><br />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx?end=1">Terminar</asp:HyperLink></div>
</asp:Content>

