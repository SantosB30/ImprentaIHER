Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports EASendMail 'Add EASendMail Namespace

Namespace Controllers
    Public Class UsuariosController
        Inherits Controller
        Public cadenaConexion As String = "Data Source=(LocalDB)\SQLIHER;Initial Catalog=IH;Integrated Security=true;"
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
                Session("usuarioEditar") = Nothing
                Session("nombreusuarioEditar") = Nothing
                Session("correoUsuarioEditar") = Nothing
                Session("estadoUsuarioEditar") = Nothing
                Dim query = "SELECT * FROM TBL_MS_USUARIO WHERE ID_ROL<>1"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of UsuariosModel)
                While (lector.Read())
                    Dim detalles = New UsuariosModel()
                    detalles.usuario = lector("USUARIO").ToString()
                    detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.estado = lector("ESTADO_USUARIO").ToString()
                    detalles.contraseña = lector("CONTRASEÑA").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                Return View("EditarUsuario", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Function EliminarUsuario() As ActionResult
            If Session("accesos") <> Nothing Then
                Session("usuarioEditar") = Nothing
                Session("nombreusuarioEditar") = Nothing
                Session("correoUsuarioEditar") = Nothing
                Session("estadoUsuarioEditar") = Nothing
                Dim query = "SELECT * FROM TBL_MS_USUARIO WHERE ID_ROL<>1"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of UsuariosModel)
                While (lector.Read())
                    Dim detalles = New UsuariosModel()
                    detalles.usuario = lector("USUARIO").ToString()
                    detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.estado = lector("ESTADO_USUARIO").ToString()
                    detalles.contraseña = lector("CONTRASEÑA").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                Return View("EliminarUsuario", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function RestablecerContraseña(usuario As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Dim usuarioR As String = Request.QueryString("usuario")

                Dim query As String = "SELECT CORREO_ELECTRONICO,NOMBRE_USUARIO  FROM TBL_MS_USUARIO WHERE USUARIO='" + usuarioR + "'"
                Dim correo As String = ""
                Dim nombre As String = ""
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector As SqlDataReader = comando.ExecuteReader()

                While lector.Read()
                    correo = lector("CORREO_ELECTRONICO").ToString()
                    nombre = lector("NOMBRE_USUARIO").ToString()
                End While
                conexion.Close()

                Dim contraseña As String = generarContraseña()
                query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA='" + contraseña + "' WHERE USUARIO='" + usuarioR + "'"

                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Dim bitacora As Bitacora = New Bitacora()
                bitacora.registrarBitacora(Session("usuario"), "REESTABLECIMIENTO DE CONTRASEÑA, USUARIO: " + usuario)

                Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                envioCorreo.enviarCorreo("Recuperación de contraseña", correo, "<html><body>Hola " + nombre + ", tu contraseña ha sido restablecida,<br>
                tu nueva contraseña es " + contraseña + "</body></html>")
                Session("mensaje") = "Password restablecida"
                ViewBag.Message = "Password restablecida"
                Return RedirectToAction("EditarUsuario", "Usuarios")
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Public Function generarContraseña() As String
            Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            Dim r As New Random
            Dim sb As New StringBuilder
            For i As Integer = 1 To 8
                Dim idx As Integer = r.Next(0, 35)
                sb.Append(s.Substring(idx, 1))
            Next
            Return sb.ToString()
        End Function


        Function EditarUsuarios(usuario As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Session("usuarioEditar") = ""
                Session("nombreusuarioEditar") = ""
                Session("correoUsuarioEditar") = ""
                Session("estadoUsuarioEditar") = ""
                Dim usuarioEditar As String = Request.QueryString("usuario")
                Session("usuarioEditar") = usuarioEditar

                Dim query As String = "SELECT USUARIO,NOMBRE_USUARIO,ESTADO_USUARIO,
                    CORREO_ELECTRONICO FROM TBL_MS_USUARIO WHERE USUARIO='" + usuarioEditar + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector As SqlDataReader = comando.ExecuteReader()

                While lector.Read()
                    Session("usuarioEditar") = lector("USUARIO").ToString()
                    Session("nombreusuarioEditar") = lector("NOMBRE_USUARIO").ToString()
                    Session("correoUsuarioEditar") = lector("CORREO_ELECTRONICO").ToString()
                    Session("estadoUsuarioEditar") = lector("ESTADO_USUARIO").ToString()
                End While
                conexion.Close()
                ViewBag.Message = "Editar Usuarios"

                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function EditarUsuarios(usuarioEditar As String, nombreCompleto As String, correo As String, usuario As String, estado As String) As ActionResult
            Dim bitacora As Bitacora = New Bitacora()
            If Session("accesos") <> Nothing Then
                Try
                    Dim query As String = "EXEC SP_ACTUALIZAR_USUARIO_ADMIN '" + Session("usuarioEditar") + "','" + usuario + "','" + nombreCompleto + "','" + correo + "','" + estado + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Session("mensaje") = "Usuario editado"
                    ViewBag.Message = "Usuario editado"
                    bitacora.registrarBitacora(Session("usuario"), "EDICIÓN DE USUARIO: " + Session("usuarioEditar"))
                    Session("usuarioEditar") = Nothing
                    Session("nombreusuarioEditar") = Nothing
                    Session("correoUsuarioEditar") = Nothing
                    Session("estadoUsuarioEditar") = Nothing
                    Return RedirectToAction("EditarUsuario", "Usuarios")
                Catch ex As Exception
                    Session("mensaje") = ex.ToString()
                    bitacora.registrarBitacora(Session("usuario"), "ERROR EN EDICIÓN DE USUARIO: " + Session("usuarioEditar"))
                    Return RedirectToAction("EditarUsuario", "Usuarios")
                End Try

            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Function EliminarUsuarios(usuario As String) As ActionResult
            Dim bitacora As Bitacora = New Bitacora()
            If Session("accesos") <> Nothing Then
                Session("usuarioEliminar") = ""
                Dim usuarioEliminar As String = Request.QueryString("usuario")
                Session("usuarioEliminar") = usuarioEliminar
                Dim query As String = "UPDATE TBL_MS_USUARIO SET ESTADO_USUARIO='INACTIVO' WHERE USUARIO='" + usuarioEliminar + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                ViewBag.Message = "Usuario eliminado"
                Session("mensaje") = "Usuario eliminado"
                bitacora.registrarBitacora(Session("usuario"), "ELIMINACIÓN DE USUARIO: " + Session("usuarioEliminar"))
                Return RedirectToAction("EliminarUsuario", "Usuarios")
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function AprobarUsuario() As ActionResult
            If Session("accesos") <> Nothing Then
                Session("usuarioEditar") = Nothing
                Session("nombreusuarioEditar") = Nothing
                Session("correoUsuarioEditar") = Nothing
                Session("estadoUsuarioEditar") = Nothing
                Dim query = "SELECT * FROM TBL_MS_USUARIO WHERE ID_ROL<>1 AND ESTADO_USUARIO='INACTIVO'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of UsuariosModel)
                While (lector.Read())
                    Dim detalles = New UsuariosModel()
                    detalles.usuario = lector("USUARIO").ToString()
                    detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.estado = lector("ESTADO_USUARIO").ToString()
                    detalles.contraseña = lector("CONTRASEÑA").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                Return View("AprobarUsuario", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Function AprobarUsuarios(usuario As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Session("usuarioEditar") = ""
                Session("nombreusuarioEditar") = ""
                Session("correoUsuarioEditar") = ""
                Session("estadoUsuarioEditar") = ""
                Dim usuarioEditar As String = Request.QueryString("usuario")
                Session("usuarioEditar") = usuarioEditar

                Dim query As String = "UPDATE TBL_MS_USUARIO SET ESTADO_USUARIO='ACTIVO' WHERE USUARIO='" + usuario + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                ViewBag.Message = "Usuario aprobado"
                Session("mensaje") = "Usuario aprobado"
                Return RedirectToAction("AprobarUsuario", "Usuarios")
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function BitacoraUsuario() As ActionResult
            If Session("accesos") <> Nothing Then

                Dim query = "SELECT u.NOMBRE_USUARIO, b.ACCION, b.FECHA FROM TBL_MS_USUARIO u, TBL_BITACORA b WHERE u.ID_USUARIO=b.USUARIO"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of UsuariosModel)
                While (lector.Read())
                    Dim detalles = New UsuariosModel()
                    detalles.Fecha = lector("FECHA").ToString()
                    detalles.usuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.Accion = lector("ACCION").ToString()


                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                Return View("BitacoraUsuario", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If

        End Function
        Function Parametros() As ActionResult
            If Session("accesos") <> Nothing Then
                Session("ParametroEditar") = Nothing
                Session("ValorEditar") = Nothing
                Dim query = "SELECT PARAMETRO, VALOR FROM TBL_MS_PARAMETROS"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of UsuariosModel)
                While (lector.Read())
                    Dim detalles = New UsuariosModel()
                    detalles.Parametro = lector("PARAMETRO").ToString()
                    detalles.Valor = lector("VALOR").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos Parametros"
                Return View("Parametros", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function






    End Class
End Namespace