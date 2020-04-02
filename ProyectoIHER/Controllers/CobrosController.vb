Imports System.Web.Mvc
Imports System.Data.SqlClient

Imports EASendMail 'Add EASendMail Namespace

Namespace Controllers
    Public Class CobrosController
        Inherits Controller
        Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public mensaje As String = ""
        Function CobrosPendientes() As ActionResult
            If Session("accesos") <> Nothing Then

                Dim query = "SELECT o.NUMERO_ORDEN, c.NOMBRE_CLIENTE, c.TELEFONO_CLIENTE, c.CORREO_CLIENTE, a.TIPO_PAGO, a.TOTAL_COTIZACION FROM TBL_CLIENTES c, TBL_COTIZACIONES a, TBL_ORDENES_PRODUCCION o WHERE o.ID_CLIENTE=c.ID_CLIENTE AND o.NUMERO_COTIZACION=a.NUMERO_COTIZACION AND a.TIPO_PAGO='CRÉDITO'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of CobrosModel)
                While (lector.Read())
                    Dim detalles = New CobrosModel()
                    detalles.Numero_Orden = lector("NUMERO_ORDEN").ToString()
                    detalles.Nombre_Cliente = lector("NOMBRE_CLIENTE").ToString()
                    detalles.Telefono_Cliente = lector("TELEFONO_CLIENTE").ToString()
                    detalles.Correo_Cliente = lector("CORREO_CLIENTE").ToString()
                    detalles.Tipo_Pago = lector("TIPO_PAGO").ToString()
                    detalles.Total_Cotizacion = lector("TOTAL_COTIZACION").ToString()

                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Cobros"
                Return View("Cobros", model)
            Else
                Return RedirectToAction("CobrosPendientes", "Cobros")
            End If

        End Function
        ' GET: Cobros
        Function Index() As ActionResult
            Return View()
        End Function
    End Class
End Namespace