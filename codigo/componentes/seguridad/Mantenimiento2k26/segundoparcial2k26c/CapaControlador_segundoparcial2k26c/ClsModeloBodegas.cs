
using CapaModelo_Seguridad.Entidades;
using CapaModelo_Seguridad.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaControlador_Seguridad
{
    public class ClsModeloBodegas
    {
        private int _Idbodega;
    private string _Nombrebodega;
    private bool _IsActive;

    private ClsRepositorioBodegas _Repositorio;
    private List<ClsModeloBodegas> _Lista;

    public EstadoEntidad Estado { private get; set; }

    public int Idbodega          { get => _Idbodega;          set => _Idbodega = value; }
public string Nombrebodega      { get => _Nombrebodega; set => _Nombrebodega = value; }
public bool IsActive { get => _IsActive; set => _IsActive = value; }

public ClsModeloBodegas ()
        {
    _Repositorio = new ClsRepositorioBodegas();
}

public string SeguridadMetGrabarCambios()
{
    string Mensaje = null;
    try
    {
        var Datos = new ClsBodega
        {
                    Idbodega          = _Idbodega,
                    Nombrebodega      = _Nombrebodega,
                    IsActive             = _IsActive
        };

        switch (Estado)
        {
            case EstadoEntidad.Added:
                _Repositorio.SeguridadMetAgregar(Datos);
                ClsModeloBitacora.SeguridadMetRegistrarAccion("INSERT", "[tblTabla]", Datos.Idbodega, "Se agregó: " + _Nombrebodega);
                Mensaje = "Grabacion exitosa";
                break;
            case EstadoEntidad.Modified:
                _Repositorio.SeguridadMetEditar(Datos);
                ClsModeloBitacora.SeguridadMetRegistrarAccion("UPDATE", "[tblTabla]", Datos.Idbodega, "Se actualizó: " + _Nombrebodega);
                Mensaje = "Actualizacion exitosa";
                break;
            case EstadoEntidad.Deleted:
                _Repositorio.SeguridadMetRemover(Datos);
                ClsModeloBitacora.SeguridadMetRegistrarAccion("DELETE", "[tblTabla]", Datos.Idbodega, "Se eliminó ID: " + Datos.Idbodega);
                Mensaje = "Eliminacion exitosa";
                break;
        }
    }
    catch (OdbcException Ex)
    {
        bool esLlaveForanea = false;
        bool esDuplicado = false;
        bool esDemasiado = false;

        foreach (OdbcError error in Ex.Errors)
        {
            if (error.NativeError == 1451 || error.NativeError == 1452) esLlaveForanea = true;
            else if (error.NativeError == 1062) esDuplicado = true;
            else if (error.NativeError == 1406) esDemasiado = true;
        }

        if (esLlaveForanea) Mensaje = "No se puede eliminar porque tiene relacion en otro modulo.";
        else if (esDuplicado) Mensaje = "Ya existe un registro con esos datos.";
        else if (esDemasiado) Mensaje = "Uno de los campos excede la longitud permitida.";
        else Mensaje = "Ocurrio un problema al procesar la solicitud.";
    }
    catch (Exception)
    {
        Mensaje = "Ocurrio un error inesperado. Intente nuevamente.";
    }
    return Mensaje;
}

public List<ClsModeloBodegas> SeguridadMetObtenerTodos()
{
    var Resultado = _Repositorio.SeguridadMetObtenerTodos();
    _Lista = new List<ClsModeloBodegas>();
    foreach (ClsBodega Item in Resultado)
            {
        _Lista.Add(new ClsModeloBodegas
        {
                    _Idbodega          = Item.Idbodega,
                    _Nombrebodega      = Item.Nombrebodega,
                    _IsActive             = Item.IsActive
        });
    }
    return _Lista;
}

    }
}
