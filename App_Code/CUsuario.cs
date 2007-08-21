using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class CUsuario
{
    protected string m_szNombre;
    protected int m_nGrupo;
    protected string m_szID;
    protected string m_szEmail;

	public CUsuario(string id, string nombre, int grupo, string email)
	{
        m_szNombre = nombre;
        m_nGrupo = grupo;
        m_szID = id;
        m_szEmail = email;
        HttpContext.Current.Session["Usuario"] = this;
	}
    public string GetNombre()
    {
        return m_szNombre;
    }
    public int GetGrupo()
    {
        return m_nGrupo;
    }
    
    public string GetID()
    {
        return m_szID;
    }

    public string GetEmail() {

        return m_szEmail;
    }

    public void Redirect()
    {
        HttpContext.Current.Response.Redirect("~/archivos.aspx");
        HttpContext.Current.Response.End();
    }

    static public CUsuario GetUsuario()
    {
        HttpContext.Current.Response.AddHeader("pragma", "no-cache");
        HttpContext.Current.Response.CacheControl = "Private";
        HttpContext.Current.Response.Expires = 0;
        CUsuario usuario = (CUsuario)HttpContext.Current.Session["Usuario"];
        if(usuario==null) {
            
            string url = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri);
            HttpContext.Current.Response.Redirect("~/Default.aspx?from=" + url);
            HttpContext.Current.Response.End();
        }
        return usuario;
    }
}
