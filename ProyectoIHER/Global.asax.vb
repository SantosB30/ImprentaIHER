Imports System.Web.Optimization

Public Class MvcApplication
    Inherits System.Web.HttpApplication
    Dim accesos As String
    Dim usuario As String
    Dim mensaje As String
    Dim usuarioEditar As String
    Dim nombreusuarioEditar As String
    Dim correoUsuarioEditar As String
    Dim estadoUsuarioEditar As String
    Dim usuarioEliminar As String
    Dim fecha_creacion As String
    Dim nombre_cliente As String
    Dim nombre_usuario As String
    Dim correo_cliente As String
    Dim direccion_cliente As String
    Dim telefono_cliente As String
    Dim numero_orden As String
    Dim fecha_entrega As String
    Dim tamaño As String
    Dim cantidad As String
    Dim numero_paginas As String
    Dim lugar_entrega As String
    Dim prioridad As String
    Dim orientacion As String
    Dim material_portada As String
    Dim gramaje_portada As String
    Dim color_portada As String
    Dim tamaño_portada As String
    Dim material_interior As String
    Dim gramaje_interior As String
    Dim color_interior As String
    Dim tamaño_interior As String
    Dim material_otro As String
    Dim gramaje_otro As String
    Dim color_otro As String
    Dim tamaño_otro As String
    Dim cantidad_resmas_portada As String
    Dim cantidad_resmas_interior As String
    Dim cantidad_resmas_otro As String
    Dim full_color_portada As String
    Dim duotono_portada As String
    Dim unicolor_portada As String
    Dim pantone_portada As String
    Dim cantidad_tinta_portada As String
    Dim full_color_interior As String
    Dim duotono_interior As String
    Dim unicolor_interior As String
    Dim pantone_interior As String
    Dim cantidad_tinta_interior As String
    Dim acabado_de_portada As String
    Dim cantidad_acabado_de_portada As String
    Dim diseño_diseño As String
    Dim diseño_imp_digital As String
    Dim diseño_ctp As String
    Dim diseño_prensa As String
    Dim diseño_reimpresion As String
    Dim portada_tiro_retiro As String
    Dim portada_tiro As String
    Dim interior_tiro_retiro As String
    Dim interior_tiro As String
    Dim cantidad_imprimir As String
    Dim encuadernacion_plegado As String
    Dim encuadernacion_pegado As String
    Dim encuadernacion_alzado As String
    Dim encuadernacion_cortado As String
    Dim encuadernacion_perforado As String
    Dim encuadernacion_grapado As String
    Dim encuadernacion_numerado As String
    Dim encuadernacion_empacado As String
    Dim observaciones_especificas As String
    Dim estado As String
    Dim estadoOrden As String
    Dim colorPortada As String
    Dim colorInterior As String
    Dim tiroPortada As String
    Dim tiroInterior As String


    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Session_Start()
        Session("accesos") = Nothing
        Session("usuario") = ""
        Session("mensaje") = Nothing
        Session("usuarioEditar") = Nothing
        Session("nombreusuarioEditar") = Nothing
        Session("correoUsuarioEditar") = Nothing
        Session("estadoUsuarioEditar") = Nothing
        Session("usuarioEliminar") = Nothing
        Session("fecha_creacion") = Nothing
        Session("nombre_cliente") = Nothing
        Session("nombre_usuario") = Nothing
        Session("correo_cliente") = Nothing
        Session("direccion_cliente") = Nothing
        Session("telefono_cliente") = Nothing
        Session("numero_orden") = Nothing
        Session("fecha_entrega") = Nothing
        Session("tamaño") = Nothing
        Session("cantidad") = Nothing
        Session("numero_paginas") = Nothing
        Session("lugar_entrega") = Nothing
        Session("prioridad") = Nothing
        Session("orientacion") = Nothing
        Session("material_portada") = Nothing
        Session("gramaje_portada") = Nothing
        Session("color_portada") = Nothing
        Session("tamaño_portada") = Nothing
        Session("material_interior") = Nothing
        Session("gramaje_interior") = Nothing
        Session("color_interior") = Nothing
        Session("tamaño_interior") = Nothing
        Session("material_otro") = Nothing
        Session("gramaje_otro") = Nothing
        Session("color_otro") = Nothing
        Session("tamaño_otro") = Nothing
        Session("cantidad_resmas_portada") = Nothing
        Session("cantidad_resmas_interior") = Nothing
        Session("cantidad_resmas_otro") = Nothing
        Session("full_color_portada") = Nothing
        Session("duotono_portada") = Nothing
        Session("unicolor_portada") = Nothing
        Session("pantone_portada") = Nothing
        Session("cantidad_tinta_portada") = Nothing
        Session("full_color_interior") = Nothing
        Session("duotono_interior") = Nothing
        Session("unicolor_interior") = Nothing
        Session("pantone_interior") = Nothing
        Session("cantidad_tinta_interior") = Nothing
        Session("acabado_de_portada") = Nothing
        Session("cantidad_acabado_de_portada") = Nothing
        Session("diseño_diseño") = Nothing
        Session("diseño_imp_digital") = Nothing
        Session("diseño_ctp") = Nothing
        Session("diseño_prensa") = Nothing
        Session("diseño_reimpresion") = Nothing
        Session("portada_tiro_retiro") = Nothing
        Session("portada_tiro") = Nothing
        Session("interior_tiro_retiro") = Nothing
        Session("interior_tiro") = Nothing
        Session("cantidad_imprimir") = Nothing
        Session("encuadernacion_plegado") = Nothing
        Session("encuadernacion_pegado") = Nothing
        Session("encuadernacion_alzado") = Nothing
        Session("encuadernacion_cortado") = Nothing
        Session("encuadernacion_perforado") = Nothing
        Session("encuadernacion_grapado") = Nothing
        Session("encuadernacion_numerado") = Nothing
        Session("encuadernacion_empacado") = Nothing
        Session("observaciones_especificas") = Nothing
        Session("estado") = Nothing
        Session("estadoOrden") = Nothing
        Session("colorPortada") = Nothing
        Session("colorInterior") = Nothing
        Session("tiroPortada") = Nothing
        Session("tiroInterior") = Nothing

    End Sub
End Class
