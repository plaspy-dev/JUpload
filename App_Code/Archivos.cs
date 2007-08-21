using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

/// <summary>
/// Summary description for Archivos
/// </summary>
public class Archivos
{
	public Archivos()
	{
		
	}

    private DataTable GetDataTable()
    {
        DataTable dt = HttpContext.Current.Session["archivos"] as DataTable;
        if (dt == null)
        {
            dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("nombre", typeof(string));
            dt.Columns.Add("tipo", typeof(string));
            dt.Columns.Add("archivo", typeof(byte []));
            dt.Columns.Add("tamano", typeof(string));
            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
            HttpContext.Current.Session["archivos"] = dt;
        }
        return dt;
    }

    public static void RemoveDataTable()
    {
        HttpContext.Current.Session.Remove("archivos");
    }

    public DataTable Select()
    {
        return GetDataTable();
    }

    public int GetCount() {
        DataTable dt = GetDataTable();
        return dt.Rows.Count;
    }

    public void Add(string nombre, string tipo, byte []archivo, string tamano)
    {
        DataTable dt = GetDataTable();
        dt.Rows.Add(new object[] { dt.Rows.Count + 1, nombre, tipo, archivo, tamano});
    }

    public void Delete(int id)
    {
        DataTable dt = GetDataTable();
        DataRow dr = dt.Rows.Find(id);
        dr.Delete();
    }

    public string SaveFiles(string nombre) {

        DataTable dt = GetDataTable();
        string mensaje="";

        for(int i=0; i<dt.Rows.Count; i++) {

            DataRow row= dt.Rows[i];
            // guardar en el sistema.
            byte[] archivo = (byte[])row["archivo"];
            string ruta = CSettings.m_szRuta + nombre + "/" + row["nombre"];
            File.Delete(ruta);
            BinaryWriter writer = new BinaryWriter(File.Open(ruta, FileMode.OpenOrCreate));
            writer.Write(archivo);
            writer.Close();
            mensaje = row["nombre"] + "\t" + row["tamano"] + "\r\n";
        }
        RemoveDataTable();
        return mensaje;
    }
}
