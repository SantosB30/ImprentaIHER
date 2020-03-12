Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class ClientesController
        Inherits Controller
        ' GET: Clientes
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=IH;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=IH;Integrated Security=true;"
        Function AgregarCliente() As ActionResult
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
        Function AgregarCliente(nombreCliente As String, direccionCliente As String,
                                    telefonoCliente As String, correo As String, nacionalidad As String) As ActionResult
            Dim query = "EXEC SP_AGREGAR_CLIENTE '" + nombreCliente + "','" + direccionCliente + "','" +
                    telefonoCliente + "','" + correo + "','" + nacionalidad + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Cliente agregado"
            Return View()
        End Function
        Function EditarCliente(cliente As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim clienteEditar As String = Request.QueryString("cliente")
                    Session("clienteEditar") = clienteEditar
                    Dim query As String = "SELECT * FROM TBL_CLIENTES WHERE NOMBRE_CLIENTE='" + clienteEditar + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()

                    While lector.Read()
                        Session("clienteEditar") = lector("NOMBRE_CLIENTE").ToString()
                        Session("direccionClienteEditar") = lector("DIRECCION_CLIENTE").ToString()
                        Session("correoClienteEditar") = lector("CORREO_CLIENTE").ToString()
                        Session("telefonoClienteEditar") = lector("TELEFONO_CLIENTE").ToString()
                        Session("nacionalidadClienteEditar") = lector("NACIONALIDAD_CLIENTE").ToString()

                    End While
                    conexion.Close()
                    ViewBag.Message = "Editar Usuarios"
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function EditarCliente(nombreCliente As String, direccionCliente As String,
                                    telefonoCliente As String, correo As String, nacionalidad As String) As ActionResult
            Dim query = "EXEC SP_EDITAR_CLIENTE '" + nombreCliente + "','" + direccionCliente + "','" +
                   telefonoCliente + "','" + correo + "','" + nacionalidad + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Cliente editado"
            Return RedirectToAction("EditarClientes", "Clientes")
        End Function
        Function EditarClientes() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim query = "SELECT * FROM TBL_CLIENTES"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ClientesModel)
                    While (lector.Read())
                        Dim detalles = New ClientesModel()
                        detalles.nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                        detalles.direccionCliente = lector("DIRECCION_CLIENTE").ToString()
                        detalles.telefonoCliente = lector("TELEFONO_CLIENTE").ToString()
                        detalles.correoCliente = lector("CORREO_CLIENTE").ToString()
                        detalles.nacionalidadCliente = lector("NACIONALIDAD_CLIENTE").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos usuario"
                    Return View("EditarClientes", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function EliminarClientes() As ActionResult
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
        Function EliminarCliente() As ActionResult
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
    End Class
End Namespace