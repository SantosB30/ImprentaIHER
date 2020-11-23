@Code
    ViewData("Title") = "Asignar Rol Usuario | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


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
        @Using Html.BeginForm("AsignarRolUsuario", "Usuarios", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <h3>Usuario:  @Session("usuarioAsignarRol")</h3>
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