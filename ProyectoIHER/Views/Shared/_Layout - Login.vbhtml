﻿
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>@ViewBag.Title</title>
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700' rel='stylesheet' type='text/css'>
    <!-- Add local styles, mostly for plugins css file -->
    @RenderSection("Styles", required:=False)


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
        <!-- Navigation -->
        <!-- Page wraper -->
        <!-- Top Navbar -->
        <!-- Main view  -->
        @RenderBody()
        <!-- Footer -->
        @Html.Partial("_Footer")
        <!-- End page wrapper-->
        <!-- Right Sidebar -->
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
    <!-- Handler for local scripts -->
    @RenderSection("scripts", required:=False)

</body>
</html>
