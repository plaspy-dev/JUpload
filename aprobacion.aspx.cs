using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SDG.Componentes;

public partial class aprobacion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack) {
            ((ingreso)Master).SetLogoSegundario("~/imagenes/hex38.jpg");
            LTopMensaje.Text = "Aprobación de trabajos";
            LUsuario.Text = Usuario.GetUsuario().GetNombre();
            HIdUsuario.Value = Usuario.GetUsuario().GetID();
            TFecha.Text = DateTime.Now.ToShortDateString();
        }
    }
}
