using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using MySql.Data.MySqlClient;

public class CTablaSQL
{
    protected CConector m_pConector;
    protected string m_szCurrentID;
    protected string m_szQuery;
    protected DataTable m_pTabla;
    protected string m_szId;
    protected string m_szId2;
    protected CQuery m_pQuery;
    protected static string m_szClassName = "CTablaSQL";
    protected ArrayList m_nNumeros;

	public CTablaSQL()
	{
        m_pConector = CConector.GetConector();
        m_szCurrentID = "";
        m_szQuery = "";
        m_szId = "";
        m_szId2 = "";
        m_pTabla = null;
        m_pQuery = null;
        m_nNumeros = new ArrayList();
	}

    public CTablaSQL(string tabla)
    {
        m_pConector = CConector.GetConector();
        m_szCurrentID = "";
        m_szQuery = "";
        m_szId = "";
        m_szId2 = "";
        m_pTabla = null;
        m_pQuery = new CQuery(tabla);
        m_nNumeros = new ArrayList();
        Clean();
    }

    public void SetID(string id) {
        m_szId = id;
    }
    public void SetID2(string id) {
        m_szId2 = id;
    }
    public CConector GetConector() {
        return m_pConector;
    }
    public CQuery GetQuery() {
        return m_pQuery;
    }
    public void SetQuery(string query) {
        m_szQuery = query;
    }
    public void SetHidden(int columna) {
        m_nNumeros.Add(columna);
    }
    public bool EsVisible(int columna)
    {
        for(int i=0; i<m_nNumeros.Count; i++) {
            if (columna == (int)m_nNumeros[i])
                return false;
        }
        return true;
    }
    virtual protected int GetColumns(MySqlDataReader reader)
    {
        if (reader == null || m_pTabla == null)
            return 0;
        for (int i = 0; i < reader.FieldCount; i++)
        {
            m_pTabla.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
        }
        if (m_szId2.Length==0)
            m_pTabla.PrimaryKey = new DataColumn[] { m_szId.Length==0 ? m_pTabla.Columns[reader.GetName(0)] : m_pTabla.Columns[m_szId]};
        else m_pTabla.PrimaryKey = new DataColumn[] { m_pTabla.Columns[m_szId], m_pTabla.Columns[m_szId2] };
        return reader.FieldCount;
    }
    virtual protected DataTable GetDataTable()
    {
        CTablaSQL tabla = (CTablaSQL)HttpContext.Current.Session[m_szClassName];
        m_szCurrentID = HttpContext.Current.Session[m_szId] == null ? "" : HttpContext.Current.Session[m_szId].ToString();
        if (tabla != null && tabla!=this)
        {
            return tabla.GetDataTable();
        }
        if (m_szCurrentID == null || m_szCurrentID == "")
        {
            m_szCurrentID = "0";
        }
        m_pTabla = new DataTable();
        MySqlDataReader reader = m_pConector.Query(m_szQuery.Replace("%d", m_szCurrentID));
        if (reader != null)
        {
            GetColumns(reader);
            object[] todos = new object[reader.FieldCount];
            while (reader.Read())
            {
                reader.GetValues(todos);
                m_pTabla.Rows.Add(todos);
            }
        } else {
            HttpContext.Current.Response.Write(m_pConector.GetLastError());
        }
        HttpContext.Current.Session[m_szClassName] = this;
        return m_pTabla;
    }
    virtual public void RebuilTable()
    {
        CTablaSQL tabla=GetTabla(m_pQuery.GetTabla());
        tabla.m_pTabla = null;
    }
    virtual public void Clean() {
        HttpContext.Current.Session[m_szClassName] = null;
    }
    virtual public int GetNumero()
    {
        DataTable tabla = GetTabla();
        int numero = 1;
        if (tabla != null)
            numero = tabla.Rows.Count + 1;
        return numero;
    }
    virtual public DataTable GetTabla()
    {
        CTablaSQL tabla = (CTablaSQL)HttpContext.Current.Session[m_szClassName];
        if (tabla == null)
            return null;
        return tabla.m_pTabla;
    }
    virtual public DataTable Select()
    {
        return GetDataTable();
    }
    public static CTablaSQL GetTabla(string szTabla) {
        CTablaSQL tabla = (CTablaSQL)HttpContext.Current.Session[m_szClassName];
        if (tabla==null)
            tabla = new CTablaSQL(szTabla);
        return tabla;
    }
    public virtual int FillGrilla(GridView grid)
    {
        return FillGrilla(grid, true);
    }
    public virtual int FillGrilla(GridView grid, bool columnas) {

        if (m_pTabla == null)
            Select();
        if (columnas)
        {
            for (int i = 0; i < m_pTabla.Columns.Count; i++)
            {

                if (EsVisible(i))
                {
                    BoundField data = new BoundField();
                    data.DataField = data.SortExpression = data.HeaderText = m_pTabla.Columns[i].Caption;
                    data.Visible = EsVisible(i);
                    grid.Columns.Add(data);
                }
            }
        }
        if (m_szId2.Length == 0)
            grid.DataKeyNames = new string[] { m_szId };
        else grid.DataKeyNames = new string[] { m_szId, m_szId2};
        grid.DataBind();
        return m_pTabla.Columns.Count;
    }
}
