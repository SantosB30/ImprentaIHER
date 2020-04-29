@Code

    ViewData("Title") = "Ver órdenes | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    @ModelType IEnumerable(Of ProyectoIHER.OrdenesModel)

End Code

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

@If Session("mensaje") <> Nothing Then

    If Session("mensaje").ToString().Equals("Flujo adelantado") Then
        @<script>
             window.onload = function () {
                 swal({
                     title: "Confirmación",
                     text: "¡El trabajo avanzó exitosamente!",
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
                     text: "¡El trabajo retrocedió exitosamente!",
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
                     text: "¡El trabajo finalizó correctamente!",
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
<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong> Ver órdenes</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
        @Using Html.BeginForm("VerOrdenes", "OrdenesDeProduccion", FormMethod.Post)
            @<div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-md-4" id="data_5">
                            <br>
                            <label class="font-normal" id="rangoFecha"><strong>Rango de fechas:</strong></label>
                            <div class="input-daterange input-group" id="datepicker">
                                <span class="input-group-addon">Del</span>
                                <input type="text" class="input-sm form-control" id="date1" name="date1" />
                                <span class="input-group-addon">al</span>
                                <input type="text" class="input-sm form-control" id="date2" name="date2" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <br>
                            <br>
                            <button class="btn btn-primary" type="submit" name="submit" id="submit" value="generar"><span><i class="fa fa-eye" aria-hidden="true"></i></span> Ver</button>
                            <button class="btn btn-primary" type="submit" name="submit" id="submit" value="exportar"><span><i class="fa fa-save" aria-hidden="true"></i></span> PDF</button>
                        </div>
                        <div class="col-md-12">
                            <small>
                                <strong>
                                    Para obtener todos los registros deje en blanco las fechas
                                </strong>
                            </small>
                        </div>
                        @If ViewBag.Message <> Nothing Then
                            @<div class="table-responsive col-lg-12">
                                <table class="table table-striped table-bordered table-hover dataTables-example">
                                    <thead>
                                        <tr>
                                            <td align="center"><strong>Número orden</strong></td>
                                            <td align="center"><strong>Fecha creación</strong></td>
                                            <td align="center"><strong>Área</strong></td>
                                            <td align="center"><strong>Cliente</strong></td>
                                            <td align="center"><strong>Usuario</strong></td>
                                            <td align="center"><strong>Acciones</strong></td>
                                            <td align="center"><strong>Asignar Estado</strong></td>
                                            <td align="center"><strong>Flujo de producción</strong></td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        @If Session("accesos") <> Nothing Then

                                            @For Each item In Model
                                                Dim estadoAnterior As String = ""
                                                Dim estadoPosterior As String = ""

                                                If item.estadoOrden.Equals("ADMINISTRACION") Then
                                                    estadoAnterior = "ADMINISTRACION"
                                                    estadoPosterior = "DISEÑO"
                                                ElseIf item.estadoOrden.Equals("DISEÑO") Then
                                                    estadoAnterior = "ADMINISTRACION"
                                                    estadoPosterior = "IMPRESION"
                                                ElseIf item.estadoOrden.Equals("IMPRESION") Then
                                                    estadoAnterior = "DISEÑO"
                                                    estadoPosterior = "ACABADO"
                                                ElseIf item.estadoOrden.Equals("ACABADO") Then
                                                    estadoAnterior = "IMPRESION"
                                                    estadoPosterior = "BODEGA"
                                                ElseIf item.estadoOrden.Equals("BODEGA") Then
                                                    estadoAnterior = "ACABADO"
                                                    estadoPosterior = "FINALIZAR"
                                                End If
                                                @<tr>
                                                    <td>@item.numeroOrden</td>
                                                    <td>@item.fechaCreacion</td>
                                                    <td align="center">@item.estadoOrden</td>
                                                    <td>@item.nombreCliente</td>
                                                    <td>@item.nombreUsuario</td>
                                                    <td>
                                                        <div class="col-lg-12">
                                                            @Html.ActionLink("Ver", "VerOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-success col-md-12"})
                                                        </div>
                                                        @If item.estadoOrden.Equals("ADMINISTRACION") Then
                                                            @<div Class="col-lg-12">
                                                                @Html.ActionLink("Editar", "EditarOrden", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden}, New With {.class = "badge badge-warning col-md-12"})
                                                            </div>
                                                        End If
                                                    </td>
                                                    <td>
                                                        @If item.estado.Equals("PENDIENTE") Then
                                                            @<div Class="col-lg-12">
                                                                @Html.ActionLink("EN PROCESO", "AsignarEstado", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = "EN PROCESO"}, New With {.class = "badge badge-primary col-md-12"})
                                                            </div>
                                                        ElseIf item.estado.Equals("EN PROCESO") Then
                                                            @<div Class="col-lg-12">
                                                                @Html.ActionLink("PENDIENTE", "AsignarEstado", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = "PENDIENTE"}, New With {.class = "badge badge-warning col-md-12"})
                                                            </div>
                                                        Else
                                                            @<div Class="col-lg-12" align="center">
                                                                <strong>FINALIZADA</strong>
                                                            </div>

                                                        End If
                                                    </td>
                                                    <td>
                                                        @If Not item.estadoOrden.Equals("FINALIZADA") Then
                                                            @<div Class="col-lg-12">
                                                                @Html.ActionLink("Avanzar al área de " + estadoPosterior, "AvanzarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoPosterior}, New With {.class = "badge badge-success col-md-12"})
                                                            </div>
                                                            @<div Class="col-lg-12">
                                                                @Html.ActionLink("Regresar al área de " + estadoAnterior, "RegresarFlujo", "OrdenesDeProduccion", New With {.numeroOrden = item.numeroOrden, .nuevoEstado = estadoAnterior}, New With {.class = "badge badge-danger col-md-12"})
                                                            </div>
                                                        Else
                                                            @<div Class="col-lg-12" align="center">
                                                                <strong>FINALIZADO</strong>
                                                            </div>
                                                        End If

                                                    </td>
                                                    <td>
                                                </tr>
                                            Next
                                        End If

                                    </tbody>
                                </table>
                            </div>

                        End If
                    </div>
                </div>

            </div>
        End Using
    </div>

</div>

@Section Styles
    @Styles.Render("~/Content/plugins/dataTables/dataTablesStyles")
    @Styles.Render("~/plugins/sweetAlertStyles")
    @Styles.Render("~/plugins/dateRangeStyles")

End Section
@Section Scripts
    @Scripts.Render("~/plugins/sweetAlert")
    <script>
        $(function () {
            $('input[type="text"]').change(function () {
                this.value = $.trim(this.value);
            });
        });
    </script>
    @Scripts.Render("~/plugins/dataTables")
    <script type="text/javascript">
        $(document).ready(function () {

            $('.dataTables-example').DataTable({
                pageLengtd: 25,
                dom: '<"html5buttons"B>lTfgitp',
                buttons: [
                    { extend: 'copy' },
                    { extend: 'excel', title: 'Bitacoa De Usuarios' }
                ]

            });



        });

    </script>
    @Scripts.Render("~/plugins/iCheck")
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
        $('#fecha').change(function () {
            if ($(this).val() == "FECHA DE CREACIÓN" || $(this).val() == "FECHA DE MODIFICACIÓN") {
                $('#date1').prop("disabled", false);
                $('#date2').prop("disabled", false);

            } else {
                $('#date1').prop("disabled", true);
                $('#date2').prop("disabled", true);
            }
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            var $image = $(".image-crop > img")
            $($image).cropper({
                aspectRatio: 1.618,
                preview: ".img-preview",
                done: function (data) {
                    // Output the result data for cropping image.
                }
            });

            var $inputImage = $("#inputImage");
            if (window.FileReader) {
                $inputImage.change(function () {
                    var fileReader = new FileReader(),
                        files = this.files,
                        file;

                    if (!files.length) {
                        return;
                    }

                    file = files[0];

                    if (/^image\/\w+$/.test(file.type)) {
                        fileReader.readAsDataURL(file);
                        fileReader.onload = function () {
                            $inputImage.val("");
                            $image.cropper("reset", true).cropper("replace", this.result);
                        };
                    } else {
                        showMessage("Please choose an image file.");
                    }
                });
            } else {
                $inputImage.addClass("hide");
            }

            $("#download").click(function () {
                window.open($image.cropper("getDataURL"));
            });

            $("#zoomIn").click(function () {
                $image.cropper("zoom", 0.1);
            });

            $("#zoomOut").click(function () {
                $image.cropper("zoom", -0.1);
            });

            $("#rotateLeft").click(function () {
                $image.cropper("rotate", 45);
            });

            $("#rotateRight").click(function () {
                $image.cropper("rotate", -45);
            });

            $("#setDrag").click(function () {
                $image.cropper("setDragMode", "crop");
            });

            $('#data_1 .input-group.date').datepicker({
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true
            });

            $('#data_2 .input-group.date').datepicker({
                startView: 1,
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                autoclose: true
            });

            $('#data_3 .input-group.date').datepicker({
                startView: 2,
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                autoclose: true
            });

            $('#data_4 .input-group.date').datepicker({
                minViewMode: 1,
                keyboardNavigation: false,
                forceParse: false,
                autoclose: true,
                todayHighlight: true
            });

            $('#data_5 .input-daterange').datepicker({
                keyboardNavigation: false,
                forceParse: false,
                autoclose: true,
                format: "dd/mm/yyyy"

            });

            var elem = document.querySelector('.js-switch');
            var switchery = new Switchery(elem, { color: '#1AB394' });

            var elem_2 = document.querySelector('.js-switch_2');
            var switchery_2 = new Switchery(elem_2, { color: '#ED5565' });

            var elem_3 = document.querySelector('.js-switch_3');
            var switchery_3 = new Switchery(elem_3, { color: '#1AB394' });

            var elem_4 = document.querySelector('.js-switch_4');
            var switchery_4 = new Switchery(elem_4, { color: '#f8ac59' });
            switchery_4.disable();

            $('.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-green',
                radioClass: 'iradio_square-green',
            });

            $('.demo1').colorpicker();

            var divStyle = $('.back-change')[0].style;
            $('#demo_apidemo').colorpicker({
                color: divStyle.backgroundColor
            }).on('changeColor', function (ev) {
                divStyle.backgroundColor = ev.color.toHex();
            });

            $('.clockpicker').clockpicker();

            $('input[name="daterange"]').daterangepicker();

            $('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));

            $('#reportrange').daterangepicker({
                format: 'MM/DD/YYYY',
                startDate: moment().subtract(29, 'days'),
                endDate: moment(),
                minDate: '01/01/2012',
                maxDate: '12/31/2015',
                dateLimit: { days: 60 },
                showDropdowns: true,
                showWeekNumbers: true,
                timePicker: false,
                timePickerIncrement: 1,
                timePicker12Hour: true,
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                },
                opens: 'right',
                drops: 'down',
                buttonClasses: ['btn', 'btn-sm'],
                applyClass: 'btn-primary',
                cancelClass: 'btn-default',
                separator: ' to ',
                locale: {
                    applyLabel: 'Submit',
                    cancelLabel: 'Cancel',
                    fromLabel: 'From',
                    toLabel: 'To',
                    customRangeLabel: 'Custom',
                    daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                    monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                    firstDay: 1
                }
            }, function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
                $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            });


            $(".select2_demo_1").select2();
            $(".select2_demo_2").select2();
            $(".select2_demo_3").select2({
                placeholder: "Select a state",
                allowClear: true
            });

            $(".touchspin1").TouchSpin({
                buttondown_class: 'btn btn-white',
                buttonup_class: 'btn btn-white'
            });

            $(".touchspin2").TouchSpin({
                min: 0,
                max: 100,
                step: 0.1,
                decimals: 2,
                boostat: 5,
                maxboostedstep: 10,
                postfix: '%',
                buttondown_class: 'btn btn-white',
                buttonup_class: 'btn btn-white'
            });

            $(".touchspin3").TouchSpin({
                verticalbuttons: true,
                buttondown_class: 'btn btn-white',
                buttonup_class: 'btn btn-white'
            });

            $('.tagsinput').tagsinput({
                tagClass: 'label label-primary'
            });

            $('.dual_select').bootstrapDualListbox({
                selectorMinimalHeight: 160
            });


        });


        $('.chosen-select').chosen({ width: "100%" });

        $("#ionrange_1").ionRangeSlider({
            min: 0,
            max: 5000,
            type: 'double',
            prefix: "$",
            maxPostfix: "+",
            prettify: false,
            hasGrid: true
        });

        $("#ionrange_2").ionRangeSlider({
            min: 0,
            max: 10,
            type: 'single',
            step: 0.1,
            postfix: " carats",
            prettify: false,
            hasGrid: true
        });

        $("#ionrange_3").ionRangeSlider({
            min: -50,
            max: 50,
            from: 0,
            postfix: "°",
            prettify: false,
            hasGrid: true
        });

        $("#ionrange_4").ionRangeSlider({
            values: [
                "January", "February", "March",
                "April", "May", "June",
                "July", "August", "September",
                "October", "November", "December"
            ],
            type: 'single',
            hasGrid: true
        });

        $("#ionrange_5").ionRangeSlider({
            min: 10000,
            max: 100000,
            step: 100,
            postfix: " km",
            from: 55000,
            hideMinMax: true,
            hideFromTo: false
        });

        $(".dial").knob();

        $("#basic_slider").noUiSlider({
            start: 40,
            behaviour: 'tap',
            connect: 'upper',
            range: {
                'min': 20,
                'max': 80
            }
        });

        $("#range_slider").noUiSlider({
            start: [40, 60],
            behaviour: 'drag',
            connect: true,
            range: {
                'min': 20,
                'max': 80
            }
        });

        $("#drag-fixed").noUiSlider({
            start: [40, 60],
            behaviour: 'drag-fixed',
            connect: true,
            range: {
                'min': 20,
                'max': 80
            }
        });</script>
    @Scripts.Render("~/plugins/toastr")
    <script type="text/javascript">
        $(document).ready(function () {

            var i = -1;
            var toastCount = 0;
            var $toastlast;
            var getMessage = function () {
                var msg = 'Hi, welcome to Inspinia. This is example of Toastr notification box.';
                return msg;
            };

            $('#showsimple').click(function () {
                // Display a success toast, with a title
                toastr.success('Without any options', 'Simple notification!')
            });
            $('#showtoast').click(function () {
                var shortCutFunction = $("#toastTypeGroup input:radio:checked").val();
                var msg = $('#message').val();
                var title = $('#title').val() || '';
                var $showDuration = $('#showDuration');
                var $hideDuration = $('#hideDuration');
                var $timeOut = $('#timeOut');
                var $extendedTimeOut = $('#extendedTimeOut');
                var $showEasing = $('#showEasing');
                var $hideEasing = $('#hideEasing');
                var $showMethod = $('#showMethod');
                var $hideMethod = $('#hideMethod');
                var toastIndex = toastCount++;
                toastr.options = {
                    closeButton: $('#closeButton').prop('checked'),
                    debug: $('#debugInfo').prop('checked'),
                    progressBar: $('#progressBar').prop('checked'),
                    positionClass: $('#positionGroup input:radio:checked').val() || 'toast-top-right',
                    onclick: null
                };
                if ($('#addBehaviorOnToastClick').prop('checked')) {
                    toastr.options.onclick = function () {
                        alert('You can perform some custom action after a toast goes away');
                    };
                }
                if ($showDuration.val().length) {
                    toastr.options.showDuration = $showDuration.val();
                }
                if ($hideDuration.val().length) {
                    toastr.options.hideDuration = $hideDuration.val();
                }
                if ($timeOut.val().length) {
                    toastr.options.timeOut = $timeOut.val();
                }
                if ($extendedTimeOut.val().length) {
                    toastr.options.extendedTimeOut = $extendedTimeOut.val();
                }
                if ($showEasing.val().length) {
                    toastr.options.showEasing = $showEasing.val();
                }
                if ($hideEasing.val().length) {
                    toastr.options.hideEasing = $hideEasing.val();
                }
                if ($showMethod.val().length) {
                    toastr.options.showMethod = $showMethod.val();
                }
                if ($hideMethod.val().length) {
                    toastr.options.hideMethod = $hideMethod.val();
                }
                if (!msg) {
                    msg = getMessage();
                }
                $("#toastrOptions").text("Command: toastr["
                    + shortCutFunction
                    + "](\""
                    + msg
                    + (title ? "\", \"" + title : '')
                    + "\")\n\ntoastr.options = "
                    + JSON.stringify(toastr.options, null, 2)
                );
                var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
                $toastlast = $toast;
                if ($toast.find('#okBtn').length) {
                    $toast.delegate('#okBtn', 'click', function () {
                        alert('you clicked me. i was toast #' + toastIndex + '. goodbye!');
                        $toast.remove();
                    });
                }
                if ($toast.find('#surpriseBtn').length) {
                    $toast.delegate('#surpriseBtn', 'click', function () {
                        alert('Surprise! you clicked me. i was toast #' + toastIndex + '. You could perform an action here.');
                    });
                }
            });
            function getLastToast() {
                return $toastlast;
            }
            $('#clearlasttoast').click(function () {
                toastr.clear(getLastToast());
            });
            $('#cleartoasts').click(function () {
                toastr.clear();
            });

        });
    </script>

    <script>

        var waitingDialog = waitingDialog || (function ($) {
            'use strict';

            // Creating modal dialog's DOM
            var $dialog = $(
                '<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
                '<div class="modal-dialog modal-m">' +
                '<div class="modal-content">' +
                '<div class="modal-header"><h3 style="margin:0;"></h3></div>' +
                '<div class="modal-body">' +
                '<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
                '</div>' +
                '</div></div></div>');

            return {


                show: function (message, options) {
                    // Assigning defaults
                    if (typeof options === 'undefined') {
                        options = {};
                    }
                    if (typeof message === 'undefined') {
                        message = 'Loading';
                    }
                    var settings = $.extend({
                        dialogSize: 'm',
                        progressType: '',
                        onHide: null // This callback runs after the dialog was hidden
                    }, options);

                    // Configuring dialog
                    $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
                    $dialog.find('.progress-bar').attr('class', 'progress-bar');
                    if (settings.progressType) {
                        $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
                    }
                    $dialog.find('h3').text(message);
                    // Adding callbacks
                    if (typeof settings.onHide === 'function') {
                        $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                            settings.onHide.call($dialog);
                        });
                    }
                    // Opening dialog
                    $dialog.modal();
                },
                /**
                 * Closes dialog
                 */
                hide: function () {
                    $dialog.modal('hide');
                }
            };

        })(jQuery);

    </script>

    @Scripts.Render("~/plugins/sweetAlert")
    <script type="text/javascript">
        $(document).ready(function () {

            $('.demo1').click(function () {
                swal({
                    title: "",
                    text: ""
                });
            });

            $('.demo2').click(function () {
                swal({
                    title: "",
                    text: "",
                    type: "success"
                });
            });

            $('.demo3').click(function () {
                swal({
                    title: "?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "",
                    closeOnConfirm: false
                }, function () {
                    swal("!", "Y.", "success");
                });
            });

            $('.demo4').click(function () {
                swal({
                    title: "?",
                    text: "!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "!",
                    cancelButtonText: "!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            swal("!", ".", "success");
                        } else {
                            swal("", ":)", "error");
                        }
                    });
            });

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

End Section