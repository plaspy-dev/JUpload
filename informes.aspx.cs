using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SDG.Componentes;
using MySql.Data.MySqlClient;

public partial class informes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack) {

            CUsuario usuario = (CUsuario)Usuario.GetUsuario();

            GResultados.Columns[1].Visible = DUsuarios.Visible = usuario.GetGrupo() == 1;
            DUsuarios.DataBind();
            DUsuarios.SelectedValue= usuario.GetID();

            GResultados.DataBind();
            LResultados.Text = GResultados.Rows.Count + " Resultados";
        }
    }

    protected void BRealizar_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;
        GResultados.DataBind();
        LResultados.Text= GResultados.Rows.Count + " Resultados";
    }
}
