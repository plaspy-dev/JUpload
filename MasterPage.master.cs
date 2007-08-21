using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CUsuario usuario = CUsuario.GetUsuario();
        // de gratis para todos los usuarios
        if (!IsPostBack)
        {
            m_SUsuario.Text = usuario.GetNombre();
            m_MenuPrincipal.Items.Add(new MenuItem("Archivos", "", "", "~/archivos.aspx"));
            if (usuario.GetGrupo() == 1)
            {
                m_MenuPrincipal.Items.Add(new MenuItem("Usuarios", "", "", "~/usuarios.aspx"));
            }
            m_MenuPrincipal.Items.Add(new MenuItem("Cambiar contraseña", "", "", "~/contrasena.aspx"));
            m_MenuPrincipal.Items.Add(new MenuItem("Salir", "", "", "~/Default.aspx?end=1"));

            m_SVersion.Text= "Versión " + CConector.GetVersion();
        }
    }
}
