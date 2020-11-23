Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Namespace Controllers
    Public Class SeguridadController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=ImprentaIHER;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim bitacora As Bitacora = New Bitacora()

        ' GET: Seguridad
        Function RespaldoBDD() As ActionResult

            If Session("accesos") <> Nothing Then
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE RESPALDO BDD")
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function RespaldoBDD(submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            If Session("accesos") <> Nothing Then
                ''REALIZANDO RESPALDO
                Try
                    If submit.Equals("respaldo") Then
                        Dim nombreArchivo As String = "Imprenta IHER - " + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak"
                        'Dim directorio As String = Server.MapPath("/Backups/" + nombreArchivo)
                        Dim directorio As String = "C:\Respaldos\" + nombreArchivo
                        Dim queryRespaldo = "EXEC SP_RESPALDO_BDD '" + Session("usuario").ToString() + "','" + directorio + "'"
                        Dim conexionRespaldo As SqlConnection = New SqlConnection(cadenaConexion)
                        conexionRespaldo.Open()
                        Dim comandoRespaldo As SqlCommand = New SqlCommand(queryRespaldo, conexionRespaldo)
                        comandoRespaldo.ExecuteNonQuery()
                        conexionRespaldo.Close()
                        bitacora.registrarBitacora(Session("usuario").ToString(), "RESPALDO DE BDD REALIZADO: " + directorio)
                        Session("mensaje") = "Respaldo completado"
                        Return View()
                    ElseIf submit.Equals("generar") Then
                        Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESPALDO,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESPALDO,108) FECHA_RESPALDO,A.FECHA_RESPALDO,A.NOMBRE_ARCHIVO,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESPALDOS A 
                        INNER JOIN TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO"
                        If date1 <> Nothing Then
                            query = query + "  WHERE CAST(A.FECHA_RESPALDO AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                                "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                        End If
                        query = query + " ORDER BY CAST(A.FECHA_RESPALDO AS DATETIME) ASC"

                        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        Dim comando As SqlCommand = New SqlCommand(query, conexion)
                        Dim lector = comando.ExecuteReader()
                        Dim model As New List(Of RespaldosModel)
                        While (lector.Read())
                            Dim detalles = New RespaldosModel()
                            detalles.fechaRespaldo = lector("FECHA_RESPALDO").ToString()
                            detalles.usuario = lector("NOMBRE_USUARIO").ToString()
                            detalles.nombreArchivo = lector("NOMBRE_ARCHIVO").ToString()
                            detalles.resultado = lector("RESULTADO").ToString()
                            model.Add(detalles)
                        End While
                        conexion.Close()
                        bitacora.registrarBitacora(Session("usuario").ToString(), "GENERACIÓN DE HISTORIAL DE RESPALDO DE BDD")
                        ViewBag.Message = "Datos respaldo"
                        Return View("RespaldoBDD", model)
                    Else
                        bitacora.registrarBitacora(Session("usuario"), "EXPORTAR HISTORIAL DE RESPALDOS DE BDD")
                        Dim dsRespaldos As New DsRespaldos()
                        Dim fila As DataRow
                        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESPALDO,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESPALDO,108) FECHA_RESPALDO,A.NOMBRE_ARCHIVO,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESPALDOS A 
                        INNER JOIN TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO"
                        If date1 <> Nothing Then
                            query = query + "  WHERE CAST(A.FECHA_RESPALDO AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                                "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                        End If
                        query = query + " ORDER BY CAST(A.FECHA_RESPALDO AS DATETIME) ASC"

                        Dim comando As SqlCommand = New SqlCommand(query, conexion)
                        Dim lector = comando.ExecuteReader()
                        While (lector.Read())
                            fila = dsRespaldos.Tables("DataTable1").NewRow()
                            fila.Item("fecha") = lector("FECHA_RESPALDO").ToString()
                            fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                            fila.Item("archivo") = lector("NOMBRE_ARCHIVO").ToString()
                            fila.Item("resultado") = lector("RESULTADO").ToString()
                            dsRespaldos.Tables("DataTable1").Rows.Add(fila)
                        End While
                        conexion.Close()
                        Dim nombreArchivo As String = "Reporte de respaldos.pdf"
                        Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                        If System.IO.File.Exists(directorio) Then
                            System.IO.File.Delete(directorio)
                        End If
                        Dim crystalReport As ReportDocument = New ReportDocument()
                        crystalReport.Load(Server.MapPath("~/ReporteRespaldos.rpt"))
                        crystalReport.SetDataSource(dsRespaldos)
                        crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                        Response.ContentType = "application/octet-stream"
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                        Response.TransmitFile(directorio)
                        Response.End()
                        Return View()
                    End If

                Catch ex As Exception
                    Session("mensaje") = "Error"
                    Return View()

                End Try
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function

        Function validarRespaldoActualizado() As Integer

            Dim respuesta As Integer = 0
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim query = "SELECT COUNT(*) FROM BITACORA_RESPALDOS WHERE CAST(FECHA_RESPALDO AS DATE) 
                        BETWEEN  DATEADD(DAY,-1,GETDATE()) AND CAST(GETDATE() AS DATE)"
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                respuesta = Convert.ToInt32(comando.ExecuteScalar().ToString())
                conexion.Close()
                Return respuesta

        End Function


        Function RestaurarBDD() As ActionResult
            If Session("accesos") <> Nothing Then
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE RESTAURACIÓN DE BDD")
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function RestaurarBDD(archivo As String, submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            If Session("accesos") <> Nothing Then
                'cadenaConexion = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=master;Integrated Security=true;"
                'cadenaConexion = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=master;Integrated Security=true;"
                cadenaConexion = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
                If submit.Equals("restaurar") Then
                    If validarRespaldoActualizado() > 0 Then
                        Dim query = "EXEC SP_RESTAURAR_BDD '" + archivo + "','" + Session("usuario") + "'"
                        Dim conexionRestaurar As SqlConnection = New SqlConnection(cadenaConexion)
                        conexionRestaurar.Open()
                        Dim comandoRestaurar As SqlCommand = New SqlCommand(query, conexionRestaurar)
                        comandoRestaurar.CommandTimeout = 600
                        comandoRestaurar.ExecuteNonQuery()
                        conexionRestaurar.Close()
                        Session("mensaje") = "Restauración exitosa"
                        bitacora.registrarBitacora(Session("usuario").ToString(), "RESTAURACIÓN DE BDD")
                        Return View()
                    Else
                        Session("mensaje") = "Realizar respaldo"
                        Return View()
                    End If
                ElseIf submit.Equals("generar") Then
                    Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESTAURACION,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESTAURACION,108) FECHA_RESTAURACION,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESTAURACIONES A 
                        INNER JOIN [Imprenta-IHER].[DBO].TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO"
                    If date1 <> Nothing Then
                        query = query + "  WHERE CAST(A.FECHA_RESTAURACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                    End If
                    query = query + " ORDER BY CAST(A.FECHA_RESTAURACION AS DATETIME) ASC"

                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of RespaldosModel)
                    While (lector.Read())
                        Dim detalles = New RespaldosModel()
                        detalles.fechaRespaldo = lector("FECHA_RESTAURACION").ToString()
                        detalles.usuario = lector("NOMBRE_USUARIO").ToString()
                        detalles.resultado = lector("RESULTADO").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    bitacora.registrarBitacora(Session("usuario").ToString(), "GENERACIÓN DE HISTORIAL DE RESTAURACIONES DE BDD")
                    ViewBag.Message = "Datos respaldo"
                    Return View("RestaurarBDD", model)
                Else
                    bitacora.registrarBitacora(Session("usuario"), "EXPORTAR HISTORIAL DE RESTAURACIONES DE BDD")
                    Dim dsRespaldos As New DsRespaldos()
                    Dim fila As DataRow
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESTAURACION,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESTAURACION,108) FECHA_RESTAURACION,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESTAURACIONES A 
                        INNER JOIN [Imprenta-IHER].[DBO].TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO"
                    If date1 <> Nothing Then
                        query = query + "  WHERE CAST(A.FECHA_RESTAURACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                            "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                    End If
                    query = query + " ORDER BY CAST(A.FECHA_RESTAURACION AS DATETIME) ASC"

                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    While (lector.Read())
                        fila = dsRespaldos.Tables("DataTable1").NewRow()
                        fila.Item("fecha") = lector("FECHA_RESTAURACION").ToString()
                        fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                        fila.Item("resultado") = lector("RESULTADO").ToString()
                        dsRespaldos.Tables("DataTable1").Rows.Add(fila)
                    End While
                    conexion.Close()
                    Dim nombreArchivo As String = "Reporte de restauraciones.pdf"
                    Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                    If System.IO.File.Exists(directorio) Then
                        System.IO.File.Delete(directorio)
                    End If
                    Dim crystalReport As ReportDocument = New ReportDocument()
                    crystalReport.Load(Server.MapPath("~/ReporteRestauraciones.rpt"))
                    crystalReport.SetDataSource(dsRespaldos)
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                    Response.ContentType = "application/octet-stream"
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                    Response.TransmitFile(directorio)
                    Response.End()
                    Return View()
                End If
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function
    End Class


End Namespace