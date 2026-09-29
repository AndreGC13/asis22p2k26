using CapaModelo_Seguridad.Contratos;
using CapaModelo_Seguridad.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Seguridad.Repositorios
{
    public class ClsRepositorioBodegas : ClsSentencias, IRepositorioGenerico<ClsBodega>
    {
        private readonly string _SelectAll;
    private readonly string _Insert;
    private readonly string _Update;
    private readonly string _Delete;
    private readonly string _SelectReporte;

    public ClsRepositorioBodegas ()
        {
            _SelectAll = "SELECT idbodega, nombrebodega, is_active FROM tblbodegas";

            _Insert = "INSERT INTO tblbodegas (nombrebodega, is_active) VALUES (?, ?, ?)";

            _Update = "UPDATE tblbodegas SET nombrebodega=?, is_active=? WHERE idbodega=?";

            _Delete = "DELETE FROM tblbodegas WHERE idbodega=?";

            // Aliases con mayuscula inicial para que coincidan con el RDLC
            _SelectReporte = "SELECT idbodega AS idbodega, nombrebodega AS nombrebodega, CAST(is_active AS UNSIGNED) AS IsActive FROM tblbodegas";
        }

public int SeguridadMetAgregar(ClsBodega Entidad)
{
    var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("nombrebodega",      OdbcType.VarChar) { Value = Entidad.Nombrebodega },
                new OdbcParameter("is_active",            OdbcType.Bit)     { Value = Entidad.IsActive }
            };
    return SeguridadMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
}

public int SeguridadMetEditar(ClsBodega Entidad)
{
    var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("nombrebodega",      OdbcType.VarChar) { Value = Entidad.Nombrebodega },
                new OdbcParameter("is_active",            OdbcType.Bit)     { Value = Entidad.IsActive },
                new OdbcParameter("idbodega",            OdbcType.Int)     { Value = Entidad.Idbodega }
            };
    return SeguridadMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
}

public int SeguridadMetRemover(ClsBodega Entidad)
{
    var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("idbodega", OdbcType.Int) { Value = Entidad.Idbodega }
            };
    return SeguridadMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
}

public IEnumerable<ClsBodega> SeguridadMetObtenerTodos()
{
    var Tabla = SeguridadMetEjecucionConsulta(_SelectAll, CommandType.Text);
    var Lista = new List<ClsBodega>();
    foreach (DataRow Fila in Tabla.Rows)
    {
        Lista.Add(new ClsBodega
        {
                    Idbodega          = (int)Fila["idbodega"],
                    Nombrebodega      = Fila["nombrebodega"].ToString(),
                    IsActive = (Fila["is_active"].ToString() == "1")
        });
    }
    return Lista;
}

// Metodo extra para el ReportViewer (DataTable con aliases)
public DataTable SeguridadMetObtenerReporte()
{
    return SeguridadMetEjecucionConsulta(_SelectReporte, CommandType.Text);
}
    }
}

