@Code
    ViewData("Title") = "Editar Parametro | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Usuario editado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Parametro editado exitosamente!",
                     type: "success"
                 });
             };
        </script>

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
        <h3> <strong>Editar parametro</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("EditarParametros", "Usuarios", FormMethod.Post)
            @<div class="row">
                @If ViewBag.Message <> Nothing Then
                    @<div Class="col-lg-12">
                        <div Class="row">

                            <div Class="col-md-5" id="data_5">
                                <Label Class="font-normal"><strong>Parametro</strong></Label>
                                <input type="text" Class="form-control" id="Parametro" name="Parametro" maxlength="75"
                                       value="@Session("ParametroEditar")"  onkeyup="this.value = this.value.toUpperCase();" readonly/>
                            </div>
                            <div Class="col-md-5" id="data_5">
                                <Label Class="font-normal"><strong>Valor</strong></Label>
                                <input type="text" Class="form-control" id="Valor" name="Valor" maxlength="75"
                                       value="@Session("ValorEditar")"  onkeyup="this.value = this.value.toUpperCase();" required min="1"/>
                            </div>
                            <div Class="col-md-3">
                                <br>
                                <Button Class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</Button>
                            </div>
                            <input type="text" Class="form-control" id="ParametroEditar" name="ParametroEditar" maxlength="100"
                                   value="@Session("Id_ParametroEditar")" hidden style="visibility:hidden;padding:0px"  onkeyup="this.value = this.value.toUpperCase();"/>
                        </div>
                    </div>
                End If
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
            $('#Id_ParametroEditar').on('keypress', function (e) {
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