@Code
    ViewData("Title") = "Confirmar registro"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Confirmar registro</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("ConfirmarRegistro", "Cuentas", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12" >
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Pregunta de seguridad 1:</strong></label>
                            <select class="form-control" id="pregunta1" name="pregunta1" required="required">
                                <option value="">------SELECCIONE UNA PREGUNTA------</option>
                                @Code Dim preguntas As List(Of String) = TempData("preguntas") End Code
                                @For Each pregunta As String In preguntas
                                    @<option value="@pregunta">@pregunta</option>
                                Next
                            </select>
                            <label class="font-normal"><strong>Respuesta:</strong></label>
                            <input type="text" requeride"" class="form-control" id="respuesta1" name="respuesta1"  oninvalid=" this.setCustomValidity('Respuesta a pregunta 1')" oninput="setCustomValidity('')"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Pregunta de seguridad 2:</strong></label>
                            <select class="form-control" id="pregunta2" name="pregunta2" required="required">
                                <option value="">------SELECCIONE UNA PREGUNTA------</option>
                                @Code Dim preguntas2 As List(Of String) = TempData("preguntas") End Code
                                @For Each pregunta As String In preguntas2
                                    @<option value="@pregunta">@pregunta</option>
                                Next
                            </select>
                            <label class="font-normal"><strong>Respuesta:</strong></label>
                            <input type="text" required="required" class="form-control" id="respuesta2" name="respuesta2"  oninvalid=" this.setCustomValidity('Respuesta a pregunta 2')" oninput="setCustomValidity('')"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Nueva contraseña:</strong></label>
                            <input type="password" class="form-control" id="password1" name="password1" required="required" maxlength="15" minlength="8"   oninvalid="this.setCustomValidity('La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un carácter especial y un número.')" oninput="setCustomValidity('')"/>
                            <input type="checkbox" onclick="mostrarContraseña()">Mostrar contraseña
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Confirmar contraseña:</strong></label>
                            <input type="password" class="form-control" id="password2" required="required" name="password2" maxlength="15" minlength="8" />
                            <span id='message'></span>
                        </div>
                        <div class="col-md-3">
                            <br>
                            <button class="btn btn-primary" type="submit" value="submit" onclick="CheckPassword(document.form1.text1)"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
                        </div>
                    </div>
                </div>
            </div>
                                    End Using
    </div>
</div>
@Section Styles
    @Styles.Render("~/plugins/sweetAlertStyles")
End Section

@Section Scripts

    @Scripts.Render("~/plugins/sweetAlert")

    <script>
        $(function () {
            $('#password1').on('keypress', function (e) {
                if (e.which == 32) {
                    return false;
                }
            });
        });

    </script>
    <script>
        $(function () {
            $('#password2').on('keypress', function (e) {
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
        $(function () {
            $('input[type="password1"]').change(function () {
                this.value = $.trim(this.value);
            });
        });
    </script>
    <script>
        $(function () {
            $('input[type="password2"]').change(function () {
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
        function test_str() {
            var res;
            var str =
                document.getElementById("password1").value;
            if (str.match(/[a-z]/g) && str.match(
                /[A-Z]/g) && str.match(
                    /[0-9]/g) && str.match(
                        /[^a-zA-Z\d]/g) && str.length >= 8)
                res = "TRUE";
            else
                res = "FALSE";
            document.getElementById("t2").value = res;

        }
    </script>
End Section