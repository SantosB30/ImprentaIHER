@Code
    ViewData("Title") = "Nueva cotización | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Nueva cotización</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
        @Code Dim listaProductos As String = "" End Code
        @Using Html.BeginForm("NuevaCotizacion", "Cotizaciones", FormMethod.Post)
            @<div class="row">
                <div class="col-md-4" id="data_5">
                    <label class="font-normal"><strong>Cliente:</strong></label>
                    <select class="select2_demo_1 form-control" id="cliente" name="cliente" required>
                        <option value="">--- SELECCIONE ---</option>
                        @Code Dim clientes As List(Of String) = TempData("clientes") End Code
                        @For Each cliente As String In clientes
                            @<option value="@cliente">@cliente</option>
                        Next
                    </select>
                </div>
                <div class="col-md-2" id="data_5">
                    <label class="font-normal"><strong>Tipo de pago:</strong></label>
                    <select class="select2_demo_1 form-control" id="tipoPago" name="tipoPago" required>
                        <option value="">--- SELECCIONE ---</option>
                        <option value="CRÉDITO">CRÉDITO</option>
                        <option value="CONTADO">CONTADO</option>
                    </select>
                </div>
                <div class="col-md-4" id="data_5">
                    <label class="font-normal"><strong>Nombre contacto:</strong></label>
                    <input class="form-control" type="text" id="nombreContacto" name="nombreContacto" required onkeyup="this.value = this.value.toUpperCase();" />
                </div>
                <div class="col-md-2" id="data_5">
                    <label class="font-normal"><strong>Teléfono contacto:</strong></label>
                    <input class="form-control" type="number" id="telefonoContacto" name="telefonoContacto" required onkeyup="this.value = this.value.toUpperCase();" pattern="[0-9]{8}" />

                </div>
                <div class="col-md-3" id="data_5">
                    <br>
                    <label class="font-normal"><strong>1. Producto:</strong></label>
                    <select class="select2_demo_1 form-control" id="producto" name="producto" required>
                        <option value=""> ---SELECCIONE - --</option>
                        @Code Dim productos As List(Of String) = TempData("productos") End Code
                        @For Each producto As String In productos
                            @<option value="@producto">@producto</option>


                        Next
                    </select>
                </div>

                <div class="col-md-2" id="data_5">
                    <br>
                    <label class="font-normal"><strong>Precio:</strong></label>
                    <input class="form-control" type="number" id="precioProducto" name="precioProducto" required value="0" min="0" align="right" onkeyup="this.value = this.value.toUpperCase();" />
                </div>
                <div class="col-md-2" id="data_5">
                    <br>
                    <label class="font-normal"><strong>Cantidad:</strong></label>
                    <input class="form-control" type="number" id="cantidadProducto" name="cantidadProducto" required value="0" min="0" align="right" onkeyup="this.value = this.value.toUpperCase();" />
                </div>

                <div class="col-md-4" id="data_5">
                    <br>
                    <label class="font-normal"><strong>Comentario:</strong></label>
                    <input class="form-control" type="text" id="comentario" name="comentario" required onkeyup="this.value = this.value.toUpperCase();" />
                </div>

                @*@For cantidad As Double = 1 To 10 Step +1
                        @<div class="col-md-3" id="divProducto_@cantidad" style="display: none;">
                            <br>
                            <select class="select2_demo_1 form-control" id="producto_@cantidad" name="producto_@cantidad" required>
                                <option value=""> ---SELECCIONE - --</option>
                                @For Each producto As String In productos
                                    @<option value="@producto">@producto</option>
                                Next
                            </select>
                        </div>
                        @<div class="col-md-1" id="divPrecioProducto_@cantidad" style="display: none;">
                            <br>
                            <label class="font-normal"><strong>Precio:</strong></label>
                            <input class="form-control" type="number" id="precioProducto_@cantidad" name="precioProducto_@cantidad" required value="0" min="0" />
                        </div>
                        @<div class="col-md-1" id="divCantidadProducto_@cantidad" style="display: none;">
                            <br>
                            <label class="font-normal"><strong>Cantidad:</strong></label>
                            <input class="form-control" type="number" id="cantidadProducto_@cantidad" name="cantidadProducto_@cantidad" required value="0" min="0" />
                        </div>
                        @<div class="col-md-1" id="divSubTotalProducto_@cantidad" style="display: none;">
                            <br>
                            <label class="font-normal"><strong>Subtotal:</strong></label>
                            <input class="form-control" type="number" id="subTotal_@cantidad" name="subTotal_@cantidad" required disabled value="0" min="0" />
                        </div>
                        @<div class="col-md-5" id="divComentarioProducto_@cantidad" style="display: none;">
                            <br>
                            <label class="font-normal"><strong>Comentario:</strong></label>
                            <input class="form-control" type="text" id="comentario_@cantidad" name="comentario_@cantidad" required />
                        </div>

                    Next*@
                <div class="col-md-1" id="data_5">
                    <br>
                    <label class="font-normal"><strong>Producto:</strong></label>
                    <button class="btn btn-primary btn-circle" type="button" id="agregar" name="agregar" data-toggle="tooltip" data-placement="top" title="Añadir producto" onclick="nuevoProducto()">
                        <i class="fa fa-plus"></i>
                    </button>
                    <button class="btn btn-danger btn-circle" type="button" id="eliminar" name="eliminar" data-toggle="tooltip" data-placement="top" title="Eliminar producto" onclick="eliminarProducto()">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
                <!--Nuevos elementos-->

                <div class="col-md-3" id="divProducto">

                </div>
                <div class="col-md-2" id="divPrecioProducto">

                </div>
                <div class="col-md-2" id="divCantidadProducto">

                </div>

                <div class="col-md-4" id="divComentarioProducto">

                </div>
                <!---->
                <div class="col-md-2">
                    <br>
                    <label class="font-normal"><strong>¿Cliente exonerado?</strong></label>
                    <div class="radio i-checks"><label> <input type="radio" value="SI" name="exoneracion" id="exoneracion"> <i></i> Si </label></div>
                    <div class="radio i-checks"><label> <input type="radio" checked="" value="NO" name="exoneracion" id="exoneracion"> <i></i> No </label></div>
                </div>
                <div class="col-md-10" id="data_5">
                    <br>
                    <label class="font-normal"><strong>Observaciones:</strong></label>
                    <textarea class="form-control" type="text" id="observacion" name="observacion" rows="3" required onkeyup="this.value = this.value.toUpperCase();"></textarea>
                </div>
                <div class="col-md-12">
                    <br>
                    <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Generar</button>
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
    <script>
        $('input#nombreContacto')
            .keypress(function (event) {
                if (event.which == 49 || event.which == 50 || event.which == 51 || event.which == 52
                    || event.which == 53 || event.which == 54 || event.which == 55 || event.which == 56 || event.which == 57 || event.which == 48) {
                    return false;
                }
            });


    </script>
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
    <script>
        $('input#telefonoContacto')
            .keypress(function (event) {
                if (event.which < 48 || event.which > 57 || this.value.length === 8) {
                    return false;
                }
            });


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
        $(document).ready(function() {
        $('#producto').change(function() {
        var str = this.options[this.selectedIndex].value;
        $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
    });
});
    </script>
    <script>
        $(document).on("change", "#producto_1", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_1').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_2", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_2').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_3", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_3').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_4", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_4').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_5", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_5').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_6", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_6').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_7", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_7').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_8", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_8').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
    </script>
    <script>
        $(document).on("change", "#producto_9", function (event) {
             var str = this.options[this.selectedIndex].value;
            $.ajax('@Url.Action("ObtenerPrecioProducto", "Cotizaciones")', {
            type: 'POST',
            dataType: 'json',
            data: {'producto': str },
            success: function(data, status, jqXHR) {
                if ("success" === status) {
                    console.log(data.precio)
                   // document.getElementById('#precioProducto').value = data.precio;
                    $('#precioProducto_9').val(data.precio);

                } else {
                    console.log('No existe el producto selecionado!');
                }
            }
        });
        });
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
