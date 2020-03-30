﻿@Code
    ViewData("Title") = "Enviar a producción | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Enviar a producción</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
          @Using Html.BeginForm("EnviarAProduccion", "Cotizaciones", FormMethod.Post, New With {.id = "form"})
            @<div id="wizard">
    <h1>Datos complementarios</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-6">
                <Label class="font-normal"><strong>Lugar de entrega:</strong></Label>
                <input id="lugarEntrega" name="lugarEntrega" type="text" class="form-control required">
            </div>
            <div class="col-lg-2">
                <Label class="font-normal"><strong>Fecha de entrega:</strong></Label>
                <input type="date" class="form-control" required id="fechaEntrega" name="fechaEntrega" value="@DateTime.Now.ToString("dd/MM/yyyy")" />
            </div>
            <div class="col-lg-2">
                <Label class="font-normal"><strong>Cantidad:</strong></Label>
                <input type="number" class="form-control" required value="0" min="0" id="cantidad" name="cantidad" />
            </div>
            <div class="col-lg-2">
                <Label class="font-normal"><strong>Número de páginas:</strong></Label>
                <input type="number" class="form-control" required value="0" min="0" id="numeroPaginas" name="numeroPaginas" />
            </div>
            <div class="col-lg-2">
                <br>
                <Label class="font-normal"><strong>Orientación:</strong></Label>
                <select class="form-control" required id="orientacion" name="orientacion">
                    <option value=""> ---SELECCIONE - --</option>
                    <option value="VERTICAL" selected>VERTICAL</option>
                    <option value="HORIZONTAL"> HORIZONTAL</option>
                </select>
            </div>
            <div class="col-lg-2">
                <br>
                <Label class="font-normal"><strong>Prioridad:</strong></Label>
                <select class="form-control" required id="prioridad" name="prioridad">
                    <option value=""> ---SELECCIONE - --</option>
                    <option value="URGENTE" selected>URGENTE</option>
                    <option value="NORMAL"> NORMAL</option>
                </select>
            </div>
            <div class="col-lg-2">
                <br>
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required  id="tamaño" name="tamaño" />
            </div>
        </div>
    </div>
    <h1> Material</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Portada:</strong></Label>
                <input type="text" class="form-control" required value="" id="materialPortada" name="materialPortada" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Gramaje:</strong></Label>
                <input type="text" class="form-control" required value="" id="gramajePortada" name="gramajePortada" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Color:</strong></Label>
                <input type="text" class="form-control" required value="" id="colorPortada" name="colorPortada" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required value="" id="tamañoPortada" name="tamañoPortada" />
            </div>

        </div>
        <div class="row">
            <br>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Interior:</strong></Label>
                <input type="text" class="form-control" required value="" id="materialInterior" name="materialInterior" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Gramaje:</strong></Label>
                <input type="text" class="form-control" required value="" id="gramajeInterior" name="gramajeInterior" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Color:</strong></Label>
                <input type="text" class="form-control" required value="" id="colorInterior" name="colorInterior" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required value="" id="tamañoInterior" name="tamañoInterior" />
            </div>

        </div>
        <div class="row">
            <br>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Otro:</strong></Label>
                <input type="text" class="form-control" required value="" id="materialOtro" name="materialOtro" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Gramaje:</strong></Label>
                <input type="text" class="form-control" required value="" id="gramajeOtro" name="gramajeOtro" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Color:</strong></Label>
                <input type="text" class="form-control" required value="" id="colorOtro" name="colorOtro" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required value="" id="tamañoOtro" name="tamañoOtro" />
            </div>

        </div>
        <div class="row">
            <br>
            <div class="col-lg-12">
                <h3> Cantidad de resmas a utilizar:</h3>
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Portada:</strong></Label>
                <input type="text" class="form-control" required value="" id="cantidadResmasPortada" name="cantidadResmasPortada" />
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Interior:</strong></Label>
                <input type="text" class="form-control" required value="" id="cantidadResmasInterior" name="cantidadResmasInterior" />
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Otro:</strong></Label>
                <input type="number" class="form-control" required value="" id="cantidadResmasOtro" name="cantidadResmasOtro" />
            </div>
        </div>
    </div>
    <h1> Color</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-4">
                <h3> Portada :    </h3>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Full color:</strong></Label>
                    <input class="form-control" type="text" required id="fullColorPortada" name="fullColorPortada" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Duotono:</strong></Label>
                    <input class="form-control" type="text" required id="duotonoPortada" name="duotonoPortada" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Un color:</strong></Label>
                    <input class="form-control" type="text" required id="uniColorPortada" name="uniColorPortada" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Pantone:</strong></Label>
                    <input class="form-control" type="text" required id="pantonePortada" name="pantonePortada" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Cantidad de tinta:</strong></Label>
                    <input class="form-control" type="number" required id="cantidadTintaPortada" name="cantidadTintaPortada" />
                </div>

            </div>
            <div class="col-lg-4">
                <h3> Interior :    </h3>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Full color:</strong></Label>
                    <input class="form-control" type="text" required id="fullColorInterior" name="fullColorInterior" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Duotono:</strong></Label>
                    <input class="form-control" type="text" required id="duotonoInterior" name="duotonoInterior" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Un color:</strong></Label>
                    <input class="form-control" type="text" required id="uniColorInterior" name="uniColorInterior" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Pantone:</strong></Label>
                    <input class="form-control" type="text" required id="pantoneInterior" name="pantoneInterior" />
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Cantidad de tinta:</strong></Label>
                    <input class="form-control" type="number" required id="cantidadTintaInterior" name="cantidadTintaInterior" />
                </div>
            </div>
            <div class="col-lg-4">
                <h3> Acabado de portada:</h3>
                <div class="col-lg-12">
                    <input type="radio" id="acabadoPortada" name="acabadoPortada" value="BARNIZ MATE">
                    <Label for="acabadoPortada">Barníz mate</Label><br>
                    <input type="radio" id="acabadoPortada" name="acabadoPortada" value="BARNIZ BRILLANTE">
                    <Label for="acabadoPortada">Barníz brillante</Label><br>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Cantidad:</strong></Label>
                    <input class="form-control" type="number" required id="cantidadAcabadoPortada" name="cantidadAcabadoPortada" />
                </div>
            </div>
        </div>
    </div>
    <h1> Diseño e impresión</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-4">
                <h3> Diseño :    </h3>
                <Label class="font-normal"><strong>Destino final:</strong></Label>
                <div class="col-lg-12">
                    <input type="checkbox" id="diseñoDiseño" name="diseñoDiseño" value="DISEÑO">
                    <Label for="diseñoDiseño">Diseño</Label><br>
                    <input type="checkbox" id="diseñoImpDigital" name="diseñoImpDigital" value="IMP. DIGITAL">
                    <Label for="diseñoImpDigital">Imp. digital</Label><br>
                    <input type="checkbox" id="diseñoCTP" name="diseñoCTP" value="CTP">
                    <Label for="diseñoCTP">CTP</Label><br>
                    <input type="checkbox" id="diseñoReimpresion" name="diseñoReimpresion" value="REIMPRESIÓN">
                    <Label for="diseñoReimpresion">Reimpresión</Label><br>
                    <input type="checkbox" id="diseñoPrensa" name="diseñoPrensa" value="PRENSA">
                    <Label for="diseñoPrensa">Prensa</Label><br>
                </div>
            </div>
            <div class="col-lg-4">
                <h3> Impresión :    </h3>
                <Label class="font-normal"><strong>Portada:</strong></Label>
                <div class="col-lg-12">
                    <input type="checkbox" id="tiroRetiroPortada" name="tiroRetiroPortada" value="TIRO/RETIRO">
                    <Label for="tiroRetiroPortada">Tiro/retiro</Label><br>
                    <input type="checkbox" id="tiroPortada" name="tiroPortada" value="TIRO">
                    <Label for="tiroPortada">Tiro</Label><br>
                </div>
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Interior:</strong></Label>
                <div class="col-lg-12">
                    <input type="checkbox" id="tiroRetiroInterior" name="tiroRetiroInterior" value="TIRO/RETIRO">
                    <Label for="tiroRetiroInterior">Tiro/retiro</Label><br>
                    <input type="checkbox" id="tiroInterior" name="tiroInterior" value="TIRO">
                    <Label for="tiroInterior">Tiro</Label><br>
                </div>
            </div>
            <div class="col-lg-4">

            </div>
            <div class="col-lg-8">
                <Label class="font-normal"><strong>Cantidad a imprimir (Ya incluye excedente):</strong></Label>
                <input class="form-control" type="number" required id="cantidadImprimir" name="cantidadImprimir" />
            </div>
        </div>
    </div>
    <h1> Encuadernación</h1>
    <div class="step-content">
        <div class="col-lg-4">
            <div class="col-lg-12">
                <input type="checkbox" id="plegado" name="plegado" value="PLEGADO">
                <Label for="plegado">Plegado</Label><br>
                <input type="checkbox" id="perforado" name="perforado" value="PERFORADO">
                <Label for="perforado">Perforado</Label><br>
                <input type="checkbox" id="pegado" name="pegado" value="PEGADO">
                <Label for="pegado">Pegado</Label><br>
                <input type="checkbox" id="grapado" name="grapado" value="GRAPADO">
                <Label for="grapado">Grapado</Label><br>
                <input type="checkbox" id="alzado" name="alzado" value="ALZADO">
                <Label for="alzado">Alzado</Label><br>
                <input type="checkbox" id="numerado" name="numerado" value="NUMERADO">
                <Label for="numerado">Numerado</Label><br>
                <input type="checkbox" id="cortado" name="cortado" value="CORTADO">
                <Label for="cortado">Cortado</Label><br>
                <input type="checkbox" id="empacado" name="empacado" value="EMPACADO">
                <Label for="empacado">Empacado</Label><br>
            </div>
        </div>
    </div>
    <h1> Observaciones específicas</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-12">
                <Label class="font-normal"><strong>Observaciones:</strong></Label>
                <textarea class="form-control" type="text" id="observacionesEspecificas" name="observacionesEspecificas" rows="3" required></textarea>
            </div>
        </div>
    </div>
