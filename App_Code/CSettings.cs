using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CSettings
/// </summary>
public class CSettings
{
    public static string m_szURL="https://service.example.invalid";
    public static string m_szConnection = "Server=CONFIGURAR_EN_ENTORNO;Database=CONFIGURAR_EN_ENTORNO;User ID=CONFIGURAR_EN_ENTORNO;Password=CONFIGURAR_EN_ENTORNO;Pooling=false";
    // public static string m_szRuta = "/tmp/";
    public static string m_szRuta = "/home/ftp/";
    public static string m_szEmailNotificacion = "demo@example.invalid";
    public static string m_szEmailFrom = "Dao Digital <demo@example.invalid>";
    public static string m_szSMTPServer = "localhost";
    public static string m_szFirma = "\r\n\r\nAtentamente Dao Digital\r\nhttps://service.example.invalid";
}
