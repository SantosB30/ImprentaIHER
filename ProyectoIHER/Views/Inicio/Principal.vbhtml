@Code
    ViewData("Title") = "CrearUsuario"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Actualizado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Usuario actualizado exitosamente!",
                     type: "success"
                 });
             };
        </script>
    ElseIf ViewBag.Message.ToString().Contains("Guardado con error") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Advertencia!",
                     text: "¡No fue posible enviar el correo!",
                     type: "warning"
                 });
             };
        </script>
        @<h3>@ViewBag.Message</h3>
    End If
End If



<script>
     @Scripts.Render("~/plugins/sweetAlert")
</script>
<style>
   @Styles.Render("~/plugins/sweetAlertStyles")
</style>
<script>
    $(function () {
        $('#password').on('keypress', function (e) {
            if (e.which == 32) {
                return false;
            }
        });
    });

</script>

<script>
    $(function () {
        $('#correo').on('keypress', function (e) {
            if (e.which == 32) {
                return false;
            }
        });
    });
</script>
<script>
    $(function () {
        $('#usuario').on('keypress', function (e) {
            if (e.which == 32) {
                return false;
            }
        });
    });
</script>

<script>
    $(function () {
        $('input[type="text"]').change(function () {
            this.value = $.trim(this.value);
        });
    });
</script>
