Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class CuentasController
        Inherits Controller
        Dim bitacora As Bitacora = New Bitacora()
        Public cadenaConexion As String = "Data Source=(LocalDB)\SQLIHER;Initial Catalog=IH;Integrated Security=true;"
        Public mensaje As String = ""

        ' GET: Cuentas
        Function Login() As ActionResult
            Session("accesos") = ""
            Session("usuario") = ""
            Return View()
        End Function

        <HttpPost()>
        Function Login(usuario As String, contraseña As String) As ActionResult
            Dim respuesta As String = validarCredenciales(usuario, contraseña)
            If respuesta = Nothing Then
                mensaje = "¡Usuario o contraseña inválidos!"
                ViewBag.Message = mensaje
                bitacora.registrarBitacora(usuario, "INGRESO DE CREDENCIALES INCORRECTAS")
            Else
                Session("accesos") = respuesta
                Session("usuario") = usuario
                bitacora.registrarBitacora(usuario, "INICIO DE SESIÓN")
                Dim estadoUsuario As String = obtenerEstadoUsuario(Session("usuario").ToString())
                If (estadoUsuario.Equals("NUEVO")) Then
                    Return RedirectToAction("ConfirmarRegistro", "Cuentas")
                Else
                    Return RedirectToAction("Principal", "Inicio")
                End If
            End If
            Return View()
        End Function

        Public Function obtenerEstadoUsuario(usuario As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT ESTADO_USUARIO FROM TBL_MS_USUARIO
                            WHERE USUARIO='" + usuario + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("ESTADO_USUARIO").ToString()
            End While
            conexion.Close()
            Return respuesta
        End Function

        Function Registrarse() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Function Registrarse(nombre As String, correo As String, contraseña As String, usuario As String) As ActionResult
            nuevoUsuario(nombre, correo, contraseña, usuario)
            Return View()
        End Function

        Public Sub nuevoUsuario(nombre As String, correo As String, contraseña As String, usuario As String)
            Dim query = "INSERT INTO TBL_MS_USUARIO VALUES ('" + usuario + "','" + nombre + "','" + "ESTADO" + "','" +
            contraseña + "','" + ")"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            conexion.Close()
        End Sub

        Public Function validarCredenciales(usuario As String, contraseña As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT '<USUARIO>'+CAST(A.ID_USUARIO AS VARCHAR)++'</USUARIO>'+'<ROL>'+B.DESCRIPCION+'</ROL>'+
                    '<ESTADO>'+A.ESTADO_USUARIO+'</ESTADO>' RESPUESTA FROM TBL_MS_USUARIO A
                        INNER JOIN TBL_MS_ROLES B
                            ON A.ID_ROL=B.ID_ROL
                        WHERE A.USUARIO='" + usuario + "' AND A.CONTRASEÑA='" + contraseña + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("RESPUESTA").ToString()
            End While


            conexion.Close()
            Return respuesta
        End Function

        Function ConfirmarRegistro() As ActionResult
            If Session("accesos") <> Nothing Then
                Dim preguntas As New List(Of String)
                Dim query = "SELECT PREGUNTA FROM TBL_PREGUNTAS"

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector As SqlDataReader = comando.ExecuteReader()
                While lector.Read()
                    preguntas.Add(lector("PREGUNTA").ToString())
                End While
                conexion.Close()
                TempData("preguntas") = preguntas
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If

        End Function

        <HttpPost>
        Function ConfirmarRegistro(pregunta1 As String, pregunta2 As String, password1 As String, password2 As String, respuesta1 As String, respuesta2 As String) As ActionResult
            Dim preguntas As New List(Of String)
            Dim query = "EXEC SP_CONFIRMAR_REGISTRO '" + Session("usuario").ToString() + "','" + password1 + "','" + pregunta1 + "','" + pregunta2 + "','" + respuesta1 + "','" + respuesta2 + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Actualizado"
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function RecuperarContraseña() As ActionResult
            Dim preguntas As New List(Of String)
            Dim query = "SELECT PREGUNTA FROM TBL_PREGUNTAS"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                preguntas.Add(lector("PREGUNTA").ToString())
            End While
            conexion.Close()
            TempData("preguntas") = preguntas
            Return View()

        End Function

        <HttpPost>
        Function RecuperarContraseña(usuario As String, tipoRecuperacion As String, pregunta1 As String, pregunta2 As String, respuesta1 As String, respuesta2 As String) As ActionResult
            Try
                If (tipoRecuperacion.Equals("preguntas")) Then
                    ViewBag.Message = "Preguntas"
                Else
                    Dim preguntas As New List(Of String)
                    Dim query = "SELECT PREGUNTA FROM TBL_PREGUNTAS"

                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        preguntas.Add(lector("PREGUNTA").ToString())
                    End While
                    conexion.Close()
                    TempData("preguntas") = preguntas

                    query = "SELECT CORREO_ELECTRONICO,NOMBRE_USUARIO  FROM TBL_MS_USUARIO WHERE NOMBRE_USUARIO='" + usuario + "'"
                    Dim correo As String = ""
                    Dim nombre As String = ""
                    conexion = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    comando = New SqlCommand(query, conexion)
                    lector = comando.ExecuteReader()
                    While lector.Read()
                        correo = lector("CORREO_ELECTRONICO").ToString()
                        nombre = lector("NOMBRE_USUARIO").ToString()
                    End While
                    conexion.Close()

                    Dim contraseña As String = generarContraseña()
                    query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA='" + contraseña + "' WHERE NOMBRE_USUARIO='" + usuario + "'"

                    conexion = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    comando = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()

                    bitacora.registrarBitacora(usuario, "RESETEO DE CONTRASEÑA")

                    Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                    envioCorreo.enviarCorreo("Recuperación de contraseña", correo, "<html><body>Hola " + nombre + ", tu contraseña ha sido reestablecida,<br>
                tu nueva contraseña es " + contraseña + "</body></html>")
                    ViewBag.Message = "Correo"

                End If
                Return View()
            Catch ex As Exception
                ViewBag.Message = ex.ToString()
                Return View()
            End Try

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
    End Class
End Namespace