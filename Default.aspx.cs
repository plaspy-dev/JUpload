using System;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack) {
            string end = Request.QueryString["end"];
            if (end == "1")
            {
                Session.RemoveAll();
                Response.Redirect(CSettings.m_szURL);
                Response.End();
            }
            m_EUsuario.Text = CConector.GetCookie("usuario");
            if (Request.Form["m_EUsuario"] != null && Request.Form["m_EContrasena"] != null)
            {
                m_EUsuario.Text = Request.Form["m_EUsuario"].ToString();
                m_EContrasena.Text = Request.Form["m_EContrasena"].ToString();
                m_BIngresar_Click(sender, e);
            }
        }
    }

    protected void m_BIngresar_Click(object sender, EventArgs e)
    {
        CConector conector = CConector.GetConector();
        string usuario = m_EUsuario.Text;
        string contrasena = m_EContrasena.Text;
        string email="";

        if (contrasena.Length > 0 && usuario.Length > 0)
        {
            int grupo=0;
            string id = "";
            MySqlDataReader reader = conector.Query("SELECT id,nombre,contrasena,grupo,email FROM usuarios WHERE usuario='" + usuario + "' AND (estado IS NULL OR estado!=-1)");
            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader["contrasena"].ToString() == contrasena)
                    {
                        usuario = reader["nombre"].ToString();
                        id = reader["id"].ToString();
                        grupo = Convert.ToInt32(reader["grupo"].ToString());
                        email = reader["email"].ToString();
                        break;
                    }
                }
            }
            if (id.Length!= 0)
            {
                CConector.SetCookie("usuario", m_EUsuario.Text);
                CUsuario pUsuario = new CUsuario(id, usuario, grupo, email);
                string url= Request.QueryString["from"];
                if (url==null || url.Length == 0)
                    pUsuario.Redirect();
                else Response.Redirect(url);
                Response.End();
                return;
            }
        }
        m_EContrasena.Text = "";
        m_EUsuario.Focus();
        m_SMensaje.Text = "Usuario o contraseña equivocada.";
        System.Threading.Thread.Sleep(3000);
    }
}
