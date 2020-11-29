@Code
    ViewData("Title") = "Agregar rol | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Rol agregado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Rol agregado bajo los estándares del sistema!",
                     type: "success"
                 });
             };
        </script>
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
        @Using Html.BeginForm("AgregarRoles", "Cuentas", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Nombre completo:</strong></label>
                            <input type="text" class="form-control" id="rol" name="rol" maxlength="100" required placeholder="Nombre del rol" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, '');" oninvalid="this.setCustomValidity('Nombre del rol')" oninput="setCustomValidity('')"  />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Nombre completo:</strong></label>
                            <input type="text" class="form-control" id="descripcion" name="descripcion" maxlength="100" required placeholder="Descripción del rol" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, ' ');" oninvalid="this.setCustomValidity('Descripción del rol')" oninput="setCustomValidity('')" />
                        </div>
                        <div class="col-md-5">
                            <br>
                            <br>
                            <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
                            <button class="btn btn-danger" type="button" onclick="window.location='/Inicio/Principal';"><span><i class="fa fa-times" aria-hidden="true"></i></span> Cancelar</button>
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