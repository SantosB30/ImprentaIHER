﻿@Code

    ViewData("Title") = "Editar Cliente | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.ClientesModel)

End Code

@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Cliente editado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Cliente editado exitosamente bajo los estándares del sistema!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Cliente eliminado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Cliente eliminado exitosamente bajo los estándares del sistema!",
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
        <h3> <strong>Editar clientes</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("EliminarCliente", "Clientes", FormMethod.Post, New With {.name = "form", .Id = "form"})
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>Nombre</strong></td>
                                        <td align="center"><strong>RTN</strong></td>
                                        <td align="center"><strong>Dirección</strong></td>
                                        <td align="center"><strong>Teléfono</strong></td>
                                        <td align="center"><strong>Correo</strong></td>
                                        <td align="center"><strong>Nacionalidad</strong></td>
                                        <td align="center"><strong>Estado</strong></td>
                                        <td align="center"><strong>Acciones</strong></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each item In Model
                                        @<tr>
                                            <td>@item.nombreCliente</td>
                                            <td>@item.rtnCliente</td>
                                            <td>@item.direccionCliente</td>
                                            <td>@item.telefonoCliente</td>
                                            <td>@item.correoCliente</td>
                                            <td>@item.nacionalidadCliente</td>
                                            <td>@item.estado</td>

                                            <td>
                                                <div class="col-lg-12">
                                                    @Html.ActionLink("Editar", "EditarCliente", "Clientes", New With {.cliente = item.nombreCliente}, New With {.class = "badge badge-success col-md-12 "})
                                                    @Html.ActionLink("Eliminar", "EliminarCliente", "Clientes", New With {.cliente = item.nombreCliente}, New With {.class = "badge badge-danger col-md-12 deleteBtn"})
                                                    <input class="form-control" type="hidden" name="nombreCliente" value="@item.nombreCliente" id="nombreCliente" />
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
                    { extend: 'excel', title: 'Clientes' }

                ]

            });



        });

    </script>

    <script>
        $('.deleteBtn').on('click', function (e) {
            e.preventDefault();
            var form = $(this).parents('form');
            swal({
                title: "¿Está seguro que desea eliminar este cliente?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Si, deseo eliminarlo!",
                showButtonText: "Cancelar",
                closeOnConfirm: false
            }, function (isConfirm) {
                if (isConfirm) form.submit();
            });
        });
    </script>
End Section