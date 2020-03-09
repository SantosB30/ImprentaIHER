Imports System.Data.SqlClient

Public Class Bitacora
    Public Sub registrarBitacora(usuario As String, accion As String)
        Dim cadenaConexion As String = "Data Source=" + System.Environment.MachineName + ";Initial Catalog=IH;Integrated Security=true;"
        Dim query As String = "EXEC SP_REGISTRAR_BITACORA '" + usuario + "','" + accion + "'"
        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
        conexion.Open()
        Dim comando As SqlCommand = New SqlCommand(query, conexion)
        comando.ExecuteNonQuery()
        conexion.Close()
    End Sub
End Class
