Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class ProductosController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Function AgregarProducto() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
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
                Dim query = "EXEC SP_AGREGAR_PRODUCTO '" + nombreProducto + "','" + descripcionProducto + "'," +
                    precioProducto

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Session("mensaje") = "Producto agregado"
                Return View()
            Catch ex As Exception
                Session("mensaje") = ex.ToString()
                Return View()
            End Try
        End Function
        Function EditarProducto(producto As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
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
                    End While
                    conexion.Close()
                    ViewBag.Message = "Editar Producto"
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function EditarProducto(nombreProducto As String, descripcionProducto As String, precioProducto As String) As ActionResult
            Dim query = "EXEC SP_EDITAR_PRODUCTO '" + nombreProducto + "','" + descripcionProducto + "','" +
                  precioProducto + "','" + Session("productoEditar") + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Producto editado"
            Return RedirectToAction("EditarProductos", "Productos")
        End Function
        Function EditarProductos() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
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
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos producto"
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
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
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
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos producto"
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
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim productoEliminar As String = Request.QueryString("producto")
                    Session("productoEliminar") = productoEliminar
                    Dim query As String = "EXEC SP_ELIMINAR_PRODUCTO '" + productoEliminar + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Session("mensaje") = "Producto eliminado"
                    Return RedirectToAction("EliminarProductos", "Productos")
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
    End Class
End Namespace