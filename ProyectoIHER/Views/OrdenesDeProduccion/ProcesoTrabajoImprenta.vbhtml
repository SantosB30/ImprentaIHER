@Code
    ViewData("Title") = "Proceso de trabajo - Imprenta | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Proceso de trabajo - Imprenta | Orden número: @Session("numeroOrden").ToString()</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
        @Using Html.BeginForm("ProcesoTrabajoImprenta", "OrdenesDeProduccion", FormMethod.Post, New With {.id = "form"})
            @<div id="wizard">
    <h1>Prepensa</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Fecha y hora de inicio:</strong></Label>
                <input type="datetime-local" class="form-control" required id="fechaInicialPreprensa" name="fechaInicialPreprensa"
                       value="@DateTime.Now.ToString("yyyy-MM-ddTHH:mm")" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Estado del trabajo:</strong></Label>
                <div class="col-lg-12">
                    <input type="checkbox" id="terminado" name="terminado" value="SI">
                    <Label for="otro">Terminado</Label><br>
                    <input type="checkbox" id="entregado" name="entregado" value="SI">
                    <Label for="entregado">Entregado</Label><br>
                </div>
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Tamaño planchas:</strong></Label>
                <input type="text" class="form-control" required id="tamañoPlanchas" name="tamañoPlanchas" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Interior cantidad:</strong></Label>
                <input type="text" class="form-control" required id="interiorCantidad" name="interiorCantidad" />
            </div>
        </div>
        <div class="row">
            <div class="col-lg-3">
                <br>
                <Label class="font-normal"><strong>Portada cantidad:</strong></Label>
                <input type="text" class="form-control" required id="portadaCantidad" name="portadaCantidad" />
            </div>
            <div class="col-lg-3">
                <br>
                <Label class="font-normal"><strong>Fecha y hora final:</strong></Label>
                <input type="datetime-local" class="form-control" required id="fechaFinalPreprensa" name="fechaFinalPreprensa"
                       value="@DateTime.Now.ToString("yyyy-MM-ddTHH:mm")" />
            </div>
            <div class="col-lg-12">
                <br>
                <Label class="font-normal"><strong>Comentario:</strong></Label>
                <textarea class="form-control" type="text" id="comentarioPrepensa" name="comentarioPrepensa" rows="3" required onkeyup="this.value = this.value.toUpperCase();"></textarea>
            </div>
        </div>
    </div>
    <h1>Prensa</h1>
    <div class="step-content">
        <div class="row">
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Fecha y hora de inicio:</strong></Label>
                <input type="datetime-local" class="form-control" required id="fechaInicialPrensa" name="fechaInicialPrensa"
                       value="@DateTime.Now.ToString("yyyy-MM-ddTHH:mm")" />
            </div>
            <div class="col-lg-3">
                <Label class="font-normal"><strong>Fecha y hora final:</strong></Label>
                <input type="datetime-local" class="form-control" required id="fechaFinalPrensa" name="fechaFinalPrensa"
                       value="@DateTime.Now.ToString("yyyy-MM-ddTHH:mm")" />
            </div>
            <div class="col-lg-3">
                <div class="col-lg-12">
                    <input type="checkbox" id="portada" name="portada" value="SI">
                    <Label for="otro">Portada</Label><br>
                </div>
            </div>
            </div>
        <div class="row">
            <div class="col-lg-2">
                <br>
                @For i As Integer = 1 To 10
                    @<input type="checkbox" id="plieg_@i.ToString()" name="plieg_@i.ToString()" value="SI">
                    @<Label for="plieg_@i.ToString()"> Plieg @i.ToString() </Label>
                    @<br>
                Next

            </div>
            <div class="col-lg-2">
                <br>
                @For i As Integer = 11 To 20
                    @<input type="checkbox" id="plieg_@i.ToString()" name="plieg_@i.ToString()" value="SI">
                    @<Label for="plieg_@i.ToString()"> Plieg @i.ToString() </Label>
                    @<br>
                Next

            </div>
            <div class="col-lg-2">
                <br>
                @For i As Integer = 21 To 30
                    @<input type="checkbox" id="plieg_@i.ToString()" name="plieg_@i.ToString()" value="SI">
                    @<Label for="plieg_@i.ToString()"> Plieg @i.ToString() </Label>
                    @<br>
                Next

            </div>
            <div class="col-lg-6">
                <Label class="font-normal"><strong>Responsable:</strong></Label>
                <input type="text" class="form-control" name="responsable" id="responsable" onkeyup="this.value = this.value.toUpperCase();"/>
            </div>
            <div class="col-lg-6">
                <br>
                <Label class="font-normal"><strong>Comentario:</strong></Label>
                <textarea class="form-control" type="text" id="comentarioPrensa" name="comentarioPrensa" rows="3" required onkeyup="this.value = this.value.toUpperCase();"></textarea>
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
