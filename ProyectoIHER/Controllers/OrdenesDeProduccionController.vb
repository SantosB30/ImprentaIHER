Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class OrdenesDeProduccionController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        ' GET: OrdenesDeProduccion
        Function VerOrdenes() As ActionResult
            Dim query As String = ""
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector = comando.ExecuteReader()
            Dim model As New List(Of OrdenesModel)
            While (lector.Read())
                Dim detalles = New OrdenesModel()
                detalles.fechaCreacion = lector("FECHA_CREACION").ToString()
                detalles.numeroCotizacion = lector("NUMERO_COTIZACION").ToString()
                detalles.nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                detalles.numeroOrden = lector("NUMERO_ORDEN").ToString()
                detalles.estadoCotizacion = lector("ESTADO_ORDEN").ToString()
                model.Add(detalles)
            End While
            conexion.Close()
            ViewBag.Message = "Datos cotizacion"
            Return View("BuscarCotizaciones", model)
        End Function
    End Class
End Namespace