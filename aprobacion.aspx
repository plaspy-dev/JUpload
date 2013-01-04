<%@ Page Title="Aprobación de trabajos" Language="C#" MasterPageFile="~/ingreso.master" AutoEventWireup="true" CodeFile="aprobacion.aspx.cs" Inherits="aprobacion" %>
<%@ Register src="historial.ascx" tagname="historial" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" src="js/jquery.labelify.js"></script>
<script type="text/javascript">
    $(function() {
        $('textarea').attr("title", "Ingrese sus comentarios").labelify({ labelledClass: "comentarioso" });
        // , text: function(input) { return "Ingrese sus comentarios"; }
        $(".baprobar,.brechazar").click(function() {
            var comentarios = $(this).parent().parent().prev().find('textarea[id*="TComentarios"]');
            if (comentarios.val() == "Ingrese sus comentarios")
                comentarios.val("");
            if (comentarios.val().length == 0) {
                if (!confirm("¿Esta seguro de enviar sin comentarios?")) {
                    comentarios.focus();
                    return false;
                }
            }
            return true;
        });
    });
</script>
<style type="text/css">
.orden {
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    width: 57px;
    text-align: center;
    font-family: co_headlineregular;
    font-size: 15px;
}

.archivo {
	overflow: hidden;
    text-overflow: ellipsis;
	word-wrap:break-word;
    width: 73px;
	height: 49px;
	color: #00A0E4;
	font-size: 10px;
	vertical-align: middle;
	text-decoration: none;
}
.archivo:hover 
{
    text-decoration: underline;
}
.comentarios 
{
    border: 1px solid #000000;
}
.comentarioso 
{
    color: Gray;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table style="width: 100%;">
    <tr>
        <td align="left">
            <asp:Label runat="server" ID="LUsuario" Font-Bold="True"></asp:Label>
            <asp:DropDownList ID="DUsuarios" runat="server" DataSourceID="SUsuarios" AutoPostBack="true" 
                DataTextField="nombre" DataValueField="id">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SUsuarios" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MySQLConnectionString %>" 
                ProviderName="<%$ ConnectionStrings:MySQLConnectionString.ProviderName %>" 
                SelectCommand="SELECT id, nombre FROM usuariosweb WHERE estado IS NULL OR estado=1 ORDER BY nombre">
            </asp:SqlDataSource>
        </td>
        <td align="right" rowspan="2">
            <b>Estado:</b> 
            <asp:DropDownList runat="server" ID="DEstado" AutoPostBack="true">
                <asp:ListItem Text="Sin respuesta" Value="0"></asp:ListItem>
                <asp:ListItem Text="Aprobados" Value="1"></asp:ListItem>
                <asp:ListItem Text="Rechazados" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LTopMensaje" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">
            Hola, usted tiene estos trabajos por responder.</td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:DataList ID="DAprobar" runat="server" RepeatColumns="2" DataSourceID="SAprobar" DataKeyField="id" Width="100%">
                <HeaderTemplate>
                    <table width="100%">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td colspan="2" class="separadorh" style="min-width: auto;background-position-y:10px;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="120" align="center">
                        <table style="display: inline-table;" border="0" cellpadding="0" cellspacing="0" width="92">
                        <!-- fwtable fwsrc="papel.png" fwpage="Página 1" fwbase="papel.jpg" fwstyle="Dreamweaver" fwdocid = "607392938" fwnested="0" -->
                          <tr>
                           <td><img src="papel/spacer.gif" width="2" height="1" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="32" height="1" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="41" height="1" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="17" height="1" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="1" height="1" border="0" alt="" /></td>
                          </tr>
                          <tr>
                           <td colspan="4"><img name="papel_r1_c1" src="papel/papel_r1_c1.jpg" width="92" height="11" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="1" height="11" border="0" alt="" /></td>
                          </tr>
                          <tr>
                           <td colspan="2"><img name="papel_r2_c1" src="papel/papel_r2_c1.jpg" width="34" height="23" border="0" alt="" /></td>
                           <td colspan="2" style="border: 1px #333 solid;"><div class="orden">
                           <a class="archivo" href='<%# string.Format("envio.aspx?id={0}", Eval("trabajo")) %>'><%# Eval("ordendetrabajo") %></a>
                           </div></td>
                           <td><img src="papel/spacer.gif" width="1" height="23" border="0" alt="" /></td>
                          </tr>
                          <tr>
                           <td rowspan="2"><img name="papel_r3_c1" src="papel/papel_r3_c1.jpg" width="2" height="66" border="0" alt="" /></td>
                           <td colspan="2"><div class="archivo"><a class="archivo" href='<%# string.Format("archivo.aspx?f={0}&t={1}&ri={2}", Eval("nombre"), Eval("trabajo"), Eval("id")) %>'><%# Eval("nombre") %></a></div></td>
                           <td rowspan="2"><img name="papel_r3_c4" src="papel/papel_r3_c4.jpg" width="17" height="66" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="1" height="49" border="0" alt="" /></td>
                          </tr>
                          <tr>
                           <td colspan="2"><img name="papel_r4_c2" src="papel/papel_r4_c2.jpg" width="73" height="17" border="0" alt="" /></td>
                           <td><img src="papel/spacer.gif" width="1" height="17" border="0" alt="" /></td>
                          </tr>
                        </table>
                        <td valign="top">
                            <%# Eval("fecha") %>-<%# Eval("estado") %><br />
                            <%# Eval("observaciones") %><br />
                            <asp:TextBox runat="server" ID="TComentarios" TextMode="MultiLine" Width="99%" Text='<%# Eval("comentarios") %>' CssClass="comentarios"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><a class="bdescargar" href='<%# string.Format("archivo.aspx?f={0}&t={1}&ri={2}", Eval("nombre"), Eval("trabajo"), Eval("id")) %>'>Descargar</a></td>
                        <td align="center">
                            <a href="#" class="baprobar" style='<%# Eval("nestado").ToString()=="1" ? "" : "display:none;" %>'>Aprobado</a>
                            <a href="#" class="brechazar" style='<%# Eval("nestado").ToString()=="2" ? "" : "display:none;" %>'>Rechazado</a>
                            <asp:LinkButton CssClass="baprobar" runat="server" ID="BAprobar" Text="Aprobar" OnClick="BAprobar_Clicked" Visible='<%# Eval("nestado").ToString()=="" ? true : false %>'></asp:LinkButton>
                            <asp:LinkButton CssClass="brechazar" runat="server" ID="BRechazar" Text="Rechazar" OnClick="BRechazar_Clicked" Visible='<%# Eval("nestado").ToString()=="" ? true : false %>'></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            <asp:SqlDataSource ID="SAprobar" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MySQLConnectionString %>" 
                ProviderName="<%$ ConnectionStrings:MySQLConnectionString.ProviderName %>" 
                SelectCommand="SELECT revision.id,revision.fecha,revision.observaciones
                                        ,CASE revision.estado WHEN 1 THEN 'Aprobado' WHEN 2 THEN 'Rechazado' ELSE 'Sin repuesta' END AS estado
                                        ,revision.estado AS nestado
                                        ,revision.ordendetrabajo 
                                        ,revision.nombre,revision.tamano,revision.trabajo,revision.comentarios 
                                        FROM revision 
                                        LEFT JOIN trabajo ON (trabajo.id=revision.trabajo) 
                                        WHERE 
                                        trabajo.usuario=@usuario 
                                        AND ((@estado=0 AND revision.estado IS NULL) OR (revision.estado=@estado)) 
                                        ORDER BY revision.fecha DESC;">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DUsuarios" Name="usuario" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="DEstado" Name="estado" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:Panel Visible="false" ID="PSinResponder" runat="server">
<table cellpadding=0 cellspacing=0>
    <tr>
        <td class="separador" valign="top" style="min-height: 380px;white-space: nowrap">
        <span style="font-family: co_headlineregular;font-size: 13px">HISTORIAL DE TRABAJOS SIN RESPONDER</span><br /><br />
        <asp:DataList ID="DHistorial" runat="server" DataSourceID="SHistorial" DataKeyField="id">
        <ItemTemplate>
            <a href="aprobacion.aspx" class="ahistorial">
            <%# Eval("fecha")%><br />
            <%# Eval("nombre")%><br /><br /></a>
        </ItemTemplate>
        </asp:DataList>
        <asp:SqlDataSource ID="SHistorial" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MySQLConnectionString %>" 
            ProviderName="<%$ ConnectionStrings:MySQLConnectionString.ProviderName %>" 
            SelectCommand="SELECT revision.fecha,revision.id
                                        ,revision.ordendetrabajo
                                        ,revision.nombre
                                        FROM revision
                                        LEFT JOIN trabajo ON (trabajo.id=revision.trabajo)
                                        WHERE 
                                        trabajo.usuario=@usuario 
                                        AND revision.estado IS NULL
                                        ORDER BY revision.fecha DESC;">
            <SelectParameters>
                <asp:ControlParameter ControlID="ContentPlaceHolder1$DUsuarios" Name="usuario" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel Visible="false" ID="PSinRevisar" runat="server">
<table cellpadding=0 cellspacing=0>
    <tr>
        <td class="separador" valign="top" style="min-height: 380px;white-space: nowrap">
        <span style="font-family: co_headlineregular;font-size: 13px">HISTORIAL DE TRABAJOS SIN REVISAR</span><br /><br />
        <asp:DataList ID="DTrabajos" runat="server" DataSourceID="STrabajos" DataKeyField="id">
        <ItemTemplate>
            <a href="envio.aspx?id=<%# Eval("id")%>" class="ahistorial">
            <%# Eval("fecha")%><br />
            <%# Eval("usuario")%><br />
            <%# Eval("archivos")%><br /><br /></a>
        </ItemTemplate>
        </asp:DataList>
        <asp:SqlDataSource ID="STrabajos" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MySQLConnectionString %>" 
            ProviderName="<%$ ConnectionStrings:MySQLConnectionString.ProviderName %>" 
            SelectCommand="SELECT trabajo.id,trabajo.fecha,GROUP_CONCAT(trabajo_archivo.nombre ORDER BY trabajo_archivo.nombre SEPARATOR '<br/>') AS archivos
                                ,usuariosweb.nombre AS usuario
                                FROM trabajo
                                LEFT JOIN usuariosweb ON (trabajo.usuario=usuariosweb.id)
                                LEFT JOIN trabajo_archivo ON (trabajo_archivo.trabajo=trabajo.id)
                                LEFT JOIN revision ON (revision.trabajo=trabajo.id)
                                WHERE
                                revision.id IS NULL
                                GROUP BY trabajo.id
                                ORDER BY trabajo.fecha DESC LIMIT 5">
        </asp:SqlDataSource>
        </td>
    </tr>
</table>
</asp:Panel>
</asp:Content>
