@Code
    ViewData("Title") = "Editar usuario"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Usuario editado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Usuario editado exitosamente!",
                     type: "success"
                 });
             };
        </script>
    ElseIf Session("mensaje").ToString().Contains("Guardado con error") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Advertencia!",
                     text: "¡No fue posible enviar el correo!",
                     type: "warning"
                 });
             };
        </script>
        @<h3>@Session("mensaje")</h3>
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
        @<h3>@Session("mensaje")</h3>
    End If
End If
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Editar usuario</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("EditarUsuarios", "Usuarios", FormMethod.Post)
            @<div class="row">
                @If ViewBag.Message <> Nothing Then
                    @<div Class="col-lg-12">
                        <div Class="row">
                            <div Class="col-md-5" id="data_5">
                                <Label Class="font-normal"><strong>Nombre completo:</strong></Label>
                                <input type="text" Class="form-control" id="nombreCompleto" name="nombreCompleto" 
                                       value="@Session("nombreUsuarioEditar")" maxlength="15"/>
                            </div>
                            <div Class="col-md-5" id="data_5">
                                <Label Class="font-normal"><strong>Correo electrónico:</strong></Label>
                                <input type="email" Class="form-control" id="correo" name="correo" maxlength="50"
                                       value="@Session("correoUsuarioEditar")" />
                            </div>
                            <div Class="col-md-5" id="data_5">
                                <br>
                                <Label Class="font-normal"><strong>Estado:</strong></Label>
                                <select Class="form-control" id="estado" name="estado" required="required">
                                    @If Session("estadoUsuarioEditar").ToString().Equals("NUEVO") Then
                                        @<option value="NUEVO" selected>NUEVO</option>
                                        @<option value="ACTIVO">ACTIVO</option>
                                        @<option value="BLOQUEADO">BLOQUEADO</option>
                                        @<option value="INACTIVO">INACTIVO</option>
                                    ElseIf Session("estadoUsuarioEditar").ToString().Equals("ACTIVO") Then
                                        @<option value="NUEVO">NUEVO</option>
                                        @<option value="ACTIVO" selected>ACTIVO</option>
                                        @<option value="BLOQUEADO">BLOQUEADO</option>
                                        @<option value="INACTIVO">INACTIVO</option>
                                    ElseIf Session("estadoUsuarioEditar").ToString().Equals("BLOQUEADO") Then
                                        @<option value="NUEVO">NUEVO</option>
                                        @<option value="ACTIVO">ACTIVO</option>
                                        @<option value="BLOQUEADO" selected>BLOQUEADO</option>
                                        @<option value="INACTIVO">INACTIVO</option>
                                    ElseIf Session("estadoUsuarioEditar").ToString().Equals("INACTIVO") Then
                                        @<option value="NUEVO">NUEVO</option>
                                        @<option value="ACTIVO">ACTIVO</option>
                                        @<option value="BLOQUEADO">BLOQUEADO</option>
                                        @<option value="INACTIVO" selected>INACTIVO</option>
                                    End If
                                </select>
                            </div>
                            <div Class="col-md-5" id="data_5">
                                <br>
                                <Label Class="font-normal"><strong>Usuario:</strong></Label>
                                <input type="text" Class="form-control" id="usuario" name="usuario" onkeyup="this.value = this.value.toUpperCase();" maxlength="15"
                                       value="@Session("usuarioEditar")" />
                            </div>

                            <div Class="col-md-3">
                                <br>
                                <Button Class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</Button>
                            </div>

                            <input type="text" Class="form-control" id="usuarioEditar" name="usuarioEditar" maxlength="100"
                                   value="@Session("usuarioEditar")" hidden style="visibility:hidden;padding:0px"/>
                        </div>

                    </div>
                End If

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

