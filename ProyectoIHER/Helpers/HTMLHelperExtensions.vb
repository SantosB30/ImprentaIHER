Public Class HTMLHelperExtensions
    Public Function IsSelected(html As HtmlHelper, controller As String, action As String, cssClass As String)
        If String.IsNullOrEmpty(cssClass) Then
            cssClass = "active"
        End If
        Dim currentAction As String
        Dim currentController As String

        currentAction = Str(html.ViewContext.RouteData.Values("action"))
        currentController = Str(html.ViewContext.RouteData.Values("controller"))


        If String.IsNullOrEmpty(controller) Then
            controller = currentController
        End If

        If String.IsNullOrEmpty(action) Then
            action = currentAction
        End If

        Return controller
    End Function

    Public Function PageClass(html As HtmlHelper)
        Dim currentAction As String = Str(html.ViewContext.RouteData.Values("action"))
        Return currentAction

    End Function

End Class
