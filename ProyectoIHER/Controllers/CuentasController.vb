Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class CuentasController
        Inherits Controller
        Dim validaciones As Validaciones = New Validaciones()
        Dim bitacora As Bitacora = New Bitacora()
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=ImprentaIHER;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public mensaje As String = ""

        ' GET: Cuentas
        Function Login() As ActionResult
            Session("accesos") = ""
            Session("usuario") = ""
            Session("permisos") = ""
            Session("parametros") = ""

            Return View()
        End Function

        <HttpPost()>
        Function Login(usuario As String, contraseña As String) As ActionResult
            Dim respuesta As String = validarCredenciales(usuario, contraseña)
            Dim estadoUsuario As String = obtenerEstadoUsuario(usuario)
            Dim permisosUsuario As String = obtenerPermisosUsuario(usuario)
            Session("adminIntentos") = parametrosAplicacion("ADMIN_INTENTOS")
            Session("adminPreguntas") = parametrosAplicacion("ADMIN_PREGUNTAS")

            If respuesta = Nothing Then
                If (estadoUsuario.Equals("BLOQUEADO")) Then
                    mensaje = "¡El usuario se encuentra bloqueado!"
                    ViewBag.Message = mensaje
                    bitacora.registrarBitacora(usuario, "INTENTO DE INGRESO CON USUARIO BLOQUEADO")
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
                Session("permisos") = permisosUsuario
                bitacora.registrarBitacora(usuario, "INICIO DE SESIÓN")
                If (estadoUsuario.Equals("NUEVO")) Then
                    bitacora.registrarBitacora(usuario, "INICIO DE SESIÓN - REDIRIGIDO A CONFIRMAR REGISTRO")
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
                    bitacora.registrarBitacora(usuario, "INICIO DE SESIÓN")

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

        Public Function obtenerPermisosUsuario(usuario As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT A.* FROM TBL_MS_PERMISOS_USUARIOS A
	            INNER JOIN TBL_MS_USUARIO B
		            ON A.ID_USUARIO=B.ID_USUARIO
			            WHERE B.USUARIO='" + usuario + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("PERMISOS").ToString()
            End While
            conexion.Close()
            Return respuesta
        End Function
        Public Function parametrosAplicacion(parametro As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT * FROM TBL_MS_PARAMETROS WHERE PARAMETRO='" + parametro + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                respuesta = lector("VALOR").ToString()
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

            If (intentosFallidos >= Double.Parse(Session("adminIntentos").ToString())) Then
                query = "UPDATE TBL_MS_USUARIO 
                        SET ESTADO_USUARIO='BLOQUEADO'
                        WHERE ID_USUARIO=" + idUsuario
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                bitacora.registrarBitacora(usuario, "USUARIO BLOQUEADO")

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
                Session("Estado_Usuario") = respuesta
            End While
            conexion.Close()
            If respuesta = Nothing Then
                respuesta = "NO EXISTE"
            End If
            Return respuesta
        End Function

        Function Registrarse() As ActionResult
            bitacora.registrarBitacora("N/D", "INGRESO A MÓDULO DE REGISTRO")

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
                respuesta1 As String, pregunta2 As String, respuesta2 As String, Telefono As String) As ActionResult
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
            Dim usuarioExistente As Double = Double.Parse(validar.validarExistenciaUsuario(usuario, correo))

            If (usuarioExistente = 0) Then
                nuevoUsuario(validaciones.removerEspacios(nombre), validaciones.removerEspacios(correo), validaciones.removerEspacios(contraseña),
                                                                     validaciones.removerEspacios(usuario), validaciones.removerEspacios(Telefono), validaciones.removerEspacios(pregunta1), validaciones.removerEspacios(respuesta1), validaciones.removerEspacios(pregunta2), validaciones.removerEspacios(respuesta2))
                Session("mensaje") = "Registrado correctamente"
                Dim cuerpoCorreo = "<html><body>Hola " + nombre + "!<br>Le damos la bienvenida al sistema Imprenta-IHER, con estos datos podrá ingresar al sistema:<br>Usuario: " + usuario + " <br>Contraseña: " + contraseña + "<br>Ingrese a www.iher.hn para utilizar el sistema<br>Cualquier consulta puede escribirnos a iher90@hotmail.com o llamar al (504) 2237-9356, (504) 2220-6657</body></html>"
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
                bitacora.registrarBitacora(Session("usuario").ToString(), "INTENTO FALLIDO DE CREAR USUARIO: USUARIO YA EXISTENTE")
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

        Public Sub nuevoUsuario(nombre As String, correo As String, contraseña As String, usuario As String, Telefono As String, pregunta1 As String,
            respuesta1 As String, pregunta2 As String, respuesta2 As String)
            Dim query = "INSERT INTO TBL_MS_USUARIO VALUES ('" + usuario + "','" + nombre + "','INACTIVO','" +
            contraseña + "',2,NULL,NULL,NULL,NULL,'" + correo + "','" + usuario + "',GETDATE(),NULL,NULL,'" + Telefono + "'" + ")"
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

            bitacora.registrarBitacora(Session("usuario").ToString(), "CREACIÓN DE USUARIO " + nombre)

        End Sub

        Public Function validarCredenciales(usuario As String, contraseña As String) As String
            Dim respuesta As String = Nothing
            Dim query = "SELECT '<USUARIO>'+CAST(A.ID_USUARIO AS VARCHAR)++'</USUARIO>'+'<ROL>'+B.DESCRIPCION+'</ROL>'+
                    '<ESTADO>'+A.ESTADO_USUARIO+'</ESTADO>' RESPUESTA FROM TBL_MS_USUARIO A
                        INNER JOIN TBL_MS_ROLES B
                            ON A.ID_ROL=B.ID_ROL
                        WHERE A.USUARIO='" + usuario + "' AND A.CONTRASEÑA=HASHBYTES('SHA2_512', '" + contraseña + "')"

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
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE CONFIRMACIÓN DE REGISTRO")

                Return View()

            Else
                Return RedirectToAction("Login", "Cuentas")
            End If

        End Function

        <HttpPost>
        Function ConfirmarRegistro(pregunta1 As String, pregunta2 As String, password1 As String, password2 As String, respuesta1 As String, respuesta2 As String) As ActionResult

            If Session("accesos") <> Nothing Then
                Dim preguntas As New List(Of String)
                Dim query = "EXEC SP_CONFIRMAR_REGISTRO '" + Session("usuario").ToString() + "','" + password1 + "','" + pregunta1 + "','" + pregunta2 + "','" + validaciones.removerEspacios(respuesta1) + "','" + validaciones.removerEspacios(respuesta2) + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()

                bitacora.registrarBitacora(Session("usuario").ToString(), "CONFIRMACIÓN DE REGISTRO")
                Session("mensaje") = "Confirmar registro"
                Return RedirectToAction("Login", "Cuentas")
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function

        Function RecuperarContraseña() As ActionResult
            Dim preguntas As New List(Of String)
            Dim query = "SELECT PREGUNTA FROM TBL_PREGUNTAS"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            If lector.HasRows() Then
                While lector.Read()
                    preguntas.Add(lector("PREGUNTA").ToString())
                End While
            Else
                preguntas.Add("NO SE ENCONTRARON REGISTROS EN LA BASE DE DATOS")
            End If
            conexion.Close()
            TempData("preguntas") = preguntas
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE RECUPERACIÓN DE CONTRASEÑA")

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
                                WHERE ID_USUARIO=" + idUsuario + " AND ID_PREGUNTA=" + idPregunta1 + " AND RESPUESTA='" + respuesta1 + "'
                                    UNION
                                 SELECT COUNT(*) CANTIDAD,'PREGUNTA 2' PREGUNTA FROM TBL_MS_PREGUNTAS_USUARIO
                                WHERE ID_USUARIO=" + idUsuario + " AND ID_PREGUNTA=" + idPregunta2 + " AND RESPUESTA='" + respuesta2 + "') P"

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
                        query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA=HASHBYTES('SHA2_512', '" + nuevaContraseña + "') WHERE USUARIO='" + usuario + "'"

                        conexion = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        comando = New SqlCommand(query, conexion)
                        comando.ExecuteNonQuery()
                        conexion.Close()

                        bitacora.registrarBitacora(usuario, "RESETEO DE CONTRASEÑA")
                        Session("mensaje") = "Nueva contraseña " + nuevaContraseña
                        'ViewBag.Message = "Password temporal generada: " + nuevaContraseña
                        Dim envioCorreo As New EnvioCorreo
                        envioCorreo.enviarCorreo("Contraseña restablecida", correo,
                                                 "<html><body>Hola " + nombre + "<br>Su contraseña ha sido restablecida,
                                                 <br>Su nueva contraseña es: " + contraseña +
                                                 "<br>Saludos.</body></html>")

                        bitacora.registrarBitacora(Session("usuario").ToString(), "RECUPERACIÓN DE CONTRASEÑA MEDIANTE PREGUNTAS SECRETAS")
                        Return View()
                    Else
                        ViewBag.Message = "Respuestas incorrectas"
                        bitacora.registrarBitacora(Session("usuario").ToString(), "RECUPERACIÓN FALLIDA DE CONTRASEÑA MEDIANTE PREGUNTAS SECRETAS: RESPUESTAS INCORRECTAS")
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
                    query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA=HASHBYTES('SHA2_512','" + contraseña + "') WHERE USUARIO='" + usuario + "'"

                    conexion = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    comando = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()

                    bitacora.registrarBitacora(usuario, "RESETEO DE CONTRASEÑA")

                    Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                    envioCorreo.enviarCorreo("Recuperación de contraseña", correo, "<html><body>Hola " + nombre + ", su contraseña ha sido restablecida,<br>
                su nueva contraseña es: " + contraseña + "<br>Ingrese a www.iher.hn para utilizar el sistema<br>Cualquier consulta puede escribirnos a iher90@hotmail.com o llamar al (504) 2237-9356, (504) 2220-6657</body></html>")
                    ViewBag.Message = "Correo"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "RECUPERACIÓN DE CONTRASEÑA MEDIANTE CORREO ELECTRÓNICO")
                End If
                Return View()
            Catch ex As Exception
                ViewBag.Message = ex.ToString()
                Return View()
            End Try

        End Function
        Function CambiarContraseña() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE CAMBIO DE CONTRASEÑA")
            Return View()
        End Function
        Public Function validarContraseña(usuario As String, contraseña As String) As Integer
            Dim respuesta As Integer = 0
            Dim query = "SELECT COUNT(*) FROM TBL_MS_USUARIO WHERE USUARIO='" + usuario + "' AND CONTRASEÑA=HASHBYTES('SHA2_512','" + contraseña + "')"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            respuesta = Convert.ToInt32(comando.ExecuteScalar().ToString())
            conexion.Close()
            Return respuesta
        End Function

        <HttpPost>
        Function CambiarContraseña(contraseña As String, contraseñaAnterior As String, confirmacontraseña As String) As ActionResult
            Dim contraseñaValida = validarContraseña(Session("usuario").ToString(), contraseñaAnterior)
            If contraseñaValida > 0 Then
                Dim query = "UPDATE TBL_MS_USUARIO SET CONTRASEÑA=HASHBYTES('SHA2_512','" + contraseña + "') WHERE USUARIO='" + Session("usuario").ToString() + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Session("mensaje") = "Contraseña actualizada"
                bitacora.registrarBitacora(Session("usuario").ToString(), "CAMBIO DE CONTRASEÑA")
                Return View()
            Else
                bitacora.registrarBitacora(Session("usuario").ToString(), "CAMBIO FALLIDO DE CONTRASEÑA: CONTRASEÑA INCORRECTA")
                Session("mensaje") = "Contraseña incorrecta"
                Return View()
            End If

            Return View()
        End Function
        Public Function generarContraseña() As String
            Dim letras As String = "abcdefghijklmnopqrstuvwxyz"
            Dim numeros As String = "0123456789"
            Dim caracteres As String = "!#$%&()=?¡*-"
            Dim r As New Random
            Dim sb As New StringBuilder
            For i As Integer = 1 To 1
                Dim idx As Integer = r.Next(0, 11)
                sb.Append(caracteres.Substring(idx, 1))
            Next
            For i As Integer = 1 To 1
                Dim idx As Integer = r.Next(0, 25)
                sb.Append(letras.Substring(idx, 1).ToUpper())
            Next
            For i As Integer = 1 To 8
                Dim idx As Integer = r.Next(0, 25)
                sb.Append(letras.Substring(idx, 1))
            Next
            For i As Integer = 1 To 1
                Dim idx As Integer = r.Next(0, 9)
                sb.Append(numeros.Substring(idx, 1))
            Next
            Return sb.ToString()
        End Function

        Function AgregarRoles() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A CREACIÓN DE ROLES")
            Return View()
        End Function

        <HttpPost>
        Function AgregarRoles(rol As String, descripcion As String) As ActionResult
            Dim query As String
            query = "INSERT INTO TBL_MS_ROLES VALUES ('" + rol + "','" + descripcion + "','" + Session("usuario").ToString() + "',GETDATE(),'" + Session("usuario").ToString() + "',GETDATE(),'<2801><2802><2803><2804><2805><2806><2807><2808><2809><2810><2811><2812><2813><2814><2815><2816><2817><2818><2819><2820><2821><2822><2823><2824><2825><2826><2827><2828><2829><2830><2831><2832><2833><2834><2835>')"
            Dim conexion As New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            bitacora.registrarBitacora(Session("usuario").ToString(), "CREACIÓN DE ROL")
            Session("mensaje") = "Rol agregado"
            Return View()
        End Function
    End Class
End Namespace