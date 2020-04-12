@Code
    ViewData("Title") = "Editar cotización | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Editar cotización</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
        @Code Dim listaProductos As String = "" End Code
        @Using Html.BeginForm("EditarCotizacion", "Cotizaciones", FormMethod.Post)
            @<div class="row">
    <div class="col-md-4" id="data_5">
        <label class="font-normal"><strong>Cliente:</strong></label>
        <select class="select2_demo_1 form-control" id="cliente" name="cliente" required>
            <option value="">--- SELECCIONE ---</option>
            @Code Dim clientes As List(Of String) = TempData("clientes") End Code
            @For Each cliente As String In clientes
                If cliente.Equals(Session("cliente").ToString()) Then
                    @<option value="@cliente" selected>@cliente</option>
                Else
                    @<option value="@cliente">@cliente</option>
                End If
            Next
        </select>
    </div>
    <div class="col-md-2" id="data_5">
        <label class="font-normal"><strong>Tipo de pago:</strong></label>
        <select class="select2_demo_1 form-control" id="tipoPago" name="tipoPago" required>
            <option value="">--- SELECCIONE ---</option>
            @If Session("tipoPago").ToString().Equals("CRÉDITO") Then
                @<option value="CRÉDITO" selected> CRÉDITO</option>
                @<option value="CONTADO"> CONTADO</option>
            Else
                @<option value="CRÉDITO"> CRÉDITO</option>
                @<option value="CONTADO" selected> CONTADO</option>
            End If
        </select>
    </div>
    <div Class="col-md-4" id="data_5">
        <Label Class="font-normal"><strong>Nombre contacto:</strong></Label>
        <input Class="form-control" type="text" id="nombreContacto" name="nombreContacto" required onkeyup="this.value = this.value.toUpperCase();" value="@Session("nombreContacto").ToString()" />
    </div>
    <div Class="col-md-2" id="data_5">
        <Label Class="font-normal"><strong>Teléfono contacto:</strong></Label>
        <input Class="form-control" type="text" id="telefonoContacto" name="telefonoContacto" required onkeyup="this.value = this.value.toUpperCase();" pattern="[0-9]{8}" value="@Session("telefonoContacto").ToString()" />

    </div>
    @Code Dim nombreProductos As List(Of String) = TempData("nombreProductos") End Code
    @Code Dim precioProductos As List(Of String) = TempData("precioProductos") End Code
    @Code Dim cantidadProducto As List(Of String) = TempData("cantidadProducto") End Code
    @Code Dim comentarioProductos As List(Of String) = TempData("comentarioProductos") End Code
    @For cantidad As Double = 0 To Session("cantidadProductos") - 1 Step +1
        @<div class="col-md-3" id="divProducto_@cantidad">
            <br>
            <label class="font-normal"><strong>Producto:</strong></label>
            <select class="select2_demo_1 form-control" id="producto_@cantidad" name="producto_@cantidad" required>
                <option value=""> ---SELECCIONE - --</option>
                @Code Dim productos As List(Of String) = TempData("productos") End Code
                @For Each producto As String In productos
                    @If producto.Equals(nombreProductos(cantidad)) Then
                        @<option value="@producto" selected>@producto</option>
                    Else
                        @<option value="@producto">@producto</option>
                    End If
                Next
            </select>
        </div>
        @<div class="col-md-2" id="divPrecioProducto_@cantidad">
            <br>
            <label class="font-normal"><strong>Precio:</strong></label>
            <input class="form-control" type="number" id="precioProducto_@cantidad" name="precioProducto_@cantidad" required  min="0" value="@precioProductos(cantidad)" />
        </div>
        @<div class="col-md-2" id="divCantidadProducto_@cantidad">
            <br>
            <label class="font-normal"><strong>Cantidad:</strong></label>
            <input class="form-control" type="number" id="cantidadProducto_@cantidad" name="cantidadProducto_@cantidad" required value="@cantidadProducto(cantidad)" min="0" />
        </div>

        @<div class="col-md-5" id="divComentarioProducto_@cantidad">
            <br>
            <label class="font-normal"><strong>Comentario:</strong></label>
            <input class="form-control" type="text" id="comentario_@cantidad" name="comentario_@cantidad" required value="@comentarioProductos(cantidad)" />
        </div>

                    Next
    <!--Nuevos elementos-->
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
        <textarea class="form-control" type="text" id="observacion" name="observacion" rows="3" required onkeyup="this.value = this.value.toUpperCase();" value="@Session("observacion").ToString()"></textarea>
    </div>
    <div class="col-md-12">
        <br>
        <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
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
