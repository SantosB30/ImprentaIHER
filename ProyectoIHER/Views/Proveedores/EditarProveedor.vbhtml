@Code
    ViewData("Title") = "Editar Proveedor  | Imprenta IHER"
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
@If Session("mensaje") <> Nothing Then
    If Session("mensaje").ToString().Equals("Proveedor editado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Proveedor creado exitosamente bajo los estándares del sistema!",
                     type: "success"
                 });
             };
        </script>
        Session("mensaje") = Nothing
    Else
        @<script>
             window.onload = function () {
                 swal({
                     title: "¡Error!",
                     text: "¡Ha ocurrido un error!",
                     type: "error"
                 });
             };
        </script>
        @<h3>@Session("mensaje")</h3>
        Session("mensaje") = Nothing
    End If
End If
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
<div Class="ibox float-e-margins">
    <div Class="ibox-title">
        <h3> <strong>Editar proveedor</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("EditarProveedor", "Proveedores", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Nombre:</strong></label>
                            <input type="text" class="form-control" id="nombreProveedor" name="nombreProveedor" required value="@Session("proveedorEditar")" placeholder="Nombre" onkeyup="this.value = this.value.toUpperCase();" oninvalid="this.setCustomValidity('Nombre completo del proveedor')" oninput="setCustomValidity('')"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Dirección:</strong></label>
                            <input type="text" class="form-control" id="direccionProveedor" name="direccionProveedor" required value="@Session("direccionProveedor")" placeholder="Dirección" onkeyup="this.value = this.value.toUpperCase();" oninvalid="this.setCustomValidity('Dirección')" oninput="setCustomValidity('')"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Teléfono:</strong></label>
                            <input type="text" class="form-control" id="telefonoProveedor" name="telefonoProveedor" required value="@Session("telefonoProveedor")" placeholder="Teléfono" onkeyup="this.value = this.value.toUpperCase();" oninvalid="this.setCustomValidity('Número de telefono')" oninput="setCustomValidity('')"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Correo electrónico:</strong></label>
                            <input type="email" class="form-control" id="correoProveedor" name="correoProveedor" maxlength="50" required value="@Session("correoProveedor")" placeholder="Correo electrónico" onkeyup="this.value = this.value.toUpperCase();" />
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Nombre contacto:</strong></label>
                            <input type="text" class="form-control" id="nombreContactoProveedor" name="nombreContactoProveedor" required value="@Session("nombreContactoProveedor")" placeholder="Nombre de contacto" onkeyup="this.value = this.value.toUpperCase();" oninvalid="this.setCustomValidity('Nombre completo del contacto')" oninput="setCustomValidity('')"/>
                        </div>
                        <div class="col-md-5" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Teléfono contacto:</strong></label>
                            <input type="text" class="form-control" id="telefonoContactoProveedor" name="telefonoContactoProveedor" required value="@Session("telefonoContactoProveedor")" placeholder="Teléfono de contacto" onkeyup="this.value = this.value.toUpperCase();" oninvalid="this.setCustomValidity('Número de telefono del contacto')" oninput="setCustomValidity('')"/>
                        </div>
                        <div Class="col-md-3" id="data_5">
                            <br>
                            <Label Class="font-normal"><strong>Estado:</strong></Label>
                            <select Class="form-control" id="estado" name="estado" required="required">
                                @If Session("estadoProveedor").ToString().Equals("ACTIVO") Then
                                    @<option value="ACTIVO" selected>ACTIVO</option>
                                    @<option value="INACTIVO">INACTIVO</option>
                                Else
                                    @<option value="ACTIVO">ACTIVO</option>
                                    @<option value="INACTIVO" selected>INACTIVO</option>

                                End If
                            </select>
                        </div>
                        <div class="col-md-5">
                            <br>
                            <br>
                            <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
                        </div>
                    </div>
                </div>
            </div>
        End Using
    </div>
</div>

@Section Styles
    @Styles.Render("~/plugins/sweetAlertStyles")
End Section

@Section Scripts
    @Scripts.Render("~/plugins/sweetAlert")
    <script>
        $('input#nombreProveedor')
            .keypress(function (event) {
                if (event.which == 49 || event.which == 50 || event.which == 51 || event.which == 52
                    || event.which == 53 || event.which == 54 || event.which == 55 || event.which == 56 || event.which == 57 || event.which == 48) {
                    return false;
                }
            });


    </script>
    <script>
        $('input#nombreContactoProveedor')
            .keypress(function (event) {
                if (event.which == 49 || event.which == 50 || event.which == 51 || event.which == 52
                    || event.which == 53 || event.which == 54 || event.which == 55 || event.which == 56 || event.which == 57 || event.which == 48) {
                    return false;
                }
            });


    </script>
    <script>
        $(function () {
            $('#password').on('keypress', function (e) {
                if (e.which == 32) {
                    return false;
                }
            });
        });

    </script>
    <script>
        $('input#telefonoProveedor')
            .keypress(function (event) {
                if (event.which < 48 || event.which > 57 || this.value.length === 8) {
                    return false;
                }
            });


    </script>
    <script>
        $('input#telefonoContactoProveedor')
            .keypress(function (event) {
                if (event.which < 48 || event.which > 57 || this.value.length === 8) {
                    return false;
                }
            });


    </script>
    <script>
        $(function () {
            $('#correo').on('keypress', function (e) {
                if (e.which == 32) {
                    return false;
                }
            });
        });
    </script>
    <script>
        $(function () {
            $('#usuario').on('keypress', function (e) {
                if (e.which == 32) {
                    return false;
                }
            });
        });
    </script>
    <script>
        $(function () {
            $('input[type="text"]').change(function () {
                this.value = $.trim(this.value);
            });
        });
    </script>
    <script>
        function autocomplete(inp, arr) {

            var currentFocus;
            inp.addEventListener("input", function (e) {
                var a, b, i, val = this.value;
                closeAllLists();
                if (!val) { return false; }
                currentFocus = -1;
                a = document.createElement("DIV");
                a.setAttribute("id", this.id + "autocomplete-list");
                a.setAttribute("class", "autocomplete-items");
                this.parentNode.appendChild(a);
                for (i = 0; i < arr.length; i++) {
                    if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                        b = document.createElement("DIV");
                        b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                        b.innerHTML += arr[i].substr(val.length);
                        b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                        b.addEventListener("click", function (e) {
                            inp.value = this.getElementsByTagName("input")[0].value;
                            closeAllLists();
                        });
                        a.appendChild(b);
                    }
                }
            });
            inp.addEventListener("keydown", function (e) {
                var x = document.getElementById(this.id + "autocomplete-list");
                if (x) x = x.getElementsByTagName("div");
                if (e.keyCode == 40) {
                    currentFocus++;
                    addActive(x);
                } else if (e.keyCode == 38) { //up
                    currentFocus--;
                    addActive(x);
                } else if (e.keyCode == 13) {
                    e.preventDefault();
                    if (currentFocus > -1) {
                        if (x) x[currentFocus].click();
                    }
                }
            });
            function addActive(x) {
                if (!x) return false;
                removeActive(x);
                if (currentFocus >= x.length) currentFocus = 0;
                if (currentFocus < 0) currentFocus = (x.length - 1);
                x[currentFocus].classList.add("autocomplete-active");
            }
            function removeActive(x) {
                for (var i = 0; i < x.length; i++) {
                    x[i].classList.remove("autocomplete-active");
                }
            }
            function closeAllLists(elmnt) {
                var x = document.getElementsByClassName("autocomplete-items");
                for (var i = 0; i < x.length; i++) {
                    if (elmnt != x[i] && elmnt != inp) {
                        x[i].parentNode.removeChild(x[i]);
                    }
                }
            }
            /*execute a function when someone clicks in the document:*/
            document.addEventListener("click", function (e) {
                closeAllLists(e.target);
            });
        }

        var countries = ["Afganistán", "Albania", "Alemania", "Andorra", "Angola", "Antigua y Barbuda", "Arabia Saudita", "Argelia", "Argentina", "Armenia", "Australia", "Austria", "Azerbaiyán", "Bahamas", "Bangladés", "Barbados", "Baréin", "Bélgica", "Belice", "Benín", "Bielorrusia", "Birmania", "Bolivia", "Bosnia y Herzegovina", "Botsuana", "Brasil", "Brunéi", "Bulgaria", "Burkina Faso", "Burundi", "Bután", "Cabo Verde", "Camboya", "Camerún", "Canadá", "Catar", "Chad", "Chile", "China", "Chipre", "Ciudad del Vaticano", "Colombia", "Comoras", "Corea del Norte", "Corea del Sur", "Costa de Marfil", "Costa Rica", "Croacia", "Cuba", "Dinamarca", "Dominica", "Ecuador", "Egipto", "El Salvador", "Emiratos Árabes Unidos", "Eritrea", "Eslovaquia", "Eslovenia", "España", "Estados Unidos", "Estonia", "Etiopía", "Filipinas", "Finlandia", "Fiyi", "Francia", "Gabón", "Gambia", "Georgia", "Ghana", "Granada", "Grecia", "Guatemala", "Guyana", "Guinea", "Guinea ecuatorial", "Guinea-Bisáu", "Haití", "Honduras", "Hungría", "India", "Indonesia", "Irak", "Irán", "Irlanda", "Islandia", "Islas Marshall", "Islas Salomón", "Israel", "Italia", "Jamaica", "Japón", "Jordania", "Kazajistán", "Kenia", "Kirguistán", "Kiribati", "Kuwait", "Laos", "Lesoto", "Letonia", "Líbano", "Liberia", "Libia", "Liechtenstein", "Lituania", "Luxemburgo", "Macedonia del Norte", "Madagascar", "Malasia", "Malaui", "Maldivas", "Malí", "Malta", "Marruecos", "Mauricio", "Mauritania", "México", "Micronesia", "Moldavia", "Mónaco", "Mongolia", "Montenegro", "Mozambique", "Namibia", "Nauru", "Nepal", "Nicaragua", "Níger", "Nigeria", "Noruega", "Nueva Zelanda", "Omán", "Países Bajos", "Pakistán", "Palaos", "Panamá", "Papúa Nueva Guinea", "Paraguay", "Perú", "Polonia", "Portugal", "Reino Unido", "República Centroafricana", "República Checa", "República del Congo", "República Democrática del Congo", "República Dominicana", "Ruanda", "Rumanía", "Rusia", "Samoa", "San Cristóbal y Nieves", "San Marino", "San Vicente y las Granadinas", "Santa Lucía", "Santo Tomé y Príncipe", "Senegal", "Serbia", "Seychelles", "Sierra Leona", "Singapur", "Siria", "Somalia", "Sri Lanka", "Suazilandia", "Sudáfrica", "Sudán", "Sudán del Sur", "Suecia", "Suiza", "Surinam", "Tailandia", "Tanzania", "Tayikistán", "Timor Oriental", "Togo", "Tonga", "Trinidad y Tobago", "Túnez", "Turkmenistán", "Turquía", "Tuvalu", "Ucrania", "Uganda", "Uruguay", "Uzbekistán", "Vanuatu", "Venezuela", "Vietnam", "Yemen", "Yibuti", "Zambia", "Zimbabue"];

        autocomplete(document.getElementById("nacionalidad"), countries);
    </script>
End Section