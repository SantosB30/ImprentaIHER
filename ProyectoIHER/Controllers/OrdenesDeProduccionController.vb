Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class OrdenesDeProduccionController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        ' GET: OrdenesDeProduccion
        Function VerOrdenes() As ActionResult
            Dim query As String = "SELECT A.*,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO"
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
                detalles.estadoOrden = lector("ESTADO_ORDEN").ToString()
                model.Add(detalles)
            End While
            conexion.Close()
            ViewBag.Message = "Datos cotizacion"
            Return View("VerOrdenes", model)
        End Function

        Function VerOrden(numeroOrden As String) As ActionResult
            Dim numOrden As String = Request.QueryString("numeroOrden")
            Session("numeroOrden") = numOrden


            Return View()
        End Function
    End Class
End Namespace