<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">

    <title>@ViewBag.Title</title>
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700' rel='stylesheet' type='text/css'>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">


    @Styles.Render("~/plugins/sweetAlertStyles")
    @Styles.Render("~/Content/plugins/iCheck/iCheckStyles")
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
    @Styles.Render("~/plugins/toastrStyles")
    @Styles.Render("~/plugins/sweetAlertStyles")
    @Styles.Render("~/Content/plugins/dataTables/dataTablesStyles")
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700' rel='stylesheet' type='text/css'>
    <!-- Add local styles, mostly for plugins css file -->
    <!-- Add jQuery Style direct - used for jQGrid plugin -->
    <link href="@Url.Content("~/Scripts/plugins/jquery-ui/jquery-ui.min.css")" rel="stylesheet" type="text/css" />

    <!-- Primary Inspinia style -->
    @Styles.Render("~/Content/css")
    @Styles.Render("~/font-awesome/css")

</head>
<body>

    <!-- Skin configuration box -->
    <!-- @Html.Partial("_SkinConfig") -->
    <!-- Wrapper-->
    <!-- PageClass give you ability to specify custom style for specific view based on action -->
    <div id="wrapper">
        <!-- Navigation -->
        <!-- Page wraper -->
        <div id="wrapper">
            <!-- Navigation -->
            @Html.Partial("_Navigation")
            <!-- Page wraper -->
            <div id="page-wrapper" class="gray-bg @ViewBag.SpecialClass">
                <!-- Top Navbar -->
                @Html.Partial("_TopNavbar")
                <!-- Main view  -->
                @RenderBody()
                <!-- Footer -->
                @Html.Partial("_Footer")
            </div>
            <!-- End page wrapper-->
            <!-- Right Sidebar -->
            @Html.Partial("_RightSidebar")
        </div>
        <!-- End page wrapper-->
        <!-- Right Sidebar -->
    </div>
    <!-- End wrapper-->
    <!-- Section for main scripts render -->
    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @Scripts.Render("~/plugins/slimScroll")
    @Scripts.Render("~/bundles/inspinia")

    <!-- Skin config script - only for demo purpose-->
    @Scripts.Render("~/bundles/skinConfig")
    @Scripts.Render("~/plugins/toastr")

    <!-- Handler for local scripts -->
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
    @Scripts.Render("~/plugins/wizardSteps")
    @Scripts.Render("~/plugins/iCheck")
    @Scripts.Render("~/plugins/validate")
    @Scripts.Render("~/plugins/sweetAlert")
    @Scripts.Render("~/plugins/dataTables")
</body>
</html>
