@Code
    ViewData("Title") = "Cambiar Contraseña | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Contraseña actualizada") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Contraseña actualizada exitosamente!",
                     type: "success"
                 });
             };
        </script>
    ElseIf Session("mensaje").ToString().Contains("Contraseña incorrecta") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Error!",
                     text: "¡Contraseña incorrecta!",
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
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Cambiar Contraseña</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("CambiarContraseña", "Cuentas", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-4">
                            <input type="password" class="form-control" placeholder="Contraseña actual" required="" id="contraseñaAnterior" name="contraseñaAnterior" maxlength="15" minlength="8" >
                        </div>
                        <div class="col-md-4">
                            <input type="password" class="form-control" placeholder="Nueva contraseña" required="" id="contraseña" name="contraseña" maxlength="15" minlength="8" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$">
                            <input type="checkbox" onclick="mostrarContraseña()" />Mostrar contraseña
                        </div>
                        <div class="col-md-4">
                            <input type="password" class="form-control" placeholder="Confirmar nueva contraseña" required="" id="confirmacontraseña" name="confirmacontraseña" maxlength="15" minlength="8" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$">
                            <span id='message'></span>
                        </div>
                        <div class="col-md-5">
                            <br>
                            <br>
                            <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
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
    <script>
        $('#contraseña, #confirmacontraseña').on('keyup', function () {
            if ($('#contraseña').val() == $('#confirmacontraseña').val()) {
                $('#message').html('La contraseña coincide').css('color', 'green');
            } else
                $('#message').html('La contraseña no coincide').css('color', 'red');
        });
    </script>
    @Scripts.Render("~/plugins/sweetAlert")
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
        function mostrarContraseña() {
            var x = document.getElementById("contraseña");
            var y = document.getElementById("confirmacontraseña");

            if (x.type === "password") {
                x.type = "text";
                y.type = "text";
            } else {
                x.type = "password";
                y.type = "password";
            }
        }
    </script>
    <script>
        $(function () {
            $('#contraseñaAnterior').on('keypress', function (e) {
                if (e.which == 32) {
                    return false;
                }
            });
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
            $('#confirmacontraseña').on('keypress', function (e) {
                if (e.which == 32) {
                    return false;
                }
            });
        });

    </script>
End Section
