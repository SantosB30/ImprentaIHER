﻿@Code

    ViewData("Title") = "Ver órdenes | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.OrdenesModel)

End Code

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Ver órdenes</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
       <div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>Número orden</strong></td>
                                        <td align="center"><strong>Fecha creación</strong></td>
                                        <td align="center"><strong>Estado</strong></td>
                                        <td align="center"><strong>Cliente</strong></td>
                                        <td align="center"><strong>Usuario</strong></td>
                                        <td align="center"><strong>Acciones</strong></td>
                                        <td align="center"><strong>Flujo de producción</strong></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    @If Session("accesos") <> Nothing Then
                                        @If Session("accesos").ToString().Contains("ADMINISTRACION") Then

                                            @For Each item In Model
                                                Dim estadoAnterior As String = "DISEÑO"
                                                Dim estadoPosterior As String = "DISEÑO"


                                                If item.estadoOrden.Equals("ADMINISTRACION") Then
                                                    estadoPosterior = "DISEÑO"
                                                ElseIf item.estadoOrden.Equals("DISEÑO") Then
                                                    estadoAnterior = "ADMINISTRACION"
                                                    estadoPosterior = "IMPRESION"
                                                ElseIf item.estadoOrden.Equals("IMPRESION") Then
                                                    estadoAnterior = "DISEÑO"
                                                    estadoPosterior = "ACABADO"
                                                ElseIf item.estadoOrden.Equals("ACABADO") Then
                                                    estadoAnterior = "IMPRESION"
                                                    estadoPosterior = "BODEGA"
                                                ElseIf item.estadoOrden.Equals("BODEGA") Then
                                                    estadoAnterior = "ACABADO"
                                                    estadoPosterior = "BODEGA"
                                                End If
                                                @<tr>
                                                    <td>@item.numeroOrden</td>
                                                    <td>@item.fechaCreacion</td>
                                                    <td align="center">EN @item.estadoOrden</td>
                                                    <td>@item.nombreCliente</td>
                                                    <td>@item.nombreUsuario</td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Ver", "VerOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Avanzar al área de " + estadoPosterior, "AvanzarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoPosterior}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Regresar al área de " + estadoAnterior, "RegresarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoAnterior}, New With {.class = "badge badge-danger col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            Next
                                        End If
                                    End IF
                                    @If Session("accesos") <> Nothing Then
                                        @If Session("accesos").ToString().Contains("DISEÑO") Then

                                            @For Each item In Model
                                                Dim estadoAnterior As String = "DISEÑO"
                                                Dim estadoPosterior As String = "DISEÑO"

                                                If item.estadoOrden.Equals("DISEÑO") Then
                                                    estadoAnterior = "ADMINISTRACION"
                                                    estadoPosterior = "IMPRESION"

                                                End If
                                                @<tr>
                                                    <td>@item.numeroOrden</td>
                                                    <td>@item.fechaCreacion</td>
                                                    <td align="center">EN @item.estadoOrden</td>
                                                    <td>@item.nombreCliente</td>
                                                    <td>@item.nombreUsuario</td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Ver", "VerOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Avanzar al área de " + estadoPosterior, "AvanzarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoPosterior}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Regresar al área de " + estadoAnterior, "RegresarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoAnterior}, New With {.class = "badge badge-danger col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            Next
                                        End If
                                    End IF

                                    @If Session("accesos") <> Nothing Then
                                        @If Session("accesos").ToString().Contains("IMPRESION") Then

                                            @For Each item In Model
                                                Dim estadoAnterior As String = "DISEÑO"
                                                Dim estadoPosterior As String = "DISEÑO"

                                                If item.estadoOrden.Equals("IMPRESION") Then
                                                    estadoAnterior = "DISEÑO"
                                                    estadoPosterior = "ACABADO"
                                                End If
                                                @<tr>
                                                    <td>@item.numeroOrden</td>
                                                    <td>@item.fechaCreacion</td>
                                                    <td align="center">EN @item.estadoOrden</td>
                                                    <td>@item.nombreCliente</td>
                                                    <td>@item.nombreUsuario</td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Ver", "VerOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Avanzar al área de " + estadoPosterior, "AvanzarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoPosterior}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Regresar al área de " + estadoAnterior, "RegresarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoAnterior}, New With {.class = "badge badge-danger col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            Next
                                        End If
                                    End IF
                                    @If Session("accesos") <> Nothing Then
                                        @If Session("accesos").ToString().Contains("ACABADO") Then

                                            @For Each item In Model
                                                Dim estadoAnterior As String = "DISEÑO"
                                                Dim estadoPosterior As String = "DISEÑO"

                                                If item.estadoOrden.Equals("ACABADO") Then
                                                    estadoAnterior = "IMPRESION"
                                                    estadoPosterior = "BODEGA"
                                                End If
                                                @<tr>
                                                    <td>@item.numeroOrden</td>
                                                    <td>@item.fechaCreacion</td>
                                                    <td align="center">EN @item.estadoOrden</td>
                                                    <td>@item.nombreCliente</td>
                                                    <td>@item.nombreUsuario</td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Ver", "VerOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Avanzar al área de " + estadoPosterior, "AvanzarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoPosterior}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Regresar al área de " + estadoAnterior, "RegresarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoAnterior}, New With {.class = "badge badge-danger col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            Next
                                        End If
                                    End IF
                                    @If Session("accesos") <> Nothing Then
                                        @If Session("accesos").ToString().Contains("BODEGA") Then

                                            @For Each item In Model
                                                Dim estadoAnterior As String = "DISEÑO"
                                                Dim estadoPosterior As String = "DISEÑO"

                                                If item.estadoOrden.Equals("BODEGA") Then
                                                    estadoAnterior = "ACABADO"
                                                    estadoPosterior = "BODEGA"
                                                End If
                                                @<tr>
                                                    <td>@item.numeroOrden</td>
                                                    <td>@item.fechaCreacion</td>
                                                    <td align="center">EN @item.estadoOrden</td>
                                                    <td>@item.nombreCliente</td>
                                                    <td>@item.nombreUsuario</td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Ver", "VerOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Avanzar al área de " + estadoPosterior, "AvanzarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoPosterior}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Regresar al área de " + estadoAnterior, "RegresarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoAnterior}, New With {.class = "badge badge-danger col-md-12"})
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            Next
                                        End If
                                    End IF
                                </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
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
