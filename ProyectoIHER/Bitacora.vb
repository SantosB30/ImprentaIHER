Imports System.Data.SqlClient

Public Class Bitacora
    'Registrar bitácora
    Public Sub registrarBitacora(usuario As String, accion As String)
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=IH;Integrated Security=true;"
        Dim cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=IH;Integrated Security=true;"
        Dim query As String = "EXEC SP_REGISTRAR_BITACORA '" + usuario + "','" + accion + "'"
        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
        conexion.Open()
        Dim comando As SqlCommand = New SqlCommand(query, conexion)
        comando.ExecuteNonQuery()
        conexion.Close()
    End Sub
End Class
