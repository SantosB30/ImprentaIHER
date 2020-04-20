Imports System.Web.Mvc

Namespace Controllers
    Public Class InicioController
        Inherits Controller
        Dim bitacora As Bitacora = New Bitacora()

        ' GET: Inicio
        Function Principal() As ActionResult
            If Session("accesos") <> Nothing Then
                bitacora.registrarBitacora(Session("usuario").ToString(), "PANTALLA DE INICIO")
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
    End Class
End Namespace