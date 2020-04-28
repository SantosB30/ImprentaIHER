Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iText.Html2pdf
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Namespace Controllers
    Public Class OrdenesDeProduccionController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        ' GET: OrdenesDeProduccion
        Dim validaciones As Validaciones = New Validaciones()
        Dim bitacora As Bitacora = New Bitacora()
        Function VerOrdenes() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE VISUALIZACIÓN DE ÓRDENES DE PRODUCCIÓN")
            Return View()
        End Function

        <HttpPost>
        Function VerOrdenes(submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            If submit.Equals("generar") Then
                Dim query As String = "SELECT A.*,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO"
                If (Session("accesos").ToString().Contains("DISEÑO")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='DISEÑO'"
                ElseIf (Session("accesos").ToString().Contains("IMPRESION")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='IMPRESION'"
                ElseIf (Session("accesos").ToString().Contains("ACABADO")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='ACABADO'"
                ElseIf (Session("accesos").ToString().Contains("BODEGA")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='BODEGA'"
                End If

                If date1 <> Nothing Then
                    query = query + "  AND CAST(A.FECHA_CREACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
                query = query + " ORDER BY CAST(A.FECHA_CREACION AS DATETIME) ASC"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of OrdenesModel)
                While (lector.Read())
                    Dim detalles = New OrdenesModel()
                    detalles.fechaCreacion = lector("FECHA_CREACION").ToString()
                    detalles.numeroCotizacion = lector("NUMERO_COTIZACION").ToString()
                    detalles.nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                    detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.numeroOrden = lector("NUMERO_ORDEN").ToString()
                    detalles.estadoOrden = lector("ESTADO_ORDEN").ToString()
                    detalles.estado = lector("ESTADO").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos cotizacion"
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE VISUALIZACIÓN DE ÓRDENES DE PRODUCCIÓN")
                Return View("VerOrdenes", model)
            Else
                bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR BÚSQUEDA DE COTIZACIONES")
                Dim query As String = "SELECT A.*,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO"
                If (Session("accesos").ToString().Contains("ADMINISTRACION")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='ADMINISTRACION'"
                ElseIf (Session("accesos").ToString().Contains("DISEÑO")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='DISEÑO'"
                ElseIf (Session("accesos").ToString().Contains("IMPRESION")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='IMPRESION'"
                ElseIf (Session("accesos").ToString().Contains("ACABADO")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='ACABADO'"
                ElseIf (Session("accesos").ToString().Contains("BODEGA")) Then
                    query = query + " WHERE A.ESTADO_ORDEN='BODEGA'"
                End If

                If date1 <> Nothing Then
                    query = query + "  AND CAST(A.FECHA_CREACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
                query = query + " ORDER BY CAST(A.FECHA_CREACION AS DATETIME) ASC"
                Dim dsOrdenes As New DsOrdenes()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    fila = dsOrdenes.Tables("DataTable1").NewRow()
                    fila.Item("fechaCreacion") = lector("FECHA_CREACION").ToString()
                    fila.Item("numeroCotizacion") = lector("NUMERO_COTIZACION").ToString()
                    fila.Item("cliente") = lector("NOMBRE_CLIENTE").ToString()
                    fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                    fila.Item("numeroOrden") = lector("NUMERO_ORDEN").ToString()
                    fila.Item("estadoOrden") = lector("ESTADO_ORDEN").ToString()
                    fila.Item("estado") = lector("ESTADO").ToString()
                    dsOrdenes.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "Reporte de órdenes.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeOrdenes.rpt"))
                crystalReport.SetDataSource(dsOrdenes)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
            End If

        End Function

        Function VerOrden(numeroOrden As String) As ActionResult
            Dim numOrden As String = Request.QueryString("numeroOrden")
            Session("numeroOrden") = numOrden
            Dim fecha_creacion As String = ""
            Dim nombre_cliente As String = ""
            Dim nombre_usuario As String = ""
            Dim correo_cliente As String = ""
            Dim direccion_cliente As String = ""
            Dim telefono_cliente As String = ""
            Dim numero_orden As String = ""
            Dim fecha_entrega As String = ""
            Dim tamaño As String = ""
            Dim cantidad As String = ""
            Dim numero_paginas As String = ""
            Dim lugar_entrega As String = ""
            Dim prioridad As String = ""
            Dim orientacion As String = ""
            Dim material_portada As String = ""
            Dim gramaje_portada As String = ""
            Dim color_portada As String = ""
            Dim tamaño_portada As String = ""
            Dim material_interior As String = ""
            Dim gramaje_interior As String = ""
            Dim color_interior As String = ""
            Dim tamaño_interior As String = ""
            Dim material_otro As String = ""
            Dim gramaje_otro As String = ""
            Dim color_otro As String = ""
            Dim tamaño_otro As String = ""
            Dim cantidad_resmas_portada As String = ""
            Dim cantidad_resmas_interior As String = ""
            Dim cantidad_resmas_otro As String = ""
            Dim full_color_portada As String = ""
            Dim duotono_portada As String = ""
            Dim unicolor_portada As String = ""
            Dim pantone_portada As String = ""
            Dim cantidad_tinta_portada As String = ""
            Dim full_color_interior As String = ""
            Dim duotono_interior As String = ""
            Dim unicolor_interior As String = ""
            Dim pantone_interior As String = ""
            Dim cantidad_tinta_interior As String = ""
            Dim acabado_de_portada As String = ""
            Dim cantidad_acabado_de_portada As String = ""
            Dim diseño_diseño As String = ""
            Dim diseño_imp_digital As String = ""
            Dim diseño_ctp As String = ""
            Dim diseño_prensa As String = ""
            Dim diseño_reimpresion As String = ""
            Dim portada_tiro_retiro As String = ""
            Dim portada_tiro As String = ""
            Dim interior_tiro_retiro As String = ""
            Dim interior_tiro As String = ""
            Dim cantidad_imprimir As String = ""
            Dim encuadernacion_plegado As String = ""
            Dim encuadernacion_pegado As String = ""
            Dim encuadernacion_alzado As String = ""
            Dim encuadernacion_cortado As String = ""
            Dim encuadernacion_perforado As String = ""
            Dim encuadernacion_grapado As String = ""
            Dim encuadernacion_numerado As String = ""
            Dim encuadernacion_empacado As String = ""
            Dim observaciones_especificas As String = ""
            Dim estado As String = ""
            Dim estadoOrden As String = ""
            Dim colorPortada As String = ""
            Dim colorInterior As String = ""
            Dim tiroPortada As String = ""
            Dim tiroInterior As String = ""
            Dim descripcionDelTrabajo As String = ""

            Dim dsOrdenes As New DsOrdenesDeProduccion()
            Dim filaOrden As DataRow
            Dim filaDetalleOrden As DataRow

            Dim query As String = "SELECT A.FECHA_CREACION,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO,B.CORREO_CLIENTE,B.DIRECCION_CLIENTE,
	B.TELEFONO_CLIENTE,D.*,A.ESTADO_ORDEN,A.ESTADO,E.NOMBRE_PRODUCTO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE 
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO
                                        INNER JOIN DETALLES_ORDENES_PRODUCCION D
											ON A.NUMERO_ORDEN=D.NUMERO_ORDEN 
                                            INNER JOIN TBL_PRODUCTOS E
                                                ON A.ID_PRODUCTO=E.ID_PRODUCTO
                                        WHERE A.NUMERO_ORDEN=" + numOrden
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector = comando.ExecuteReader()
            While (lector.Read())
                'fecha_creacion = lector("FECHA_CREACION").ToString()
                'nombre_cliente = lector("NOMBRE_CLIENTE").ToString()
                'nombre_usuario = lector("NOMBRE_USUARIO").ToString()
                'correo_cliente = lector("CORREO_CLIENTE").ToString()
                'direccion_cliente = lector("DIRECCION_CLIENTE").ToString()
                'telefono_cliente = lector("TELEFONO_CLIENTE").ToString()
                'numero_orden = lector("NUMERO_ORDEN").ToString()
                'fecha_entrega = lector("FECHA_ENTREGA").ToString()
                'tamaño = lector("TAMAÑO").ToString()
                'cantidad = lector("CANTIDAD").ToString()
                'numero_paginas = lector("NUMERO_PAGINAS").ToString()
                'lugar_entrega = lector("LUGAR_ENTREGA").ToString()
                'prioridad = lector("PRIORIDAD").ToString()
                'orientacion = lector("ORIENTACION").ToString()
                'material_portada = lector("MATERIAL_PORTADA").ToString()
                'gramaje_portada = lector("GRAMAJE_PORTADA").ToString()
                'color_portada = lector("COLOR_PORTADA").ToString()
                'tamaño_portada = lector("TAMAÑO_PORTADA").ToString()
                'material_interior = lector("MATERIAL_INTERIOR").ToString()
                'gramaje_interior = lector("GRAMAJE_INTERIOR").ToString()
                'color_interior = lector("COLOR_INTERIOR").ToString()
                'tamaño_interior = lector("TAMAÑO_INTERIOR").ToString()
                'material_otro = lector("MATERIAL_OTRO").ToString()
                'gramaje_otro = lector("GRAMAJE_OTRO").ToString()
                'color_otro = lector("COLOR_OTRO").ToString()
                'tamaño_otro = lector("TAMAÑO_OTRO").ToString()
                'cantidad_resmas_portada = lector("CANTIDAD_RESMAS_PORTADA").ToString()
                'cantidad_resmas_interior = lector("CANTIDAD_RESMAS_INTERIOR").ToString()
                'cantidad_resmas_otro = lector("CANTIDAD_RESMAS_OTRO").ToString()
                'full_color_portada = lector("FULL_COLOR_PORTADA").ToString()
                'duotono_portada = lector("DUOTONO_PORTADA").ToString()
                'unicolor_portada = lector("UNICOLOR_PORTADA").ToString()
                'pantone_portada = lector("PANTONE_PORTADA").ToString()
                'cantidad_tinta_portada = lector("CANTIDAD_TINTA_PORTADA").ToString()
                'full_color_interior = lector("FULL_COLOR_INTERIOR").ToString()
                'duotono_interior = lector("DUOTONO_INTERIOR").ToString()
                'unicolor_interior = lector("UNICOLOR_INTERIOR").ToString()
                'pantone_interior = lector("PANTONE_INTERIOR").ToString()
                'cantidad_tinta_interior = lector("CANTIDAD_TINTA_INTERIOR").ToString()
                'acabado_de_portada = lector("ACABADO_DE_PORTADA").ToString()
                'cantidad_acabado_de_portada = lector("CANTIDAD_ACABADO_DE_PORTADA").ToString()
                'diseño_diseño = lector("DISEÑO_DISEÑO").ToString()
                'diseño_imp_digital = lector("DISEÑO_IMP_DIGITAL").ToString()
                'diseño_ctp = lector("DISEÑO_CTP").ToString()
                'diseño_prensa = lector("DISEÑO_PRENSA").ToString()
                'diseño_reimpresion = lector("DISEÑO_REIMPRESION").ToString()
                'portada_tiro_retiro = lector("PORTADA_TIRO_RETIRO").ToString()
                'portada_tiro = lector("PORTADA_TIRO").ToString()
                'interior_tiro_retiro = lector("INTERIOR_TIRO_RETIRO").ToString()
                'interior_tiro = lector("INTERIOR_TIRO").ToString()
                'cantidad_imprimir = lector("CANTIDAD_IMPRIMIR").ToString()
                'encuadernacion_plegado = lector("ENCUADERNACION_PLEGADO").ToString()
                'encuadernacion_pegado = lector("ENCUADERNACION_PEGADO").ToString()
                'encuadernacion_alzado = lector("ENCUADERNACION_ALZADO").ToString()
                'encuadernacion_cortado = lector("ENCUADERNACION_CORTADO").ToString()
                'encuadernacion_perforado = lector("ENCUADERNACION_PERFORADO").ToString()
                'encuadernacion_grapado = lector("ENCUADERNACION_GRAPADO").ToString()
                'encuadernacion_numerado = lector("ENCUADERNACION_NUMERADO").ToString()
                'encuadernacion_empacado = lector("ENCUADERNACION_EMPACADO").ToString()
                'observaciones_especificas = lector("OBSERVACIONES_ESPECIFICAS").ToString()
                'estado = lector("ESTADO").ToString()
                'estadoOrden = lector("ESTADO_ORDEN").ToString()
                'colorPortada = lector("COLORPORTADA").ToString()
                'colorInterior = lector("COLORINTERIOR").ToString()
                'tiroPortada = lector("TIROPORTADA").ToString()
                'tiroInterior = lector("TIROINTERIOR").ToString()
                'descripcionDelTrabajo = lector("DESCRIPCION_DEL_TRABAJO").ToString()

                filaOrden = dsOrdenes.Tables("TBL_ORDENES_PRODUCCION").NewRow()
                filaOrden.Item("FECHA_CREACION") = lector("FECHA_CREACION").ToString()
                filaOrden.Item("NOMBRE_CLIENTE") = lector("NOMBRE_CLIENTE").ToString()
                filaOrden.Item("NOMBRE_USUARIO") = lector("NOMBRE_USUARIO").ToString()
                filaOrden.Item("CORREO_CLIENTE") = lector("CORREO_CLIENTE").ToString()
                filaOrden.Item("DIRECCION_CLIENTE") = lector("DIRECCION_CLIENTE").ToString()
                filaOrden.Item("TELEFONO_CLIENTE") = lector("TELEFONO_CLIENTE").ToString()
                filaOrden.Item("NUMERO_ORDEN") = lector("NUMERO_ORDEN").ToString()
                filaOrden.Item("ESTADO") = lector("ESTADO").ToString()
                filaOrden.Item("ESTADO_ORDEN") = lector("ESTADO_ORDEN").ToString()
                dsOrdenes.Tables("TBL_ORDENES_PRODUCCION").Rows.Add(filaOrden)

                filaDetalleOrden = dsOrdenes.Tables("DETALLES_ORDENES_PRODUCCION").NewRow()
                filaDetalleOrden.Item("NUMERO_ORDEN") = lector("NUMERO_ORDEN").ToString()
                filaDetalleOrden.Item("FECHA_ENTREGA") = lector("FECHA_ENTREGA").ToString()
                filaDetalleOrden.Item("TAMAÑO") = lector("TAMAÑO").ToString()
                filaDetalleOrden.Item("CANTIDAD") = lector("CANTIDAD").ToString()
                filaDetalleOrden.Item("NUMERO_PAGINAS") = lector("NUMERO_PAGINAS").ToString()
                filaDetalleOrden.Item("LUGAR_ENTREGA") = lector("LUGAR_ENTREGA").ToString()
                filaDetalleOrden.Item("PRIORIDAD") = lector("PRIORIDAD").ToString()
                filaDetalleOrden.Item("ORIENTACION") = lector("ORIENTACION").ToString()
                filaDetalleOrden.Item("MATERIAL_PORTADA") = lector("MATERIAL_PORTADA").ToString()
                filaDetalleOrden.Item("GRAMAJE_PORTADA") = lector("GRAMAJE_PORTADA").ToString()
                filaDetalleOrden.Item("COLOR_PORTADA") = lector("COLOR_PORTADA").ToString()
                filaDetalleOrden.Item("TAMAÑO_PORTADA") = lector("TAMAÑO_PORTADA").ToString()
                filaDetalleOrden.Item("MATERIAL_INTERIOR") = lector("MATERIAL_INTERIOR").ToString()
                filaDetalleOrden.Item("GRAMAJE_INTERIOR") = lector("GRAMAJE_INTERIOR").ToString()
                filaDetalleOrden.Item("COLOR_INTERIOR") = lector("COLOR_INTERIOR").ToString()
                filaDetalleOrden.Item("TAMAÑO_INTERIOR") = lector("TAMAÑO_INTERIOR").ToString()
                filaDetalleOrden.Item("MATERIAL_OTRO") = lector("MATERIAL_OTRO").ToString()
                filaDetalleOrden.Item("GRAMAJE_OTRO") = lector("GRAMAJE_OTRO").ToString()
                filaDetalleOrden.Item("COLOR_OTRO") = lector("COLOR_OTRO").ToString()
                filaDetalleOrden.Item("TAMAÑO_OTRO") = lector("TAMAÑO_OTRO").ToString()
                filaDetalleOrden.Item("CANTIDAD_RESMAS_PORTADA") = lector("CANTIDAD_RESMAS_PORTADA").ToString()
                filaDetalleOrden.Item("CANTIDAD_RESMAS_INTERIOR") = lector("CANTIDAD_RESMAS_INTERIOR").ToString()
                filaDetalleOrden.Item("CANTIDAD_RESMAS_OTRO") = lector("CANTIDAD_RESMAS_OTRO").ToString()
                filaDetalleOrden.Item("FULL_COLOR_PORTADA") = lector("FULL_COLOR_PORTADA").ToString()
                filaDetalleOrden.Item("DUOTONO_PORTADA") = lector("DUOTONO_PORTADA").ToString()
                filaDetalleOrden.Item("UNICOLOR_PORTADA") = lector("UNICOLOR_PORTADA").ToString()
                filaDetalleOrden.Item("PANTONE_PORTADA") = lector("PANTONE_PORTADA").ToString()
                filaDetalleOrden.Item("CANTIDAD_TINTA_PORTADA") = lector("CANTIDAD_TINTA_PORTADA").ToString()
                filaDetalleOrden.Item("FULL_COLOR_INTERIOR") = lector("FULL_COLOR_INTERIOR").ToString()
                filaDetalleOrden.Item("DUOTONO_INTERIOR") = lector("DUOTONO_INTERIOR").ToString()
                filaDetalleOrden.Item("UNICOLOR_INTERIOR") = lector("UNICOLOR_INTERIOR").ToString()
                filaDetalleOrden.Item("PANTONE_INTERIOR") = lector("PANTONE_INTERIOR").ToString()
                filaDetalleOrden.Item("CANTIDAD_TINTA_INTERIOR") = lector("CANTIDAD_TINTA_INTERIOR").ToString()
                filaDetalleOrden.Item("ACABADO_DE_PORTADA") = lector("ACABADO_DE_PORTADA").ToString()
                filaDetalleOrden.Item("CANTIDAD_ACABADO_DE_PORTADA") = lector("CANTIDAD_ACABADO_DE_PORTADA").ToString()
                filaDetalleOrden.Item("DISEÑO_DISEÑO") = lector("DISEÑO_DISEÑO").ToString()
                filaDetalleOrden.Item("DISEÑO_IMP_DIGITAL") = lector("DISEÑO_IMP_DIGITAL").ToString()
                filaDetalleOrden.Item("DISEÑO_CTP") = lector("DISEÑO_CTP").ToString()
                filaDetalleOrden.Item("DISEÑO_PRENSA") = lector("DISEÑO_PRENSA").ToString()
                filaDetalleOrden.Item("DISEÑO_REIMPRESION") = lector("DISEÑO_REIMPRESION").ToString()
                filaDetalleOrden.Item("PORTADA_TIRO_RETIRO") = lector("PORTADA_TIRO_RETIRO").ToString()
                filaDetalleOrden.Item("PORTADA_TIRO") = lector("PORTADA_TIRO").ToString()
                filaDetalleOrden.Item("INTERIOR_TIRO_RETIRO") = lector("INTERIOR_TIRO_RETIRO").ToString()
                filaDetalleOrden.Item("INTERIOR_TIRO") = lector("INTERIOR_TIRO").ToString()
                filaDetalleOrden.Item("CANTIDAD_IMPRIMIR") = lector("CANTIDAD_IMPRIMIR").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_PLEGADO") = lector("ENCUADERNACION_PLEGADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_PEGADO") = lector("ENCUADERNACION_PEGADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_ALZADO") = lector("ENCUADERNACION_ALZADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_CORTADO") = lector("ENCUADERNACION_CORTADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_PERFORADO") = lector("ENCUADERNACION_PERFORADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_GRAPADO") = lector("ENCUADERNACION_GRAPADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_NUMERADO") = lector("ENCUADERNACION_NUMERADO").ToString()
                filaDetalleOrden.Item("ENCUADERNACION_EMPACADO") = lector("ENCUADERNACION_EMPACADO").ToString()
                filaDetalleOrden.Item("OBSERVACIONES_ESPECIFICAS") = lector("OBSERVACIONES_ESPECIFICAS").ToString()

                filaDetalleOrden.Item("COLORPORTADA") = lector("COLORPORTADA").ToString()
                filaDetalleOrden.Item("COLORINTERIOR") = lector("COLORINTERIOR").ToString()
                filaDetalleOrden.Item("TIROPORTADA") = lector("TIROPORTADA").ToString()
                filaDetalleOrden.Item("TIROINTERIOR") = lector("TIROINTERIOR").ToString()
                filaDetalleOrden.Item("DESCRIPCION_DEL_TRABAJO") = lector("DESCRIPCION_DEL_TRABAJO").ToString()
                filaDetalleOrden.Item("NOMBRE_PRODUCTO") = lector("NOMBRE_PRODUCTO").ToString()
                dsOrdenes.Tables("DETALLES_ORDENES_PRODUCCION").Rows.Add(filaDetalleOrden)
            End While
            conexion.Close()

            Dim nombreArchivo As String = "Orden - " + numeroOrden + ".pdf"
            Dim nombreArchivoOrden As String = "Orden " + numeroOrden + ".pdf"
            Dim directorioOrden As String = Server.MapPath("~/pdf/" + nombreArchivoOrden)

            Dim crystalReport As ReportDocument = New ReportDocument()
            If System.IO.File.Exists(directorioOrden) Then

                'System.IO.File.Delete(directorioOrden)
            Else


                crystalReport.Load(Server.MapPath("~/OrdenDeProduccion.rpt"))
            crystalReport.SetDataSource(dsOrdenes)
            crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorioOrden)
            End If

            '''' GENERANDO PDF PROCESO DE TRABAJO ''''
            query = "SELECT * FROM TBL_PROCESO_DE_TRABAJO WHERE NUMERO_ORDEN=" + numOrden
            Dim dsProcesoTrabajo As New DsProcesoTrabajo()
            Dim filaProcesoTrabajo As DataRow
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            lector = comando.ExecuteReader()
            While lector.Read()
                filaProcesoTrabajo = dsProcesoTrabajo.Tables("TBL_PROCESO_DE_TRABAJO").NewRow()
                filaProcesoTrabajo.Item("NUMERO_ORDEN") = lector("NUMERO_ORDEN").ToString()
                filaProcesoTrabajo.Item("FECHA_INICIAL_DISEÑO") = lector("FECHA_INICIAL_DISEÑO").ToString()
                filaProcesoTrabajo.Item("PORTADA_DISEÑADA") = lector("PORTADA_DISEÑADA").ToString()
                filaProcesoTrabajo.Item("INTERIOR_DIAGRAMADO_DISEÑO") = lector("INTERIOR_DIAGRAMADO_DISEÑO").ToString()
                filaProcesoTrabajo.Item("OTRO_DISEÑO") = lector("OTRO_DISEÑO").ToString()
                filaProcesoTrabajo.Item("ENTREGADO_DISEÑO") = lector("ENTREGADO_DISEÑO").ToString()
                filaProcesoTrabajo.Item("FECHA_FINAL_DISEÑO") = lector("FECHA_FINAL_DISEÑO").ToString()
                filaProcesoTrabajo.Item("RESPONSABLE_PREPRENSA") = lector("RESPONSABLE_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("FECHA_INICIAL_PREPRENSA") = lector("FECHA_INICIAL_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("TAMAÑO_PLANCHAS_PREPRENSA") = lector("TAMAÑO_PLANCHAS_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("CANTIDAD_INTERIOR_PREPRENSA") = lector("CANTIDAD_INTERIOR_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("CANTIDAD_PORTADA_PREPRENSA") = lector("CANTIDAD_PORTADA_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("TERMINADO_PREPRENSA") = lector("TERMINADO_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("ENTREGADO_PREPRENSA") = lector("ENTREGADO_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("FECHA_FINAL_PREPRENSA") = lector("FECHA_FINAL_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("FECHA_INICIAL_PRENSA") = lector("FECHA_INICIAL_PRENSA").ToString()
                filaProcesoTrabajo.Item("FECHA_FINAL_PRENSA") = lector("FECHA_FINAL_PRENSA").ToString()
                filaProcesoTrabajo.Item("PORTADA_PRENSA") = lector("PORTADA_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_1_PRENSA") = lector("PLIEG_1_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_2_PRENSA") = lector("PLIEG_2_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_3_PRENSA") = lector("PLIEG_3_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_4_PRENSA") = lector("PLIEG_4_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_5_PRENSA") = lector("PLIEG_5_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_6_PRENSA") = lector("PLIEG_6_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_7_PRENSA") = lector("PLIEG_7_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_8_PRENSA") = lector("PLIEG_8_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_9_PRENSA") = lector("PLIEG_9_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_10_PRENSA") = lector("PLIEG_10_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_11_PRENSA") = lector("PLIEG_11_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_12_PRENSA") = lector("PLIEG_12_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_13_PRENSA") = lector("PLIEG_13_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_14_PRENSA") = lector("PLIEG_14_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_15_PRENSA") = lector("PLIEG_15_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_16_PRENSA") = lector("PLIEG_16_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_17_PRENSA") = lector("PLIEG_17_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_18_PRENSA") = lector("PLIEG_18_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_19_PRENSA") = lector("PLIEG_19_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_20_PRENSA") = lector("PLIEG_20_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_21_PRENSA") = lector("PLIEG_21_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_22_PRENSA") = lector("PLIEG_22_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_23_PRENSA") = lector("PLIEG_23_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_24_PRENSA") = lector("PLIEG_24_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_25_PRENSA") = lector("PLIEG_25_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_26_PRENSA") = lector("PLIEG_26_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_27_PRENSA") = lector("PLIEG_27_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_28_PRENSA") = lector("PLIEG_28_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_29_PRENSA") = lector("PLIEG_29_PRENSA").ToString()
                filaProcesoTrabajo.Item("PLIEG_30_PRENSA") = lector("PLIEG_30_PRENSA").ToString()
                filaProcesoTrabajo.Item("RESPONSABLE_PRENSA") = lector("RESPONSABLE_PRENSA").ToString()
                filaProcesoTrabajo.Item("FECHA_INICIAL_PLEGADORA") = lector("FECHA_INICIAL_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("FECHA_FINAL_PLEGADORA") = lector("FECHA_FINAL_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PORTADA_PLEGADORA") = lector("PORTADA_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_1_PLEGADORA") = lector("PLIEG_1_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_2_PLEGADORA") = lector("PLIEG_2_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_3_PLEGADORA") = lector("PLIEG_3_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_4_PLEGADORA") = lector("PLIEG_4_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_5_PLEGADORA") = lector("PLIEG_5_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_6_PLEGADORA") = lector("PLIEG_6_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_7_PLEGADORA") = lector("PLIEG_7_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_8_PLEGADORA") = lector("PLIEG_8_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_9_PLEGADORA") = lector("PLIEG_9_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_10_PLEGADORA") = lector("PLIEG_10_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_11_PLEGADORA") = lector("PLIEG_11_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_12_PLEGADORA") = lector("PLIEG_12_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_13_PLEGADORA") = lector("PLIEG_13_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_14_PLEGADORA") = lector("PLIEG_14_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_15_PLEGADORA") = lector("PLIEG_15_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_16_PLEGADORA") = lector("PLIEG_16_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_17_PLEGADORA") = lector("PLIEG_17_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_18_PLEGADORA") = lector("PLIEG_18_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_19_PLEGADORA") = lector("PLIEG_19_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_20_PLEGADORA") = lector("PLIEG_20_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_21_PLEGADORA") = lector("PLIEG_21_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_22_PLEGADORA") = lector("PLIEG_22_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_23_PLEGADORA") = lector("PLIEG_23_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_24_PLEGADORA") = lector("PLIEG_24_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_25_PLEGADORA") = lector("PLIEG_25_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_26_PLEGADORA") = lector("PLIEG_26_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_27_PLEGADORA") = lector("PLIEG_27_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_28_PLEGADORA") = lector("PLIEG_28_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_29_PLEGADORA") = lector("PLIEG_29_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("PLIEG_30_PLEGADORA") = lector("PLIEG_30_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("RESPONSABLE_PLEGADORA") = lector("RESPONSABLE_PLEGADORA").ToString()
                filaProcesoTrabajo.Item("FECHA_INICIAL_RB5") = lector("FECHA_INICIAL_RB5").ToString()
                filaProcesoTrabajo.Item("TOTAL_TERMINADO_RB5") = lector("TOTAL_TERMINADO_RB5").ToString()
                filaProcesoTrabajo.Item("FECHA_FINAL_RB5") = lector("FECHA_FINAL_RB5").ToString()
                filaProcesoTrabajo.Item("FECHA_INICIAL_EMPAQUE") = lector("FECHA_INICIAL_EMPAQUE").ToString()
                filaProcesoTrabajo.Item("TOTAL_EMPACADO_EMPAQUE") = lector("TOTAL_EMPACADO_EMPAQUE").ToString()
                filaProcesoTrabajo.Item("FECHA_FINAL_EMPAQUE") = lector("FECHA_FINAL_EMPAQUE").ToString()
                filaProcesoTrabajo.Item("PESO_TOTAL") = lector("PESO_TOTAL").ToString()
                filaProcesoTrabajo.Item("ENTREGA_DOMICILIO") = lector("ENTREGA_DOMICILIO").ToString()
                filaProcesoTrabajo.Item("RECOMENDACION") = lector("RECOMENDACION").ToString()
                filaProcesoTrabajo.Item("COMENTARIO_DISEÑO") = lector("COMENTARIO_DISEÑO").ToString()
                filaProcesoTrabajo.Item("COMENTARIO_PRENSA") = lector("COMENTARIO_PRENSA").ToString()
                filaProcesoTrabajo.Item("COMENTARIO_PREPRENSA") = lector("COMENTARIO_PREPRENSA").ToString()
                filaProcesoTrabajo.Item("COMENTARIO_EMPAQUE") = lector("COMENTARIO_EMPAQUE").ToString()
                filaProcesoTrabajo.Item("COMENTARIO_RB5") = lector("COMENTARIO_RB5").ToString()
                filaProcesoTrabajo.Item("COMENTARIO_PLEGADORA") = lector("COMENTARIO_PLEGADORA").ToString()
                dsProcesoTrabajo.Tables("TBL_PROCESO_DE_TRABAJO").Rows.Add(filaProcesoTrabajo)
            End While
            conexion.Close()
            Dim nombreArchivoProcesoTrabajo As String = "Proceso - " + numeroOrden + ".pdf"
            Dim directorioArchivoProcesoTrabajo As String = Server.MapPath("~/pdf/" + nombreArchivoProcesoTrabajo)

            crystalReport = New ReportDocument()

            If System.IO.File.Exists(directorioArchivoProcesoTrabajo) Then
                ' System.IO.File.Delete(directorioArchivoProcesoTrabajo)
            Else

                crystalReport.Load(Server.MapPath("~/ProcesoDeTrabajo.rpt"))
            crystalReport.SetDataSource(dsProcesoTrabajo)
            crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorioArchivoProcesoTrabajo)
            End If

            bitacora.registrarBitacora(Session("usuario").ToString(), "VISUALIZACIÓN DE ÓRDEN DE PRODUCCIÓN")

            ''''UNIENDO PDF''''
            Dim directorio As String = Server.MapPath("~/pdf/" + nombreArchivo)
            If System.IO.File.Exists(directorio) Then
                System.IO.File.Delete(directorio)
            End If
            Dim document As Document = New Document()
            Dim archivos As ArrayList = New ArrayList()
            archivos.Add(directorioOrden)
            archivos.Add(directorioArchivoProcesoTrabajo)

            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(directorio, FileMode.Create))
            document.Open()
            Dim cb as PdfContentByte  = writer.DirectContent
            Dim Page As PdfImportedPage 
            Dim n As Integer= 0
            Dim rotation As Integer= 0

            For Each archivo As String In archivos
                Dim reader As PdfReader = New PdfReader(archivo)
                n = reader.NumberOfPages
                Dim i As Integer = 0
                While (i < n)
                    i = i + 1
                    document.SetPageSize(reader.GetPageSizeWithRotation(1))
                    document.NewPage()
                    If i = 1 Then
                        Dim fileRef As Chunk = New Chunk(" ")
                        fileRef.SetLocalDestination(archivo)
                        document.Add(fileRef)
                    End If
                    Page = writer.GetImportedPage(reader, i)
                    rotation = reader.GetPageRotation(i)

                    If rotation = 90 And rotation = 270 Then
                        cb.AddTemplate(Page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                    Else
                        cb.AddTemplate(Page, 1.0F, 0, 0, 1.0F, 0, 0)
                    End If
                End While
            Next
            document.Close()

            Session("archivoOrden") = "../pdf/" + nombreArchivo
            Session("nombreArchivo") = nombreArchivo
            Return RedirectToAction("VerOrdenProduccion", "OrdenesDeProduccion")


            ''Response.ContentType = "application/octet-stream"
            ''Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
            ''Response.TransmitFile(directorio)
            ''Response.End()

            '            Dim directorioLogo As String = Server.MapPath("~/Images/" + "logo3.png")
            '            Dim nombreArchivo As String = "Orden" + "-" + numOrden.ToString() + ".pdf"
            '            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            '            'document.Save(directorio)
            '            Dim nombreArchivoHTML As String = "Orden" + "-" + numOrden.ToString() + ".html"
            '            Dim directorioHTML As String = Server.MapPath("/pdf/" + nombreArchivoHTML)
            '            Dim file As System.IO.StreamWriter
            '            If System.IO.File.Exists(directorioHTML) Then
            '                System.IO.File.Delete(directorioHTML)
            '                System.IO.File.Delete(directorio)
            '            End If
            '            file = My.Computer.FileSystem.OpenTextFileWriter(directorioHTML, True)
            '            file.WriteLine("<html>
            '    <head>
            '        <link rel=" & ControlChars.Quote & "stylesheet" & ControlChars.Quote & " href=" & ControlChars.Quote & "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" & ControlChars.Quote & ">
            '        <script src=" & ControlChars.Quote & "https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js" & ControlChars.Quote & "></script>
            '        <script src=" & ControlChars.Quote & "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js" & ControlChars.Quote & "></script>
            '        <style>
            '           .table {
            '        font-size: 5px;
            '    }
            '        </style>
            '    </head>
            '    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
            '        <table  width=" & ControlChars.Quote & "100%" & ControlChars.Quote & " >
            '            <tr>
            '                <td align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
            '                 <img src=" & ControlChars.Quote & directorioLogo & ControlChars.Quote & " width=" & ControlChars.Quote & "155.33858268" & ControlChars.Quote & " height=" & ControlChars.Quote & "70.11023622" & ControlChars.Quote & ">
            '                </td></tr>
            '        </table>
            '    </div>
            '    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
            '        <br>
            '            <h2><strong>ORDEN DE PRODUCCIÓN</strong></h2>
            '    </div>
            '    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & " >
            '        <br>
            '        <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
            '           <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & " width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
            '               <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Cliente: " + nombre_cliente + "</td></tr>
            '               <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Contacto: " + telefono_cliente + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Teléfono:" + telefono_cliente + "</td><td>Email: " + correo_cliente + "</td></tr>
            '               <tr><td  colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Nombre y descripción del trabajo: " + observaciones_especificas + "</td></tr>
            '               <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Tamaño: " + tamaño + "</td><td>Cantidad:" + cantidad + "</td><td>" + numero_paginas + "</td></tr>
            '               <tr><td  colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Lugar de entrega: " + lugar_entrega + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Fecha de entrega: " + Date.Parse(fecha_entrega).ToString("dd/mm/yyyy") + "</td></tr>
            '               <tr><td  colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Prioridad: " + prioridad + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Orientación: " + orientacion + "</td></tr>
            '               <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Orden elaborada por: " + nombre_usuario + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Fecha: " + fecha_creacion + "</td></tr>
            '           </table>
            '            <table  class=" & ControlChars.Quote & "table" & ControlChars.Quote & " width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
            '                <tr><td><strong>ÁREA EN QUE SE ENCUENTRA LA ORDEN: " + estadoOrden + "</strong></td><td><strong>ESTADO DE LA ORDEN: " + estado + "</strong></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><h3>Características del trabajo</h3></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Descripción</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Material:</strong></td></tr>
            '                <tr><td width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Portada " + material_portada + "</td><td  width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Gramaje " + gramaje_portada + "</td><td  width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Color  " + color_portada + "</td><td  width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Tamaño " + tamaño_portada + "</td></tr>
            '                <tr><td>Interior " + material_interior + "</td><td>Gramaje " + gramaje_interior + "</td><td>Color " + color_otro + "</td><td>Tamaño " + tamaño_interior + "</td></tr>
            '                <tr><td>Otro " + material_otro + "</td><td>Gramaje " + gramaje_otro + "</td><td>Color</td><td>Tamaño " + tamaño_otro + "</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br>Cantidad de resmas a utilizar:</td></tr>
            '                <tr><td>Portada " + cantidad_resmas_portada + "</td><td>Interior " + cantidad_tinta_interior + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "> " + cantidad_resmas_otro + "</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Color:</strong></td></tr>
            '                <tr><td width=" & ControlChars.Quote & "33%" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">Portada:</td><td width=" & ControlChars.Quote & "33%" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">Interior</td><td width=" & ControlChars.Quote & "33%" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & " colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Acabado de portada</td></tr>
            '                <tr><td>Full color:  " + colorPortada + "</td><td>Full color " + colorInterior + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Barníz Mate  " + acabado_de_portada + "</label><br></td></tr>
            '                <tr><td>Duotono</td><td>Duotono </td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Barníz Brillante</label><br></td></tr>
            '                <tr><td>Un color</td><td>Un color </td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "></td></tr>
            '                <tr><td>Pantone</td><td>Pantone</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "></td></tr>
            '                <tr><td>Cantidad de tinta: " + cantidad_tinta_portada + "</td><td>Cantidad de tinta: " + cantidad_tinta_interior + "</td><td  colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Cantidad: " + cantidad_acabado_de_portada + "</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Diseño:</strong></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Destino final</td></tr>
            '                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Diseño " + diseño_diseño + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> CTP " + diseño_ctp + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Reimpresión " + diseño_reimpresion + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Imp. Digital " + diseño_imp_digital + "</label><br></td></tr>
            '                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Prensa " + diseño_prensa + "</label><br></td><td></td><td colspan=" & ControlChars.Quote & "3" & ControlChars.Quote & "></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Impresión:</strong></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Portada</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Interior</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro/retiro:  " + tiroPortada + "</label><br></td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro/retiro: " + tiroInterior + "</label><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro</label><br></td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro </label><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Cantidad a imprimir (Ya incluye excedente)</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Encuadernación:</strong></td></tr>
            '                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Plegado " + encuadernacion_plegado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Pegado " + encuadernacion_pegado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Alzado " + encuadernacion_alzado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Cortado " + encuadernacion_cortado + "</label><br></td></tr>
            '                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Perforado " + encuadernacion_perforado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Grapado " + encuadernacion_grapado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Numerado " + encuadernacion_numerado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Empacado " + encuadernacion_empacado + "</label><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Observaciones específicas: " + observaciones_especificas + "</strong></td></tr>
            '                <tr><td colspan=4></td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
            '                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
            '                <tr><td colspan=4><strong>NO PROCESE SI TIENE DUDAS, Consulte a su jefe inmediato, De no hacerlo le acarreará responsabilidad directa en el resultado del proceso.</strong></td></tr>
            '            </table>
            '        </div>
            '    </div>

            '</html>")
            '            file.Close()

            '            'Dim Renderer = New IronPdf.HtmlToPdf()
            '            'Dim PDF = Renderer.RenderHTMLFileAsPdf(directorioHTML)
            '            'Dim OutputPath = directorio
            '            'PDF.SaveAs(OutputPath)
            '            Dim baseUri As String = "src/main/resources/html/"

            '            Dim properties As ConverterProperties = New ConverterProperties()
            '            HtmlConverter.ConvertToPdf(
            '                New FileInfo(directorioHTML),
            '                New FileInfo(directorio))
            '            'FIN


            'Session("archivoOrden") = "../pdf/" + nombreArchivo
            'Session("nombreArchivo") = nombreArchivo

        End Function

        Function VerOrdenProduccion() As ActionResult
            Return View()
        End Function

        <HttpPost>
        Function VerOrdenProduccion(submit As String) As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR ÓRDEN DE PRODUCCIÓN")
            Dim nombreArchivo As String = Session("nombreArchivo").ToString()
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            Response.ContentType = "application/octet-stream"
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
            Response.TransmitFile(directorio)
            Response.End()
            Return View()
        End Function

        Function AvanzarFlujo(numeroOrden As String, nuevoEstado As String) As ActionResult
            If nuevoEstado.Equals("FINALIZAR") Then
                Session("numeroOrden") = numeroOrden
                Return RedirectToAction("FinalizarFlujo", "OrdenesDeProduccion")
            Else
                bitacora.registrarBitacora(Session("usuario").ToString(), "AVANZAR FLUJO DE PRODUCCIÓN A " + nuevoEstado)
                Dim query As String = "EXEC SP_AVANZAR_REGRESAR_FLUJO '" + Session("usuario").ToString() + "','" + nuevoEstado + "','" + numeroOrden + "'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                If nuevoEstado.Equals("IMPRESION") Then
                    Session("numeroOrden") = numeroOrden
                    Return RedirectToAction("ProcesoTrabajoDiseño", "OrdenesDeProduccion")
                ElseIf nuevoEstado.Equals("ACABADO") Then
                    Session("numeroOrden") = numeroOrden
                    Return RedirectToAction("ProcesoTrabajoImprenta", "OrdenesDeProduccion")
                ElseIf nuevoEstado.Equals("BODEGA") Then
                    Session("numeroOrden") = numeroOrden
                    Return RedirectToAction("ProcesoTrabajoAcabado", "OrdenesDeProduccion")
                Else
                    Session("mensaje") = "Flujo adelantado"
                    Return RedirectToAction("Principal", "Inicio")
                End If

            End If

        End Function
        Function RegresarFlujo(numeroOrden As String, nuevoEstado As String) As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "REGRESAR FLUJO DE PRODUCCIÓN A " + nuevoEstado)
            Dim query As String = "EXEC SP_AVANZAR_REGRESAR_FLUJO '" + Session("usuario").ToString() + "','" + nuevoEstado + "','" + numeroOrden + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Flujo retrasado"
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function AsignarEstado(numeroOrden As String, nuevoEstado As String) As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "CAMBIO DE ESTADO " + nuevoEstado)
            Dim query As String = "EXEC SP_ASIGNAR_ESTADO '" + Session("usuario").ToString() + "','" + nuevoEstado + "','" + numeroOrden + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Estado asignado"
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function FinalizarFlujo() As ActionResult
            Return View()
        End Function
        <HttpPost>
        Function FinalizarFlujo(bodega As String) As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "FINALIZACIÓN DE FLUJO")
            Dim query As String = "EXEC SP_FINALIZAR_FLUJO '" + Session("usuario").ToString() + "','" + bodega + "','" + Session("numeroOrden").ToString() + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Flujo finalizado"
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function ReporteDeOrdenes() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE VISUALIZACIÓN DE ÓRDENES DE PRODUCCIÓN")
            Return View()
        End Function

        <HttpPost>
        Function ReporteDeOrdenes(submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            If submit.Equals("generar") Then
                Dim query As String = "SELECT A.*,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO"
                If date1 <> Nothing Then
                    query = query + "  AND CAST(A.FECHA_CREACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If

                query = query + " ORDER BY CAST(A.FECHA_CREACION AS DATETIME) ASC"

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of OrdenesModel)
                While (lector.Read())
                    Dim detalles = New OrdenesModel()
                    detalles.fechaCreacion = lector("FECHA_CREACION").ToString()
                    detalles.numeroCotizacion = lector("NUMERO_COTIZACION").ToString()
                    detalles.nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                    detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.numeroOrden = lector("NUMERO_ORDEN").ToString()
                    detalles.estadoOrden = lector("ESTADO_ORDEN").ToString()
                    detalles.estado = lector("ESTADO").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos cotizacion"
                bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE VISUALIZACIÓN DE ÓRDENES DE PRODUCCIÓN")
                Return View("ReporteDeOrdenes", model)
            Else
                bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR BÚSQUEDA DE COTIZACIONES")
                Dim query As String = "SELECT A.*,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO"

                If date1 <> Nothing Then
                    query = query + "  AND CAST(A.FECHA_CREACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
                query = query + " ORDER BY CAST(A.FECHA_CREACION AS DATETIME) ASC"

                Dim dsOrdenes As New DsOrdenes()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    fila = dsOrdenes.Tables("DataTable1").NewRow()
                    fila.Item("fechaCreacion") = lector("FECHA_CREACION").ToString()
                    fila.Item("numeroCotizacion") = lector("NUMERO_COTIZACION").ToString()
                    fila.Item("cliente") = lector("NOMBRE_CLIENTE").ToString()
                    fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                    fila.Item("numeroOrden") = lector("NUMERO_ORDEN").ToString()
                    fila.Item("estadoOrden") = lector("ESTADO_ORDEN").ToString()
                    fila.Item("estado") = lector("ESTADO").ToString()
                    dsOrdenes.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "Reporte de órdenes.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeOrdenes.rpt"))
                crystalReport.SetDataSource(dsOrdenes)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
            End If
        End Function

        Function ReporteDeBodega() As ActionResult
            'CARGANDO PRODUCTOS'
            Dim productos As New List(Of String)
            Dim listadoProductos As String = ""
            Dim queryProductos = "SELECT NOMBRE_PRODUCTO FROM TBL_PRODUCTOS"
            Dim conexionProductos As SqlConnection = New SqlConnection(cadenaConexion)
            conexionProductos.Open()
            Dim comandoProductos As SqlCommand = New SqlCommand(queryProductos, conexionProductos)
            Dim lectorProductos As SqlDataReader = comandoProductos.ExecuteReader()
            While lectorProductos.Read()
                productos.Add(lectorProductos("NOMBRE_PRODUCTO").ToString())
            End While
            conexionProductos.Close()
            TempData("productos") = productos
            Return View()
        End Function

        <HttpPost>
        Function ReporteDeBodega(submit As String, bodega As String, fecha As String, producto As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            'CARGANDO PRODUCTOS'
            Dim productos As New List(Of String)
            Dim listadoProductos As String = ""
            Dim queryProductos = "SELECT NOMBRE_PRODUCTO FROM TBL_PRODUCTOS"
            Dim conexionProductos As SqlConnection = New SqlConnection(cadenaConexion)
            conexionProductos.Open()
            Dim comandoProductos As SqlCommand = New SqlCommand(queryProductos, conexionProductos)
            Dim lectorProductos As SqlDataReader = comandoProductos.ExecuteReader()
            While lectorProductos.Read()
                productos.Add(lectorProductos("NOMBRE_PRODUCTO").ToString())
            End While
            conexionProductos.Close()
            TempData("productos") = productos

            'Dim query = "SELECT A.*,B.NOMBRE_USUARIO,D.ID_PRODUCTO,E.NOMBRE_PRODUCTO FROM TBL_INGRESO_BODEGAS A
            'INNER JOIN TBL_MS_USUARIO B
            '     On A.USUARIO=B.ID_USUARIO
            '          INNER JOIN TBL_ORDENES_PRODUCCION C
            '           On A.NUMERO_ORDEN=C.NUMERO_ORDEN
            '           INNER JOIN TBL_PRODUCTOS_COTIZACION D
            '           On C.NUMERO_COTIZACION=D.NUMERO_COTIZACION
            '           INNER JOIN TBL_PRODUCTOS E
            '           On D.ID_PRODUCTO=E.ID_PRODUCTO"

            Dim query = "SELECT C.NUMERO_ORDEN, A.USUARIO, A.FECHA_INGRESO, A.BODEGA, B.NOMBRE_USUARIO, C.ID_PRODUCTO, D.NOMBRE_PRODUCTO
FROM TBL_ORDENES_PRODUCCION C, TBL_INGRESO_BODEGAS A, TBL_MS_USUARIO B, TBL_PRODUCTOS D
WHERE C.NUMERO_ORDEN=A.NUMERO_ORDEN AND A.USUARIO=B.ID_USUARIO AND C.ID_PRODUCTO=D.ID_PRODUCTO"

            Dim campoFecha = Nothing
            If fecha.Equals("FECHA DE INGRESO") Then
                campoFecha = "FECHA_INGRESO"
            ElseIf fecha.Equals("FECHA DE INGRESO") Then
                campoFecha = "FECHA_INGRESO"
            End If

            If Not bodega.Equals("TODOS") Then
                query = query + " AND A.BODEGA='" + bodega + "'"
                If campoFecha <> Nothing And date1 <> Nothing And date2 <> Nothing Then
                    query = query + " AND " + campoFecha + " BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
                If Not producto.Equals("TODOS") Then
                    query = query + " AND C.NOMBRE_PRODUCTO='" + producto + "'"
                End If
            Else
                If campoFecha <> Nothing And date1 <> Nothing And date2 <> Nothing Then
                    query = query + " AND " + campoFecha + " BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"

                End If
                If Not producto.Equals("TODOS") Then
                    query = query + " AND D.NOMBRE_PRODUCTO='" + producto + "'"
                End If
            End If

            If submit.Equals("generar") Then
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of BodegaModel)
                While (lector.Read())
                    Dim detalles = New BodegaModel()
                    detalles.numeroOrden = lector("NUMERO_ORDEN").ToString()
                    detalles.usuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.fechaIngreso = lector("FECHA_INGRESO").ToString()
                    detalles.bodega = lector("BODEGA").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE BODEGA EN PANTALLA")
                Return View("ReporteDeBodega", model)
            Else
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE BODEGA EN PDF")
                Dim dsReporteBodega As New DsReporteBodega()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    fila = dsReporteBodega.Tables("DataTable1").NewRow()
                    fila.Item("numeroOrden") = lector("NUMERO_ORDEN").ToString()
                    fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                    fila.Item("fechaIngreso") = lector("FECHA_INGRESO").ToString()
                    fila.Item("bodega") = lector("BODEGA").ToString()
                    dsReporteBodega.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "ReporteDeBodega.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeBodega.rpt"))
                crystalReport.SetDataSource(dsReporteBodega)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
                Return View()
            End If
        End Function

        Function ReporteDeInventario() As ActionResult
            Return View()
        End Function
        <HttpPost>
        Function ReporteDeInventario(submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult

            Dim query = "SELECT A.*,B.NOMBRE_USUARIO,C.NOMBRE_PRODUCTO FROM TBL_INVENTARIO A
	        INNER JOIN TBL_MS_USUARIO B
		        ON A.USUARIO=B.ID_USUARIO	
			        INNER JOIN TBL_PRODUCTOS C
				        ON A.ID_PRODUCTO=C.ID_PRODUCTO"
            Dim campoFecha = Nothing
            If date1 <> Nothing Then
                query = query + "  AND CAST(A.FECHA_INGRESO AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
            End If
            query = query + " ORDER BY CAST(A.FECHA_INGRESO AS DATETIME) ASC"
            If submit.Equals("generar") Then
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of InventarioModel)
                While (lector.Read())
                    Dim detalles = New InventarioModel()
                    detalles.numeroOrden = lector("NUMERO_ORDEN").ToString()
                    detalles.usuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.fechaIngreso = lector("FECHA_INGRESO").ToString()
                    detalles.producto = lector("NOMBRE_PRODUCTO").ToString()
                    detalles.cantidadProducto = lector("CANTIDAD").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE INVENTARIO EN PANTALLA")
                Return View("ReporteDeInventario", model)
            Else
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE INVENTARIO EN PDF")
                Dim dsReporteInventario As New DsReporteInventario()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    fila = dsReporteInventario.Tables("DataTable1").NewRow()
                    fila.Item("numeroOrden") = lector("NUMERO_ORDEN").ToString()
                    fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                    fila.Item("fechaIngreso") = lector("FECHA_INGRESO").ToString()
                    fila.Item("producto") = lector("NOMBRE_PRODUCTO").ToString()
                    fila.Item("cantidad") = Decimal.Parse(lector("CANTIDAD").ToString()).ToString("#,##0")
                    dsReporteInventario.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "ReporteDeInventario.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeInventario.rpt"))
                crystalReport.SetDataSource(dsReporteInventario)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
                Return View()
            End If
        End Function

        Function GestionDeInventario() As ActionResult
            bitacora.registrarBitacora(Session("usuario"), "INGRESO MÓDULO DE GESTIÓN DE INVENTARIO")

            Dim productos As New List(Of String)
            Dim listadoProductos As String = ""
            Dim query = "SELECT NOMBRE_PRODUCTO FROM TBL_PRODUCTOS"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                productos.Add(lector("NOMBRE_PRODUCTO").ToString())
            End While
            conexion.Close()
            TempData("productos") = productos
            Return View()
        End Function

        Function validarCantidadProductos(producto As String) As Double
            Dim cantidadProductos As Double = 0
            Dim query = "SELECT ISNULL(CANTIDAD,0) CANTIDAD FROM INVENTARIO
	            WHERE NOMBRE_PRODUCTO='" + producto + "' "

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim cantidad As String = Nothing
            cantidad = comando.ExecuteScalar()
            If cantidad = Nothing Then
                cantidadProductos = 0
            Else
                cantidadProductos = Integer.Parse(comando.ExecuteScalar.ToString())
            End If
            conexion.Close()
            Return cantidadProductos
        End Function

        <HttpPost>
        Function GestionDeInventario(submit As String, tipoGestion As String, producto As String,
                                        cantidadProducto As String, comentario As String) As ActionResult

            Dim productosExistentes As Double = validarCantidadProductos(producto)

            If tipoGestion.Equals("RETIRAR") Then
                If productosExistentes < Double.Parse(cantidadProducto) Then
                    Session("mensaje") = "¡La cantidad de productos excede el existente!"
                    Return RedirectToAction("Principal", "Inicio")
                Else
                    Dim query = "EXEC SP_GESTION_INVENTARIO '" + producto + "','" + cantidadProducto + "','" + Session("usuario").ToString() +
                   "','" + tipoGestion + "','" + comentario + "'"

                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Dim operacion As String

                    If tipoGestion.Equals("AGREGAR") Then
                        operacion = "agregado"
                        bitacora.registrarBitacora(Session("usuario"), "SE AGREGÓ PRODUCTO AL INVENTARIO")
                    Else
                        operacion = "retirado"
                        bitacora.registrarBitacora(Session("usuario"), "SE RETIRÓ PRODUCTO DEL INVENTARIO")
                    End If

                    Session("mensaje") = "Producto " + operacion + " exitosamente!"
                    Return RedirectToAction("Principal", "Inicio")
                End If
            Else
                Dim query = "EXEC SP_GESTION_INVENTARIO '" + producto + "','" + cantidadProducto + "','" + Session("usuario").ToString() +
                    "','" + tipoGestion + "','" + comentario + "'"

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Dim operacion As String

                If tipoGestion.Equals("AGREGAR") Then
                    operacion = "agregado"
                    bitacora.registrarBitacora(Session("usuario"), "SE AGREGÓ PRODUCTO AL INVENTARIO")
                Else
                    operacion = "retirado"
                    bitacora.registrarBitacora(Session("usuario"), "SE RETIRÓ PRODUCTO DEL INVENTARIO")
                End If

                Session("mensaje") = "Producto " + operacion + " exitosamente!"
                Return RedirectToAction("Principal", "Inicio")
            End If

        End Function

        Function Inventario() As ActionResult
            Return View()
        End Function
        <HttpPost>
        Function Inventario(submit As String) As ActionResult
            Dim query = "SELECT * FROM INVENTARIO"
            If submit.Equals("generar") Then
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of InventarioModel)
                While (lector.Read())
                    Dim detalles = New InventarioModel()
                    detalles.producto = lector("NOMBRE_PRODUCTO").ToString()
                    detalles.cantidadProducto = lector("CANTIDAD").ToString()
                    detalles.bodega = lector("BODEGA").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos usuario"
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE INVENTARIO EN PANTALLA")
                Return View("Inventario", model)
            Else
                bitacora.registrarBitacora(Session("usuario"), "GENERACIÓN DE REPORTE DE INVENTARIO EN PDF")
                Dim dsReporteInventario As New DsReporteInventario()
                Dim fila As DataRow
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    fila = dsReporteInventario.Tables("DataTable1").NewRow()
                    fila.Item("producto") = lector("NOMBRE_PRODUCTO").ToString()
                    fila.Item("cantidad") = Decimal.Parse(lector("CANTIDAD").ToString()).ToString("#,##0")
                    fila.Item("bodega") = lector("BODEGA").ToString()
                    dsReporteInventario.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                Dim nombreArchivo As String = "ReporteDeInventario.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeInventario2.rpt"))
                crystalReport.SetDataSource(dsReporteInventario)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
                Return View()
            End If
        End Function

        Function EditarOrden(numeroOrden As String) As ActionResult
            Dim numOrden As String = Request.QueryString("numeroOrden")
            Session("numeroOrden") = numOrden
            If Session("numeroOrden") <> Nothing Then
                Dim fecha_creacion As String = ""
                Dim nombre_cliente As String = ""
                Dim nombre_usuario As String = ""
                Dim correo_cliente As String = ""
                Dim direccion_cliente As String = ""
                Dim telefono_cliente As String = ""
                Dim numero_orden As String = ""
                Dim fecha_entrega As String = ""
                Dim tamaño As String = ""
                Dim cantidad As String = ""
                Dim numero_paginas As String = ""
                Dim lugar_entrega As String = ""
                Dim prioridad As String = ""
                Dim orientacion As String = ""
                Dim material_portada As String = ""
                Dim gramaje_portada As String = ""
                Dim color_portada As String = ""
                Dim tamaño_portada As String = ""
                Dim material_interior As String = ""
                Dim gramaje_interior As String = ""
                Dim color_interior As String = ""
                Dim tamaño_interior As String = ""
                Dim material_otro As String = ""
                Dim gramaje_otro As String = ""
                Dim color_otro As String = ""
                Dim tamaño_otro As String = ""
                Dim cantidad_resmas_portada As String = ""
                Dim cantidad_resmas_interior As String = ""
                Dim cantidad_resmas_otro As String = ""
                Dim full_color_portada As String = ""
                Dim duotono_portada As String = ""
                Dim unicolor_portada As String = ""
                Dim pantone_portada As String = ""
                Dim cantidad_tinta_portada As String = ""
                Dim full_color_interior As String = ""
                Dim duotono_interior As String = ""
                Dim unicolor_interior As String = ""
                Dim pantone_interior As String = ""
                Dim cantidad_tinta_interior As String = ""
                Dim acabado_de_portada As String = ""
                Dim cantidad_acabado_de_portada As String = ""
                Dim diseño_diseño As String = ""
                Dim diseño_imp_digital As String = ""
                Dim diseño_ctp As String = ""
                Dim diseño_prensa As String = ""
                Dim diseño_reimpresion As String = ""
                Dim portada_tiro_retiro As String = ""
                Dim portada_tiro As String = ""
                Dim interior_tiro_retiro As String = ""
                Dim interior_tiro As String = ""
                Dim cantidad_imprimir As String = ""
                Dim encuadernacion_plegado As String = ""
                Dim encuadernacion_pegado As String = ""
                Dim encuadernacion_alzado As String = ""
                Dim encuadernacion_cortado As String = ""
                Dim encuadernacion_perforado As String = ""
                Dim encuadernacion_grapado As String = ""
                Dim encuadernacion_numerado As String = ""
                Dim encuadernacion_empacado As String = ""
                Dim observaciones_especificas As String = ""
                Dim estado As String = ""
                Dim estadoOrden As String = ""
                Dim colorPortada As String = ""
                Dim colorInterior As String = ""
                Dim tiroPortada As String = ""
                Dim tiroInterior As String = ""

                Dim query As String = "SELECT A.FECHA_CREACION,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO,B.CORREO_CLIENTE,B.DIRECCION_CLIENTE,
	B.TELEFONO_CLIENTE,D.*,A.ESTADO_ORDEN,A.ESTADO,CONVERT(NVARCHAR,D.FECHA_ENTREGA,23) FECHA_ENTREGA_CONVERTIDA
        FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE 
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO
                                        INNER JOIN DETALLES_ORDENES_PRODUCCION D
											ON A.NUMERO_ORDEN=D.NUMERO_ORDEN WHERE A.NUMERO_ORDEN=" + numOrden
                Dim model As New List(Of EditarOrdenModel)
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                While (lector.Read())
                    Dim detalles = New EditarOrdenModel()
                    detalles.fecha_creacion = lector("FECHA_CREACION").ToString()
                    detalles.nombre_cliente = lector("NOMBRE_CLIENTE").ToString()
                    detalles.nombre_usuario = lector("NOMBRE_USUARIO").ToString()
                    detalles.correo_cliente = lector("CORREO_CLIENTE").ToString()
                    detalles.direccion_cliente = lector("DIRECCION_CLIENTE").ToString()
                    detalles.telefono_cliente = lector("TELEFONO_CLIENTE").ToString()
                    detalles.numero_orden = lector("NUMERO_ORDEN").ToString()
                    detalles.fecha_entrega = lector("FECHA_ENTREGA_CONVERTIDA").ToString()
                    detalles.tamaño = lector("TAMAÑO").ToString()
                    detalles.cantidad = lector("CANTIDAD").ToString()
                    detalles.numero_paginas = lector("NUMERO_PAGINAS").ToString()
                    detalles.lugar_entrega = lector("LUGAR_ENTREGA").ToString()
                    detalles.prioridad = lector("PRIORIDAD").ToString()
                    detalles.orientacion = lector("ORIENTACION").ToString()
                    detalles.material_portada = lector("MATERIAL_PORTADA").ToString()
                    detalles.gramaje_portada = lector("GRAMAJE_PORTADA").ToString()
                    detalles.color_portada = lector("COLOR_PORTADA").ToString()
                    detalles.tamaño_portada = lector("TAMAÑO_PORTADA").ToString()
                    detalles.material_interior = lector("MATERIAL_INTERIOR").ToString()
                    detalles.gramaje_interior = lector("GRAMAJE_INTERIOR").ToString()
                    detalles.color_interior = lector("COLOR_INTERIOR").ToString()
                    detalles.tamaño_interior = lector("TAMAÑO_INTERIOR").ToString()
                    detalles.material_otro = lector("MATERIAL_OTRO").ToString()
                    detalles.gramaje_otro = lector("GRAMAJE_OTRO").ToString()
                    detalles.color_otro = lector("COLOR_OTRO").ToString()
                    detalles.tamaño_otro = lector("TAMAÑO_OTRO").ToString()
                    detalles.cantidad_resmas_portada = lector("CANTIDAD_RESMAS_PORTADA").ToString()
                    detalles.cantidad_resmas_interior = lector("CANTIDAD_RESMAS_INTERIOR").ToString()
                    detalles.cantidad_resmas_otro = lector("CANTIDAD_RESMAS_OTRO").ToString()
                    detalles.full_color_portada = lector("FULL_COLOR_PORTADA").ToString()
                    detalles.duotono_portada = lector("DUOTONO_PORTADA").ToString()
                    detalles.unicolor_portada = lector("UNICOLOR_PORTADA").ToString()
                    detalles.pantone_portada = lector("PANTONE_PORTADA").ToString()
                    detalles.cantidad_tinta_portada = lector("CANTIDAD_TINTA_PORTADA").ToString()
                    detalles.full_color_interior = lector("FULL_COLOR_INTERIOR").ToString()
                    detalles.duotono_interior = lector("DUOTONO_INTERIOR").ToString()
                    detalles.unicolor_interior = lector("UNICOLOR_INTERIOR").ToString()
                    detalles.pantone_interior = lector("PANTONE_INTERIOR").ToString()
                    detalles.cantidad_tinta_interior = lector("CANTIDAD_TINTA_INTERIOR").ToString()
                    detalles.acabado_de_portada = lector("ACABADO_DE_PORTADA").ToString()
                    detalles.cantidad_acabado_de_portada = lector("CANTIDAD_ACABADO_DE_PORTADA").ToString()
                    detalles.diseño_diseño = lector("DISEÑO_DISEÑO").ToString()
                    detalles.diseño_imp_digital = lector("DISEÑO_IMP_DIGITAL").ToString()
                    detalles.diseño_ctp = lector("DISEÑO_CTP").ToString()
                    detalles.diseño_prensa = lector("DISEÑO_PRENSA").ToString()
                    detalles.diseño_reimpresion = lector("DISEÑO_REIMPRESION").ToString()
                    detalles.portada_tiro_retiro = lector("PORTADA_TIRO_RETIRO").ToString()
                    detalles.portada_tiro = lector("PORTADA_TIRO").ToString()
                    detalles.interior_tiro_retiro = lector("INTERIOR_TIRO_RETIRO").ToString()
                    detalles.interior_tiro = lector("INTERIOR_TIRO").ToString()
                    detalles.cantidad_imprimir = lector("CANTIDAD_IMPRIMIR").ToString()
                    detalles.encuadernacion_plegado = lector("ENCUADERNACION_PLEGADO").ToString()
                    detalles.encuadernacion_pegado = lector("ENCUADERNACION_PEGADO").ToString()
                    detalles.encuadernacion_alzado = lector("ENCUADERNACION_ALZADO").ToString()
                    detalles.encuadernacion_cortado = lector("ENCUADERNACION_CORTADO").ToString()
                    detalles.encuadernacion_perforado = lector("ENCUADERNACION_PERFORADO").ToString()
                    detalles.encuadernacion_grapado = lector("ENCUADERNACION_GRAPADO").ToString()
                    detalles.encuadernacion_numerado = lector("ENCUADERNACION_NUMERADO").ToString()
                    detalles.encuadernacion_empacado = lector("ENCUADERNACION_EMPACADO").ToString()
                    detalles.observaciones_especificas = lector("OBSERVACIONES_ESPECIFICAS").ToString()
                    detalles.estado = lector("ESTADO").ToString()
                    detalles.estadoOrden = lector("ESTADO_ORDEN").ToString()
                    detalles.colorPortada = lector("COLORPORTADA").ToString()
                    detalles.colorInterior = lector("COLORINTERIOR").ToString()
                    detalles.tiroPortada = lector("TIROPORTADA").ToString()
                    detalles.tiroInterior = lector("TIROINTERIOR").ToString()
                    detalles.descripcionTrabajo = lector("DESCRIPCION_DEL_TRABAJO").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                Return View("EditarOrden", model)
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function EditarOrden(lugarEntrega As String, fechaEntrega As DateTime, cantidad As String, numeroPaginas As String, tamaño As String,
                                orientacion As String, prioridad As String, descripcionDelTrabajo As String, ByVal Optional materialPortada As String = "NO",
                                ByVal Optional gramajePortada As String = "NO", ByVal Optional colorPortada As String = "NO",
                                ByVal Optional tamañoPortada As String = "NO", ByVal Optional materialInterior As String = "NO",
                                ByVal Optional gramajeInterior As String = "NO", ByVal Optional colorInterior As String = "NO",
                                ByVal Optional tamañoInterior As String = "NO", ByVal Optional materialOtro As String = "NO",
                                ByVal Optional gramajeOtro As String = "NO", ByVal Optional colorOtro As String = "NO",
                                ByVal Optional tamañoOtro As String = "NO", ByVal Optional cantidadResmasPortada As String = "NO",
                                ByVal Optional cantidadResmasInterior As String = "NO", ByVal Optional cantidadResmasOtro As String = "NO",
                                ByVal Optional fullColorPortada As String = "NO", ByVal Optional duotonoPortada As String = "NO",
                                ByVal Optional uniColorPortada As String = "NO", ByVal Optional pantonePortada As String = "NO",
                                ByVal Optional cantidadTintaPortada As String = "NO", ByVal Optional fullColorInterior As String = "NO",
                                ByVal Optional duotonoInterior As String = "NO", ByVal Optional uniColorInterior As String = "NO",
                                ByVal Optional pantoneInterior As String = "NO", ByVal Optional cantidadTintaInterior As String = "NO",
                                ByVal Optional acabadoPortada As String = "NO", ByVal Optional cantidadAcabadoPortada As String = "NO",
                                ByVal Optional diseñoDiseño As String = "NO", ByVal Optional diseñoImpDigital As String = "NO",
                                ByVal Optional diseñoCTP As String = "NO", ByVal Optional diseñoReimpresion As String = "NO",
                                ByVal Optional diseñoPrensa As String = "NO", ByVal Optional tiroRetiroPortada As String = "NO",
                                ByVal Optional tiroPortada As String = "NO", ByVal Optional tiroRetiroInterior As String = "NO",
                                ByVal Optional tiroInterior As String = "NO", ByVal Optional cantidadImprimir As String = "NO",
                                ByVal Optional plegado As String = "NO", ByVal Optional perforado As String = "NO",
                                ByVal Optional pegado As String = "NO", ByVal Optional grapado As String = "NO",
                                ByVal Optional alzado As String = "NO", ByVal Optional numerado As String = "NO",
                                ByVal Optional cortado As String = "NO", ByVal Optional empacado As String = "NO",
                                ByVal Optional observacionesEspecificas As String = "NO",
                                ByVal Optional colorPortada2 As String = "NO", ByVal Optional colorInterior2 As String = "NO",
                                ByVal Optional portadaTiro As String = "NO", ByVal Optional interiorTiro As String = "NO",
                                ByVal Optional comentarioDeEdicion As String = "SIN COMENTARIO") As ActionResult

            Dim query = "EXEC SP_EDITAR_ORDEN_PRODUCCION '" + Session("usuario").ToString() + "','" + validaciones.removerEspacios(lugarEntrega) +
                             "','" + validaciones.removerEspacios(fechaEntrega.ToString("yyyy-MM-dd")) + "','" + validaciones.removerEspacios(tamaño) + "','" + validaciones.removerEspacios(cantidad) + "','" + validaciones.removerEspacios(numeroPaginas) + "','" + prioridad + "','" + validaciones.removerEspacios(orientacion) + "','" + validaciones.removerEspacios(materialPortada) +
                             "','" + validaciones.removerEspacios(gramajePortada) + "','" + validaciones.removerEspacios(colorPortada) + "','" + validaciones.removerEspacios(tamañoPortada) + "','" + validaciones.removerEspacios(materialInterior) + "','" + validaciones.removerEspacios(gramajeInterior) +
                             "','" + validaciones.removerEspacios(colorInterior) + "','" + validaciones.removerEspacios(tamañoInterior) + "','" + validaciones.removerEspacios(materialOtro) + "','" + validaciones.removerEspacios(gramajeOtro) + "','" + validaciones.removerEspacios(colorOtro) + "','" + validaciones.removerEspacios(tamañoOtro) +
                             "','" + validaciones.removerEspacios(cantidadResmasPortada) + "','" + validaciones.removerEspacios(cantidadResmasInterior) + "','" + validaciones.removerEspacios(cantidadResmasOtro) + "','" + validaciones.removerEspacios(fullColorPortada) + "','" + validaciones.removerEspacios(duotonoPortada) +
                             "','" + validaciones.removerEspacios(uniColorPortada) + "','" + validaciones.removerEspacios(pantonePortada) + "','" + validaciones.removerEspacios(cantidadTintaPortada) + "','" + validaciones.removerEspacios(fullColorInterior) + "','" + validaciones.removerEspacios(duotonoInterior) + "','" + validaciones.removerEspacios(uniColorInterior) +
                             "','" + validaciones.removerEspacios(pantoneInterior) + "','" + validaciones.removerEspacios(cantidadTintaInterior) + "','" + validaciones.removerEspacios(acabadoPortada) + "','" + validaciones.removerEspacios(cantidadAcabadoPortada) + "','" + validaciones.removerEspacios(diseñoDiseño) + "','" + validaciones.removerEspacios(diseñoImpDigital) +
                             "','" + validaciones.removerEspacios(diseñoCTP) + "','" + validaciones.removerEspacios(diseñoReimpresion) + "','" + validaciones.removerEspacios(diseñoPrensa) + "','" + validaciones.removerEspacios(tiroRetiroPortada) + "','" + validaciones.removerEspacios(tiroPortada) + "','" + validaciones.removerEspacios(tiroRetiroInterior) + "','" + validaciones.removerEspacios(tiroInterior) +
                             "','" + validaciones.removerEspacios(cantidadImprimir) + "','" + validaciones.removerEspacios(plegado) + "','" + validaciones.removerEspacios(perforado) + "','" + validaciones.removerEspacios(pegado) + "','" + validaciones.removerEspacios(grapado) + "','" + validaciones.removerEspacios(alzado) + "','" + validaciones.removerEspacios(numerado) + "','" + validaciones.removerEspacios(cortado) + "','" + validaciones.removerEspacios(empacado) +
                             "','" + validaciones.removerEspacios(observacionesEspecificas) + "','" + validaciones.removerEspacios(colorPortada2) + "','" + validaciones.removerEspacios(colorInterior2) + "','" + validaciones.removerEspacios(portadaTiro) + "','" + validaciones.removerEspacios(interiorTiro) + "','" + validaciones.removerEspacios(descripcionDelTrabajo) + "','" + validaciones.removerEspacios(comentarioDeEdicion) + "','" + Session("numeroOrden").ToString() + "'"

            '''' VALIDANDO PRODUCTOS PENDIENTES DE ENVIO ''''
            Dim productoEnviar As String = ""
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Orden editada"
            bitacora.registrarBitacora(Session("usuario").ToString(), "EDICION DE ORDEN DE PRODUCCIÓN " + Session("numeroOrden").ToString())
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function ProcesoTrabajoDiseño() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A FORMULARIO PROCESO DE TRABAJO DISEÑO, ORDEN:" + Session("numeroOrden").ToString())
            Return View()
        End Function
        <HttpPost>
        Function ProcesoTrabajoDiseño(fechaInicial As DateTime, fechaFinal As DateTime, ByVal Optional portadaDiseñada As String = "NO",
                                             ByVal Optional interiorDiagramado As String = "NO",
                                      ByVal Optional otro As String = "NO", ByVal Optional entregado As String = "NO",
                                      ByVal Optional comentario As String = "SIN COMENTARIO") As ActionResult
            Dim query As String = "EXEC SP_PROCESO_TRABAJO_DISEÑO '" + fechaInicial.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaFinal.ToString("yyyy-MM-dd HH:mm:ss") + "','" + portadaDiseñada +
                             "','" + interiorDiagramado + "','" + otro + "','" + entregado + "','" + comentario + "','" + Session("numeroOrden").ToString() + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Flujo adelantado"
            bitacora.registrarBitacora(Session("usuario").ToString(), "GUARDADO DE PROCESO DE TRABAJO DISEÑO, ORDEN:" + Session("numeroOrden").ToString())
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function ProcesoTrabajoImprenta() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO PROCESO DE TRABAJO IMPRENTA, ORDEN:" + Session("numeroOrden").ToString())
            Return View()
        End Function

        <HttpPost>
        Function ProcesoTrabajoImprenta(fechaInicialPreprensa As DateTime, fechaFinalPreprensa As DateTime,
                                        fechaInicialPrensa As DateTime, fechaFinalPrensa As DateTime,
                                      ByVal Optional terminado As String = "NO",
                                      ByVal Optional entregado As String = "NO",
                                      ByVal Optional tamañoPlanchas As String = "NO PROPORCIONADO",
                                      ByVal Optional interiorCantidad As String = "NO PROPORCIONADO",
                                      ByVal Optional portadaCantidad As String = "NO PROPORCIONADO",
                                      ByVal Optional comentarioPrepensa As String = "SIN COMENTARIO",
                                      ByVal Optional portada As String = "NO PROPORCIONADO",
                                      ByVal Optional plieg_1 As String = "NO",
                                    ByVal Optional plieg_2 As String = "NO",
                                    ByVal Optional plieg_3 As String = "NO",
                                    ByVal Optional plieg_4 As String = "NO",
                                    ByVal Optional plieg_5 As String = "NO",
                                    ByVal Optional plieg_6 As String = "NO",
                                    ByVal Optional plieg_7 As String = "NO",
                                    ByVal Optional plieg_8 As String = "NO",
                                    ByVal Optional plieg_9 As String = "NO",
                                    ByVal Optional plieg_10 As String = "NO",
                                    ByVal Optional plieg_11 As String = "NO",
                                    ByVal Optional plieg_12 As String = "NO",
                                    ByVal Optional plieg_13 As String = "NO",
                                    ByVal Optional plieg_14 As String = "NO",
                                    ByVal Optional plieg_15 As String = "NO",
                                    ByVal Optional plieg_16 As String = "NO",
                                    ByVal Optional plieg_17 As String = "NO",
                                    ByVal Optional plieg_18 As String = "NO",
                                    ByVal Optional plieg_19 As String = "NO",
                                    ByVal Optional plieg_20 As String = "NO",
                                    ByVal Optional plieg_21 As String = "NO",
                                    ByVal Optional plieg_22 As String = "NO",
                                    ByVal Optional plieg_23 As String = "NO",
                                    ByVal Optional plieg_24 As String = "NO",
                                    ByVal Optional plieg_25 As String = "NO",
                                    ByVal Optional plieg_26 As String = "NO",
                                    ByVal Optional plieg_27 As String = "NO",
                                    ByVal Optional plieg_28 As String = "NO",
                                    ByVal Optional plieg_29 As String = "NO",
                                    ByVal Optional plieg_30 As String = "NO",
                                    ByVal Optional responsable As String = "NO PROPORCIONADO",
                                     ByVal Optional comentarioPrensa As String = "SIN COMENTARIO") As ActionResult
            Dim query As String = "EXEC SP_PROCESO_TRABAJO_IMPRENTA '" + fechaInicialPreprensa.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaFinalPreprensa.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaInicialPrensa.ToString("yyyy-MM-dd HH:mm:ss") +
                             "','" + fechaFinalPrensa.ToString("yyyy-MM-dd HH:mm:ss") + "','" + terminado + "','" + entregado + "','" + tamañoPlanchas + "','" + interiorCantidad + "','" +
                             portadaCantidad + "','" + comentarioPrepensa + "','" + portada + "','" + plieg_1 + "','" + plieg_2 + "','" + plieg_3 + "','" + plieg_4 + "','" +
                             plieg_5 + "','" + plieg_6 + "','" + plieg_7 + "','" + plieg_8 + "','" + plieg_9 + "','" + plieg_10 + "','" + plieg_11 + "','" + plieg_12 + "','" + plieg_13 + "','" +
                             plieg_14 + "','" + plieg_15 + "','" + plieg_16 + "','" + plieg_17 + "','" + plieg_18 + "','" + plieg_19 + "','" + plieg_20 + "','" + plieg_21 + "','" + plieg_22 + "','" + plieg_23 + "','" + plieg_24 + "','" + plieg_25 + "','" +
                             plieg_26 + "','" + plieg_27 + "','" + plieg_28 + "','" + plieg_29 + "','" + plieg_30 + "','" + responsable + "','" + comentarioPrensa + "','" + Session("numeroOrden").ToString() + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Flujo adelantado"
            bitacora.registrarBitacora(Session("usuario").ToString(), "GUARDADO DE PROCESO DE TRABAJO IMPRENTA, ORDEN:" + Session("numeroOrden").ToString())

            Return RedirectToAction("Principal", "Inicio")

            Return View()
        End Function

        Function ProcesoTrabajoAcabado() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO PROCESO DE TRABAJO ACABADO, ORDEN:" + Session("numeroOrden").ToString())
            Return View()
        End Function

        <HttpPost>
        Function ProcesoTrabajoAcabado(fechaInicialPlegadora As DateTime, fechaFinalPlegadora As DateTime,
                                       fechaInicialRB As DateTime, fechaFinalRB As DateTime,
                                       fechaInicialEmpaque As DateTime, fechaFinalEmpaque As DateTime,
                                     ByVal Optional responsable As String = "NO PROPORCIONADO",
                                     ByVal Optional comentarioPlegadora As String = "NO PROPORCIONADO",
                                     ByVal Optional plieg_1 As String = "NO",
                                   ByVal Optional plieg_2 As String = "NO",
                                   ByVal Optional plieg_3 As String = "NO",
                                   ByVal Optional plieg_4 As String = "NO",
                                   ByVal Optional plieg_5 As String = "NO",
                                   ByVal Optional plieg_6 As String = "NO",
                                   ByVal Optional plieg_7 As String = "NO",
                                   ByVal Optional plieg_8 As String = "NO",
                                   ByVal Optional plieg_9 As String = "NO",
                                   ByVal Optional plieg_10 As String = "NO",
                                   ByVal Optional plieg_11 As String = "NO",
                                   ByVal Optional plieg_12 As String = "NO",
                                   ByVal Optional plieg_13 As String = "NO",
                                   ByVal Optional plieg_14 As String = "NO",
                                   ByVal Optional plieg_15 As String = "NO",
                                   ByVal Optional plieg_16 As String = "NO",
                                   ByVal Optional plieg_17 As String = "NO",
                                   ByVal Optional plieg_18 As String = "NO",
                                   ByVal Optional plieg_19 As String = "NO",
                                   ByVal Optional plieg_20 As String = "NO",
                                   ByVal Optional plieg_21 As String = "NO",
                                   ByVal Optional plieg_22 As String = "NO",
                                   ByVal Optional plieg_23 As String = "NO",
                                   ByVal Optional plieg_24 As String = "NO",
                                   ByVal Optional plieg_25 As String = "NO",
                                   ByVal Optional plieg_26 As String = "NO",
                                   ByVal Optional plieg_27 As String = "NO",
                                   ByVal Optional plieg_28 As String = "NO",
                                   ByVal Optional plieg_29 As String = "NO",
                                   ByVal Optional plieg_30 As String = "NO",
                                   ByVal Optional totalTerminado As String = "NO PROPORCIONADO",
                                   ByVal Optional pesoTotal As String = "NO PROPORCIONADO",
                                   ByVal Optional comentarioRB As String = "NO PROPORCIONADO",
                                   ByVal Optional totalEmpacado As String = "NO PROPORCIONADO",
                                   ByVal Optional entregaDomicilio As String = "NO PROPORCIONADO",
                                   ByVal Optional comentarioEmpaque As String = "NO PROPORCIONADO") As ActionResult
            Dim query As String = "EXEC SP_PROCESO_TRABAJO_ACABADO '" + fechaInicialPlegadora.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaFinalPlegadora.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaInicialRB.ToString("yyyy-MM-dd HH:mm:ss") +
                             "','" + fechaFinalRB.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaInicialEmpaque.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fechaFinalEmpaque.ToString("yyyy-MM-dd HH:mm:ss") + "','" + responsable + "','" + comentarioPlegadora + "','" +
                              plieg_1 + "','" + plieg_2 + "','" + plieg_3 + "','" + plieg_4 + "','" +
                             plieg_5 + "','" + plieg_6 + "','" + plieg_7 + "','" + plieg_8 + "','" + plieg_9 + "','" + plieg_10 + "','" + plieg_11 + "','" + plieg_12 + "','" + plieg_13 + "','" +
                             plieg_14 + "','" + plieg_15 + "','" + plieg_16 + "','" + plieg_17 + "','" + plieg_18 + "','" + plieg_19 + "','" + plieg_20 + "','" + plieg_21 + "','" + plieg_22 + "','" + plieg_23 + "','" + plieg_24 + "','" + plieg_25 + "','" +
                             plieg_26 + "','" + plieg_27 + "','" + plieg_28 + "','" + plieg_29 + "','" + plieg_30 + "','" + comentarioEmpaque + "','" + totalTerminado + "','" + pesoTotal + "','" + comentarioRB + "','" + totalEmpacado + "','" +
                             entregaDomicilio + "','" + Session("numeroOrden").ToString() + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Flujo adelantado"
            bitacora.registrarBitacora(Session("usuario").ToString(), "GUARDADO PROCESO DE TRABAJO ACABADO, ORDEN:" + Session("numeroOrden").ToString())
            Return RedirectToAction("Principal", "Inicio")
        End Function
    End Class

End Namespace