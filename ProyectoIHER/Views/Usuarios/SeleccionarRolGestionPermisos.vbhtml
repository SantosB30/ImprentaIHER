@Code
    ViewData("Title") = "Selección de rol | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<style>


    /*the container must be positioned relative:*/
    .autocomplete {
        position: relative;
        display: inline-block;
    }


    .autocomplete-items {
        position: absolute;
        border: 1px solid #d4d4d4;
        border-bottom: none;
        border-top: none;
        z-index: 99;
        /*position the autocomplete items to be the same width as the container:*/
        top: 100%;
        left: 0;
        right: 0;
    }

        .autocomplete-items div {
            padding: 10px;
            cursor: pointer;
            background-color: #fff;
            border-bottom: 1px solid #d4d4d4;
        }

            /*when hovering an item:*/
            .autocomplete-items div:hover {
                background-color: #e9e9e9;
            }

    /*when navigating through the items using the arrow keys:*/
    .autocomplete-active {
        background-color: DodgerBlue !important;
        color: #ffffff;
    }
</style>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Selección de rol</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("SeleccionarRolGestionPermisos", "Usuarios", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Rol:</strong></label>
                            <select class="select2_demo_1 form-control" id="rol" name="rol" required>
                                <option value=""> ---SELECCIONE - --</option>
                                @Code Dim roles As List(Of String) = TempData("roles") End Code
                                @For Each rol As String In roles
                                    @<option value="@rol">@rol</option>

                                Next
                            </select>
                        </div>
                        <div class="col-md-5">
                            <br>
                            <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Aceptar</button>
                        </div>
                    </div>

                </div>
            </div>
                                    End Using
    </div>
</div>
@Section Styles
    @Styles.Render("~/plugins/select2Styles")
    @Styles.Render("~/Content/plugins/iCheck/iCheckStyles")
End Section

@Section Scripts
    @Scripts.Render("~/plugins/iCheck")
    <script type="text/javascript">
        $(document).ready(function () {

            $('.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-green',
                radioClass: 'iradio_square-green',
            });

        });
    </script>
    @Scripts.Render("~/plugins/select2")
    <script type="text/javascript">
        $(".select2_demo_1").select2();
        $(".select2_demo_2").select2();
    </script>

    <script type="text/JavaScript">
        var clicks = 0;
        var cantidadProductos = 1;
        function nuevoProducto() {
            // First create a DIV element.

            if (clicks <= 8) {
                clicks += 1;
                cantidadProductos = clicks + 1;
                var txtNewInputBox = document.createElement('div');

                txtNewInputBox.innerHTML = "<br><label class=\"font-normal\"><strong>" + cantidadProductos + ". Producto:</strong></label>" +
                    "<select class=\"select2_demo_1 form-control\" id=\"producto_" + clicks + "\" name=\"producto_" + clicks + "\" required></select>";

                document.getElementById("divProducto").appendChild(txtNewInputBox);
                var $options = $("#producto > option").clone();
                $('#producto_' + clicks).append($options);



                ///Insertando input precios
                var txtNewInputBox = document.createElement('div');

                txtNewInputBox.innerHTML = "<br>" +
                    "<label class=\"font-normal\"><strong>Precio:</strong></label>" +
                    "<input class=\"form-control\" type=\"number\" id=\"precioProducto_" + clicks + "\" name=\"precioProducto_" + clicks + "\" required value=\"0\" min=\"0\"/>";

                document.getElementById("divPrecioProducto").appendChild(txtNewInputBox);

                ///Insertando input cantidad
                var txtNewInputBox = document.createElement('div');

                txtNewInputBox.innerHTML = "<br>" +
                    "<label class=\"font-normal\"><strong>Cantidad:</strong></label>" +
                    "<input class=\"form-control\" type=\"number\" id=\"cantidadProducto_" + clicks + "\" name=\"cantidadProducto_" + clicks + "\" required value=\"0\" min=\"0\"/>";

                document.getElementById("divCantidadProducto").appendChild(txtNewInputBox);

                ///Insertando input subtotal
                ///var txtNewInputBox = document.createElement('div');

                //txtNewInputBox.innerHTML = "<br>" +
                //    "<label class=\"font-normal\"><strong>Subtotal:</strong></label>" +
                //    "<input class=\"form-control\" type=\"number\" id=\"subTotal_" + clicks + "\" name=\"subTotal_" + clicks + "\" required value=\"0\" min=\"0\" disabled/>";

                //document.getElementById("divSubTotalProducto").appendChild(txtNewInputBox);

                ///Insertando input comentario
                var txtNewInputBox = document.createElement('div');

                txtNewInputBox.innerHTML = "<br>" +
                    "<label class=\"font-normal\"><strong>Comentario:</strong></label>" +
                    "<input class=\"form-control\" type=\"text\" id=\"comentario_" + clicks + "\" name=\"comentario_" + clicks + "\" required/>";

                document.getElementById("divComentarioProducto").appendChild(txtNewInputBox);
                $('#producto_' + clicks).selectmenu("refresh");
            }
        }


        function eliminarProducto() {
            var elementos = $("#divProducto > div").length
            console.log(elementos)
            if (clicks > 0) {
                clicks -= 1;
                cantidadProductos -= 1;
            }
            document.getElementById("divProducto").removeChild(document.getElementById("divProducto").childNodes[elementos - 1])
            document.getElementById("divPrecioProducto").removeChild(document.getElementById("divPrecioProducto").childNodes[elementos - 1])
            document.getElementById("divCantidadProducto").removeChild(document.getElementById("divCantidadProducto").childNodes[elementos - 1])
            document.getElementById("divComentarioProducto").removeChild(document.getElementById("divComentarioProducto").childNodes[elementos - 1])
        }
    </script>
    <script type="text/JavaScript">
        var clicks = 0;
        function mostrarProducto() {
            clicks += 1;
            if (clicks <= 9) {
                document.getElementById("divProducto_" + clicks).style.display = "block";
                document.getElementById("divPrecioProducto_" + clicks).style.display = "block";
                document.getElementById("divCantidadProducto_" + clicks).style.display = "block";
                document.getElementById("divComentarioProducto_" + clicks).style.display = "block";
            }
            //$('#producto_' + clicks).css('width', '284.8px');

        }
    </script>


    <script>
                                        /*
                                        $(function () {
                                            $('#precioProducto, #cantidadProducto').keyup(function () {
                                                var precio = parseFloat($('#precioProducto').val()) || 0;
                                                var cantidad = parseFloat($('#cantidadProducto').val()) || 0;
                                                $('#subTotal').val(precio * cantidad);
                                            });
                                        });*/
    </script>
    <script>
/*
                                         $(document).on("keyup", "#cantidadProducto_1", function (event) {
                                             console.log("Invocado")
                                             var precio = parseFloat($('#precioProducto_1').val()) || 0;
                                             var cantidad = parseFloat($('#cantidadProducto_1').val()) || 0;
                                             $('#subTotal_1').val(precio * cantidad);
                                         }*/
    </script>


End Section
