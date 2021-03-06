﻿<nav class="navbar-default navbar-static-side" role="navigation">
    <div class="sidebar-collapse">
        <ul class="nav" id="side-menu">
            <li class="nav-header">
                <div class="dropdown profile-element">
                    <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                        <span class="clear">
                            <span class="block m-t-xs">
                                <strong class="font-bold">@Session("usuario")</strong>
                            </span> <span class="text-muted text-xs block">Opciones<b class="caret"></b></span>
                        </span>
                    </a>
                    <ul class="dropdown-menu animated fadeInRight m-t-xs">
                        <li><a href="@Url.Action("CambiarContraseña", "Cuentas")">Cambiar contraseña</a></li>
                    </ul>
                    <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                        <span class="clear">
                            <span class="block m-t-xs">
                            </span>
                        </span>
                    </a>
                </div>
                <div class="logo-element">
                    <img src="~/Images/logo3.png" height="20" />
                </div>
            </li>

            <li class="">
                @If Session("Estado_Usuario").ToString.Equals("ACTIVO") Then
                @<a href = "@Url.Action("Principal", "Inicio")"><i Class="fa fa-home"></i> <span Class="nav-label" data-i18n="nav.graphs">Inicio</span></a>
                End If
                    @If Session("permisos") <> Nothing Then
                        @If Session("permisos").ToString().Contains("2801") Or
                            Session("permisos").ToString().Contains("2802") Or
                            Session("permisos").ToString().Contains("2803") Or
                            Session("permisos").ToString().Contains("2804") Or
                            Session("permisos").ToString().Contains("2805") Then
                            @<li class="active">
            
                                <a href="#"><i class="fa fa-user"></i> <span class="nav-label" data-i18n="nav.graphs">Gestión de usuarios</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2801") Then
                                        @<li> <a href="@Url.Action("CrearUsuario", "Usuarios")">Crear usuario</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2802") Then
                                        @<li> <a href="@Url.Action("EditarUsuario", "Usuarios")">Gestión de usuarios</a></li>
                                    End If
                                   
                                    @If Session("permisos").ToString().Contains("2804") Then
                                        @<li> <a href="@Url.Action("AprobarUsuario", "Usuarios")">Aprobar usuario</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2805") Then
                                        @<li> <a href="@Url.Action("ReporteUsuarios", "Usuarios")">Reportes de usuarios</a></li>
                                    End If

                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2806") Or
                           Session("permisos").ToString().Contains("2807") Or
                           Session("permisos").ToString().Contains("2808") Or
                           Session("permisos").ToString().Contains("2809") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-shield"></i> <span class="nav-label" data-i18n="nav.graphs">Seguridad</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2806") Then
                                        @<li> <a href="@Url.Action("BitacoraUsuario", "Usuarios")">Bitacora de Usuarios</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2807") Then
                                        @<li> <a href="@Url.Action("Parametros", "Usuarios")">Parámetros</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2808") Then
                                        @<li> <a href="@Url.Action("RespaldoBDD", "Seguridad")">Respaldos BD</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2809") Then
                                        @<li> <a href="@Url.Action("RestaurarBDD", "Seguridad")">Restaurar BD</a></li>
                                    End If
                                    <li> <a href="@Url.Action("SeleccionarUsuarioGestionPermisos", "Usuarios")">Permisos por usuario</a></li>
                                    <li> <a href="@Url.Action("SeleccionarRolGestionPermisos", "Usuarios")">Permisos por rol</a></li>
                                    @If Session("permisos").ToString().Contains("2809") Then
                                        @<li> <a href="@Url.Action("AgregarRoles", "Cuentas")">Agregar roles</a></li>
                                    End If
                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2810") Or
                          Session("permisos").ToString().Contains("2811") Or
                          Session("permisos").ToString().Contains("2812") Or
                          Session("permisos").ToString().Contains("2813") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-address-card"></i> <span class="nav-label" data-i18n="nav.graphs">Clientes</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2810") Then
                                        @<li> <a href="@Url.Action("AgregarCliente", "Clientes")">Agregar cliente</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2811") Then
                                        @<li> <a href="@Url.Action("EditarClientes", "Clientes")">Gestión de clientes</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2813") Then
                                        @<li> <a href="@Url.Action("ReporteClientes", "Clientes")">Reporte de clientes</a></li>
                                    End If
                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2814") Or
                          Session("permisos").ToString().Contains("2815") Or
                          Session("permisos").ToString().Contains("2816") Or
                          Session("permisos").ToString().Contains("2817") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-truck"></i> <span class="nav-label" data-i18n="nav.graphs">Proveedores</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2814") Then
                                        @<li> <a href="@Url.Action("AgregarProveedor", "Proveedores")">Agregar proveedor</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2815") Then
                                        @<li> <a href="@Url.Action("EditarProveedores", "Proveedores")">Gestión de proveedores</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2817") Then
                                        @<li> <a href="@Url.Action("ReporteProveedores", "Proveedores")">Reporte de proveedores</a></li>
                                    End If
                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2818") Or
                          Session("permisos").ToString().Contains("2819") Or
                          Session("permisos").ToString().Contains("2820") Or
                          Session("permisos").ToString().Contains("2821") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-shopping-cart"></i> <span class="nav-label" data-i18n="nav.graphs">Productos</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2818") Then
                                        @<li> <a href="@Url.Action("AgregarProducto", "Productos")">Agregar producto</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2819") Then
                                        @<li> <a href="@Url.Action("EditarProductos", "Productos")">Gestión de productos</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2821") Then
                                        @<li> <a href="@Url.Action("ReporteProductos", "Productos")">Reporte de productos</a></li>
                                    End If

                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2822") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-money"></i> <span class="nav-label" data-i18n="nav.graphs">Cobros</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2822") Then
                                        @<li> <a href="@Url.Action("CobrosPendientes", "Cobros")">Cobros pendientes</a></li>
                                    End If
                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2823") Or
                          Session("permisos").ToString().Contains("2824") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-book"></i> <span class="nav-label" data-i18n="nav.graphs">Cotizaciones</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2823") Then
                                        @<li> <a href="@Url.Action("NuevaCotizacion", "Cotizaciones")">Nueva cotización</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2824") Then
                                        @<li> <a href="@Url.Action("BuscarCotizaciones", "Cotizaciones")">Buscar cotizaciones</a></li>
                                    End If

                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2828") Or
                          Session("permisos").ToString().Contains("2829") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-table"></i> <span class="nav-label" data-i18n="nav.graphs">Órdenes</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2828") Then
                                        @<li> <a href="@Url.Action("VerOrdenes", "OrdenesDeProduccion")">Ver órdenes</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2829") Then
                                        @<li> <a href="@Url.Action("ReporteDeOrdenes", "OrdenesDeProduccion")">Reporte de órdenes</a></li>
                                    End If


                                </ul>
                            </li>
                        End If
                        @If Session("permisos").ToString().Contains("2832") Or
                          Session("permisos").ToString().Contains("2833") Or
                          Session("permisos").ToString().Contains("2834") Or
                          Session("permisos").ToString().Contains("2835") Then
                            @<li class="active">
                                <a href="#"><i class="fa fa-cubes"></i> <span class="nav-label" data-i18n="nav.graphs">Bodega</span><span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level collapse in">
                                    @If Session("permisos").ToString().Contains("2832") Then
                                        @<li> <a href="@Url.Action("ReporteDeBodega", "OrdenesDeProduccion")">Reporte de bodega</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2833") Then
                                        @<li> <a href="@Url.Action("ReporteDeInventario", "OrdenesDeProduccion")">Reporte de inventario</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2834") Then
                                        @<li> <a href="@Url.Action("GestionDeInventario", "OrdenesDeProduccion")">Gestión de inventario</a></li>
                                    End If
                                    @If Session("permisos").ToString().Contains("2835") Then
                                        @<li> <a href="@Url.Action("Inventario", "OrdenesDeProduccion")">Inventario</a></li>
                                    End If
                                </ul>
                            </li>
                        End If
                    End IF
            </ul>
</div>
</nav>
