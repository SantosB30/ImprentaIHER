﻿Imports System.Data.SqlClient

Public Class Validaciones
    Public Function validarExistenciaUsuario(usuario As String, correo As String
                                             ) As String
        Dim respuesta As String = ""
        'Dim cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=ImprentaIHER;Integrated Security=true;"
        'Dim cadenaConexion As String = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim query As String = "SELECT COUNT(*) CANTIDAD FROM TBL_MS_USUARIO WHERE USUARIO='" + usuario.Trim + "' OR CORREO_ELECTRONICO='" + correo + "'"
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

    Public Function removerEspacios(palabra As String) As String
        Dim respuesta As String = ""
        respuesta = Regex.Replace(palabra, "\s{2,}", " ")
        Return respuesta
    End Function
End Class
