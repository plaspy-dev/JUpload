using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SDG.Componentes;

public partial class historial : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HIdUsuario.Value = Usuario.GetUsuario().GetID();
    }
}
