﻿@Code

    ViewData("Title") = "Parametros | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.UsuariosModel)

End Code

@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Parametro editado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Parámetro editado exitosamente bajo los estándares del sistema!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing

        @<h3>@Session("mensaje")</h3>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Parametro incorrecto") Then
            @<script>
                 window.onload = function () {
                     swal({
                         title: "Confirmación",
                         text: "¡Valor de parámetro no permitido!",
                         type: "error"
                     });
                 };
            </script>
        Session("mensaje") = Nothing

            @<h3>@Session("mensaje")</h3>
        Session("mensaje") = Nothing

    End If
End If
<link rel = "stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type = "text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src = "https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js" ></script>
<div Class="ibox float-e-margins">
<div Class="ibox-title">
<h3> <strong>Parámetros</strong></h3>
<div Class="ibox-tools">
<a Class="collapse-link">
    <i Class="fa fa-chevron-up"></i>
</a>
</div>
</div>
<div Class="ibox-content">
@Using Html.BeginForm("Parametros", "Usuarios", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>Parámetro</strong></td>
                                        <td align="center"><strong>Valor</strong></td>
                                        <td align="center"><strong>Acción</strong></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each item In Model
                                        @<tr>

                                            <td>@item.Parametro</td>
                                            <td>@item.Valor</td>

                                            <td>
                                                <div class="col-lg-12">
                                                    @Html.ActionLink("Editar", "EditarParametros", "Usuarios", New With {.Parametro = item.Parametro}, New With {.class = "badge badge-success col-md-12"})
                                                </div>
                                            </td>
                                        </tr>
                                    Next

                                </tbody>
                            </table>
                            <div class="col-md-3">
                                <br>
                                <br>
                                <button class="btn btn-primary" type="submit" name="submit" id="submit" value="exportar"><span><i class="fa fa-save" aria-hidden="true"></i></span> PDF</button>
                                <button class="btn btn-danger" type="button" onclick="window.location='/Inicio/Principal';"><span><i class="fa fa-times" aria-hidden="true"></i></span> Cancelar</button>

                            </div>
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
                    { extend: 'excel', title: 'Parámetros' }
                ]

            });



        });

    </script>
    @Scripts.Render("~/plugins/sweetAlert")
End Section