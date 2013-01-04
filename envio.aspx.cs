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

        public static string GetHumanSize(double size) {

            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (size >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                size = size / 1024;
            }

            return String.Format("{0:0.##} {1}", size, sizes[order]);
        }

        public Archivo(string nombre, double size)
        {
            trabajo = "";
            name = nombre;
            tamano = GetHumanSize(size);
        }

        public Archivo(string nombre, double size, string idtrabajo) {

            trabajo = idtrabajo;
            name = nombre;
            tamano = GetHumanSize(size);
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
            PRevisionesH.Visible = false;

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

                        if (usuario.GetGrupo() == 1)
                        {
                            DUsuarios.SelectedValue = reader["usuario"].ToString();
                            LUsuario.Text = DUsuarios.SelectedItem.Text;
                        }

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
                        {
                            PSWF.Visible = false;
                            BAceptarRevision.Visible = false;
                        }
                        PRevisiones.Visible = true;

                        LUsuario.Visible = true;
                        DUsuarios.Visible = false;

                        BAceptar.Visible = false;

                        GRevisiones.DataBind();
                        LRevisiones.Text = GRevisiones.Rows.Count + " Revisi";
                        LRevisiones.Text += GRevisiones.Rows.Count == 1 ? "ón" : "ones";
                        PRevisionesH.Visible = true;
                        // PRevisionesH.Visible = GEvoluciones.Rows.Count > 0;

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
        MailMessage msg = new MailMessage();
        msg.From= new MailAddress(CUsuario.GetOnlyOneMail(ConfigurationManager.AppSettings["notifyemail"]));
        msg.To.Add(usuario["email"].Replace(";", ","));

        MailMessage msga = new MailMessage();
        
        msga.From= new MailAddress(CUsuario.GetOnlyOneMail(usuario["email"]), usuario.GetNombre());
        msga.To.Add(ConfigurationManager.AppSettings["notifyemail"].Replace(";", ","));

        // msg.Bcc.Add(new MailAddress("pruebas"));
        msga.IsBodyHtml = msg.IsBodyHtml = true;
        msga.Subject = "Nuevo trabajo: " + TTitulo.Text;
        msg.Subject = TTitulo.Text;
        msga.ReplyTo = msg.ReplyTo = new MailAddress("demo@example.invalid");

        StreamReader archivohtml = new StreamReader(Server.MapPath("trabajo.html"));
        msg.Body = archivohtml.ReadToEnd();
        archivohtml.Close();

        msga.Body = msg.Body = ((ingreso)Master).ReplaceContenido(this.Controls, msg.Body);
        msga.Body = msg.Body = msg.Body.Replace("$Usuario$", usuario.GetNombre());
        msga.Body = msg.Body = msg.Body.Replace("$szTitulo$", "<a href='" + ConfigurationManager.AppSettings["servidorURL"] + "envio.aspx?id=" + res.ToString() + "'>" + TTitulo.Text + "</a>");

        msg.Body = msg.Body.Replace("$GArchivos$", archivos.Replace(",", "<br/>"));

        string tablaarchivos = "";

        // final = usuario.GetPrivatePath(res.ToString());

        for (int i = 0; i < values.Count; i++)
        {
            string archivo = System.IO.Path.GetFileName(values[i]);
            if (tablaarchivos.Length > 0)
                tablaarchivos += "<br/>";

            tablaarchivos+= string.Format("<a href='{0}archivo.aspx?f={1}&t={2}'>{1}</a>"
                , ConfigurationManager.AppSettings["servidorURL"]
                , archivo
                , res
            );
            // tablaarchivos += "<a href='" + final + archivo + "'>" + archivo + "</a>";
        }
        msga.Body = msga.Body.Replace("$GArchivos$", tablaarchivos);

        msg.Body = msg.Body.Replace("$szMensaje$", "Este es un mensaje automático informandole que sus archivos fueron recibidos de manera satisfactoría.");
        msga.Body = msga.Body.Replace("$szMensaje$", "Nuevo trabajo recibido en la plataforma.");
        // build message contents 
        SmtpClient client = new SmtpClient();
        try
        { // enviamos al cliente
            client.Send(msga);
        }
        catch { }
        try
        {
            client.Send(msg);
        }
        catch  { }

        Session["mensaje"] = "Nuevo trabajo: " + TTitulo.Text + " Creado.";

        BCancelar_Click(sender, e);
    }

    protected void BCancelar_Click(object sender, EventArgs e)
    {
        if(sender==BCancelar)
            Response.Redirect("~/inicio.aspx");
        else Response.Redirect(Request.Url.AbsolutePath);
    }

    protected bool AddRevision(Query query, string archivo) {

        CConector conector = new CConector();

        if (!string.IsNullOrEmpty(archivo)) {
            query["nombre"] = System.IO.Path.GetFileName(archivo);
            query["tamano"] = new FileInfo(archivo).Length.ToString();
        }

        int res = conector.QueryMod(query.GetInsert(), true);
        if (res == -1)
        {
            LMensaje.Text = conector.GetLastError();
            return false;
        }

        if (!string.IsNullOrEmpty(archivo))
        {
            // movemos los archivos a la posicion definitiva
            System.IO.File.Move(archivo, 
                                GetRevisionFinalPath(res.ToString()) + System.IO.Path.GetFileName(archivo));
        }

        CUsuario usuario = (CUsuario)Usuario.GetUsuario();

        MySqlDataReader reader= conector.Query("SELECT nombre,email FROM usuariosweb WHERE id=" + (usuario.GetGrupo()==1 ? DUsuarios.SelectedValue : usuario.GetID()));
        if(reader==null || !reader.Read()) {
            return false;
        }

        // enviamos el correo al usuario
        MailMessage msg = new MailMessage();
        
        msg.From= new MailAddress(CUsuario.GetOnlyOneMail(ConfigurationManager.AppSettings["notifyemail"]));
        msg.To.Add(reader["email"].ToString().Replace(";", ","));

        msg.IsBodyHtml = true;
        msg.Subject = "Revisión: " + TTitulo.Text;
        msg.ReplyTo = msg.ReplyTo = new MailAddress("demo@example.invalid");

        StreamReader archivohtml = new StreamReader(Server.MapPath("revision.html"));
        msg.Body = archivohtml.ReadToEnd();
        archivohtml.Close();

        msg.Body = ((ingreso)Master).ReplaceContenido(this.Controls, msg.Body);

        msg.Body = msg.Body.Replace("$szFecha$", DateTime.Now.ToString());
        msg.Body = msg.Body.Replace("$GArchivos$"
            , string.Format("<a href='{0}archivo.aspx?f={1}&t={2}&ri={3}'>{1}</a>"
            ,ConfigurationManager.AppSettings["servidorURL"]
            ,System.IO.Path.GetFileName(archivo)
            ,HId.Value
            ,res
            ));
        msg.Body = msg.Body.Replace("$Usuario$", reader["nombre"].ToString());
        msg.Body = msg.Body.Replace("$szTrabajo$", "<a href='" + ConfigurationManager.AppSettings["servidorURL"] + "envio.aspx?id=" + HId.Value + "'>" + TTitulo.Text + "</a>");

        msg.Body = msg.Body.Replace("$szMensaje$", "Este es un mensaje automático informandole que se ha solicitado una revisión al trabajo.");
        
        // build message contents 
        SmtpClient client = new SmtpClient();
        try
        {
            client.Send(msg);
        }
        catch { }

        return true;
    }

    protected string GetRevisionTempPath() {

        string temppath = "";
        CConector conector = new CConector();
        MySqlDataReader reader = conector.Query("SELECT fecha,usuario FROM trabajo WHERE id=" + Query.GetValor(HId.Value));
        if (reader != null && reader.Read())
        {
            DateTime fecha = (DateTime)reader["fecha"];
            string szUsuario = conector.QueryString("SELECT usuario FROM usuariosweb WHERE id=" + Query.GetValor(reader["usuario"].ToString()));
            temppath = CUsuario.GetRevisionTempPath(szUsuario, HId.Value, fecha);
        }
        return temppath;
    }

    protected string GetRevisionFinalPath(string idrevision)
    {
        string temppath = "";
        CConector conector = new CConector();
        MySqlDataReader reader = conector.Query("SELECT fecha,usuario FROM trabajo WHERE id=" + Query.GetValor(HId.Value));
        if (reader != null && reader.Read())
        {
            DateTime fecha = (DateTime)reader["fecha"];
            string szUsuario = conector.QueryString("SELECT usuario FROM usuariosweb WHERE id=" + Query.GetValor(reader["usuario"].ToString()));
            temppath = CUsuario.GetFinalRevisionPath(szUsuario, HId.Value, fecha, idrevision);
        }
        return temppath;
    }

    protected void BAceptarRevision_Click(object sender, EventArgs e) {

        // aqui agregamos la revisión
        Query query = new Query("revision");
        bool archivos = false;

        query["trabajo"]=HId.Value;
        query["fecha"]="SYSDATE()";
        query["ordendetrabajo"]=TOrden.Text;
        query["observaciones"]=TComentariosRevision.Text;

        foreach (string archivo in System.IO.Directory.GetFiles(GetRevisionTempPath(), "*.*"))
        {
            if (!AddRevision(query, archivo))
                return;
            archivos = true;
        }
        if (!archivos)
            if (!AddRevision(query, ""))
                return;

        Session["mensaje"] = "Nuevo revisión: " + TOrden.Text + " agregada.";
        BCancelarRevision_Click(sender, e);
    }

    protected void BCancelarRevision_Click(object sender, EventArgs e)
    {
        // aqui cancelamos la revision
        Response.Redirect(Request.Url.ToString());
    }

    protected void BArchivosRevision_Click(object sender, EventArgs e)
    {
        List<Archivo> values = new List<Archivo>();

        foreach (string archivo in System.IO.Directory.GetFiles(GetRevisionTempPath(), "*.*"))
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

    protected void GRevisiones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            try
            {
                e.Row.Cells[6].Text = Archivo.GetHumanSize(Convert.ToDouble(e.Row.Cells[6].Text));
            } catch {}
        }
    }
}
