@Code

    ViewData("Title") = "Permisos | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.AccesosModel)

End Code


<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Permisos, usuario: @Session("usuarioPermisos").ToString()</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("GestionPermisos", "Usuarios", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
               <button class="btn btn-primary" type="submit" name="submit" id="submit" value="exportar"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
               </div>
                            <div class="col-lg-12">
                    <div class="row">
                        <div class="table-responsive col-lg-12">
                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <td align="center"><strong>MÓDULO</strong></td>
                                        <td align="center"><strong>SECCIÓN</strong></td>
                                        <td align="center"><strong>¿ACCESO?</strong></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each item In Model
                                        @<tr>
                                            <td>@item.modulo</td>
                                            <td>@item.seccion</td>
                                            <td align="center">
                                                <select class="form-control col-md-12" name="@item.campo"  id="@item.campo">
                                                    <option value="NO">NO</option>
                                                    <option value="@item.acceso" selected>SI</option>
                                                </select>
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
End Section