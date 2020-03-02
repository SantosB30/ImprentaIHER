Imports System.Web.Mvc

Namespace Controllers
    Public Class InicioController
        Inherits Controller

        ' GET: Inicio
        Function Principal() As ActionResult
            If Session("accesos") <> Nothing Then
                Return View()
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
    End Class
End Namespace