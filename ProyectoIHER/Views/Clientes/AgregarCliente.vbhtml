﻿@Code
    ViewData("Title") = "Agregar Cliente | Imprenta IHER"
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
    If Session("mensaje").ToString().Equals("Cliente agregado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡Cliente creado exitosamente bajo los estándares del sistema!",
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
        <h3> <strong>Agregar cliente</strong></h3>
        <div Class="ibox-tools">
            <a Class="collapse-link">
                <i Class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div Class="ibox-content">
        @Using Html.BeginForm("AgregarCliente", "Clientes", FormMethod.Post, New With {.class = "needs-validation", .novalidate = "novalidate"})
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-5" id="data_5">
                            <label class="font-normal"><strong>Nombre:</strong></label>
                            <input type="text" class="form-control" id="nombreCliente" name="nombreCliente" required placeholder="Nombre" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, ' ');" oninput="setCustomValidity('')" />
                            <div class="invalid-feedback">
                                Debe proporcionar el nombre del cliente
                            </div>
                        </div>
                        <div class="col-md-3" id="data_5">

                            <label class="font-normal"><strong>RTN:</strong></label>
                            <input type="text" class="form-control" id="rtnCliente" name="rtnCliente" required placeholder="RTN" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, '');"  oninput="setCustomValidity('')" />
                        </div>
                        <div class="col-md-4" id="data_5">
                            <label class="font-normal"><strong>Dirección:</strong></label>
                            <input type="text" class="form-control" id="direccionCliente" name="direccionCliente" required placeholder="Dirección" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, ' ');" oninvalid="this.setCustomValidity('Dirección')" oninput="setCustomValidity('')" />
                        </div>
                        <div class="col-md-4" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Teléfono:</strong></label>
                            <input type="text" class="form-control" id="telefonoCliente" name="telefonoCliente" required placeholder="Teléfono" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, '');" oninvalid="this.setCustomValidity('Número de telefono')" oninput="setCustomValidity('')" />
                        </div>
                        <div class="col-md-4" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Correo electrónico:</strong></label>
                            <input type="email" class="form-control" id="correo" name="correo" maxlength="50" required placeholder="Correo electrónico" onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, '');" />
                        </div>


                        <div class="col-md-4" id="data_5">
                            <br>
                            <label class="font-normal"><strong>Nacionalidad:</strong></label>
                            <input type="text" placeholder="Nacionalidad..." class="form-control" id="nacionalidad" name="nacionalidad" required onkeyup="this.value = this.value.toUpperCase().replace(/\s+$/, '');" oninvalid="this.setCustomValidity('Nacionalidad')" oninput="setCustomValidity('')" />
                        </div>
                        <div class="col-md-5">
                            <br>
                            <br>
                            <button class="btn btn-primary" type="submit"><span><i class="fa fa-save" aria-hidden="true"></i></span> Guardar</button>
                            <button class="btn btn-danger" type="button" onclick="window.location='/Inicio/Principal';"><span><i class="fa fa-times" aria-hidden="true"></i></span> Cancelar</button>

                        </div>
                    </div>

                </div>
            </div>
        End Using
    </div>
</div>
<script>

@Section Scripts
                                @Scripts.Render("~/plugins/sweetAlert")
</script>
<style>
                                @Styles.Render("~/plugins/sweetAlertStyles")
</style>
<script>
    $('input#nombreCliente')
        .keypress(function (event) {
            if (event.which == 49 || event.which == 50 || event.which == 51 || event.which == 52
                || event.which == 53 || event.which == 54 || event.which == 55 || event.which == 56 || event.which == 57 || event.which == 48) {
                return false;
            }
        });


</script>
<script>
    $(function () {
        $('#nombreCliente').on('keyup', function (e) {
            var $th = $(this);
            $th.val($th.val().replace(/(\s{2,})|[^a-zA-Z']/g, ' '));
            $th.val($th.val().replace(/^\s*/, ''));
        });
    });
</script>
<script>
    $('input#telefonoCliente')
        .keypress(function (event) {
            if (event.which < 48 || event.which > 57 || this.value.length === 8) {
                return false;
            }
        });


</script>
<script>
    $('input#rtnCliente')
        .keypress(function (event) {
            if (event.which < 48 || event.which > 57 || this.value.length === 15) {
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

<script>
    (function () {
        'use strict';
        window.addEventListener('load', function () {
            var forms = document.getElementsByClassName('needs-validation');
            var validation = Array.prototype.filter.call(forms, function (form) {
                form.addEventListener('submit', function (event) {
                    if (form.checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                    form.classList.add('was-validated');
                }, false);
            });
        }, false);
    })();
</script>
End Section