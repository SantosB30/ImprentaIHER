@Code
    ViewData("Title") = "Home | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Enviado a producción") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡La cotización fue enviada a producción!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Flujo adelantado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El flujo de producción avanzó exitosamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Flujo retrasado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El flujo de producción retrocedió exitosamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Estado asignado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Se asignó el estado correctamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Flujo finalizado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El flujo de producción finalizó correctamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    End If
End If

@Section Styles
    @Styles.Render("~/plugins/sweetAlertStyles")
End Section

@Section Scripts
    @Scripts.Render("~/plugins/sweetAlert")
End Section


