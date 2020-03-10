@Code

    ViewData("Title") = "EditarUsuario | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.UsuariosModel)

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
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Contains("Password restablecida") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Confirmación!",
                     text: "¡Se restableció la contraseña!",
                     type: "success"
                 });
             };
        </script>
        @<h3>@Session("mensaje")</h3>
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
        @<h3>@Session("mensaje")</h3>
        Session("mensaje") = Nothing
    End If
End If
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong> Bitacora de usuarios</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("BitacoraUsuario", "Usuarios", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>Fecha</strong></td>
                                        <td align="center"><strong>Usuario</strong></td>
                                        <td align="center"><strong>Accion</strong></td>
                                                                            </tr>
                                </thead>
                                <tbody>

                                    @For Each item In Model
                                        @<tr>
                                            <td>@item.Fecha</td>
                                            <td>@item.usuario</td>
                                            <td>@item.Accion</td>

</tr>
                                    Next


                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>
            </div>
        End Using
    </div>
</div>
@Scripts.Render("~/plugins/sweetAlert")
@Styles.Render("~/Content/plugins/dataTables/dataTablesStyles")
@Styles.Render("~/plugins/sweetAlertStyles")

<script>
    $(function () {
        $('input[type="text"]').change(function () {
            this.value = $.trim(this.value);
        });
    });
</script>
@Scripts.Render("~/plugins/dataTables")
<script type="text/javascript">
    $(document).ready(function () {

        $('.dataTables-example').DataTable({
            pageLengtd: 25,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'copy' },
                { extend: 'excel', title: 'Bitacoa De Usuarios' }
            ]

        });



    });

</script>
