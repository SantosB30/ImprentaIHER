<nav class="navbar-default navbar-static-side" role="navigation">
    <div class="sidebar-collapse">
        <ul class="nav" id="side-menu">
            <li class="nav-header">
                <div class="dropdown profile-element">
                    <span>
                        <img alt="image" class="img-circle" src="~/Images/profile.png" height="25%" width="25%" />
                    </span>
                    <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                        <span class="clear">
                            <span class="block m-t-xs">
                                <strong class="font-bold">@Session("usuario")</strong>
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
                        </ul>
                    </li>
                End IF
            End IF
        </ul>
                        </div>
</nav>
