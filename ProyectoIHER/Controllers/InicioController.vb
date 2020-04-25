Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class InicioController
        Inherits Controller
        Dim bitacora As Bitacora = New Bitacora()
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"

        ' GET: Inicio
        Function Principal() As ActionResult
            If Session("accesos") <> Nothing Then
                bitacora.registrarBitacora(Session("usuario").ToString(), "PANTALLA DE INICIO")

                Dim query = "SELECT COUNT(*) FROM TBL_MS_USUARIO"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Session("cantidadUsuarios") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM TBL_PROVEEDORES"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadProveedores") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM (TBL_ORDENES_PRODUCCION O INNER JOIN DETALLES_ORDENES_PRODUCCION D ON O.NUMERO_ORDEN=D.NUMERO_ORDEN) where O.ESTADO <>'FINALIZADA' AND D.PRIORIDAD='URGENTE'"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadOrdenesUrgente") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM (TBL_ORDENES_PRODUCCION O INNER JOIN DETALLES_ORDENES_PRODUCCION D ON O.NUMERO_ORDEN=D.NUMERO_ORDEN) where O.ESTADO <>'FINALIZADA' AND D.PRIORIDAD='NORMAL'"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadOrdenesNormales") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM TBL_CLIENTES c, TBL_COTIZACIONES a, TBL_ORDENES_PRODUCCION o WHERE o.ID_CLIENTE=c.ID_CLIENTE AND o.NUMERO_COTIZACION=a.NUMERO_COTIZACION AND a.TIPO_PAGO='CRÉDITO'"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadCobrosPendientes") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM TBL_ORDENES_PRODUCCION WHERE ESTADO <> 'FINALIZADA'"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadOrdenes") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM TBL_PRODUCTOS"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadProductos") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()

                query = "SELECT COUNT(*) FROM TBL_CLIENTES"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                Session("cantidadClientes") = Double.Parse(comando.ExecuteScalar().ToString()).ToString("#,###,##0")
                conexion.Close()
                Return View()



            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
    End Class
End Namespace