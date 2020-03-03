
@Code
    Layout = Nothing
End Code

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet">
    <link href="~/Fonts/font-awesome/css/font-awesome.css" rel="stylesheet">
    <link href="~/Content/animate.css" rel="stylesheet">
    <link href="~/Content/style.css" rel="stylesheet">
</head>
<body class="FondoEdificio">
    <div class="middle-box text-center loginscreen   animated fadeInDown">
        <div>
            <div>
                <img class="LogoLogin" src="~/Images/logo.png" />
            </div>
            <p></p>
            <div class="fondotransparenteblanco row">
                <h3 class="letrasnegras">Regístrese</h3>
                @Using Html.BeginForm("Registrarse", "Cuentas", FormMethod.Post)

                    @<form class="m-t" role="form" action="#">
                        <div class="form-group">
                            <input type="text" class="form-control" placeholder="Nombre completo" required="" id="nombre" name="nombre">
                        </div>

                        <div class="form-group">
                            <input type="email" class="form-control" placeholder="Correo electrónico" required="" id="correo" name="correo">
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" placeholder="Usuario" required="" id="usuario" name="usuario" onkeyup="this.value = this.value.toUpperCase();">
                        </div>
                        <div class="form-group">
                            <input type="password" class="form-control" placeholder="Contraseña" required="" id="contraseña" name="contraseña" maxlength="15" minlength="8">
                            <input type="checkbox" onclick="mostrarContraseña()">Mostrar contraseña
                        </div>
                        <div class="form-group">
                            <select class="form-control" id="pregunta1">
                                <option value="">PREGUNTA DE SEGURIDAD 1</option>
                                <option value="1">PREGUNTA2</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <select class="form-control" id="pregunta1">
                                <option value="">PREGUNTA DE SEGURIDAD 2</option>
                                <option value="1">PREGUNTA2</option>
                            </select>
                        </div>

                        <button type="submit" class="btn btn-primary block full-width m-b">Registrarse</button>
                        <p class="text-muted text-center"><small>¿Ya tiene una cuenta?</small></p>
                        <a class="btn btn-sm btn-white btn-block" href="@Url.Action("Login", "Cuentas")">Iniciar sesión</a>
                    </form>
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