</div>
          End Using
    </div>
</div>
@Section Styles
    @Styles.Render("~/plugins/wizardStepsStyles")
    @Styles.Render("~/plugins/awesomeCheckboxStyles")
    @Styles.Render("~/plugins/clockpickerStyles")
    @Styles.Render("~/plugins/dateRangeStyles")
    @Styles.Render("~/Content/plugins/iCheck/iCheckStyles")
    @Styles.Render("~/Content/plugins/chosen/chosenStyles")
    @Styles.Render("~/plugins/switcheryStyles")
    @Styles.Render("~/plugins/jasnyBootstrapStyles")
    @Styles.Render("~/plugins/nouiSliderStyles")
    @Styles.Render("~/plugins/dataPickerStyles")
    @Styles.Render("~/Content/plugins/ionRangeSlider/ionRangeStyles")
    @Styles.Render("~/plugins/imagecropperStyles")
    @Styles.Render("~/Content/plugins/colorpicker/colorpickerStyles")
    @Styles.Render("~/plugins/select2Styles")
    @Styles.Render("~/plugins/touchSpinStyles")
    @Styles.Render("~/plugins/tagInputsStyles")
    @Styles.Render("~/plugins/duallistStyles")
End Section

@Section Scripts
    @Scripts.Render("~/plugins/dataPicker")
    @Scripts.Render("~/plugins/ionRange")
    @Scripts.Render("~/plugins/nouiSlider")
    @Scripts.Render("~/plugins/jasnyBootstrap")
    @Scripts.Render("~/plugins/switchery")
    @Scripts.Render("~/plugins/chosen")
    @Scripts.Render("~/plugins/knob")
    @Scripts.Render("~/plugins/imagecropper")
    @Scripts.Render("~/plugins/colorpicker")
    @Scripts.Render("~/plugins/clockpicker")
    @Scripts.Render("~/plugins/dateRange")
    @Scripts.Render("~/plugins/select2")
    @Scripts.Render("~/plugins/touchSpin")
    @Scripts.Render("~/plugins/tagInputs")
    @Scripts.Render("~/plugins/duallist")
<script>
    console.log("@Session("prueba")")
    </script>
    <script type="text/javascript">
        $(document).ready(function () {


            $('#data_1 .input-group.date').datepicker({
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true,
                dateFormat: 'dd/mm/yyyy'
            });

            $('input[name="daterange"]').daterangepicker();


            $('#data_2 .input-group.date').datepicker({
                startView: 1,
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                autoclose: true,
                format: 'dd/mm/yyyy'
            });

        });

    </script>
    @Scripts.Render("~/plugins/wizardSteps")
    @Scripts.Render("~/plugins/iCheck")
    @Scripts.Render("~/plugins/validate")
    <script type="text/javascript">
        var settings = {
            onFinished: function (event, currentIndex) {
                $("#form").submit();
            },
            labels: {
                cancel: "Cancelar",
                current: "Paso actual:",
                pagination: "Paginación",
                finish: "Finalizar",
                next: "Siguiente",
                previous: "Anterior",
                loading: "Cargando ..."
            }
        

        };

        $(document).ready(function () {
            $("#wizard").steps(settings);
            $("#form").steps({
                onFinished: function (event, currentIndex) {
                    var form = $(this);
                    form.submit();
                }
            })

        });

    </script>
End Section
