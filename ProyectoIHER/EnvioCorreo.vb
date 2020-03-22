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
            'Dim SmtpServer As SmtpClient = New SmtpClient("smtp.gmail.com")
            'mail.From = New MailAddress(adminCorreo)
            'mail.To.Add(destinatario)
            'mail.Subject = asunto
            'mail.IsBodyHtml = True
            'mail.Body = cuerpoCorreo
            'SmtpServer.Port = puerto
            'SmtpServer.Credentials = New System.Net.NetworkCredential(adminCorreo, contraseña)
            'SmtpServer.EnableSsl = False
            'SmtpServer.Send(mail)

            'Dim correo As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            'correo.From = New System.Net.Mail.MailAddress(adminCorreo)
            'correo.To.Add(destinatario)
            'correo.Subject = asunto
            'correo.Body = cuerpoCorreo
            'correo.IsBodyHtml = True
            'correo.Priority = System.Net.Mail.MailPriority.Normal
            'Dim smtp As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient()
            'smtp.Credentials = New System.Net.NetworkCredential(adminCorreo, contraseña)
            'smtp.Host = "smtp.gmail.com"
            'smtp.Port = puerto
            'smtp.EnableSsl = False
            'smtp.Send(correo)

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
        Dim cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        'Dim cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
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
