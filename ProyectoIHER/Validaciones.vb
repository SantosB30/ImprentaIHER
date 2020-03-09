Imports System.Data.SqlClient

Public Class Validaciones
    Public Function validarExistenciaUsuario(usuario As String) As String
        Dim respuesta As String = ""
        Dim cadenaConexion As String = "Data Source=" + System.Environment.MachineName + ";Initial Catalog=IH;Integrated Security=true;"
        Dim query As String = "SELECT COUNT(*) CANTIDAD FROM TBL_MS_USUARIO WHERE USUARIO='" + usuario + "'"

        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
        conexion.Open()
        Dim comando As SqlCommand = New SqlCommand(query, conexion)
        Dim lector As SqlDataReader = comando.ExecuteReader()

        While lector.Read()
            respuesta = lector("CANTIDAD").ToString()
        End While
        conexion.Close()
        Return respuesta
    End Function
End Class
