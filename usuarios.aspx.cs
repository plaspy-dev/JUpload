using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;

public partial class usuarios : System.Web.UI.Page
{
    protected CTablaSQL tabla;
    protected CConector conector;
    protected CUsuario usuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        usuario = CUsuario.GetUsuario();
        if (usuario.GetGrupo() != 1)
        {
            usuario.Redirect();
        }
        if(!IsPostBack) {
            tabla = new CTablaSQL("usuarios");
            tabla.SetID("id");
            tabla.SetHidden(0);
            CQuery query = tabla.GetQuery();
            query["id"] = "id";
            query["Nombre"]= "nombre";
            query["Usuario"] = "usuario";
            query["Grupo"] = "grupo";
            query["EMail"] = "email";
            tabla.SetQuery(query.GetSelect("(estado IS NULL OR estado!=-1)"));
            tabla.FillGrilla(GridView1);
        } else tabla = CTablaSQL.GetTabla("usuarios");
        conector = tabla.GetConector();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow row= tabla.GetTabla().Rows.Find(GridView1.SelectedDataKey.Value);
        if (row != null)
        {
            m_BCancelar_Click(sender, e);

            m_BInsertar.Text = "Editar";
            m_BEliminar.Visible = true;

            m_HId.Value = GridView1.SelectedDataKey.Value.ToString();
            m_ENombre.Text = row["Nombre"].ToString();
            m_EUsuario.Text = row["Usuario"].ToString();
            m_EEMail.Text = row["EMail"].ToString();
            CConector.SelectInCombo(m_CBGrupo, row["Grupo"].ToString());
            RequiredFieldValidator3.Enabled = false;
        }
    }

    protected void m_BInsertar_Click(object sender, EventArgs e)
    {
        string consulta;
        string mensaje = "Usuario " + m_ENombre.Text;

        CQuery query = new CQuery("usuarios");
        query["nombre"] = m_ENombre.Text;
        query["usuario"] = m_EUsuario.Text;
        query["email"] = m_EEMail.Text;

	    if(m_EContrasena.Text.Length>0)
	        query["contrasena"] = m_EContrasena.Text;
        query["grupo"] = m_CBGrupo.SelectedValue;

        if (m_BInsertar.Text == "Nuevo")
        {
            consulta = query.GetInsert();
            mensaje += " ingresado.";
        }
        else
        {
            consulta = query.GetUpdate("id=" + m_HId.Value);
            mensaje += " editado.";
        }
        if (conector.QueryMod(consulta) == -1)
        {
            m_SMensaje.Text = conector.GetLastError();
        }
        else
        {
            m_BCancelar_Click(sender, e);
            m_SMensaje.Text = mensaje;
        }
    }

    protected void m_BCancelar_Click(object sender, EventArgs e)
    {
        m_ENombre.Text = "";
        m_EContrasena.Text = "";
        m_EConfirmar.Text = "";
        m_EUsuario.Text = "";
        m_EEMail.Text = "";
        m_HId.Value = "";
        m_CBGrupo.SelectedIndex = 0;
        m_BInsertar.Text = "Nuevo";
        
        if (sender != GridView1)
        {
            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
        }
        m_SMensaje.Text = "";
        m_BEliminar.Visible = false;
        RequiredFieldValidator3.Enabled = true;
    }

    protected void m_BEliminar_Click(object sender, EventArgs e)
    {
        CQuery query = new CQuery("usuarios");
        query["estado"] = "-1";
        if (conector.QueryMod(query.GetUpdate("id=" + GridView1.SelectedDataKey.Value)) == -1)
        {
            m_SMensaje.Text = conector.GetLastError();
        }
        else
        {
            m_BCancelar_Click(sender, e);
            m_SMensaje.Text = "Usuario " + m_ENombre.Text + " eliminado.";
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowIndex!=-1)
            e.Row.Cells[3].Text= e.Row.Cells[3].Text=="1" ? "Administradores" : "Cliente";
    }
}
