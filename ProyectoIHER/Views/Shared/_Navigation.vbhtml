﻿<nav class="navbar-default navbar-static-side" role="navigation">
    <div class="sidebar-collapse">
        <ul class="nav" id="side-menu">
            <li class="nav-header">
                <div class="dropdown profile-element">

                    <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                        <span class="clear">
                            <span class="block m-t-xs">
                                <strong class="font-bold">@Session("usuario")</strong>
                            </span>
                        </span>
                    </a>
                </div>
                <div class="logo-element">
                    IHER
                </div>
            </li>
            @If Session("accesos") <> Nothing Then
                @If Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    @<li class="active">
                        <a href="#"><i class="fa fa-user"></i> <span class="nav-label" data-i18n="nav.graphs">Gestión de usuarios</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("CrearUsuario", "Usuarios")">Crear usuario</a></li>
                            <li> <a href="@Url.Action("EditarUsuario", "Usuarios")">Editar usuario</a></li>
                            <li> <a href="@Url.Action("EliminarUsuario", "Usuarios")">Eliminar usuario</a></li>
                            <li> <a href="@Url.Action("AprobarUsuario", "Usuarios")">Aprobar usuario</a></li>

                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-shield"></i> <span class="nav-label" data-i18n="nav.graphs">Seguridad</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("BitacoraUsuario", "Usuarios")">Bitacora de Usuarios</a></li>
                            <li> <a href="@Url.Action("Parametros", "Usuarios")">Parametros</a></li>
                            <li> <a href="@Url.Action("RespaldoBDD", "Seguridad")">Respaldos BD</a></li>
                        </ul>


                    </li>
                End IF
            End IF
            @If Session("accesos") <> Nothing Then
                @If Session("accesos").ToString().Contains("DISEÑO") Then
                    @<li class="active">
                        <a href="#"><i class="fa fa-tasks"></i> <span class="nav-label" data-i18n="nav.graphs">Órdenes</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("VerOrdenes", "OrdenesDeProduccion")">Ver órdenes</a></li>
                        </ul>
                    </li>
                End IF
            End IF
            @If Session("accesos") <> Nothing Then
                @If Session("accesos").ToString().Contains("IMPRESION") Then
                    @<li class="active">
                        <a href="#"><i class="fa fa-tasks"></i> <span class="nav-label" data-i18n="nav.graphs">Órdenes</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("VerOrdenes", "OrdenesDeProduccion")">Ver órdenes</a></li>
                        </ul>
                    </li>
                End IF
            End IF
            @If Session("accesos") <> Nothing Then
                @If Session("accesos").ToString().Contains("ACABADO") Then
                    @<li class="active">
                        <a href="#"><i class="fa fa-tasks"></i> <span class="nav-label" data-i18n="nav.graphs">Órdenes</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("VerOrdenes", "OrdenesDeProduccion")">Ver órdenes</a></li>
                        </ul>
                    </li>
                End IF
            End IF
            @If Session("accesos") <> Nothing Then
                @If Session("accesos").ToString().Contains("BODEGA") Then
                    @<li class="active">
                        <a href="#"><i class="fa fa-tasks"></i> <span class="nav-label" data-i18n="nav.graphs">Órdenes</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("VerOrdenes", "OrdenesDeProduccion")">Ver órdenes</a></li>
                        </ul>
                    </li>
                End IF
            End IF

            @If Session("accesos") <> Nothing Then
                @If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    @<li class="active">
                        <a href="#"><i class="fa fa-user"></i> <span class="nav-label" data-i18n="nav.graphs">Gestión de usuarios</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("CrearUsuario", "Usuarios")">Crear usuario</a></li>
                            <li> <a href="@Url.Action("EditarUsuario", "Usuarios")">Editar usuario</a></li>
                            <li> <a href="@Url.Action("EliminarUsuario", "Usuarios")">Eliminar usuario</a></li>
                            <li> <a href="@Url.Action("AprobarUsuario", "Usuarios")">Aprobar usuario</a></li>

                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-address-card"></i> <span class="nav-label" data-i18n="nav.graphs">Clientes</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("AgregarCliente", "Clientes")">Agregar cliente</a></li>
                            <li> <a href="@Url.Action("EditarClientes", "Clientes")">Editar cliente</a></li>
                            <li> <a href="@Url.Action("EliminarClientes", "Clientes")">Eliminar cliente</a></li>
                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-truck"></i> <span class="nav-label" data-i18n="nav.graphs">Proveedores</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("AgregarProveedor", "Proveedores")">Agregar proveedor</a></li>
                            <li> <a href="@Url.Action("EditarProveedores", "Proveedores")">Editar proveedor</a></li>
                            <li> <a href="@Url.Action("EliminarProveedores", "Proveedores")">Eliminar proveedor</a></li>
                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-shopping-cart"></i> <span class="nav-label" data-i18n="nav.graphs">Productos</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("AgregarProducto", "Productos")">Agregar producto</a></li>
                            <li> <a href="@Url.Action("EditarProductos", "Productos")">Editar producto</a></li>
                            <li> <a href="@Url.Action("EliminarProductos", "Productos")">Eliminar producto</a></li>
                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-money"></i> <span class="nav-label" data-i18n="nav.graphs">Cobros</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("CobrosPendientes", "Cobros")">Cobros pendientes</a></li>

                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-book"></i> <span class="nav-label" data-i18n="nav.graphs">Cotizaciones</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("NuevaCotizacion", "Cotizaciones")">Nueva cotización</a></li>
                            <li> <a href="@Url.Action("BuscarCotizaciones", "Cotizaciones")">Buscar cotizaciones</a></li>
                        </ul>
                    </li>
                    @<li class="active">
                        <a href="#"><i class="fa fa-tasks"></i> <span class="nav-label" data-i18n="nav.graphs">Órdenes</span><span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level collapse in">
                            <li> <a href="@Url.Action("VerOrdenes", "OrdenesDeProduccion")">Ver órdenes</a></li>
                        </ul>
                    </li>
                End IF
            End IF


        </ul>
    </div>
</nav>
