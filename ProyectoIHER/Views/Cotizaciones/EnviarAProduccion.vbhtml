@Code
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
                <input id="lugarEntrega" name="lugarEntrega" type="text" class="form-control required"  onkeyup="this.value = this.value.toUpperCase();">
            </div>
            <div class="col-lg-2">
                <Label class="font-normal"><strong>Fecha de entrega:</strong></Label>
                <input type="date" class="form-control" required id="fechaEntrega" name="fechaEntrega" value="@DateTime.Now.ToString("dd/MM/yyyy")" />
            </div>
            <div class="col-lg-2">
                <Label class="font-normal"><strong>Cantidad:</strong></Label>
                <input type="number" class="form-control" required value="0" min="0" id="cantidad" name="cantidad"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-2">
                <Label class="font-normal"><strong>Número de páginas:</strong></Label>
                <input type="number" class="form-control" required value="0" min="0" id="numeroPaginas" name="numeroPaginas"  onkeyup="this.value = this.value.toUpperCase();"/>
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
                <input type="text" class="form-control" required  id="tamaño" name="tamaño"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
        </div>
    </div>
    <h1> Material</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Portada:</strong></Label>
                <input type="text" class="form-control" required value="" id="materialPortada" name="materialPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Gramaje:</strong></Label>
                <input type="text" class="form-control" required value="" id="gramajePortada" name="gramajePortada"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Color:</strong></Label>
                <input type="text" class="form-control" required value="" id="colorPortada" name="colorPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required value="" id="tamañoPortada" name="tamañoPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>

        </div>
        <div class="row">
            <br>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Interior:</strong></Label>
                <input type="text" class="form-control" required value="" id="materialInterior" name="materialInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Gramaje:</strong></Label>
                <input type="text" class="form-control" required value="" id="gramajeInterior" name="gramajeInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Color:</strong></Label>
                <input type="text" class="form-control" required value="" id="colorInterior" name="colorInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required value="" id="tamañoInterior" name="tamañoInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>

        </div>
        <div class="row">
            <br>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Otro:</strong></Label>
                <input type="text" class="form-control" required value="" id="materialOtro" name="materialOtro"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Gramaje:</strong></Label>
                <input type="text" class="form-control" required value="" id="gramajeOtro" name="gramajeOtro"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Color:</strong></Label>
                <input type="text" class="form-control" required value="" id="colorOtro" name="colorOtro"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño:</strong></Label>
                <input type="text" class="form-control" required value="" id="tamañoOtro" name="tamañoOtro"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>

        </div>
        <div class="row">
            <br>
            <div class="col-lg-12">
                <h3> Cantidad de resmas a utilizar:</h3>
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Portada:</strong></Label>
                <input type="text" class="form-control" required value="" id="cantidadResmasPortada" name="cantidadResmasPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Interior:</strong></Label>
                <input type="text" class="form-control" required value="" id="cantidadResmasInterior" name="cantidadResmasInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Otro:</strong></Label>
                <input type="number" class="form-control" required value="" id="cantidadResmasOtro" name="cantidadResmasOtro"  onkeyup="this.value = this.value.toUpperCase();"/>
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
                    <input class="form-control" type="text" required id="fullColorPortada" name="fullColorPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Duotono:</strong></Label>
                    <input class="form-control" type="text" required id="duotonoPortada" name="duotonoPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Un color:</strong></Label>
                    <input class="form-control" type="text" required id="uniColorPortada" name="uniColorPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Pantone:</strong></Label>
                    <input class="form-control" type="text" required id="pantonePortada" name="pantonePortada"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Cantidad de tinta:</strong></Label>
                    <input class="form-control" type="number" required id="cantidadTintaPortada" name="cantidadTintaPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>

            </div>
            <div class="col-lg-4">
                <h3> Interior :    </h3>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Full color:</strong></Label>
                    <input class="form-control" type="text" required id="fullColorInterior" name="fullColorInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Duotono:</strong></Label>
                    <input class="form-control" type="text" required id="duotonoInterior" name="duotonoInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Un color:</strong></Label>
                    <input class="form-control" type="text" required id="uniColorInterior" name="uniColorInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Pantone:</strong></Label>
                    <input class="form-control" type="text" required id="pantoneInterior" name="pantoneInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
                </div>
                <div class="col-lg-12">
                    <Label class="font-normal"><strong>Cantidad de tinta:</strong></Label>
                    <input class="form-control" type="number" required id="cantidadTintaInterior" name="cantidadTintaInterior"  onkeyup="this.value = this.value.toUpperCase();"/>
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
                    <input class="form-control" type="number" required id="cantidadAcabadoPortada" name="cantidadAcabadoPortada"  onkeyup="this.value = this.value.toUpperCase();"/>
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
                    <input type="checkbox" id="diseñoDiseño" name="diseñoDiseño" value="SI">
                    <Label for="diseñoDiseño">Diseño</Label><br>
                    <input type="checkbox" id="diseñoImpDigital" name="diseñoImpDigital" value="SI">
                    <Label for="diseñoImpDigital">Imp. digital</Label><br>
                    <input type="checkbox" id="diseñoCTP" name="diseñoCTP" value="SI">
                    <Label for="diseñoCTP">CTP</Label><br>
                    <input type="checkbox" id="diseñoReimpresion" name="diseñoReimpresion" value="SI">
                    <Label for="diseñoReimpresion">Reimpresión</Label><br>
                    <input type="checkbox" id="diseñoPrensa" name="diseñoPrensa" value="SI">
                    <Label for="diseñoPrensa">Prensa</Label><br>
                </div>
            </div>
            <div class="col-lg-4">
                <h3> Impresión :    </h3>
                <Label class="font-normal"><strong>Portada:</strong></Label>
                <div class="col-lg-12">
                    <input type="checkbox" id="tiroRetiroPortada" name="tiroRetiroPortada" value="SI"  onkeyup="this.value = this.value.toUpperCase();">
                    <Label for="tiroRetiroPortada">Tiro/retiro</Label><br>
                    <input type="checkbox" id="tiroPortada" name="tiroPortada" value="SI"  onkeyup="this.value = this.value.toUpperCase();">
                    <Label for="tiroPortada">Tiro</Label><br>
                </div>
            </div>
            <div class="col-lg-4">
                <Label class="font-normal"><strong>Interior:</strong></Label>
                <div class="col-lg-12">
                    <input type="checkbox" id="tiroRetiroInterior" name="tiroRetiroInterior" value="SI"  onkeyup="this.value = this.value.toUpperCase();">
                    <Label for="tiroRetiroInterior">Tiro/retiro</Label><br>
                    <input type="checkbox" id="tiroInterior" name="tiroInterior" value="SI"  onkeyup="this.value = this.value.toUpperCase();">
                    <Label for="tiroInterior">Tiro</Label><br>
                </div>
            </div>
            <div class="col-lg-4">

            </div>
            <div class="col-lg-8">
                <Label class="font-normal"><strong>Cantidad a imprimir (Ya incluye excedente):</strong></Label>
                <input class="form-control" type="number" required id="cantidadImprimir" name="cantidadImprimir"  onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
        </div>
    </div>
    <h1> Encuadernación</h1>
    <div class="step-content">
        <div class="col-lg-4">
            <div class="col-lg-12">
                <input type="checkbox" id="plegado" name="plegado" value="SI">
                <Label for="plegado">Plegado</Label><br>
                <input type="checkbox" id="perforado" name="perforado" value="SI">
                <Label for="perforado">Perforado</Label><br>
                <input type="checkbox" id="pegado" name="pegado" value="SI">
                <Label for="pegado">Pegado</Label><br>
                <input type="checkbox" id="grapado" name="grapado" value="SI">
                <Label for="grapado">Grapado</Label><br>
                <input type="checkbox" id="alzado" name="alzado" value="SI">
                <Label for="alzado">Alzado</Label><br>
                <input type="checkbox" id="numerado" name="numerado" value="SI">
                <Label for="numerado">Numerado</Label><br>
                <input type="checkbox" id="cortado" name="cortado" value="SI">
                <Label for="cortado">Cortado</Label><br>
                <input type="checkbox" id="empacado" name="empacado" value="SI">
                <Label for="empacado">Empacado</Label><br>
            </div>
        </div>
    </div>
    <h1> Observaciones específicas</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-12">
                <Label class="font-normal"><strong>Observaciones:</strong></Label>
                <textarea class="form-control" type="text" id="observacionesEspecificas" name="observacionesEspecificas" rows="3" required  onkeyup="this.value = this.value.toUpperCase();"></textarea>
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
