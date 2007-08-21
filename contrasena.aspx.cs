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

public partial class contrasena : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CUsuario usuario = CUsuario.GetUsuario();
    }

    protected void m_BCambiar_Click(object sender, EventArgs e)
    {
        CConector conector = CConector.GetConector();
        CUsuario usuario = CUsuario.GetUsuario();
        CQuery query= new CQuery("");
        bool existe=false;

        MySqlDataReader reader = conector.Query("SELECT contrasena FROM usuarios WHERE id='" + usuario.GetID() + "' AND (estado IS NULL OR estado!=-1)");
        if (reader != null)
        {
            while (reader.Read())
            {
                if (reader["contrasena"].ToString() == m_EActual.Text)
                {
                    query.SetTabla("usuarios");
                    existe=true;
                    break;
                }
            }
        }
        query["contrasena"]=m_EConfirmar.Text;
        if(existe) {
            if(conector.QueryMod(query.GetUpdate("id=" + usuario.GetID()))==-1) {
                m_SMensaje.Text=conector.GetLastError();
            } else {
                Response.Redirect("~/informes.aspx?mensaje=Clave de usuario Cambiada.");
            }
        } else { // usuario no existente
            m_EActual.Focus();
            m_SMensaje.Text = "contraseña equivocada.";
            System.Threading.Thread.Sleep(3000);
        }
    }
}
