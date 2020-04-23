@Code

    ViewData("Title") = "Editar Proveedores | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.ProveedoresModel)

End Code

@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Proveedor editado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡La información fue guardada bajo estándares del sistema!",
                     type: "success"
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
        @<h3>@Session("mensaje")</h3>
        Session("mensaje") = Nothing
    End If
End If
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Editar proveedores</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("EditarProveedor", "Proveedores", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>Nombre</strong></td>
                                        <td align="center"><strong>Dirección</strong></td>
                                        <td align="center"><strong>Teléfono</strong></td>
                                        <td align="center"><strong>Correo</strong></td>
                                        <td align="center"><strong>Nombre de contacto</strong></td>
                                        <td align="center"><strong>Teléfono de contacto</strong></td>
                                        <td align="center"><strong>Acciones</strong></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each item In Model
                                        @<tr>
                                            <td>@item.nombreProveedor</td>
                                            <td>@item.direccionProveedor</td>
                                            <td>@item.telefonoProveedor</td>
                                            <td>@item.correoProveedor</td>
                                            <td>@item.nombreContactoProveedor</td>
                                            <td>@item.telefonoContactoProveedor </td>
                                            <td>
                                                <div class="col-lg-12">
                                                    @Html.ActionLink("Editar", "EditarProveedor", "Proveedores", New With {.proveedor = item.nombreProveedor}, New With {.class = "badge badge-success col-md-12"})
                                                </div>
                                            </td>
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
@Section Styles
    @Styles.Render("~/Content/plugins/dataTables/dataTablesStyles")
    @Styles.Render("~/plugins/sweetAlertStyles")
End Section
@Section Scripts
    @Scripts.Render("~/plugins/sweetAlert")
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
                    { extend: 'excel', title: 'Proveedores' }
                ]

            });



        });

    </script>
End Section
