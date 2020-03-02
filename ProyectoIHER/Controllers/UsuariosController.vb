Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports EASendMail 'Add EASendMail Namespace

Namespace Controllers
    Public Class UsuariosController
        Inherits Controller
        Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER
                           ;Initial Catalog=IH;Integrated Security=true;"
        Public mensaje As String = ""
        ' GET: Usuarios
        Function CrearUsuario() As ActionResult
            If Session("accesos") <> Nothing Then
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function CrearUsuario(nombreCompleto As String, usuario As String, password As String, correo As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Try
                    Dim bitacora As Bitacora = New Bitacora()
                    Dim query As String = "EXEC SP_CREAR_USUARIO_ADMIN '" + usuario + "','" + nombreCompleto + "','" + correo + "','" + password + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Dim cuerpoCorreo = "<html><body>Hola " + nombreCompleto + "!<br>Su usuario: " + usuario + " ha sido creado, su contraseña es " + password + "<br>Saludos.</body></html>"
                    Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                    Dim respuesta As String = envioCorreo.enviarCorreo("Usuario creado exitosamente", correo, cuerpoCorreo)
                    If respuesta.Equals("Enviado") Then
                        ViewBag.Message = "Guardado"
                        bitacora.registrarBitacora(Session("usuario").ToString(), "CREACÍÓN DE USUARIO " + usuario)
                    Else
                        ViewBag.Message = "Guardado con error: " + respuesta
                        bitacora.registrarBitacora(Session("usuario").ToString(), "CREACÍÓN DE USUARIO " + usuario + ", ERROR EN ENVÍO DE CORREO")
                    End If
                    Return View()
                Catch ex As Exception
                    ViewBag.Message = ex.ToString()
                    Return View()
                End Try
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If

        End Function

        Function EditarUsuario() As ActionResult
            If Session("accesos") <> Nothing Then
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Function EliminarUsuario() As ActionResult
            If Session("accesos") <> Nothing Then
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
    End Class
End Namespace