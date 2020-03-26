@Code

    ViewData("Title") = "Buscar cotizaciones | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.CotizacionesModel)

End Code

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Buscar Cotizaciones</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("BuscarCotizaciones", "Cotizaciones", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>Número cotización</strong></td>
                                        <td align="center"><strong>Fecha creación</strong></td>
                                        <td align="center"><strong>Estado</strong></td>
                                        <td align="center"><strong>Cliente</strong></td>
                                        <td align="center"><strong>Usuario</strong></td>
                                        <td align="center"><strong>Acciones</strong></td>
                                        <td align="center"><strong>¿Enviar a producción?</strong></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each item In Model
                                        @<tr>
                                            <td>@item.numeroCotizacion</td>
                                            <td>@item.fechaCreacion</td>
                                            @If item.estadoCotizacion.Contains("VIGENTE") Then
                                                @<td align="center"> <span Class="label label-primary">@item.estadoCotizacion</span></td>
                                            Else
                                            @<td align="center"> <span Class="label label-danger">@item.estadoCotizacion</span></td>

                                            End If
                                            <td>@item.nombreCliente</td>
                                            <td>@item.nombreUsuario</td>
                                            <td>
                                                <div class="col-lg-12">
                                                    @Html.ActionLink("Ver", "BuscarCotizacion", "Cotizaciones", New With {.numeroCotizacion = item.numeroCotizacion}, New With {.class = "badge badge-success col-md-12"})
                                                </div>
                                            </td>
                                                <td>
                                                <div class="col-lg-12">
                                                    @Html.ActionLink("Enviar", "EnviarProduccion", "Cotizaciones", New With {.numeroCotizacion = item.numeroCotizacion}, New With {.class = "badge badge-primary col-md-12"})
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
                    { extend: 'excel', title: 'Cotizaciones' },
                    { extend: 'pdf', title: 'Cotizaciones' }
                ]

            });



        });

    </script>
End Section