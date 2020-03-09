@Code
    ViewData("Title") = "Recuperar contraseña"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

@If ViewBag.Message <> Nothing Then
    If ViewBag.Message.ToString().Equals("Correo") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Se envió la contraseña al correo electrónico!",
                     type: "success"
                 });
             };
        </script>
    ElseIf ViewBag.Message.ToString().Contains("Preguntas") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Se envió la contraseña al correo electrónico!",
                     type: "success"
                 });
             };
        </script>
        @<h3>@ViewBag.Message</h3>
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
        @<h3>@ViewBag.Message</h3>
    End If
End If
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Recuperar contraseña</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("RecuperarContraseña", "Cuentas", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Usuario:</strong></label>
                            <input type="text" class="form-control" id="usuario" name="usuario" onkeyup="this.value = this.value.toUpperCase();" maxlength="15" pattern="^[a-zA-Z0-9]+$"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Método de recuperación:</strong></label>
                            <select class="form-control" id="tipoRecuperacion" name="tipoRecuperacion" required="required">
                                <option value="">------SELECCIONE TIPO DE RECUPERACION------</option>
                                <option value="preguntas">PREGUNTAS DE SEGURIDAD</option>
                                <option value="correo">CORREO ELECTRÓNICO</option>
                            </select>
                        </div>
                        <div class="col-md-5" id="pregunta1" hidden>
                            <br>
                            <label class="font-normal"><strong>Pregunta de seguridad 1:</strong></label>
                            <select class="form-control" id="pregunta1" name="pregunta1" required="required">
                                <option value="-">------SELECCIONE UNA PREGUNTA------</option>
                                @Code Dim preguntas As List(Of String) = TempData("preguntas") End Code
                                @For Each pregunta As String In pregunta
                                    @<option value="@pregunta">@preguntas</option>
                                Next
                            </select>
                            <label class="font-normal"><strong>Respuesta:</strong></label>
                            <input type="text" class="form-control" id="respuesta1" name="respuesta1" value="-" />
                        </div>
                        <div class="col-md-5" id="pregunta2" hidden>
                            <br>
                            <label class="font-normal"><strong>Pregunta de seguridad 2:</strong></label>
                            <select class="form-control" id="pregunta2" name="pregunta2" required="required">
                                <option value="-">------SELECCIONE UNA PREGUNTA------</option>
                                @Code Dim preguntas2 As List(Of String) = TempData("preguntas") End Code
                                @For Each pregunta As String In preguntas2
                                    @<option value="@pregunta">@pregunta</option>
                                Next
                            </select>
                            <label class="font-normal"><strong>Respuesta:</strong></label>
                            <input type="text" class="form-control" id="respuesta2" name="respuesta2" value="-" />
                        </div>
                        <div class="col-md-3">
                            <br>
                            <button class="btn btn-primary" type="submit"><span><i class="fa fa-key" aria-hidden="true"></i></span> Recuperar</button>
                        </div>
                    </div>

                </div>
            </div>
        End Using
    </div>
</div>
<Script>
                                @Scripts.Render("~/plugins/sweetAlert")
</Script>
<Style>
                                @Styles.Render("~/plugins/sweetAlertStyles")
</Style>
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

<script>
    $('#password1, #password2').on('keyup', function () {
        if ($('#password1').val() == $('#password2').val()) {
            $('#message').html('La contraseña coincide').css('color', 'green');
        } else
            $('#message').html('La contraseña no coincide').css('color', 'red');
    });
</script>

<script>
    function mostrarContraseña() {
        var x = document.getElementById("password1");
        if (x.type === "password") {
            x.type = "text";
        } else {
            x.type = "password";
        }

        var x = document.getElementById("password2");
        if (x.type === "password") {
            x.type = "text";
        } else {
            x.type = "password";
        }
    }
</script>

<script type="text/javascript">
    $(function () {
        $("#tipoRecuperacion").change(function () {
            if ($(this).val() == "preguntas") {
                $("#pregunta1").show();
                $("#pregunta2").show();
            } else {
                $("#pregunta1").hide();
                $("#pregunta2").hide();
            }
        });
    });
</script>


