using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using SDG.Componentes;
using MySql.Data.MySqlClient;

public partial class envio : System.Web.UI.Page
{
    public class Archivo {
        public string tamano { get; set; }
        public string name { get; set; }
        public string trabajo { get; set; }

        public Archivo(string nombre, double size)
        {
            trabajo = "";
            name = nombre;

            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (size >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                size = size / 1024;
            }

            tamano = String.Format("{0:0.##} {1}", size, sizes[order]);
        }

        public Archivo(string nombre, double size, string idtrabajo) {

            trabajo = idtrabajo;
            name = nombre;

            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (size >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                size = size / 1024;
            }

            tamano = String.Format("{0:0.##} {1}", size, sizes[order]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CUsuario usuario= (CUsuario) Usuario.GetUsuario();
            ((ingreso)Master).SetLogoSegundario("~/imagenes/hex30.jpg");
            LTopMensaje.Text = "Envío de trabajos";

            LMensaje.Text = "";
            LUsuario.Text = usuario.GetNombre();
            LUsuario.Visible = usuario.GetGrupo() != 1;
            DUsuarios.Visible = usuario.GetGrupo() == 1;
            PRevisiones.Visible = false;

            if (usuario.GetGrupo() == 1) {

                DUsuarios.DataBind();
                DUsuarios.SelectedValue = usuario.GetID();
            }

            if (Session["mensaje"] != null)
            {
                if (!string.IsNullOrEmpty(Session["mensaje"].ToString()))
                {
                    LTopMensaje.Text = Session["mensaje"].ToString();
                }
                Session["mensaje"] = null;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {

                Query trabajo = new Query("trabajo");
                CConector conector = new CConector();

                trabajo += "id";
                trabajo += "titulo";
                trabajo += "observaciones";
                trabajo += "usuario";
                trabajo += "fecha";

                MySqlDataReader reader = conector.Query(trabajo.GetSelect("id=" + Query.GetValor(Request.QueryString["id"])));
                if (reader != null && reader.Read())
                {
                    if (usuario.GetGrupo() == 1 || usuario.GetID() == reader["usuario"].ToString())
                    {
                        HId.Value = reader["id"].ToString();
                        TTitulo.Text = reader["titulo"].ToString();
                        TComentarios.Text = reader["observaciones"].ToString();
                        LFecha.Text = reader["fecha"].ToString();

                        List<Archivo> values = new List<Archivo>();
                        Query queryarchivo = new Query("trabajo_archivo");
                        
                        queryarchivo += "nombre";
                        queryarchivo += "tamano";

                        reader = conector.Query(queryarchivo.GetSelect("trabajo=" + Query.GetValor(Request.QueryString["id"])));
                        if(reader!=null) {
                            while(reader.Read()) {
                                values.Add(new Archivo(reader["nombre"].ToString(), Convert.ToDouble(reader["tamano"]), Request.QueryString["id"]));
                            }
                        }

                        GArchivos.Columns[3].Visible = false;
                        GArchivos.DataSource = values;
                        GArchivos.DataBind();
                        if (usuario.GetGrupo() != 1)
                            PSWF.Visible = false;
                        else PRevisiones.Visible = true;

                        LUsuario.Visible = true;
                        DUsuarios.Visible = false;

                        BAceptar.Visible = false;

                        BArchivosRevision_Click(sender, e);
                        return;
                    }
                }
            }
            LFecha.Text = DateTime.Now.ToString();
            BArchivos_Click(sender, e);
        }
    }

    protected void BAceptar_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;

        Query trabajo = new Query("trabajo");
        CConector conector = new CConector();
        CUsuario usuario = (CUsuario)Usuario.GetUsuario();
        string archivos = "";
        List<string> values = new List<string>();
        foreach (string archivo in System.IO.Directory.GetFiles(usuario.GetTempPath(), "*.*"))
        {
            values.Add(archivo);
            if (archivos.Length != 0)
                archivos += ",";
            archivos += System.IO.Path.GetFileName(archivo);
        }
        if (archivos.Length==0 && usuario.GetGrupo()!=1)
        {
            LMensaje.Text = "Por favor ingrese por lo menos un archivo.";
            return;
        }

        trabajo["titulo"] = TTitulo.Text;
        trabajo["observaciones"] = TComentarios.Text;
        trabajo["usuario"] = usuario.GetGrupo()==1 ? DUsuarios.SelectedValue : usuario.GetID();
        trabajo["fecha"] = "SYSDATE()";

        LFecha.Text = DateTime.Now.ToString();

        int res = conector.QueryMod(trabajo.GetInsert(), true);
        if (res == -1)
        {
            LMensaje.Text = conector.GetLastError();
            return;
        }

        // agregamos los archivos
        Query queryarchivo = new Query("trabajo_archivo");
        queryarchivo["trabajo"] = res.ToString();
        
        for(int i=0; i<values.Count; i++) {

            queryarchivo["nombre"]= System.IO.Path.GetFileName(values[i]);
            queryarchivo["tamano"] = new FileInfo(values[i]).Length.ToString();

            conector.QueryMod(queryarchivo.GetInsert());
        }

        string final = usuario.GetFinalPath(res.ToString());
        // movemos los archivos a la posicion definitiva
        for (int i = 0; i < values.Count; i++)
        {
            string archivo = System.IO.Path.GetFileName(values[i]);
            System.IO.File.Move(usuario.GetTempPath() + archivo, final + archivo);
        }

        // enviamos el correo al usuario
        MailMessage msg = new MailMessage(new MailAddress(ConfigurationManager.AppSettings["notifyemail"])
            , new MailAddress(usuario["email"], usuario.GetNombre()));
        MailMessage msga = new MailMessage(new MailAddress(usuario["email"], usuario.GetNombre())
            , new MailAddress(ConfigurationManager.AppSettings["notifyemail"]));

        // msg.Bcc.Add(new MailAddress("pruebas"));
        msga.IsBodyHtml = msg.IsBodyHtml = true;
        msga.Subject = "Nuevo trabajo: " + TTitulo.Text;
        msg.Subject = TTitulo.Text;
        msga.ReplyTo = msg.ReplyTo = new MailAddress("demo@example.invalid");

        StreamReader archivohtml = new StreamReader(Server.MapPath("trabajo.html"));
        msg.Body = archivohtml.ReadToEnd();
        archivohtml.Close();

        msga.Body = msg.Body = ((ingreso)Master).ReplaceContenido(this.Controls, msg.Body);
        msg.Body = msg.Body.Replace("$GArchivos$", archivos.Replace(",", "<br/>"));
        msg.Body = msg.Body.Replace("$Usuario$", usuario.GetNombre());

        string tablaarchivos = "";
        final = usuario.GetPrivatePath(res.ToString());

        for (int i = 0; i < values.Count; i++)
        {
            string archivo = System.IO.Path.GetFileName(values[i]);
            if (tablaarchivos.Length > 0)
                tablaarchivos += "<br/>";
            tablaarchivos += "<a href='" + final + archivo + "'>" + archivo + "</a>";
        }
        msga.Body = msga.Body.Replace("$GArchivos$", tablaarchivos);
        msga.Body = msga.Body.Replace("$Usuario$", usuario.GetNombre());

        msg.Body = msg.Body.Replace("$szMensaje$", "Este es un mensaje automático informandole que sus archivos fueron recibidos de manera satisfactoría.");
        msga.Body = msga.Body.Replace("$szMensaje$", "Nuevo trabajo recibido en la plataforma.");
        // build message contents 
        SmtpClient client = new SmtpClient();
        try
        { // enviamos al cliente
            client.Send(msga);
        }
        catch (Exception ex) { }
        try
        {
            client.Send(msg);
        }
        catch (Exception ex) { }

        Session["mensaje"] = "Nuevo trabajo: " + TTitulo.Text + " Creado.";

        BCancelar_Click(sender, e);
    }

