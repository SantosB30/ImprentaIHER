

@Code
    Layout = Nothing
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
    ElseIf Session("mensaje").ToString().Contains("Usuario ya existente") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Error!",
                     text: "¡El usuario ya existe!",
                     type: "error"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    Else
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Error!",
                     text: "¡Ha ocurrido un error!",
                     type: "error"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    End If
End If
<!DOCTYPE html>
<html>
<head>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet">
    <link href="~/Fonts/font-awesome/css/font-awesome.css" rel="stylesheet">
    <link href="~/Content/animate.css" rel="stylesheet">
    <link href="~/Content/style.css" rel="stylesheet">
</head>
<body class="FondoEdificio">
    <div class="middle-box text-center loginscreen animated fadeInDown">
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
        <input type="text" class="form-control" placeholder="Nombre completo" required="" id="nombre" name="nombre" maxlength="100" onkeyup="this.value = this.value.toUpperCase();" oninvalid="this.setCustomValidity('Nombre completo')" oninput="setCustomValidity('')">
    </div>
    <div class="form-group">
        <input type="email" class="form-control" placeholder="Correo electrónico" required="" id="correo" name="correo" maxlength="50" onkeyup="this.value = this.value.toUpperCase();">
    </div>
    <div class="form-group">
        <input type="text" class="form-control" placeholder="Usuario" required="" id="usuario" name="usuario" onkeyup="this.value = this.value.toUpperCase();" maxlength="15" oninvalid=" this.setCustomValidity('Nombre de usuario')" oninput="setCustomValidity('')">
    </div>
    <div class="form-group">
        <input type="text" class="form-control" placeholder="Telefono" required="" id="Telefono" name="Telefono" onkeyup="this.value = this.value.toUpperCase();"  oninvalid="this.setCustomValidity('Telefono')" oninput="setCustomValidity('')">
    </div>
    <div class="form-group">
        <input type="password" class="form-control" placeholder="Contraseña" required="" id="contraseña" name="contraseña" maxlength="15" minlength="8" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$" oninvalid="this.setCustomValidity('La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un carácter especial y un número.')" oninput="setCustomValidity('')">
        <input type="password" class="form-control" placeholder="Confirmar Contraseña" required="" id="confirmacontraseña" name="confirmacontraseña" maxlength="15" minlength="8" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$">
        <input type="checkbox" onclick="mostrarContraseña()" />Mostrar contraseña
        <span id='message'></span>
    </div>

    <div class="form-group">
        <select class="form-control" id="pregunta1" name="pregunta1" required="required">
            <option value="">------SELECCIONE UNA PREGUNTA------</option>
            @Code Dim preguntas As List(Of String) = TempData("preguntas") End Code
            @For Each pregunta As String In preguntas
                @<option value="@pregunta">@pregunta</option>
            Next
        </select>
        <input type="text" required="" class="form-control" id="respuesta1" name="respuesta1" placeholder="Respuesta pregunta de seguridad 1" onkeyup="this.value = this.value.toUpperCase();" />
    </div>
    <div class="form-group">
        <select class="form-control" id="pregunta2" name="pregunta2" required="required">
            <option value="">------SELECCIONE UNA PREGUNTA------</option>
            @Code Dim preguntas2 As List(Of String) = TempData("preguntas") End Code
            @For Each pregunta As String In preguntas
                @<option value="@pregunta">@pregunta</option>
            Next
        </select>
        <input type="text" class="form-control" required="" id="respuesta2" name="respuesta2" placeholder="Respuesta pregunta de seguridad 1" onkeyup="this.value = this.value.toUpperCase();" />
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
    $(function () {
        $('input[type="text"]').change(function () {
            this.value = $.trim(this.value);
        });
    });
</script>
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
    $('input#Telefono')
        .keypress(function (event) {
            if (event.which < 48 || event.which > 57 || this.value.length === 8) {
                return false;
            }
        });


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
    $('#contraseña, #confirmacontraseña').on('keyup', function () {
        if ($('#contraseña').val() == $('#confirmacontraseña').val()) {
            $('#message').html('La contraseña coincide').css('color', 'green');
        } else
            $('#message').html('La contraseña no coincide').css('color', 'red');
    });
</script>
@Scripts.Render("~/plugins/sweetAlert")


@Styles.Render("~/Content/plugins/dataTables/dataTablesStyles")
@Styles.Render("~/plugins/sweetAlertStyles")

