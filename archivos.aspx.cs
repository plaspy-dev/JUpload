using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Mail;
// using System.Net.Mail;


public partial class archivos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Archivos.RemoveDataTable();
        }
        else
        {

        }
    }

    protected void Subir_Click(object sender, EventArgs e)
    {
        if (!Upload.HasFile)
        {
            m_SMensaje.Text = "Por favor seleccione un archivo.";
            return;
        }
        int len = Upload.PostedFile.ContentLength;
        if (len > 0)
        {
            byte[] pic = new byte[len];
            Upload.PostedFile.InputStream.Read(pic, 0, len);
            string filename = Upload.FileName;
            int pos = filename.LastIndexOf("\\");
            if (pos != -1)
                filename = filename.Substring(pos + 1);

            Archivos m_pArchivos = new Archivos();
            m_pArchivos.Add(filename, Upload.PostedFile.ContentType, pic, (len / 1024) + " KB");
            m_LCArchivos.DataBind();
        }
    }

    protected void Enviar_Click(object sender, EventArgs e)
    {
        Archivos m_pArchivos = new Archivos();
        if (m_pArchivos.GetCount() == 0)
        {
            m_LCArchivos.EmptyDataText = "Por lo menos suba un archivo.";
            m_LCArchivos.Focus();
            return;
        }
        string titulo = Titulo.Text;
        string comentarios = Comentarios.Text;

        CUsuario usuario = CUsuario.GetUsuario();
        Directory.CreateDirectory(CSettings.m_szRuta + usuario.GetNombre());
        string nombre = m_pArchivos.SaveFiles(usuario.GetNombre());



        // MailMessage mail = new MailMessage(new MailAddress("demo@example.invalid", "SDG (Software Developers Group)"),new MailAddress(correo, contacto));
        MailMessage mail = new MailMessage();
        mail.To = CSettings.m_szEmailNotificacion;
        mail.From = usuario.GetEmail().Length==0 ? CSettings.m_szEmailFrom : usuario.GetEmail();

        // mail.IsBodyHtml = false;

        mail.Subject = Titulo.Text;
        mail.BodyFormat = MailFormat.Text;
        mail.Priority = MailPriority.Normal;

        mail.Body = "Se recibió un trabajo.\r\n\r\n" +
                    Comentarios.Text +
                    "\r\n\r\nSe adjuntaron los siguientes archivos:\r\n" + 
                    nombre + CSettings.m_szFirma;
        try
        {
            SmtpMail.SmtpServer = CSettings.m_szSMTPServer;
            SmtpMail.Send(mail);
            // SmtpClient client = new SmtpClient("127.0.0.1");
            // client.Send(mail);
        }catch (Exception ex)
        {
            m_SMensaje.Text = ex.Message;
            return;
        }

        if (usuario.GetEmail().Length != 0) {

            MailMessage mailusuario = new MailMessage();
            mailusuario.To = usuario.GetEmail();
            mailusuario.From = CSettings.m_szEmailFrom;

            mailusuario.Subject = Titulo.Text;
            mailusuario.BodyFormat = MailFormat.Text;
            mailusuario.Priority = MailPriority.Normal;

            mailusuario.Body = "Su trabajo ha sido recibido satisfactoriamente.\r\n" +
                          "Se adjuntaron los siguientes archivos:\r\n" +
                           nombre+ CSettings.m_szFirma;

            try
            {
                SmtpMail.SmtpServer = CSettings.m_szSMTPServer;
                SmtpMail.Send(mailusuario);
            }
            catch (Exception ex)
            {
                m_SMensaje.Text = ex.Message;
                return;
            }
        }

        Response.Redirect("terminar.aspx");
        Response.End();
    }

    protected void Cancelar_Click(object sender, EventArgs e)
    {
        CUsuario usuario = CUsuario.GetUsuario();
        usuario.Redirect();
        Response.End();
    }

    protected void m_LCArchivos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Archivos m_pArchivos = new Archivos();
        m_pArchivos.Delete(Convert.ToInt32(m_LCArchivos.SelectedDataKey.Value.ToString()));
        m_LCArchivos.DataBind();
    }
}
