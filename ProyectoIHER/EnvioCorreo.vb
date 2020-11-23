Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports EASendMail

Public Class EnvioCorreo
    Public Function enviarCorreo(asunto As String, destinatario As String, cuerpoCorreo As String) As String
        Try
            Dim adminCorreo As String = obtenerParametros("ADMIN_CORREO")
            Dim contraseña As String = obtenerParametros("ADMIN_CPASS")
            Dim puerto As String = obtenerParametros("ADMIN_CPUERTO")
            Dim mail As MailMessage = New MailMessage()


            Dim subject As String = "Prueba"
            Dim body As String = "Prueba"

            Using mm As New MailMessage(adminCorreo, destinatario)
                mm.Subject = asunto
                mm.Body = cuerpoCorreo
                mm.IsBodyHtml = True
                Dim smtp As New Net.Mail.SmtpClient()
                smtp.Host = "smtp.gmail.com"
                smtp.EnableSsl = True
                Dim NetworkCred As New NetworkCredential(adminCorreo, contraseña)
                smtp.UseDefaultCredentials = True
                smtp.Credentials = NetworkCred
                smtp.Port = Integer.Parse(puerto)
                smtp.Send(mm)
            End Using
            Return "Enviado"
        Catch ex As Exception
            Return ex.ToString()
        End Try
    End Function

    Public Function obtenerParametros(parametro As String) As String
        Dim valorParametro As String = ""
        'Dim cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=ImprentaIHER;Integrated Security=true;"
        'Dim cadenaConexion As String = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim query As String = "SELECT VALOR FROM TBL_MS_PARAMETROS WHERE PARAMETRO='" + parametro + "'"
        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
        conexion.Open()
        Dim comando As SqlCommand = New SqlCommand(query, conexion)
        Dim lector As SqlDataReader = comando.ExecuteReader()
        While lector.Read()
            valorParametro = lector("VALOR").ToString()
        End While
        conexion.Close()
        Return valorParametro
    End Function
End Class
