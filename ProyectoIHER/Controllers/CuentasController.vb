Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class CuentasController
        Inherits Controller
        Dim bitacora As Bitacora = New Bitacora()
        Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
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
            Dim estadoUsuario As String = obtenerEstadoUsuario(usuario)
            If respuesta = Nothing Then
                If (estadoUsuario.Equals("BLOQUEADO")) Then
                    mensaje = "¡El usuario se encuentra bloqueado!"
                    ViewBag.Message = mensaje
                ElseIf (estadoUsuario.Equals("NO EXISTE")) Then
                    mensaje = "¡No existe el usuario ingresado!"
                    ViewBag.Message = mensaje
                    bitacora.registrarBitacora(usuario, "INTENTO DE INGRESO CON USUARIO INEXISTENTE")
                Else
                    mensaje = "¡Usuario o contraseña inválidos!"
                    ViewBag.Message = mensaje
                    bitacora.registrarBitacora(usuario, "INGRESO DE CREDENCIALES INCORRECTAS")
                    validarIntentosFallidos(usuario)

                End If

            Else
                Session("accesos") = respuesta
                Session("usuario") = usuario
                bitacora.registrarBitacora(usuario, "INICIO DE SESIÓN")
                If (estadoUsuario.Equals("NUEVO")) Then
                    Return RedirectToAction("ConfirmarRegistro", "Cuentas")
                ElseIf (estadoUsuario.Equals("BLOQUEADO")) Then
                    mensaje = "¡El usuario se encuentra bloqueado!"
                    ViewBag.Message = mensaje
                    bitacora.registrarBitacora(usuario, "INTENTO DE INGRESO CON USUARIO BLOQUEADO")
                ElseIf (estadoUsuario.Equals("INACTIVO")) Then
                    mensaje = "¡El usuario se encuentra inactivo!"
                    ViewBag.Message = mensaje
                    bitacora.registrarBitacora(usuario, "INTENTO DE INGRESO CON USUARIO INACTIVO")
                ElseIf (estadoUsuario.Equals("NO EXISTE")) Then
                    mensaje = "¡No existe el usuario ingresado!"
                    ViewBag.Message = mensaje
                    bitacora.registrarBitacora(usuario, "INTENTO DE INGRESO CON USUARIO INEXISTENTE")
                Else
                    Return RedirectToAction("Principal", "Inicio")
                End If
            End If
            Return View()
        End Function

        Public Function obtenerIdUsuario(usuario As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT ID_USUARIO FROM TBL_MS_USUARIO WHERE USUARIO='" + usuario + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("ID_USUARIO").ToString()
            End While
            conexion.Close()
            Return respuesta
        End Function

        Public Sub validarIntentosFallidos(usuario As String)
            Dim idUsuario As String = obtenerIdUsuario(usuario)
            Dim intentosFallidos As Double = Nothing
            Dim query = "SELECT COUNT(*) INTENTOS FROM TBL_BITACORA
	        WHERE CAST(FECHA AS DATETIME) BETWEEN DATEADD(HOUR,-1,GETDATE()) AND GETDATE()
		AND ACCION='INGRESO DE CREDENCIALES INCORRECTAS' AND USUARIO=" + idUsuario

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                intentosFallidos = Double.Parse(lector("INTENTOS").ToString())
            End While
            conexion.Close()

            If (intentosFallidos >= 3) Then
                query = "UPDATE TBL_MS_USUARIO 
                        SET ESTADO_USUARIO='BLOQUEADO'
                        WHERE ID_USUARIO=" + idUsuario
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
            End If
        End Sub

        Public Function obtenerEstadoUsuario(usuario As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT ISNULL(ESTADO_USUARIO,'NO EXISTE')  ESTADO_USUARIO FROM TBL_MS_USUARIO
                            WHERE USUARIO='" + usuario + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("ESTADO_USUARIO").ToString()
            End While
            conexion.Close()
            If respuesta = Nothing Then
                respuesta = "NO EXISTE"
            End If
            Return respuesta
        End Function

        Function Registrarse() As ActionResult
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

        <HttpPost()>
        Function Registrarse(nombre As String, correo As String, contraseña As String, usuario As String, pregunta1 As String,
                respuesta1 As String, pregunta2 As String, respuesta2 As String) As ActionResult
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

            Dim validar As Validaciones = New Validaciones()
            Dim usuarioExistente As Double = Double.Parse(validar.validarExistenciaUsuario(usuario))

            If (usuarioExistente = 0) Then
                nuevoUsuario(nombre, correo, contraseña, usuario, pregunta1, respuesta1, pregunta2, respuesta2)
                Session("mensaje") = "Registrado correctamente"
                Dim cuerpoCorreo = "<html><body>Hola " + nombre + "!<br>Le damos la bienvenida a nuestro sistema de Imprenta-IHER, con estos datos podrá ingresar al sistema:<br>Su usuario: " + usuario + " <br>Su contraseña: " + contraseña + "<br>Saludos.</body></html>"
                Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                Dim respuesta As String = envioCorreo.enviarCorreo("Bienvenido(a)", correo, cuerpoCorreo)
                If respuesta.Equals("Enviado") Then
                    ViewBag.Message = "Guardado"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "CREACÍÓN DE USUARIO " + usuario)
                Else
                    ViewBag.Message = "Guardado con error: " + respuesta
                    bitacora.registrarBitacora(Session("usuario").ToString(), "CREACÍÓN DE USUARIO " + usuario + ", ERROR EN ENVÍO DE CORREO")
                End If
                Return RedirectToAction("Login", "Cuentas")
            Else
                Session("mensaje") = "Usuario ya existente"
                Return View()
            End If
        End Function
        Public Function obtenerIdPregunta(pregunta As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT ID_PREGUNTA FROM TBL_PREGUNTAS
                            WHERE PREGUNTA='" + pregunta + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("ID_PREGUNTA").ToString()
            End While
            conexion.Close()
            Return respuesta
        End Function

        Public Sub nuevoUsuario(nombre As String, correo As String, contraseña As String, usuario As String, pregunta1 As String,
            respuesta1 As String, pregunta2 As String, respuesta2 As String)
            Dim query = "INSERT INTO TBL_MS_USUARIO VALUES ('" + usuario + "','" + nombre + "','INACTIVO','" +
            contraseña + "',2,NULL,NULL,NULL,NULL,'" + correo + "','" + usuario + "',GETDATE(),NULL,NULL" + ")"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()

            Dim idPregunta1 As String = obtenerIdPregunta(pregunta1)
            Dim idPregunta2 As String = obtenerIdPregunta(pregunta2)
            Dim idUsuario As String = obtenerIdUsuario(usuario)

            query = "INSERT INTO TBL_MS_PREGUNTAS_USUARIO VALUES (" + idPregunta1 + "," + idUsuario + ",'" + respuesta1 + "','" + usuario + "',GETDATE(),NULL,NULL)"
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()

            query = "INSERT INTO TBL_MS_PREGUNTAS_USUARIO VALUES (" + idPregunta2 + "," + idUsuario + ",'" + respuesta2 + "','" + usuario + "',GETDATE(),NULL,NULL)"
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
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
                    Dim idPregunta1 = obtenerIdPregunta(pregunta1)
                    Dim idPregunta2 = obtenerIdPregunta(pregunta2)
                    Dim cantidadPreguntasCorrectas As Double = 0
                    Dim idUsuario = obtenerIdUsuario(usuario)
                    Dim query = "SELECT SUM(P.CANTIDAD) CANTIDAD FROM (SELECT COUNT(*) CANTIDAD,'PREGUNTA 1' PREGUNTA FROM TBL_MS_PREGUNTAS_USUARIO
                                WHERE ID_USUARIO=" + idUsuario + " AND ID_PREGUNTA=" + idPregunta1 + "
                                    UNION
                                 SELECT COUNT(*) CANTIDAD,'PREGUNTA 2' PREGUNTA FROM TBL_MS_PREGUNTAS_USUARIO
                                WHERE ID_USUARIO=" + idUsuario + " AND ID_PREGUNTA=" + idPregunta2 + ") P"

                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        cantidadPreguntasCorrectas = Double.Parse(lector("CANTIDAD").ToString())
                    End While
                    conexion.Close()
                    ' PREGUNTAS '
                    Dim preguntas As New List(Of String)
                    query = "SELECT PREGUNTA FROM TBL_PREGUNTAS"

                    conexion = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    comando = New SqlCommand(query, conexion)
                    lector = comando.ExecuteReader()
                    While lector.Read()
                        preguntas.Add(lector("PREGUNTA").ToString())
                    End While
                    conexion.Close()
                    TempData("preguntas") = preguntas
                    ' -------- ''
                    If (cantidadPreguntasCorrectas = 2) Then
                        Dim nuevaContraseña = generarContraseña()
                        query = "SELECT CORREO_ELECTRONICO,NOMBRE_USUARIO  FROM TBL_MS_USUARIO WHERE USUARIO='" + usuario + "'"
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
                        query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA='" + nuevaContraseña + "' WHERE USUARIO='" + usuario + "'"

                        conexion = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        comando = New SqlCommand(query, conexion)
                        comando.ExecuteNonQuery()
                        conexion.Close()

                        bitacora.registrarBitacora(usuario, "RESETEO DE CONTRASEÑA")
                        Session("mensaje") = "Nueva contraseña " + nuevaContraseña
                        ViewBag.Message = "Password temporal generada: " + nuevaContraseña
                        Return View()
                    Else
                        ViewBag.Message = "Respuestas incorrectas"
                        Return View()
                    End If

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

                    query = "SELECT CORREO_ELECTRONICO,NOMBRE_USUARIO  FROM TBL_MS_USUARIO WHERE USUARIO='" + usuario + "'"
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
                    query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA='" + contraseña + "' WHERE USUARIO='" + usuario + "'"

                    conexion = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    comando = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()

                    bitacora.registrarBitacora(usuario, "RESETEO DE CONTRASEÑA")

                    Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                    envioCorreo.enviarCorreo("Recuperación de contraseña", correo, "<html><body>Hola " + nombre + ", hemos contraseña ha sido restablecida,<br>
                su nueva contraseña es: " + contraseña + "<br>Saludos!</body></html>")
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