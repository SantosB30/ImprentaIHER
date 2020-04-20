Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iText.Html2pdf

Namespace Controllers
    Public Class OrdenesDeProduccionController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        ' GET: OrdenesDeProduccion
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

            Dim query As String = "SELECT A.FECHA_CREACION,B.NOMBRE_CLIENTE,C.NOMBRE_USUARIO,B.CORREO_CLIENTE,B.DIRECCION_CLIENTE,
	B.TELEFONO_CLIENTE,D.*,A.ESTADO_ORDEN,A.ESTADO FROM TBL_ORDENES_PRODUCCION A
	                        INNER JOIN TBL_CLIENTES B
		                        ON A.ID_CLIENTE=B.ID_CLIENTE 
			                        INNER JOIN TBL_MS_USUARIO C
				                        ON A.ID_USUARIO_CREADOR=C.ID_USUARIO
                                        INNER JOIN DETALLES_ORDENES_PRODUCCION D
											ON A.NUMERO_ORDEN=D.NUMERO_ORDEN WHERE A.NUMERO_ORDEN=" + numOrden
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector = comando.ExecuteReader()
            While (lector.Read())
                fecha_creacion = lector("FECHA_CREACION").ToString()
                nombre_cliente = lector("NOMBRE_CLIENTE").ToString()
                nombre_usuario = lector("NOMBRE_USUARIO").ToString()
                correo_cliente = lector("CORREO_CLIENTE").ToString()
                direccion_cliente = lector("DIRECCION_CLIENTE").ToString()
                telefono_cliente = lector("TELEFONO_CLIENTE").ToString()
                numero_orden = lector("NUMERO_ORDEN").ToString()
                fecha_entrega = lector("FECHA_ENTREGA").ToString()
                tamaño = lector("TAMAÑO").ToString()
                cantidad = lector("CANTIDAD").ToString()
                numero_paginas = lector("NUMERO_PAGINAS").ToString()
                lugar_entrega = lector("LUGAR_ENTREGA").ToString()
                prioridad = lector("PRIORIDAD").ToString()
                orientacion = lector("ORIENTACION").ToString()
                material_portada = lector("MATERIAL_PORTADA").ToString()
                gramaje_portada = lector("GRAMAJE_PORTADA").ToString()
                color_portada = lector("COLOR_PORTADA").ToString()
                tamaño_portada = lector("TAMAÑO_PORTADA").ToString()
                material_interior = lector("MATERIAL_INTERIOR").ToString()
                gramaje_interior = lector("GRAMAJE_INTERIOR").ToString()
                color_interior = lector("COLOR_INTERIOR").ToString()
                tamaño_interior = lector("TAMAÑO_INTERIOR").ToString()
                material_otro = lector("MATERIAL_OTRO").ToString()
                gramaje_otro = lector("GRAMAJE_OTRO").ToString()
                color_otro = lector("COLOR_OTRO").ToString()
                tamaño_otro = lector("TAMAÑO_OTRO").ToString()
                cantidad_resmas_portada = lector("CANTIDAD_RESMAS_PORTADA").ToString()
                cantidad_resmas_interior = lector("CANTIDAD_RESMAS_INTERIOR").ToString()
                cantidad_resmas_otro = lector("CANTIDAD_RESMAS_OTRO").ToString()
                full_color_portada = lector("FULL_COLOR_PORTADA").ToString()
                duotono_portada = lector("DUOTONO_PORTADA").ToString()
                unicolor_portada = lector("UNICOLOR_PORTADA").ToString()
                pantone_portada = lector("PANTONE_PORTADA").ToString()
                cantidad_tinta_portada = lector("CANTIDAD_TINTA_PORTADA").ToString()
                full_color_interior = lector("FULL_COLOR_INTERIOR").ToString()
                duotono_interior = lector("DUOTONO_INTERIOR").ToString()
                unicolor_interior = lector("UNICOLOR_INTERIOR").ToString()
                pantone_interior = lector("PANTONE_INTERIOR").ToString()
                cantidad_tinta_interior = lector("CANTIDAD_TINTA_INTERIOR").ToString()
                acabado_de_portada = lector("ACABADO_DE_PORTADA").ToString()
                cantidad_acabado_de_portada = lector("CANTIDAD_ACABADO_DE_PORTADA").ToString()
                diseño_diseño = lector("DISEÑO_DISEÑO").ToString()
                diseño_imp_digital = lector("DISEÑO_IMP_DIGITAL").ToString()
                diseño_ctp = lector("DISEÑO_CTP").ToString()
                diseño_prensa = lector("DISEÑO_PRENSA").ToString()
                diseño_reimpresion = lector("DISEÑO_REIMPRESION").ToString()
                portada_tiro_retiro = lector("PORTADA_TIRO_RETIRO").ToString()
                portada_tiro = lector("PORTADA_TIRO").ToString()
                interior_tiro_retiro = lector("INTERIOR_TIRO_RETIRO").ToString()
                interior_tiro = lector("INTERIOR_TIRO").ToString()
                cantidad_imprimir = lector("CANTIDAD_IMPRIMIR").ToString()
                encuadernacion_plegado = lector("ENCUADERNACION_PLEGADO").ToString()
                encuadernacion_pegado = lector("ENCUADERNACION_PEGADO").ToString()
                encuadernacion_alzado = lector("ENCUADERNACION_ALZADO").ToString()
                encuadernacion_cortado = lector("ENCUADERNACION_CORTADO").ToString()
                encuadernacion_perforado = lector("ENCUADERNACION_PERFORADO").ToString()
                encuadernacion_grapado = lector("ENCUADERNACION_GRAPADO").ToString()
                encuadernacion_numerado = lector("ENCUADERNACION_NUMERADO").ToString()
                encuadernacion_empacado = lector("ENCUADERNACION_EMPACADO").ToString()
                observaciones_especificas = lector("OBSERVACIONES_ESPECIFICAS").ToString()
                estado = lector("ESTADO").ToString()
                estadoOrden = lector("ESTADO_ORDEN").ToString()
                colorPortada = lector("COLORPORTADA").ToString()
                colorInterior = lector("COLORINTERIOR").ToString()
                tiroPortada = lector("TIROPORTADA").ToString()
                tiroInterior = lector("TIROINTERIOR").ToString()

            End While
            conexion.Close()

            Dim directorioLogo As String = Server.MapPath("~/Images/" + "logo3.png")
            Dim nombreArchivo As String = "Orden" + "-" + numOrden.ToString() + ".pdf"
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            'document.Save(directorio)
            Dim nombreArchivoHTML As String = "Orden" + "-" + numOrden.ToString() + ".html"
            Dim directorioHTML As String = Server.MapPath("/pdf/" + nombreArchivoHTML)
            Dim file As System.IO.StreamWriter
            If System.IO.File.Exists(directorioHTML) Then
                System.IO.File.Delete(directorioHTML)
                System.IO.File.Delete(directorio)
            End If
            file = My.Computer.FileSystem.OpenTextFileWriter(directorioHTML, True)
            file.WriteLine("<html>
    <head>
        <link rel=" & ControlChars.Quote & "stylesheet" & ControlChars.Quote & " href=" & ControlChars.Quote & "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" & ControlChars.Quote & ">
        <script src=" & ControlChars.Quote & "https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js" & ControlChars.Quote & "></script>
        <script src=" & ControlChars.Quote & "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js" & ControlChars.Quote & "></script>
        <style>
           .table {
        font-size: 5px;
    }
        </style>
    </head>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
        <table  width=" & ControlChars.Quote & "100%" & ControlChars.Quote & " >
            <tr>
                <td align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
                 <img src=" & ControlChars.Quote & directorioLogo & ControlChars.Quote & " width=" & ControlChars.Quote & "155.33858268" & ControlChars.Quote & " height=" & ControlChars.Quote & "70.11023622" & ControlChars.Quote & ">
                </td></tr>
        </table>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
        <br>
            <h2><strong>ORDEN DE PRODUCCIÓN</strong></h2>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & " >
        <br>
        <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
           <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & " width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
               <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Cliente: " + nombre_cliente + "</td></tr>
               <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Contacto: " + telefono_cliente + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Teléfono:" + telefono_cliente + "</td><td>Email: " + correo_cliente + "</td></tr>
               <tr><td  colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Nombre y descripción del trabajo: " + observaciones_especificas + "</td></tr>
               <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Tamaño: " + tamaño + "</td><td>Cantidad:" + cantidad + "</td><td>" + numero_paginas + "</td></tr>
               <tr><td  colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Lugar de entrega: " + lugar_entrega + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Fecha de entrega: " + Date.Parse(fecha_entrega).ToString("dd/mm/yyyy") + "</td></tr>
               <tr><td  colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Prioridad: " + prioridad + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Orientación: " + orientacion + "</td></tr>
               <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Orden elaborada por: " + nombre_usuario + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Fecha: " + fecha_creacion + "</td></tr>
           </table>
            <table  class=" & ControlChars.Quote & "table" & ControlChars.Quote & " width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
                <tr><td><strong>ÁREA EN QUE SE ENCUENTRA LA ORDEN: " + estadoOrden + "</strong></td><td><strong>ESTADO DE LA ORDEN: " + estado + "</strong></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><h3>Características del trabajo</h3></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Descripción</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Material:</strong></td></tr>
                <tr><td width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Portada " + material_portada + "</td><td  width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Gramaje " + gramaje_portada + "</td><td  width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Color  " + color_portada + "</td><td  width=" & ControlChars.Quote & "25%" & ControlChars.Quote & ">Tamaño " + tamaño_portada + "</td></tr>
                <tr><td>Interior " + material_interior + "</td><td>Gramaje " + gramaje_interior + "</td><td>Color " + color_otro + "</td><td>Tamaño " + tamaño_interior + "</td></tr>
                <tr><td>Otro " + material_otro + "</td><td>Gramaje " + gramaje_otro + "</td><td>Color</td><td>Tamaño " + tamaño_otro + "</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br>Cantidad de resmas a utilizar:</td></tr>
                <tr><td>Portada " + cantidad_resmas_portada + "</td><td>Interior " + cantidad_tinta_interior + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "> " + cantidad_resmas_otro + "</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Color:</strong></td></tr>
                <tr><td width=" & ControlChars.Quote & "33%" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">Portada:</td><td width=" & ControlChars.Quote & "33%" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">Interior</td><td width=" & ControlChars.Quote & "33%" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & " colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Acabado de portada</td></tr>
                <tr><td>Full color:  " + colorPortada + "</td><td>Full color " + colorInterior + "</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Barníz Mate  " + acabado_de_portada + "</label><br></td></tr>
                <tr><td>Duotono</td><td>Duotono </td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Barníz Brillante</label><br></td></tr>
                <tr><td>Un color</td><td>Un color </td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "></td></tr>
                <tr><td>Pantone</td><td>Pantone</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "></td></tr>
                <tr><td>Cantidad de tinta: " + cantidad_tinta_portada + "</td><td>Cantidad de tinta: " + cantidad_tinta_interior + "</td><td  colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Cantidad: " + cantidad_acabado_de_portada + "</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Diseño:</strong></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Destino final</td></tr>
                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Diseño " + diseño_diseño + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> CTP " + diseño_ctp + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Reimpresión " + diseño_reimpresion + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Imp. Digital " + diseño_imp_digital + "</label><br></td></tr>
                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Prensa " + diseño_prensa + "</label><br></td><td></td><td colspan=" & ControlChars.Quote & "3" & ControlChars.Quote & "></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Impresión:</strong></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Portada</td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">Interior</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro/retiro:  " + tiroPortada + "</label><br></td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro/retiro: " + tiroInterior + "</label><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro</label><br></td><td colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & "><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Tiro </label><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">Cantidad a imprimir (Ya incluye excedente)</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Encuadernación:</strong></td></tr>
                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Plegado " + encuadernacion_plegado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Pegado " + encuadernacion_pegado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Alzado " + encuadernacion_alzado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Cortado " + encuadernacion_cortado + "</label><br></td></tr>
                <tr><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Perforado " + encuadernacion_perforado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Grapado " + encuadernacion_grapado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Numerado " + encuadernacion_numerado + "</label><br></td><td><input type=" & ControlChars.Quote & "checkbox" & ControlChars.Quote & "><label> Empacado " + encuadernacion_empacado + "</label><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><strong>Observaciones específicas: " + observaciones_especificas + "</strong></td></tr>
                <tr><td colspan=4></td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & ">___________________________________________________________________________________________________________________________________________________________________________________________________________________</td></tr>
                <tr><td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & "><br></td></tr>
                <tr><td colspan=4><strong>NO PROCESE SI TIENE DUDAS, Consulte a su jefe inmediato, De no hacerlo le acarreará responsabilidad directa en el resultado del proceso.</strong></td></tr>
            </table>
        </div>
    </div>
   
</html>")
            file.Close()

            'Dim Renderer = New IronPdf.HtmlToPdf()
            'Dim PDF = Renderer.RenderHTMLFileAsPdf(directorioHTML)
            'Dim OutputPath = directorio
            'PDF.SaveAs(OutputPath)
            Dim baseUri As String = "src/main/resources/html/"

            Dim properties As ConverterProperties = New ConverterProperties()
            HtmlConverter.ConvertToPdf(
                New FileInfo(directorioHTML),
                New FileInfo(directorio))
            'FIN


            Session("archivoOrden") = "../pdf/" + nombreArchivo
            Session("nombreArchivo") = nombreArchivo
            bitacora.registrarBitacora(Session("usuario").ToString(), "VISUALIZACIÓN DE ÓRDEN DE PRODUCCIÓN")
            Return RedirectToAction("VerOrdenProduccion", "OrdenesDeProduccion")
            Return View()
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
                Session("mensaje") = "Flujo adelantado"
                Return RedirectToAction("Principal", "Inicio")
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
            Return View()
        End Function

        <HttpPost>
        Function ReporteDeBodega(submit As String, bodega As String, fecha As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult

            Dim query = "SELECT A.*,B.NOMBRE_USUARIO FROM TBL_INGRESO_BODEGAS A
	                INNER JOIN TBL_MS_USUARIO B
		                ON A.USUARIO=B.ID_USUARIO"
            Dim campoFecha = Nothing
            If fecha.Equals("FECHA DE INGRESO") Then
                campoFecha = "FECHA_INGRESO"
            ElseIf fecha.Equals("FECHA DE INGRESO") Then
                campoFecha = "FECHA_INGRESO"
            End If

            If Not bodega.Equals("TODOS") Then
                query = query + " WHERE A.BODEGA='" + bodega + "'"
                If campoFecha <> Nothing And date1 <> Nothing And date2 <> Nothing Then
                    query = query + " AND " + campoFecha + " BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                End If
            Else
                If campoFecha <> Nothing And date1 <> Nothing And date2 <> Nothing Then
                    query = query + " WHERE " + campoFecha + " BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
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
                    fila.Item("cantidad") = lector("CANTIDAD").ToString()
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
        <HttpPost>
        Function GestionDeInventario(submit As String, tipoGestion As String, producto As String,
                                        cantidadProducto As String, comentario As String) As ActionResult
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
                    fila.Item("cantidad") = lector("CANTIDAD").ToString()
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
    End Class
End Namespace