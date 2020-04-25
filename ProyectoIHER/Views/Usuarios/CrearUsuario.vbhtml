@Code
    ViewData("Title") = "Crear Usuario | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@If ViewBag.Message <> Nothing Then
    If ViewBag.Message.ToString().Equals("Guardado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡La información fue guardada bajo estándares del sistema!",
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
    ElseIf ViewBag.Message.ToString().Contains("Usuario ya existe") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Error!",
                     text: "¡El usuario ya existe!",
                     type: "error"
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
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong> Creación de usuario</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("CrearUsuario", "Usuarios", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Nombre completo:</strong></label>
                            <input type="text" class="form-control" id="nombreCompleto" name="nombreCompleto" maxlength="100" required placeholder="Nombre completo" onkeyup="this.value = this.value.toUpperCase();" />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Correo electrónico:</strong></label>
                            <input type="email" class="form-control" id="correo" name="correo" maxlength="50" required placeholder="Correo electrónico" onkeyup="this.value = this.value.toUpperCase();" />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Usuario:</strong></label>
                            <input type="text" class="form-control" id="usuario" name="usuario" onkeyup="this.value = this.value.toUpperCase();" maxlength="15" required placeholder="Usuario" />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Telefono:</strong></label>
                            <input type="text" class="form-control" id="Telefono" name="Telefono" maxlength="100" required placeholder="Telefono" onkeyup="this.value = this.value.toUpperCase();" />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Contraseña:</strong></label>
                            <input type="text" class="form-control" id="password" name="password" maxlength="15" minlength="8" required placeholder="Contraseña" pattern="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$" />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Rol:</strong></label>
                            <select class="form-control" name="rol" id="rol" required>
                                <option value="">------ SELECCIONE ------</option>
                                <option value="ADMINISTRACION">ADMINISTRACION</option>
                                <option value="DISEÑO">DISEÑO</option>
                                <option value="IMPRESION">IMPRESION</option>
                                <option value="ACABADO">ACABADO</option>
                                <option value="BODEGA">BODEGA</option>
                            </select>
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
    @Scripts.Render("~/plugins/sweetAlert")
    <script>
        $('input#nombreCompleto')
            .keypress(function (event) {
                if (event.which == 49 || event.which == 50 || event.which == 51 || event.which == 52
                    || event.which == 53 || event.which == 54 || event.which == 55 || event.which == 56 || event.which == 57 || event.which == 48) {
                    return false;
                }
            });


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
End Section