﻿Imports System.Data.SqlClient

Public Class Bitacora
    'Registrar bitácora
    Public Sub registrarBitacora(usuario As String, accion As String)
        'Dim cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=ImprentaIHER;Integrated Security=true;"
        'Dim cadenaConexion As String = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim query As String = "EXEC SP_REGISTRAR_BITACORA '" + usuario + "','" + accion + "'"
        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
        conexion.Open()
        Dim comando As SqlCommand = New SqlCommand(query, conexion)
        comando.ExecuteNonQuery()
        conexion.Close()
    End Sub
End Class