    protected void BCancelar_Click(object sender, EventArgs e)
    {
        if(sender==BCancelar)
            Response.Redirect("~/inicio.aspx");
        else Response.Redirect(Request.Url.AbsolutePath);
    }

    protected void BAceptarRevision_Click(object sender, EventArgs e) {

        // aqui agregamos la revisión
    }

    protected void BCancelarRevision_Click(object sender, EventArgs e)
    {
        // aqui cancelamos la revision
        Response.Redirect(Request.Url.ToString());
    }

    protected void BArchivosRevision_Click(object sender, EventArgs e)
    {
        List<Archivo> values = new List<Archivo>();
        
        string temppath="";
        CConector conector = new CConector();
        MySqlDataReader reader = conector.Query("SELECT fecha,usuario FROM trabajo WHERE id=" + Query.GetValor(HId.Value));
        if (reader != null && reader.Read())
        {
            DateTime fecha = (DateTime)reader["fecha"];
            string szUsuario= conector.QueryString("SELECT usuario FROM usuariosweb WHERE id=" + Query.GetValor(reader["usuario"].ToString()));
            temppath = CUsuario.GetRevisionTempPath(szUsuario, HId.Value, fecha);
        }

        foreach (string archivo in System.IO.Directory.GetFiles(temppath, "*.*"))
        {
            FileInfo info = new FileInfo(archivo);
            values.Add(new Archivo(System.IO.Path.GetFileName(archivo), info.Length, HId.Value));
        }

        GArchivosRevision.DataSource = values;
        GArchivosRevision.DataBind();
    }

    protected void BArchivos_Click(object sender, EventArgs e)
    {
        List<Archivo> values = new List<Archivo>();
        CUsuario usuario = (CUsuario)Usuario.GetUsuario();

        foreach (string archivo in System.IO.Directory.GetFiles(usuario.GetTempPath(), "*.*"))
        {
            FileInfo info = new FileInfo(archivo);
            values.Add(new Archivo(System.IO.Path.GetFileName(archivo), info.Length));
        }

        GArchivos.DataSource = values;
        GArchivos.DataBind();
    }

    protected void GArchivos_SelectedIndexChanged(object sender, EventArgs e)
    {
        CUsuario usuario = (CUsuario)Usuario.GetUsuario();
        System.IO.File.Delete(usuario.GetTempPath() + GArchivos.SelectedValue);
        BArchivos_Click(sender, e);
    }

    protected void GArchivosRevision_SelectedIndexChanged(object sender, EventArgs e)
    {
        string temppath = "";
        CConector conector = new CConector();
        MySqlDataReader reader = conector.Query("SELECT fecha,usuario FROM trabajo WHERE id=" + Query.GetValor(HId.Value));
        if (reader != null && reader.Read())
        {
            DateTime fecha = (DateTime)reader["fecha"];
            string szUsuario = conector.QueryString("SELECT usuario FROM usuariosweb WHERE id=" + Query.GetValor(reader["usuario"].ToString()));
            temppath = CUsuario.GetRevisionTempPath(szUsuario, HId.Value, fecha);
        }

        System.IO.File.Delete(temppath + GArchivosRevision.SelectedValue);
        BArchivosRevision_Click(sender, e);
    }
}
