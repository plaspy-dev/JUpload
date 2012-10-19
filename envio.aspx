<%@ Page Title="Envío" Language="C#" MasterPageFile="~/ingreso.master" AutoEventWireup="true" CodeFile="envio.aspx.cs" Inherits="envio" %>

<%@ Register src="historial.ascx" tagname="historial" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" src="js/jquery.cookie.js"></script>
    <script type="text/javascript" src="upload/swfupload.js"></script>
<script type="text/javascript" src="upload/jquery.swfupload.js"></script>
<script type="text/javascript">

    function bytesToSize(bytes) {
        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        if (bytes == 0) return '0 Bytes';
        var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
    };
    
    var hiddenButton = null;
    function obs() {
        var stats = $("#<%= PSWF.ClientID %>").swfupload('getStats');
        var uploading= stats.in_progress;
        
        var retorno = true;
        if (hiddenButton == this)
            return false;
        if (!uploading && this.ejecutar != undefined) {
            if (this.ejecutar.toString().indexOf("function") == -1)
                retorno = eval(this.ejecutar);
            else retorno = this.ejecutar();
        }
        if (uploading)
            Page_ClientValidate();
        if (retorno && Page_IsValid) {
            hiddenButton = this;
            $(this).hide();
            $(this).after('<input type="button" id="dummybutton" disabled="disabled" value="' + $(this).val() + '" class="' + $(this).attr('class') + '" />');
        }
        if (uploading)
            return false;
        return retorno;
    }
    function restoreSummit() {

        $("#dummybutton").remove();
        var h = hiddenButton;
        hiddenButton = null;
        return $(h).show();
    }
    $(function() {

        $("#<%= BAceptar.ClientID %>").each(function(i) {
            if (this.ejecutar == undefined) {
                this.ejecutar = $(this).attr("onclick");
                $(this).unbind('click').attr("onclick", "").click(obs);
            }
        });

        $("#progressbar").progressbar();

        $('#<%= PSWF.ClientID %>').swfupload({
            upload_url: "upload/upload.ashx",
            file_size_limit: "1048576", // 1 GB
            file_types: "*.*",
            file_types_description: "Todos los archivos",
            file_upload_limit: "0",
            flash_url: "upload/swfupload.swf",
            button_image_url: 'upload/upload.png',
            button_width: 143,
            button_height: 29,
            // button_text_style: '.button { font-family: Helvetica, Arial, sans-serif; font-size: 14pt; } .buttonSmall { font-size: 10pt; }',
            button_text_top_padding: 1,
            button_text_left_padding: 5,
            button_placeholder: $('#bUpload')[0]
            // ,debug: true
        })
		.bind('swfuploadLoaded', function(event) {
		    // $('#log').append('<li>Loaded</li>');
		    $(this).swfupload("addPostParam", "u", $.cookie("usuario"));
		    $(this).swfupload("addPostParam", "p", $.cookie("password"));
		})
		.bind('fileQueued', function(event, file) {
		    // $('#log').append('<li>File queued - ' + file.name + '</li>');
		    // start the upload since it's queued
		    $(this).swfupload('startUpload');
		})
		.bind('fileQueueError', function(event, file, errorCode, message) {
		    $('#filename').html('Error agregando archivo: ' + message + '');
		})
		.bind('fileDialogStart', function(event) {
		    // $('#log').append('<li>File dialog start</li>');
		})
		.bind('fileDialogComplete', function(event, numFilesSelected, numFilesQueued) {
		    // $('#log').append('<li>File dialog complete</li>');
		})
		.bind('uploadStart', function(event, file) {

		    $('#filename').html(file.name);
		    $("#tamano").html(bytesToSize(file.size));
		    $("#progreso").html("");
		    $(window).bind('beforeunload', function() { return "Actualmente esta subiendo un archivo, ¿Esta seguro de salir?"; });
		    $("#tprogressbar").show();
		    $("#progressbar").progressbar('value', 0).fadeIn();
		})
		.bind('uploadProgress', function(event, file, bytesLoaded, bytesTotal) {
		    $("#progressbar").progressbar(
                'value',
                parseInt(bytesLoaded / bytesTotal * 100, 10)
            );
		    $("#progreso").html(bytesToSize(bytesLoaded) + " / " + parseInt(bytesLoaded / bytesTotal * 100, 10) + "%");
		})
		.bind('uploadSuccess', function(event, file, serverData) {
		})
		.bind('uploadComplete', function(event, file) {
		    // $('#log').append('<li>Upload complete - ' + file.name + '</li>');
		    // upload has completed, lets try the next one in the queue
		    // $('#log').html('');
		    $("#<%= BArchivos.ClientID %>").click();
		    $(window).unbind('beforeunload');
		    $("#progressbar").fadeOut();
		    $("#tprogressbar").hide();
		    $(this).swfupload('startUpload');
		    if (hiddenButton && !$(this).swfupload('getStats').in_progress) {
		        restoreSummit().click();
		    }
		})
		.bind('uploadError', function(event, file, errorCode, message) {
		    $('#filename').append('<li>Upload error - ' + message + '</li>');
		});
        /*var swfu = $.swfupload.getInstance('#<%= PSWF.ClientID %>');
        swfu.addPostParam("u", "temp");*/
    });	
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%;" cellspacing=0>
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="LUsuario" Font-Bold="True"></asp:Label></td>
            <td align="right" valign="top" rowspan="2">
                <asp:Label ID="LFecha" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="LTopMensaje" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"><asp:Panel runat="server" ID="PSWF">
                    <input type="button" id="bUpload" value="Archivo" class="hide" />
                </asp:Panel></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="bordes" colspan="2">
                <asp:Label ID="LTitulo" runat="server" AssociatedControlID="TTitulo" Text="Referencia"></asp:Label><asp:RequiredFieldValidator ControlToValidate="TTitulo" ErrorMessage="Por favor ingrese un título." ID="RequiredFieldValidator4" runat="server" Display="Dynamic" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                <br />
                <asp:TextBox ID="TTitulo" runat="server" Width="99%" CssClass="sinbordes"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="bordes hide" id="tprogressbar" colspan="2">
            <table width="100%" cellspacing=0 cellpadding=0>
                <tr>
                    <td width="20px">&nbsp;</td>
                    <td style="font-size: 12px;">Archivo en proceso</td>
                    <td align="right" rowspan="2">Total: <span id="tamano"></span></td>
                    <td width="20px">&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="font-size: 12px;color: #EF7F1B" id="filename"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2"><div id="progressbar"></div></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                    <td colspan="2" align="center" style="font-weight: normal" id="progreso"></td>
                    <td align="center">&nbsp;</td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
            <td class="bordes" colspan="2">
                Archivos<br />
                <span style="font-weight: normal">
                <asp:UpdatePanel runat="server" ID="PArchivos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GArchivos" runat="server" AllowSorting="True" 
                            AutoGenerateColumns="False" EmptyDataText="No hay archivos" ShowHeader="False" 
                            DataKeyNames="name" GridLines="None" 
                            onselectedindexchanged="GArchivos_SelectedIndexChanged" >
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="puntos">
                                    <ItemTemplate>
                                        <span style="background-color: #FFF">
                                            <a href='<%# string.Format("archivo.aspx?f={0}&t={1}", Eval("name"), Eval("trabajo")) %>'><%# Eval("name") %></a>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Relleno" DataField="" ItemStyle-Width="100px" ItemStyle-CssClass="puntos" />
                                <asp:BoundField HeaderText="Tamaño" DataField="tamano" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" />
                                <asp:CommandField SelectText="Eliminar" ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/imagenes/eliminar.gif" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button runat="server" ID="BArchivos" onclick="BArchivos_Click" CssClass="hide" CausesValidation="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                </span>
            </td>
        </tr>
        <tr>
            <td class="bordes" colspan="2">
                <asp:Label AssociatedControlID="TComentarios" runat="server" ID="LComentarios" Text="Observaciones"></asp:Label><asp:RequiredFieldValidator ControlToValidate="TComentarios" ErrorMessage="Por favor ingrese un comentario." ID="RequiredFieldValidator1" runat="server" Display="Dynamic" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                <br />
                <asp:TextBox ID="TComentarios" runat="server" TextMode="MultiLine" Width="99%" CssClass="sinbordes"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="LMensaje" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Button ID="BAceptar" runat="server" Text="Enviar" 
                    onclick="BAceptar_Click" CssClass="boton" />
                <asp:Button ID="BCancelar" runat="server" Text="Cancelar" CssClass="boton" 
                    CausesValidation="false" onclick="BCancelar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <uc1:historial ID="historial1" runat="server" />
</asp:Content>

