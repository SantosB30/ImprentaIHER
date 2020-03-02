Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        RedirectToAction("Login", "Cuentas")
        Return View()
    End Function

    Function About() As ActionResult
        RedirectToAction("Login", "Cuentas")
        Return View()
    End Function

    Function Contact() As ActionResult
        RedirectToAction("Login", "Cuentas")
        Return View()
    End Function
End Class
