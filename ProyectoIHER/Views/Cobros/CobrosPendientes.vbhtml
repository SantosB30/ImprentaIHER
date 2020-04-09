@Code

    ViewData("Title") = "Cobros Pendientes | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.CobrosModel)

End Code

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Buscar Cotizaciones</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
        @Using Html.BeginForm("CobrosPendientes", "Cobros", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered table-hover dataTables-example">
                                <thead>
                                    <tr>
                                        

                                        <td align="center"><strong>Número orden</strong></td>
                                        <td align="center"><strong>Nombre Cliente</strong></td>
                                        <td align="center"><strong>Telefono</strong></td>
                                        <td align="center"><strong>Correo</strong></td>
                                        <td align="center"><strong>Tipo pago</strong></td>
                                        <td align="center"><strong>Total a pagar</strong></td>
                                        
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each item In Model
                                        @<tr>
                                        <td>@item.Numero_Orden</td>
                                        <td>@item.Nombre_Cliente</td>
                                        <td>@item.Telefono_Cliente</td>
                                        <td>@item.Correo_Cliente</td>
                                        <td>@item.Tipo_Pago</td>
                                        <td>@item.Total_Cotizacion</td>
                                        
    
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
                    { extend: 'excel', title: 'Cotizaciones' }
                ]

            });



        });

    </script>
End Section