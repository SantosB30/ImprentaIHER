Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class SeguridadController
        Inherits Controller
        Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        ' GET: Seguridad
        Function RespaldoBDD() As ActionResult
            If Session("accesos") <> Nothing Then
                Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESPALDO,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESPALDO,108) FECHA_RESPALDO,A.FECHA_RESPALDO,A.NOMBRE_ARCHIVO,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESPALDOS A 
                        INNER JOIN TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO ORDER BY CAST(FECHA_RESPALDO AS DATETIME) DESC"
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
                ViewBag.Message = "Datos respaldo"
                Return View("RespaldoBDD", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function RespaldoBDD(submit As String) As ActionResult
            ''REALIZANDO RESPALDO
            Try
                Dim nombreArchivo As String = "Imprenta IHER - " + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak"
                'Dim directorio As String = Server.MapPath("/Backups/" + nombreArchivo)
                Dim directorio As String = "C:\" + nombreArchivo
                Dim queryRespaldo = "EXEC SP_RESPALDO_BDD '" + Session("usuario").ToString() + "','" + directorio + "'"
                Dim conexionRespaldo As SqlConnection = New SqlConnection(cadenaConexion)
                conexionRespaldo.Open()
                Dim comandoRespaldo As SqlCommand = New SqlCommand(queryRespaldo, conexionRespaldo)
                comandoRespaldo.ExecuteNonQuery()
                conexionRespaldo.Close()

                Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESPALDO,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESPALDO,108) FECHA_RESPALDO,A.FECHA_RESPALDO,A.NOMBRE_ARCHIVO,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESPALDOS A 
                        INNER JOIN TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO ORDER BY CAST(FECHA_RESPALDO AS DATETIME) DESC"
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
                Session("mensaje") = "Respaldo completado"
                ViewBag.Message = "Datos respaldo"
                Return View("RespaldoBDD", model)
            Catch ex As Exception
                Session("mensaje") = "Error"
                Dim query = "SELECT CONVERT(NVARCHAR,A.FECHA_RESPALDO,103)
                +' '+CONVERT(NVARCHAR,A.FECHA_RESPALDO,108) FECHA_RESPALDO,A.FECHA_RESPALDO,A.NOMBRE_ARCHIVO,
                        A.RESULTADO,B.NOMBRE_USUARIO FROM BITACORA_RESPALDOS A 
                        INNER JOIN TBL_MS_USUARIO B
                            ON A.USUARIO=B.ID_USUARIO ORDER BY CAST(FECHA_RESPALDO AS DATETIME) DESC"
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
                Session("mensaje") = "Respaldo completado"
                ViewBag.Message = "Datos respaldo"
                Return View("RespaldoBDD", model)
            End Try
        End Function
    End Class


End Namespace