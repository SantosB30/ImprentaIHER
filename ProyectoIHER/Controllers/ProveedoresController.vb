Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class ProveedoresController
        Inherits Controller
        Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=IH;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=IH;Integrated Security=true;"
        Function AgregarProveedor() As ActionResult
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
        Function AgregarProveedor(nombreProveedor As String, direccionProveedor As String,
                                    telefonoProveedor As String, correoProveedor As String,
                                nombreContactoProveedor As String, telefonoContactoProveedor As String) As ActionResult
            Try
                Dim query = "EXEC SP_AGREGAR_PROVEEDOR '" + nombreProveedor + "','" + direccionProveedor + "','" +
                    telefonoProveedor + "','" + correoProveedor + "','" + nombreContactoProveedor + "','" +
                    telefonoContactoProveedor + "'"

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Session("mensaje") = "Proveedor agregado"
                Return View()
            Catch ex As Exception
                Session("mensaje") = ex.ToString()
                Return View()
            End Try
        End Function
        Function EditarProveedor(proveedor As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim proveedorEditar As String = Request.QueryString("proveedor")
                    Session("proveedorEditar") = proveedorEditar
                    Dim query As String = "SELECT * FROM TBL_PROVEEDORES WHERE NOMBRE_PROVEEDOR='" + proveedorEditar + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()

                    While lector.Read()
                        Session("proveedorEditar") = lector("NOMBRE_PROVEEDOR").ToString()
                        Session("direccionProveedor") = lector("DIRECCION_PROVEEDOR").ToString()
                        Session("correoProveedor") = lector("CORREO_PROVEEDOR").ToString()
                        Session("telefonoProveedor") = lector("TELEFONO_PROVEEDOR").ToString()
                        Session("nombreContactoProveedor") = lector("NOMBRE_CONTACTO").ToString()
                        Session("telefonoContactoProveedor") = lector("TELEFONO_CONTACTO").ToString()
                    End While
                    conexion.Close()
                    ViewBag.Message = "Editar Proveedor"
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function EditarProveedor(nombreProveedor As String, direccionProveedor As String,
                                    telefonoProveedor As String, correoProveedor As String,
                                nombreContactoProveedor As String, telefonoContactoProveedor As String) As ActionResult
            Dim query = "EXEC SP_EDITAR_PROVEEDOR '" + nombreProveedor + "','" + direccionProveedor + "','" +
                  telefonoProveedor + "','" + correoProveedor + "','" + nombreContactoProveedor + "','" + telefonoContactoProveedor + "','" + Session("proveedorEditar") + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Proveedor editado"
            Return RedirectToAction("EditarProveedores", "Proveedores")
        End Function

        Function EditarProveedores() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim query = "SELECT * FROM TBL_PROVEEDORES"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ProveedoresModel)
                    While (lector.Read())
                        Dim detalles = New ProveedoresModel()
                        detalles.nombreProveedor = lector("NOMBRE_PROVEEDOR").ToString()
                        detalles.direccionProveedor = lector("DIRECCION_PROVEEDOR").ToString()
                        detalles.telefonoProveedor = lector("TELEFONO_PROVEEDOR").ToString()
                        detalles.correoProveedor = lector("CORREO_PROVEEDOR").ToString()
                        detalles.nombreContactoProveedor = lector("NOMBRE_CONTACTO").ToString()
                        detalles.telefonoContactoProveedor = lector("TELEFONO_CONTACTO").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos proveedor"
                    Return View("EditarProveedores", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function EliminarProveedores() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim query = "SELECT * FROM TBL_PROVEEDORES"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ProveedoresModel)
                    While (lector.Read())
                        Dim detalles = New ProveedoresModel()
                        detalles.nombreProveedor = lector("NOMBRE_PROVEEDOR").ToString()
                        detalles.direccionProveedor = lector("DIRECCION_PROVEEDOR").ToString()
                        detalles.telefonoProveedor = lector("TELEFONO_PROVEEDOR").ToString()
                        detalles.correoProveedor = lector("CORREO_PROVEEDOR").ToString()
                        detalles.nombreContactoProveedor = lector("NOMBRE_CONTACTO").ToString()
                        detalles.telefonoContactoProveedor = lector("TELEFONO_CONTACTO").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos proveedor"
                    Return View("EliminarProveedores", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function EliminarProveedor(proveedor As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim proveedorEliminar As String = Request.QueryString("proveedor")
                    Session("proveedorEliminar") = proveedorEliminar
                    Dim query As String = "EXEC SP_ELIMINAR_PROVEEDOR '" + proveedor + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Session("mensaje") = "Proveedor eliminado"
                    Return RedirectToAction("EliminarProveedores", "Proveedores")
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