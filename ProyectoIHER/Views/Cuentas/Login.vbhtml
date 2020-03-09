

@Code
    ViewData("Title") = "Login | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout - Login.vbhtml"
End Code
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Registrado correctamente") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Usuario creado exitosamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Contains("Password restablecida") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Confirmación!",
                     text: "¡Se restableció la contraseña!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    End If
End If
<!DOCTYPE html>
<html>
<head>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet">
    <link href="~/Fonts/font-awesome/css/font-awesome.css" rel="stylesheet">
    <link href="~/Content/animate.css" rel="stylesheet">
    <link href="~/Content/style.css" rel="stylesheet">
</head>
<body class="FondoEdificio">
    <div class="middle-box text-center loginscreen  animated fadeInDown">
        <div>
            <br>
            <br>
            <br>
            <div>
                <img class="LogoLogin" src="~/Images/logo.png" />
            </div>
            <p></p>
            <div class="fondotransparenteblanco row">

                <h3 class="letrasnegras">Inicio de sesión</h3>
                @Using Html.BeginForm("Login", "Cuentas", FormMethod.Post)
                    @<form class="m-t " role="form" action="#">
                        <div class="form-group">
                            <input type="text" class="form-control" placeholder="Usuario" required="" title="Ingrese su usuario"
                                   id="usuario" name="usuario" onkeyup="this.value = this.value.toUpperCase();">
                        </div>
                        <div class="form-group">
                            <input type="password" class="form-control" placeholder="Contraseña" required="" title="Ingrese su contraseña" id="contraseña" name="contraseña">
                            <input type="checkbox" onclick="mostrarContraseña()">Mostrar contraseña
                        </div>
                        <button type="submit" class="btn btn-primary block full-width m-b">Iniciar sesión</button>
                        <a href="@Url.Action("RecuperarContraseña", "Cuentas")"><small>¿Olvidó su usuario o contraseña?</small></a>
                        <p class="text-muted text-center"><small>¿Aún no tiene cuenta?</small></p>
                        <a class="btn btn-sm btn-white btn-block" href="@Url.Action("Registrarse", "Cuentas")">Crear cuenta</a>
                    </form>
                    If ViewBag.Message <> Nothing Then
                        @<h3><strong><font style="color:red;">@ViewBag.Message.ToString()</font></strong></h3>
                    End If
                End Using
            </div>
        </div>
    </div>
</body>
</html>
<script>
    function mostrarContraseña() {
        var x = document.getElementById("contraseña");
        if (x.type === "password") {
            x.type = "text";
        } else {
            x.type = "password";
        }
    }
</script>
<script>
    $(function () {
        $('#contraseña').on('keypress', function (e) {
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
@Scripts.Render("~/plugins/sweetAlert")
@Styles.Render("~/Content/plugins/dataTables/dataTablesStyles")
@Styles.Render("~/plugins/sweetAlertStyles")
