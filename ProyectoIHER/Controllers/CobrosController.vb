Imports System.Web.Mvc
Imports System.Data.SqlClient

Imports EASendMail 'Add EASendMail Namespace
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Namespace Controllers
    Public Class CobrosController
        Inherits Controller
        ' Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public mensaje As String = ""
        Dim bitacora As Bitacora = New Bitacora()

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
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A COBROS PENDIENTES")
                Return View("CobrosPendientes", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If

        End Function
        <HttpPost>
        Function CobrosPendientes(submit As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Dim dsCobros As New DsCobros()
                Dim fila As DataRow
                bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR REPORTE DE CUENTAS PENDIENTES")
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
                    fila = dsCobros.Tables("DataTable1").NewRow()
                    fila.Item("numeroOrden") = lector("NUMERO_ORDEN").ToString()
                    fila.Item("cliente") = lector("NOMBRE_CLIENTE").ToString()
                    fila.Item("telefono") = lector("TELEFONO_CLIENTE").ToString()
                    fila.Item("correo") = lector("CORREO_CLIENTE").ToString()
                    fila.Item("tipoPago") = lector("TIPO_PAGO").ToString()
                    fila.Item("total") = Decimal.Parse(lector("TOTAL_COTIZACION").ToString()).ToString("#,##0.#0")
                    dsCobros.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()

                Dim nombreArchivo As String = "Reporte de cuentas al crédito.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeCuentasPendientes.rpt"))
                crystalReport.SetDataSource(dsCobros)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
                ViewBag.Message = "Datos usuario"
                Return View("CobrosPendientes", model)
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function

    End Class
End Namespace