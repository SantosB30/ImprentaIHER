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

<div class="wrapper wrapper-content">
    <div class="row">
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title" style="background-color:#055683">
                    <h5><font color="white">Clientes</font></h5>
                </div>
                <div class="ibox-content">
                    <h1 class="no-margins">@Session("cantidadClientes").ToString()</h1>
                    <div class="stat-percent font-bold text-success"><i class="fa fa-level-up"></i></div>
                    <small>Total clientes</small>
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title" style="background-color:#057C39">
                    <h5><font color="white">Productos</font></h5>
                </div>
                <div class="ibox-content">
                    <h1 class="no-margins">@Session("cantidadProductos").ToString()</h1>
                    <div class="stat-percent font-bold text-info"><i class="fa fa-level-up"></i></div>
                    <small>Total productos</small>
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title" style="background-color:#E4BE17">
                    <h5><font color="white">Usuarios</font></h5>
                </div>
                <div class="ibox-content">
                    <h1 class="no-margins">@Session("cantidadUsuarios").ToString()</h1>
                    <div class="stat-percent font-bold text-navy"><i class="fa fa-level-up"></i></div>
                    <small>Total usuarios</small>
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title" style="background-color:#C71F07">
                    <h5><font color="white">Proveedores</font></h5>
                </div>
                <div class="ibox-content">
                    <h1 class="no-margins">@Session("cantidadProveedores").ToString()</h1>
                    <div class="stat-percent font-bold text-danger"><i class="fa fa-level-up"></i></div>
                    <small>Total proveedores</small>
                </div>
            </div>
        </div>
    </div>


</div>



@Section Styles
    @Styles.Render("~/plugins/sweetAlertStyles")
End Section

@Section Scripts
    @Scripts.Render("~/plugins/sweetAlert")
    @Scripts.Render("~/plugins/flot")
    @Scripts.Render("~/plugins/vectorMap")

End Section


