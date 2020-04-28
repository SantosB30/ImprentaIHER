Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Namespace Controllers
    Public Class ProductosController
        Inherits Controller
        Dim validaciones As Validaciones = New Validaciones()

        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim bitacora As Bitacora = New Bitacora()
        Function AgregarProducto() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE CREACIÓN DE PRODUCTO")
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function AgregarProducto(nombreProducto As String, descripcionProducto As String, precioProducto As String) As ActionResult
            Try
                Dim query = "EXEC SP_AGREGAR_PRODUCTO '" + validaciones.removerEspacios(nombreProducto) + "','" + validaciones.removerEspacios(descripcionProducto) + "'," +
                    validaciones.removerEspacios(precioProducto)

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Session("mensaje") = "Producto agregado"
                bitacora.registrarBitacora(Session("usuario").ToString(), "CREACIÓN DE PRODUCTO: " + nombreProducto)
                Return View()
            Catch ex As Exception
                Session("mensaje") = ex.ToString()
                Return View()
            End Try
        End Function
        Function EditarProducto(producto As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim productoEditar As String = Request.QueryString("producto")
                    Session("productoEditar") = productoEditar
                    Dim query As String = "SELECT * FROM TBL_PRODUCTOS WHERE NOMBRE_PRODUCTO='" + productoEditar + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()

                    While lector.Read()
                        Session("productoEditar") = lector("NOMBRE_PRODUCTO").ToString()
                        Session("descripcionProductoEditar") = lector("DESCRIPCION_PRODUCTO").ToString()
                        Session("precioProductoEditar") = lector("PRECIO_PRODUCTO").ToString()
                        Session("estadoProducto") = lector("ESTADO_PRODUCTO").ToString()
                    End While
                    conexion.Close()
                    ViewBag.Message = "Editar Producto"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A EDICIÓN DE PRODUCTOS: " + productoEditar)
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function EditarProducto(nombreProducto As String, descripcionProducto As String, precioProducto As String, estado As String) As ActionResult
            Dim query = "EXEC SP_EDITAR_PRODUCTO '" + validaciones.removerEspacios(nombreProducto) + "','" + validaciones.removerEspacios(descripcionProducto) + "','" +
                  validaciones.removerEspacios(precioProducto) + "','" + Session("productoEditar") + "','" + validaciones.removerEspacios(estado) + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Producto editado"
            bitacora.registrarBitacora(Session("usuario").ToString(), "EDICIÓN DE PRODUCTOS: " + nombreProducto)
            Return RedirectToAction("EditarProductos", "Productos")
        End Function
        Function EditarProductos() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim query = "SELECT * FROM TBL_PRODUCTOS"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ProductosModel)
                    While (lector.Read())
                        Dim detalles = New ProductosModel()
                        detalles.nombreProducto = lector("NOMBRE_PRODUCTO").ToString()
                        detalles.descripcionProducto = lector("DESCRIPCION_PRODUCTO").ToString()
                        detalles.precioProducto = lector("PRECIO_PRODUCTO").ToString()
                        detalles.estado = lector("ESTADO_PRODUCTO").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos producto"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A VISTA DE PRODUCTOS PARA EDICIÓN")
                    Return View("EditarProductos", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function EliminarProductos() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim query = "SELECT * FROM TBL_PRODUCTOS WHERE ESTADO_PRODUCTO='ACTIVO'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ProductosModel)
                    While (lector.Read())
                        Dim detalles = New ProductosModel()
                        detalles.nombreProducto = lector("NOMBRE_PRODUCTO").ToString()
                        detalles.descripcionProducto = lector("DESCRIPCION_PRODUCTO").ToString()
                        detalles.precioProducto = lector("PRECIO_PRODUCTO").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos producto"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A VISTA DE PRODUCTOS PARA ELIMINACIÓN")
                    Return View("EliminarProductos", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function EliminarProducto(producto As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim productoEliminar As String = Request.QueryString("producto")
                    Session("productoEliminar") = productoEliminar
                    Dim query As String = "EXEC SP_ELIMINAR_PRODUCTO '" + productoEliminar + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Session("mensaje") = "Producto eliminado"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "ELIMINACIÓN DE PRODUCTOS: " + productoEliminar)
                    Return RedirectToAction("EliminarProductos", "Productos")
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function ReporteProductos() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A REPORTE DE PRODUCTOS")
            Dim query = "SELECT * FROM TBL_PRODUCTOS WHERE ESTADO_PRODUCTO='ACTIVO'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector = comando.ExecuteReader()
            Dim model As New List(Of ProductosModel)
            While (lector.Read())
                Dim detalles = New ProductosModel()
                detalles.nombreProducto = lector("NOMBRE_PRODUCTO").ToString()
                detalles.descripcionProducto = lector("DESCRIPCION_PRODUCTO").ToString()
                detalles.precioProducto = lector("PRECIO_PRODUCTO").ToString()
                detalles.estado = lector("ESTADO_PRODUCTO").ToString()
                model.Add(detalles)
            End While
            conexion.Close()
            ViewBag.Message = "Datos producto"
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A VISTA DE PRODUCTOS PARA EDICIÓN")
            Return View("ReporteProductos", model)
        End Function
        <HttpPost>
        Function ReporteProductos(submit As String) As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR REPORTE DE PRODUCTOS")
            Dim dsProductos As New DsProductos()
            Dim fila As DataRow
            Dim query = "SELECT * FROM TBL_PRODUCTOS"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector = comando.ExecuteReader()
            Dim model As New List(Of ProductosModel)
            While (lector.Read())
                Dim detalles = New ProductosModel()
                detalles.nombreProducto = lector("NOMBRE_PRODUCTO").ToString()
                detalles.descripcionProducto = lector("DESCRIPCION_PRODUCTO").ToString()
                detalles.precioProducto = lector("PRECIO_PRODUCTO").ToString()
                detalles.estado = lector("ESTADO_PRODUCTO").ToString()
                model.Add(detalles)

                fila = dsProductos.Tables("DataTable1").NewRow()
                fila.Item("nombre") = lector("NOMBRE_PRODUCTO").ToString()
                fila.Item("descripcion") = lector("DESCRIPCION_PRODUCTO").ToString()
                fila.Item("precio") = lector("PRECIO_PRODUCTO").ToString()
                fila.Item("estado") = lector("ESTADO_PRODUCTO").ToString()
                dsProductos.Tables("DataTable1").Rows.Add(fila)
            End While
            conexion.Close()
            ViewBag.Message = "Datos producto"
            Dim nombreArchivo As String = "Reporte de productos.pdf"
            Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

            If System.IO.File.Exists(directorio) Then
                System.IO.File.Delete(directorio)
            End If
            Dim crystalReport As ReportDocument = New ReportDocument()
            crystalReport.Load(Server.MapPath("~/ReporteDeProductos.rpt"))
            crystalReport.SetDataSource(dsProductos)
            crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
            Response.ContentType = "application/octet-stream"
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
            Response.TransmitFile(directorio)
            Response.End()
            Return View("ReporteProductos", model)
        End Function
    End Class
End Namespace