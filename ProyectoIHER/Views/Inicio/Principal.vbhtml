@Code
    ViewData("Title") = "Home | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Enviado a producción") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡La cotización fue enviada a producción!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Flujo adelantado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El flujo de producción avanzó exitosamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Flujo retrasado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El flujo de producción retrocedió exitosamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Estado asignado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Se asignó el estado correctamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Equals("Flujo finalizado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El flujo de producción finalizó correctamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing

    ElseIf Session("mensaje").ToString().Contains("exitosamente") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "@Session("mensaje").ToString()",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Contains("excede") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Error",
                     text: "¡La cantidad de productos excede el disponible!",
                     type: "error"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Contains("Permisos actualizados") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Permisos actualizados exitosamente!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    ElseIf Session("mensaje").ToString().Contains("Orden editada") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Se editó la orden de producción!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    End If
End If

@If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
    @<div class="wrapper wrapper-content">
        <div class="row">

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#16a655">
                        <h5> <font color="white"> Ordenes Pendientes</font></h5>
                    </div>
                    <div Class="ibox-content ">
                        <h1 Class="no-margins">@Session("cantidadOrdenes").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de ordenes de prod. pendientes</small>
                    </div>
                </div>
            </div>

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#C71F07">
                        <h5> <font color="white"> Ordenes Urgentes</font></h5>
                    </div>
                    <div Class="ibox-content ">
                        <h1 Class="no-margins">@Session("cantidadOrdenesUrgente").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de ordenes de prod. urgentes</small>
                    </div>
                </div>
            </div>

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#046bc4">
                        <h5> <font color="white"> Ordenes Normales</font></h5>
                    </div>
                    <div Class="ibox-content ">
                        <h1 Class="no-margins">@Session("cantidadOrdenesNormales").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de ordenes de prod. normales</small>
                    </div>
                </div>
            </div>

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#f0a926">
                        <h5> <font color="white"> Productos</font></h5>
                    </div>
                    <div Class="ibox-content">
                        <h1 Class="no-margins">@Session("cantidadProductos").ToString()</h1>
                        <div class="stat-percent font-bold text-info"><i class="fa fa-level-up"></i></div>
                        <small>Total productos registrados</small>
                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="ibox float-e-margins Sombra">
                    <div class="ibox-title" style="background-color:#046bc4">
                        <h5><font color="white">Cobros Pendientes</font></h5>
                    </div>
                    <div class="ibox-content">
                        <h1 class="no-margins">@Session("cantidadCobrosPendientes").ToString()</h1>
                        <div class="stat-percent font-bold text-navy"><i class="fa fa-level-up"></i></div>
                        <small>Total cobros pendientes</small>
                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="ibox float-e-margins Sombra">
                    <div class="ibox-title" style="background-color:#f0a926">
                        <h5><font color="white">Usuarios</font></h5>
                    </div>
                    <div class="ibox-content">
                        <h1 class="no-margins">@Session("cantidadUsuarios").ToString()</h1>
                        <div class="stat-percent font-bold text-navy"><i class="fa fa-level-up"></i></div>
                        <small>Total usuarios registrados</small>
                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="ibox float-e-margins Sombra">
                    <div class="ibox-title" style="background-color:#16a655">
                        <h5><font color="white">Proveedores</font></h5>
                    </div>
                    <div class="ibox-content">
                        <h1 class="no-margins">@Session("cantidadProveedores").ToString()</h1>
                        <div class="stat-percent font-bold text-danger"><i class="fa fa-level-up"></i></div>
                        <small>Total proveedores registrados</small>
                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="ibox float-e-margins Sombra">
                    <div class="ibox-title" style="background-color:#046bc4">
                        <h5><font color="white">Clientes</font></h5>
                    </div>
                    <div class="ibox-content ">
                        <h1 class="no-margins">@Session("cantidadClientes").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de clientes registrados</small>
                    </div>
                </div>
            </div>

        </div>


    </div>
End If

@If Session("accesos").ToString().Contains("DISEÑO") Or Session("accesos").ToString().Contains("IMPRESION") Or Session("accesos").ToString().Contains("ACABADO") Or Session("accesos").ToString().Contains("BODEGA") Then
    @<div class="wrapper wrapper-content">
        <div class="row">

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#16a655">
                        <h5> <font color="white"> Ordenes Pendientes</font></h5>
                    </div>
                    <div Class="ibox-content ">
                        <h1 Class="no-margins">@Session("cantidadOrdenes").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de ordenes de prod. pendientes</small>

                    </div>
                </div>
            </div>

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#C71F07">
                        <h5> <font color="white"> Ordenes Urgentes</font></h5>
                    </div>
                    <div Class="ibox-content ">
                        <h1 Class="no-margins">@Session("cantidadOrdenesUrgente").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de ordenes de prod. urgentes</small>
                    </div>
                </div>
            </div>

            <div Class="col-lg-3">
                <div Class="ibox float-e-margins Sombra">
                    <div Class="ibox-title" style="background-color:#046bc4">
                        <h5> <font color="white"> Ordenes Normales</font></h5>
                    </div>
                    <div Class="ibox-content ">
                        <h1 Class="no-margins">@Session("cantidadOrdenesNormales").ToString()</h1>
                        <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                        <small>Total de ordenes de prod. normales</small>
                    </div>
                </div>
            </div>

        </div>


    </div>
End If



@Section Styles
    @Styles.Render("~/plugins/sweetAlertStyles")
End Section

@Section Scripts
    @Scripts.Render("~/plugins/sweetAlert")
    @Scripts.Render("~/plugins/flot")
    @Scripts.Render("~/plugins/vectorMap")



End Section


