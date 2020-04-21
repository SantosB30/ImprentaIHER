Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports EASendMail 'Add EASendMail Namespace
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Namespace Controllers
    Public Class UsuariosController
        Inherits Controller
        Dim validaciones As Validaciones = New Validaciones()
        Dim bitacora As Bitacora = New Bitacora()

        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public mensaje As String = ""
        ' GET: Usuarios
        Function CrearUsuario() As ActionResult
            If Session("accesos") <> Nothing Then
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE CREACIÓN DE USUARIO")
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function CrearUsuario(nombreCompleto As String, usuario As String, password As String, correo As String, rol As String) As ActionResult
            If Session("accesos") <> Nothing Then

                Dim validar As Validaciones = New Validaciones()
                Dim usuarioExistente As Double = Double.Parse(validar.validarExistenciaUsuario(usuario))

                If (usuarioExistente = 0) Then
                    Try
                        Dim bitacora As Bitacora = New Bitacora()
                        Dim query As String = "EXEC SP_CREAR_USUARIO_ADMIN '" + validaciones.removerEspacios(usuario) + "','" + validaciones.removerEspacios(nombreCompleto) + "','" + validaciones.removerEspacios(correo) + "','" + validaciones.removerEspacios(password) + "','" + validaciones.removerEspacios(rol) + "'"
                        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        Dim comando As SqlCommand = New SqlCommand(query, conexion)
                        comando.ExecuteNonQuery()
                        conexion.Close()
                        Dim cuerpoCorreo = "<html><body>Hola " + nombreCompleto + "!<br>Le damos la bienvenida a nuestro sistema de Imprenta-IHER, con estos datos podrá ingresar al sistema:<br>Su usuario: " + usuario + " <br>Su contraseña: " + password + "<br>Saludos.</body></html>"
                        Dim envioCorreo As EnvioCorreo = New EnvioCorreo()
                        Dim respuesta As String = envioCorreo.enviarCorreo("Bienvenido(a)", correo, cuerpoCorreo)
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
                    ViewBag.Message = "Usuario ya existe"
                    Return View()
                End If
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
                    detalles.fechaModificacion = lector("FECHA_MODIFICACION").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A VISTA PARA EDICIÓN DE USUARIOS")
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
                    detalles.fechaModificacion = lector("FECHA_MODIFICACION").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A VISTA PARA ELIMINACIÓN DE USUARIOS")
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
                bitacora.registrarBitacora(Session("usuario"), "ENVÍO DE CORREO CON REESTABLECIMIENTO DE CONTRASEÑA, USUARIO: " + usuario)
                Return RedirectToAction("EditarUsuario", "Usuarios")
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
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


        Function EditarUsuarios(usuario As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Session("usuarioEditar") = ""
                Session("nombreusuarioEditar") = ""
                Session("correoUsuarioEditar") = ""
                Session("estadoUsuarioEditar") = ""
                Dim usuarioEditar As String = Request.QueryString("usuario")
                Session("usuarioEditar") = usuarioEditar

                Dim query As String = "SELECT A.USUARIO,A.NOMBRE_USUARIO,A.ESTADO_USUARIO,
                    A.CORREO_ELECTRONICO,B.ROL FROM TBL_MS_USUARIO A
                        INNER JOIN TBL_MS_ROLES B
                                ON A.ID_ROL=B.ID_ROL WHERE USUARIO='" + usuarioEditar + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector As SqlDataReader = comando.ExecuteReader()

                While lector.Read()
                    Session("usuarioEditar") = lector("USUARIO").ToString()
                    Session("nombreusuarioEditar") = lector("NOMBRE_USUARIO").ToString()
                    Session("correoUsuarioEditar") = lector("CORREO_ELECTRONICO").ToString()
                    Session("estadoUsuarioEditar") = lector("ESTADO_USUARIO").ToString()
                    Session("rolUsuarioEditar") = lector("ROL").ToString()
                End While
                conexion.Close()
                ViewBag.Message = "Editar Usuarios"
                bitacora.registrarBitacora(Session("usuario"), "VISTA PARA EDICIÓN DE USUARIO: " + usuario)
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function EditarUsuarios(usuarioEditar As String, nombreCompleto As String, correo As String, usuario As String, estado As String, rol As String) As ActionResult
            Dim bitacora As Bitacora = New Bitacora()
            If Session("accesos") <> Nothing Then
                Try
                    Dim query As String = "EXEC SP_ACTUALIZAR_USUARIO_ADMIN '" + Session("usuarioEditar") + "','" + validaciones.removerEspacios(usuario) + "','" + validaciones.removerEspacios(nombreCompleto) + "','" + validaciones.removerEspacios(correo) + "','" + validaciones.removerEspacios(estado) + "','" + validaciones.removerEspacios(rol) + "'"
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
                    Session("rolUsuarioEditar") = Nothing
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
                bitacora.registrarBitacora(Session("usuario"), "INGRESO MÓDULO DE APROBACIÓN DE USUARIOS")
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

                Dim query As String = "UPDATE TBL_MS_USUARIO SET ESTADO_USUARIO='ACTIVO' WHERE USUARIO='" + usuarioEditar + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                ViewBag.Message = "Usuario aprobado"
                Session("mensaje") = "Usuario aprobado"
                Session("usuarioAsignarRol") = usuario
                bitacora.registrarBitacora(Session("usuario"), "APROBACIÓN DE USUARIO:  " + usuarioEditar)
                Return RedirectToAction("AsignarRolUsuario", "Usuarios")
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Function AsignarRolUsuario() As ActionResult
            bitacora.registrarBitacora(Session("usuario"), "INGRESO A MÓDULO DE ASIGNACIÓN DE ROL USUARIO")
            Return View()
        End Function

        <HttpPost>
        Function AsignarRolUsuario(rol As String) As ActionResult
            Dim query = "EXEC SP_ASIGNAR_ROL '" + Session("usuarioAsignarRol") + "','" + rol + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            ViewBag.Message = "Usuario aprobado"
            Session("mensaje") = "Usuario aprobado"
            bitacora.registrarBitacora(Session("usuario"), "ASIGNACIÓN DE ROL USUARIO: " + Session("usuarioAsignarRol") + ", ROL:" + rol)
            Return RedirectToAction("AprobarUsuario", "Usuarios")
        End Function

        Function BitacoraUsuario() As ActionResult
            Return View()
        End Function
        <HttpPost>
        Function BitacoraUsuario(submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            If Session("accesos") <> Nothing Then
                Dim query = "SELECT u.NOMBRE_USUARIO, b.ACCION, CONVERT(NVARCHAR,b.FECHA,103) +' ' + CONVERT(NVARCHAR,b.FECHA,108) FECHA
                        FROM TBL_MS_USUARIO u
                                INNER JOIN TBL_BITACORA b
                                ON u.ID_USUARIO=b.USUARIO"
                If date1 <> Nothing Then
                    query = query + "  WHERE CAST(FECHA AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                            "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
                query = query + " ORDER BY CAST(FECHA AS DATETIME) ASC"

                If submit.Equals("generar") Then
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
                    ViewBag.Message = "Datos bitácora"
                    bitacora.registrarBitacora(Session("usuario"), "BITÁCORA DE USUARIOS")
                    Return View("BitacoraUsuario", model)
                Else
                    bitacora.registrarBitacora(Session("usuario"), "EXPORTAR BITÁCORA DE USUARIOS")
                    Dim dsBitacora As New DsBitacora()
                    Dim fila As DataRow
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ReporteUsuariosModel)
                    While (lector.Read())
                        fila = dsBitacora.Tables("DataTable1").NewRow()
                        fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                        fila.Item("accion") = lector("ACCION").ToString()
                        fila.Item("fecha") = lector("FECHA").ToString()
                        dsBitacora.Tables("DataTable1").Rows.Add(fila)
                    End While
                    conexion.Close()
                    Dim nombreArchivo As String = "Bitácora de usuarios.pdf"
                    Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                    If System.IO.File.Exists(directorio) Then
                        System.IO.File.Delete(directorio)
                    End If
                    Dim crystalReport As ReportDocument = New ReportDocument()
                    crystalReport.Load(Server.MapPath("~/ReporteBitacora.rpt"))
                    crystalReport.SetDataSource(dsBitacora)
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                    Response.ContentType = "application/octet-stream"
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                    Response.TransmitFile(directorio)
                    Response.End()
                    Return View()
                End If
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
                bitacora.registrarBitacora(Session("usuario"), "PARÁMETROS DE SISTEMA")
                Return View("Parametros", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function Parametros(submit As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Dim query = "SELECT PARAMETRO, VALOR FROM TBL_MS_PARAMETROS"
                bitacora.registrarBitacora(Session("usuario"), "EXPORTAR PARÁMETROS DE SISTEMA")
                Dim dsParametros As New DsParametros()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    fila = dsParametros.Tables("DataTable1").NewRow()
                    fila.Item("parametro") = lector("PARAMETRO").ToString()
                    fila.Item("valor") = lector("VALOR").ToString()
                    dsParametros.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "Parámetros.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteParámetros.rpt"))
                crystalReport.SetDataSource(dsParametros)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()


                Session("ParametroEditar") = Nothing
                Session("ValorEditar") = Nothing
                query = "SELECT PARAMETRO, VALOR FROM TBL_MS_PARAMETROS"
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                comando = New SqlCommand(query, conexion)
                lector = comando.ExecuteReader()
                Dim model As New List(Of UsuariosModel)
                While (lector.Read())
                    Dim detalles = New UsuariosModel()
                    detalles.Parametro = lector("PARAMETRO").ToString()
                    detalles.Valor = lector("VALOR").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos Parametros"
                bitacora.registrarBitacora(Session("usuario"), "PARÁMETROS DE SISTEMA")
                Return View("Parametros", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function



        Function EditarParametros(Parametro As String) As ActionResult
            If Session("accesos") <> Nothing Then

                Session("ParametroEditar") = ""
                Session("ValorEditar") = ""

                Dim ParametroEditar As String = Request.QueryString("Parametro")
                Session("ParametroEditar") = ParametroEditar

                Dim query As String = "SELECT PARAMETRO,VALOR
                                       FROM TBL_MS_PARAMETROS WHERE PARAMETRO='" + ParametroEditar + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector As SqlDataReader = comando.ExecuteReader()

                While lector.Read()

                    Session("ParametroEditar") = lector("PARAMETRO").ToString()
                    Session("ValorEditar") = lector("VALOR").ToString()
                End While
                conexion.Close()
                ViewBag.Message = "Editar Parametros"
                bitacora.registrarBitacora(Session("usuario"), "INGRESO EDICIÓN DE PARÁMETROS DE SISTEMA")
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function EditarParametros(Parametro As String, Valor As String) As ActionResult
            Dim bitacora As Bitacora = New Bitacora()
            If Session("accesos") <> Nothing Then
                Try
                    If Valor <> Nothing And Not Valor = "0" Then
                        Dim query As String = "EXEC SP_ACTUALIZAR_PARAMETRO_ADMIN '" + validaciones.removerEspacios(Parametro) + "','" + validaciones.removerEspacios(Valor) + "'"
                        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        Dim comando As SqlCommand = New SqlCommand(query, conexion)
                        comando.ExecuteNonQuery()
                        conexion.Close()
                        Session("mensaje") = "Parametro editado"
                        ViewBag.Message = "Parametro editado"
                        bitacora.registrarBitacora(Session("usuario"), "EDICIÓN DE PARÁMETRO: " + Session("ParametroEditar"))
                        Session("ParametroEditar") = Nothing
                        Session("ValorEditar") = Nothing
                        Return RedirectToAction("Parametros", "Usuarios")
                    Else
                        Session("mensaje") = "Parametro incorrecto"
                        ViewBag.Message = "Parametro incorrecto"
                        Return RedirectToAction("Parametros", "Usuarios")
                    End If
                Catch ex As Exception
                    Session("mensaje") = ex.ToString()
                    bitacora.registrarBitacora(Session("usuario"), "ERROR EN EDICIÓN DE PARÁMETRO: " + Session("ParametroEditar"))
                    Return RedirectToAction("Parametros", "Usuarios")
                End Try

            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        Function ReporteUsuarios() As ActionResult
            bitacora.registrarBitacora(Session("usuario"), "INGRESO MÓDULO DE REPORTE DE USUARIOS")
            Return View()
        End Function
        <HttpPost>
        Function ReporteUsuarios(submit As String, fecha As String, estado As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            Dim query = "SELECT USUARIO,ESTADO_USUARIO,FECHA_CREACION,FECHA_MODIFICACION,CREADO_POR,NOMBRE_USUARIO
                    FROM TBL_MS_USUARIO "
            Dim campoFecha = Nothing
            If fecha.Equals("FECHA DE CREACIÓN") Then
                campoFecha = "FECHA_CREACION"
            ElseIf fecha.Equals("FECHA DE MODIFICACIÓN") Then
                campoFecha = "FECHA_MODIFICACION"
            End If

            If Not estado.Equals("TODOS") Then
                query = query + "WHERE ESTADO_USUARIO='" + estado + "'"
                If campoFecha <> Nothing And date1 <> Nothing And date2 <> Nothing Then
                    query = query + " AND " + campoFecha + " BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
            Else
                If campoFecha <> Nothing And date1 <> Nothing And date2 <> Nothing Then
                    query = query + " WHERE " + campoFecha + " BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
            End If





            If submit.Equals("generar") Then
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of ReporteUsuariosModel)
                While (lector.Read())
                    Dim detalles = New ReporteUsuariosModel()
                    detalles.usuario = lector("USUARIO").ToString()
                    detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.estado = lector("ESTADO_USUARIO").ToString()
                    detalles.fechaCreacion = lector("FECHA_CREACION").ToString()
                    detalles.fechaModificacion = lector("FECHA_MODIFICACION").ToString()
                    detalles.usuarioCreacion = lector("CREADO_POR").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE USUARIOS EN PANTALLA")
                Return View("ReporteUsuarios", model)
            Else
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE USUARIOS EN PDF")
                Dim dsUsuarios As New DsUsuarios()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of ReporteUsuariosModel)
                While (lector.Read())
                    fila = dsUsuarios.Tables("DataTable1").NewRow()
                    fila.Item("usuario") = lector("USUARIO").ToString()
                    fila.Item("nombreUsuario") = lector("NOMBRE_USUARIO").ToString()
                    fila.Item("estado") = lector("ESTADO_USUARIO").ToString()
                    fila.Item("fechaCreacion") = lector("FECHA_CREACION").ToString()
                    fila.Item("fechaModificacion") = lector("FECHA_MODIFICACION").ToString()
                    fila.Item("creadoPor") = lector("CREADO_POR").ToString()
                    dsUsuarios.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "ReporteDeUsuarios.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeUsuarios.rpt"))
                crystalReport.SetDataSource(dsUsuarios)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
                Return View()
            End If
        End Function
        Function SeleccionarUsuarioGestionPermisos() As ActionResult
            bitacora.registrarBitacora(Session("usuario"), "INGRESO A SELECCIÓN DE USUARIO PARA ASIGNACIÓN DE PERMISOS")
            Dim usuarios As New List(Of String)
            Dim listadoProductos As String = ""
            Dim query = "SELECT USUARIO FROM TBL_MS_USUARIO"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                usuarios.Add(lector("USUARIO").ToString())
            End While
            conexion.Close()
            TempData("usuarios") = usuarios
            Return View()
        End Function
        <HttpPost>
        Function SeleccionarUsuarioGestionPermisos(usuario As String) As ActionResult
            Session("usuarioPermisos") = usuario
            Return RedirectToAction("GestionPermisos", "Usuarios")
        End Function

        Function GestionPermisos() As ActionResult
            Dim query = "SELECT *,LOWER(REPLACE(MODULO,' ','_')+'_'+REPLACE(SECCION,' ','_')) NOMBRE_CAMPO FROM TBL_ACCESOS"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(Query, conexion)
            Dim lector = comando.ExecuteReader()
            Dim model As New List(Of AccesosModel)
            While (lector.Read())
                Dim detalles = New AccesosModel()
                detalles.modulo = lector("MODULO").ToString()
                detalles.seccion = lector("SECCION").ToString()
                detalles.acceso = lector("CODIGO").ToString()
                detalles.campo = lector("NOMBRE_CAMPO").ToString()
                model.Add(detalles)
            End While
            conexion.Close()
            ViewBag.Message = "Datos usuario"
            bitacora.registrarBitacora(Session("usuario"), "INGRESO A GESTIÓN DE PERMISOS")

            query = "SELECT A.* FROM TBL_MS_PERMISOS_USUARIOS A
	            INNER JOIN TBL_MS_USUARIO B
		            ON A.ID_USUARIO=B.ID_USUARIO WHERE B.USUARIO='" + Session("usuarioPermisos") + "'"

            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            lector = comando.ExecuteReader()
            While (lector.Read())
                Session("permisos") = lector("PERMISOS").ToString()
            End While
            conexion.Close()
            Return View("GestionPermisos", model)
        End Function
        <HttpPost>
        Function GestionPermisos(submit As String,
                                 gestion_de_usuarios_crear_usuario As String,
                                    gestion_de_usuarios_editar_usuario As String,
                                    gestion_de_usuarios_eliminar_usuario As String,
                                    gestion_de_usuarios_aprobar_usuario As String,
                                    gestion_de_usuarios_reporte_de_usuarios As String,
                                    seguridad_bitácora_de_usuarios As String,
                                    seguridad_parámetros As String,
                                    seguridad_respaldo_bd As String,
                                    seguridad_restaurar_bd As String,
                                    clientes_agregar_cliente As String,
                                    clientes_editar_cliente As String,
                                    clientes_eliminar_cliente As String,
                                    clientes_reporte_de_clientes As String,
                                    proveedores_agregar_proveedor As String,
                                    proveedores_editar_proveedor As String,
                                    proveedores_eliminar_proveedor As String,
                                    proveedores_reporte_de_proveedores As String,
                                    productos_agregar_producto As String,
                                    productos_editar_producto As String,
                                    productos_eliminar_producto As String,
                                    productos_reporte_de_productos As String,
                                    cobros_cobros_pendientes As String,
                                    cotizaciones_nueva_cotizacion As String,
                                    buscar_cotizaciones_enviar_a_produccion As String,
                                    buscar_cotizaciones_editar As String,
                                    buscar_cotizaciones_ver As String,
                                    buscar_cotizaciones_eliminar As String,
                                    ordenes_de_produccion_ver_ordenes As String,
                                    ordenes_de_produccion_editar_orden As String,
                                    ordenes_de_produccion_reporte_de_ordenes As String,
                                    ordenes_de_produccion_ver_orden As String,
                                    ordenes_de_produccion_reporte_de_bodega As String,
                                    ordenes_de_produccion_reporte_de_inventario As String,
                                    bodega_gestion_de_inventario As String,
                                  bodega_inventario As String) As ActionResult

            Dim query As String = "EXEC PERMISOS_USUARIO '" + gestion_de_usuarios_crear_usuario + "','" +
                                    gestion_de_usuarios_editar_usuario + "','" +
                                    gestion_de_usuarios_eliminar_usuario + "','" +
                                    gestion_de_usuarios_aprobar_usuario + "','" +
                                    gestion_de_usuarios_reporte_de_usuarios + "','" +
                                    seguridad_bitácora_de_usuarios + "','" +
                                    seguridad_parámetros + "','" +
                                    seguridad_respaldo_bd + "','" +
                                    seguridad_restaurar_bd + "','" +
                                    clientes_agregar_cliente + "','" +
                                    clientes_editar_cliente + "','" +
                                    clientes_eliminar_cliente + "','" +
                                    clientes_reporte_de_clientes + "','" +
                                    proveedores_agregar_proveedor + "','" +
                                    proveedores_editar_proveedor + "','" +
                                    proveedores_eliminar_proveedor + "','" +
                                    proveedores_reporte_de_proveedores + "','" +
                                    productos_agregar_producto + "','" +
                                    productos_editar_producto + "','" +
                                    productos_eliminar_producto + "','" +
                                    productos_reporte_de_productos + "','" +
                                    cobros_cobros_pendientes + "','" +
                                    cotizaciones_nueva_cotizacion + "','" +
                                    buscar_cotizaciones_enviar_a_produccion + "','" +
                                    buscar_cotizaciones_editar + "','" +
                                    buscar_cotizaciones_ver + "','" +
                                    buscar_cotizaciones_eliminar + "','" +
                                    ordenes_de_produccion_ver_ordenes + "','" +
                                    ordenes_de_produccion_editar_orden + "','" +
                                    ordenes_de_produccion_reporte_de_ordenes + "','" +
                                    ordenes_de_produccion_ver_orden + "','" +
                                    ordenes_de_produccion_reporte_de_bodega + "','" +
                                    ordenes_de_produccion_reporte_de_inventario + "','" +
                                    bodega_gestion_de_inventario + "','" +
                                    bodega_inventario + "','" +
                                    Session("usuarioPermisos").ToString() + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Permisos actualizados"
            Return RedirectToAction("Principal", "Inicio")
        End Function
    End Class
End Namespace