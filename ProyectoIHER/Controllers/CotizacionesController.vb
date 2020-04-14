Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iText.Html2pdf
Imports iTextSharp.text.pdf
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf

Namespace Controllers
    Public Class CotizacionesController
        Inherits Controller
        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        ' GET: Cotizaciones
        Dim validaciones As Validaciones = New Validaciones()
        Dim bitacora As Bitacora = New Bitacora()

        Function NuevaCotizacion() As ActionResult
            If Session("accesos") <> "NO" Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    Dim clientes As New List(Of String)
                    Dim productos As New List(Of String)

                    Dim query = "SELECT NOMBRE_CLIENTE FROM TBL_CLIENTES"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        clientes.Add(lector("NOMBRE_CLIENTE").ToString())
                    End While
                    conexion.Close()
                    TempData("clientes") = clientes

                    Dim listadoProductos As String = ""
                    query = "SELECT NOMBRE_PRODUCTO FROM TBL_PRODUCTOS"
                    conexion = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    comando = New SqlCommand(query, conexion)
                    lector = comando.ExecuteReader()
                    While lector.Read()
                        'listadoProductos = listadoProductos + "<option value=" & ControlChars.Quote & lector("NOMBRE_PRODUCTO") & ControlChars.Quote & ">" + lector("NOMBRE_PRODUCTO") + "</option>"
                        productos.Add(lector("NOMBRE_PRODUCTO").ToString())
                    End While
                    conexion.Close()
                    TempData("productos") = productos
                    'Session("listadoProductos") = listadoProductos
                    bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A NUEVA COTIZACIÓN")

                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function NuevaCotizacion(cliente As String, tipoPago As String, observacion As String, nombreContacto As String, telefonoContacto As String, exoneracion As String,
                                 producto As String, precioProducto As Double, ByVal Optional cantidadProducto As Double = 0, ByVal Optional subTotal As Double = 0, ByVal Optional comentario As String = "NO",
                                 ByVal Optional producto_1 As String = "NO", ByVal Optional precioProducto_1 As Double = 0, ByVal Optional cantidadProducto_1 As Double = 0, ByVal Optional comentario_1 As String = "NO",
                                 ByVal Optional producto_2 As String = "NO", ByVal Optional precioProducto_2 As Double = 0, ByVal Optional cantidadProducto_2 As Double = 0, ByVal Optional comentario_2 As String = "NO",
                                 ByVal Optional producto_3 As String = "NO", ByVal Optional precioProducto_3 As Double = 0, ByVal Optional cantidadProducto_3 As Double = 0, ByVal Optional comentario_3 As String = "NO",
                                 ByVal Optional producto_4 As String = "NO", ByVal Optional precioProducto_4 As Double = 0, ByVal Optional cantidadProducto_4 As Double = 0, ByVal Optional comentario_4 As String = "NO",
                                 ByVal Optional producto_5 As String = "NO", ByVal Optional precioProducto_5 As Double = 0, ByVal Optional cantidadProducto_5 As Double = 0, ByVal Optional comentario_5 As String = "NO",
                                 ByVal Optional producto_6 As String = "NO", ByVal Optional precioProducto_6 As Double = 0, ByVal Optional cantidadProducto_6 As Double = 0, ByVal Optional comentario_6 As String = "NO",
                                 ByVal Optional producto_7 As String = "NO", ByVal Optional precioProducto_7 As Double = 0, ByVal Optional cantidadProducto_7 As Double = 0, ByVal Optional comentario_7 As String = "NO",
                                 ByVal Optional producto_8 As String = "NO", ByVal Optional precioProducto_8 As Double = 0, ByVal Optional cantidadProducto_8 As Double = 0, ByVal Optional comentario_8 As String = "NO",
                                 ByVal Optional producto_9 As String = "NO", ByVal Optional precioProducto_9 As Double = 0, ByVal Optional cantidadProducto_9 As Double = 0, ByVal Optional subTotal_9 As Double = 0, ByVal Optional comentario_9 As String = "NO") As ActionResult

            'Session("totalCotizacion") = cantidadProducto * precioProducto + cantidadProducto_1 * precioProducto_1 +
            '    cantidadProducto_2 * precioProducto_2 + cantidadProducto_3 * precioProducto_3 + cantidadProducto_4 * precioProducto_4 +
            '    cantidadProducto_5 * precioProducto_5 + cantidadProducto_6 * precioProducto_6 + cantidadProducto_7 * precioProducto_7 +
            '    cantidadProducto_8 * precioProducto_8 + cantidadProducto_9 * precioProducto_9

            Dim query = "EXEC SP_NUEVA_COTIZACION '" + validaciones.removerEspacios(cliente) + "','" + Session("usuario").ToString() + "','" + validaciones.removerEspacios(tipoPago) + "','" + validaciones.removerEspacios(observacion) +
                "','" + validaciones.removerEspacios(nombreContacto) + "','" + validaciones.removerEspacios(telefonoContacto) + "','" + validaciones.removerEspacios(exoneracion) +
                "','" + validaciones.removerEspacios(producto) + "','" + validaciones.removerEspacios(precioProducto.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto.ToString()) + "','" + validaciones.removerEspacios(comentario) +
                "','" + validaciones.removerEspacios(producto_1) + "','" + validaciones.removerEspacios(precioProducto_1.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_1.ToString()) + "','" + validaciones.removerEspacios(comentario_1) +
                "','" + validaciones.removerEspacios(producto_2) + "','" + validaciones.removerEspacios(precioProducto_2.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_2.ToString()) + "','" + validaciones.removerEspacios(comentario_2) +
                "','" + validaciones.removerEspacios(producto_3) + "','" + validaciones.removerEspacios(precioProducto_3.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_3.ToString()) + "','" + validaciones.removerEspacios(comentario_3) +
                "','" + validaciones.removerEspacios(producto_4) + "','" + validaciones.removerEspacios(precioProducto_4.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_4.ToString()) + "','" + validaciones.removerEspacios(comentario_4) +
                "','" + validaciones.removerEspacios(producto_5) + "','" + validaciones.removerEspacios(precioProducto_5.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_5.ToString()) + "','" + validaciones.removerEspacios(comentario_5) +
                "','" + validaciones.removerEspacios(producto_6) + "','" + validaciones.removerEspacios(precioProducto_6.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_6.ToString()) + "','" + validaciones.removerEspacios(comentario_6) +
                "','" + validaciones.removerEspacios(producto_7) + "','" + validaciones.removerEspacios(precioProducto_7.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_7.ToString()) + "','" + validaciones.removerEspacios(comentario_7) +
                "','" + validaciones.removerEspacios(producto_8) + "','" + validaciones.removerEspacios(precioProducto_8.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_8.ToString()) + "','" + validaciones.removerEspacios(comentario_8) +
                "','" + validaciones.removerEspacios(producto_9) + "','" + validaciones.removerEspacios(precioProducto_9.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_9.ToString()) + "','" + validaciones.removerEspacios(comentario_9) + "'"



            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()

            'Generando PDF' 


            'CONSULTANDO LOS DATOS PARA MOSTRAR EN COTIZACION'
            'CONSULTANDO NUMERO DE COTIZACION CREADA
            query = "SELECT IDENT_CURRENT('TBL_COTIZACIONES') NUMERO_COTIZACION"
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            Dim numeroCotizacion As Integer = Convert.ToInt32(comando.ExecuteScalar().ToString())
            conexion.Close()

            'CONSULTADO DATOS DE COTIZACION
            Dim fechaCreacion As String = ""
            Dim nombreUsuario As String = ""
            Dim nombreCliente As String = ""
            Dim direccionCliente As String = ""
            Dim correoCliente As String = ""
            Dim telefonoCliente As String = ""
            Dim nombreContactoCliente As String = ""
            Dim telefonoContactoCliente As String = ""
            Dim comentarioCotizacion As String = ""
            Dim subTotalCotizacion As String = ""
            Dim isvCotizacion As String = ""
            Dim totalCotizacion As String = ""
            Dim tipoPagoCotizacion As String = ""
            Dim rtnCliente As String = ""


            query = "SELECT CONVERT(NVARCHAR,A.FECHA_CREACION,103) FECHA_CREACION,
                         B.NOMBRE_USUARIO, C.NOMBRE_CLIENTE, C.DIRECCION_CLIENTE,
                        C.CORREO_CLIENTE, C.TELEFONO_CLIENTE, A.NOMBRE_CONTACTO,
                        A.TELEFONO_CONTACTO, A.COMENTARIO_COTIZACION, A.SUBTOTAL_COTIZACION,
                        A.ISV_COTIZACION, A.TOTAL_COTIZACION, A.TIPO_PAGO,C.RTN
                        FROM TBL_COTIZACIONES A
		                    INNER JOIN TBL_MS_USUARIO B
                                ON A.ID_USUARIO_CREADOR=B.ID_USUARIO
				                    INNER JOIN TBL_CLIENTES C
                                        ON A.ID_CLIENTE=C.ID_CLIENTE
	                    WHERE A.NUMERO_COTIZACION = " + numeroCotizacion.ToString()
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                fechaCreacion = lector("FECHA_CREACION").ToString()
                nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                direccionCliente = lector("DIRECCION_CLIENTE").ToString()
                correoCliente = lector("CORREO_CLIENTE").ToString()
                telefonoCliente = lector("TELEFONO_CLIENTE").ToString()
                nombreContactoCliente = lector("NOMBRE_CONTACTO").ToString()
                telefonoContactoCliente = lector("TELEFONO_CONTACTO").ToString()
                comentarioCotizacion = lector("COMENTARIO_COTIZACION").ToString()
                subTotalCotizacion = lector("SUBTOTAL_COTIZACION").ToString()
                isvCotizacion = lector("ISV_COTIZACION").ToString()
                totalCotizacion = lector("TOTAL_COTIZACION").ToString()
                tipoPagoCotizacion = lector("TIPO_PAGO").ToString()
                rtnCliente = lector("RTN").ToString()
            End While
            conexion.Close()

            'OBTENIENDO DETALLE DE PRODUCTOS
            Dim detalleProductos = ""
            Dim cantidadProductos = 0

            query = "SELECT A.*,B.NOMBRE_PRODUCTO FROM TBL_PRODUCTOS_COTIZACION A
	                    INNER JOIN TBL_PRODUCTOS B
		                    ON A.ID_PRODUCTO=B.ID_PRODUCTO
                        WHERE A.NUMERO_COTIZACION=" + numeroCotizacion.ToString()
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(query, conexion)
            lector = comando.ExecuteReader()
            While lector.Read()
                cantidadProductos = cantidadProductos + 1
                detalleProductos = detalleProductos + "<tr>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + cantidadProductos.ToString() + "</td>
                                       <td>" + lector("NOMBRE_PRODUCTO").ToString() + ", " + lector("COMENTARIO").ToString() + "</td>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + Double.Parse(lector("CANTIDAD_PRODUCTO").ToString()).ToString("#,##0") + "</td>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + Double.Parse(lector("PRECIO_PRODUCTO").ToString()).ToString("#,##0.#0") + "</td>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + Double.Parse(lector("SUB_TOTAL").ToString()).ToString("#,##0.#0") + "</td>
                                   </tr>"
            End While




            'INICIO
            Dim directorioLogo As String = Server.MapPath("~/Images/" + "logo3.png")
            Dim nombreArchivo As String = "Cotizacion" + "-" + numeroCotizacion.ToString() + ".pdf"
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            'document.Save(directorio)
            Dim nombreArchivoHTML As String = "Cotizacion" + "-" + numeroCotizacion.ToString() + ".html"
            Dim directorioHTML As String = Server.MapPath("/pdf/" + nombreArchivoHTML)
            Dim file As System.IO.StreamWriter
            If System.IO.File.Exists(directorioHTML) Then
                System.IO.File.Delete(directorioHTML)
                System.IO.File.Delete(directorio)
            End If
            file = My.Computer.FileSystem.OpenTextFileWriter(directorioHTML, False)
            file.WriteLine("<html>
    <head>
        <link rel=" & ControlChars.Quote & "stylesheet" & ControlChars.Quote & " href=" & ControlChars.Quote & "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" & ControlChars.Quote & ">
        <script src=" & ControlChars.Quote & "https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js" & ControlChars.Quote & "></script>
        <script src=" & ControlChars.Quote & "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js" & ControlChars.Quote & "></script>
        <style>
            .table {
                font-size: 10;
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
        <h4><strong><i>INSTITUTO HONDUREÑO DE EDUCACIÓN POR RADIO</i></strong></h4>
        <h5><strong><i>Barrio Abajo, Avenida Lempira, Casa 305, Tel/Fax. 2237-9356, 2238-3983, 2220-6657</i></strong></h5>
        <h5><strong><i>RTN 08019999419531&nbsp;&nbsp;&nbsp;Apto. Postal 1850.&nbsp;&nbsp;&nbsp;Tegucigalpa, Honduras.</i></strong></h5>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
            <h2><strong><i><u>COTIZACIÓN</u></i></strong></h2>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & " >
        <br>
        <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
           <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & ">
               <tr>
                   <td><h5><strong><i>SEÑORES: " + nombreCliente + "</i></strong></h5></td>
                   <td><h5><strong>Documento # </strong>" + numeroCotizacion.ToString() + "-" + DateTime.Now.ToString("yyyy") + "</h5></td></tr>
               <tr>
                <td><h5><i><strong>DIRECCIÓN:</strong> " + direccionCliente + "</i></h5>
                    <td><h5><strong>Fecha:</strong> " + fechaCreacion + "</h5></td></tr>
               <tr>
                <td><h5><i><strong>TELÉFONO:</strong> " + telefonoCliente + "</i></h5></td>   
                <td><h5><strong>Ordenado por:</strong> " + nombreContacto + "</h5></td></tr>
               <tr>
                <tr><td><h5><i><strong>RTN:</strong> " + rtnCliente + "</i></h5></td>
                <td><h5><strong>Teléfono:</strong> " + telefonoContacto + "</h5></td></tr>
                <tr><td><h5><i><strong>CORREO:</strong> " + correoCliente + "</i></h5></td></tr>
           </table>
            <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & " border=" & ControlChars.Quote & "1" & ControlChars.Quote & ">
                    <tr bgcolor=" & ControlChars.Quote & "#FFE933" & ControlChars.Quote & ">
                        <td scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">No.</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Descripción</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Cantidad</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Precio unitario</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Subtotal</th>
                    </tr>" + detalleProductos + "<tr bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>Subtotal</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>" + Double.Parse(subTotalCotizacion).ToString("#,##0.#0") + "</strong></td>
                    </tr>
                    <tr  bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>15% ISV</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>" + Double.Parse(isvCotizacion).ToString("#,##0.#0") + "</strong></td>
                    </tr>
                    <tr  bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>Total</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>" + Double.Parse(totalCotizacion).ToString("#,##0.#0") + "</strong></td>
                    </tr>
            </table>
            </div>
            <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
                <table width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
                    <tr>
                        <td width=" & ControlChars.Quote & "100%" & ControlChars.Quote & " colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">
                            <h6><strong><i>OBSERVACIONES:</i></strong></h6>
                <h6><i>PAGO CON CHEQUE O CON DEPOSITO EN LA CUENTA DE AHORRO 725098221 DE BAC A NOMBRE DE ASOCIACIÓN IHER.</i></h6>
                <h6><strong><i>FAVOR NOTIFICAR AL MOMENTO DE REALIZAR SU PAGO PARA PROCEDER CON LA ELABORACIÓN DE LA FACTURA CORRESPONDIENTE, PROPORCIONANDO RTN Y NOMBRE DE LA RAZÓN.</i></strong></h6>
                        </td>
                    </tr>
                    <tr>
                        <td width=" & ControlChars.Quote & "50%" & ControlChars.Quote & "><h6>________________________</h6>
                            <h6>Vobo. Cliente</h6></td>
                        <td width=" & ControlChars.Quote & "50%" & ControlChars.Quote & "><h5><strong><i>" + nombreUsuario + "</i></strong></h6>
                            <h6><strong><i>AUXILIAR ADMINISTRATIVO</i></strong></h6>
                            <h6><strong><i>CELULAR: " + "96686519" + "</i></strong></h6>
                            <h6><strong><i>CORREO: cotizaciones@iher.hn</i></strong></h6></td>
                    </tr>
                </table>
                 <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
                    <h5><strong><i>**** VÁLIDA POR 7 DÍAS ****</i></strong></h5>
    </div>
                
            </div>
               
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


            Session("archivoCotizacion") = "../pdf/" + nombreArchivo
            Session("nombreArchivo") = nombreArchivo
            bitacora.registrarBitacora(Session("usuario").ToString(), "GENERACIÓN DE NUEVA COTIZACIÓN - NÚMERO DE COTIZACION: " + numeroCotizacion.ToString())
            Return RedirectToAction("VerCotizacion", "Cotizaciones")
        End Function
        Function VerCotizacion() As ActionResult
            If Session("accesos") <> "NO" Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    If Session("nombreArchivo") <> "NO" Then
                        bitacora.registrarBitacora(Session("usuario").ToString(), "VER COTIZACIÓN")
                        Return View()
                    Else
                        Return RedirectToAction("VerCotizacion", "Cotizaciones")
                    End If
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function VerCotizacion(submit As String) As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "DESCARGAR COTIZACIÓN")
            Dim nombreArchivo As String = Session("nombreArchivo").ToString()
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            Response.ContentType = "application/octet-stream"
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
            Response.TransmitFile(directorio)
            Response.End()
            Return View()
        End Function

        <HttpPost>
        Function ObtenerPrecioProducto(producto As String) As ActionResult

            Dim query = "SELECT PRECIO_PRODUCTO FROM TBL_PRODUCTOS WHERE NOMBRE_PRODUCTO='" + producto + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            If lector.HasRows Then
                While lector.Read()
                    Return Json(New With {.success = True, .precio = lector("PRECIO_PRODUCTO")})
                End While
                conexion.Close()
            Else
                Return Json(New With {.success = False})
            End If
            Return View()
        End Function
        Function BuscarCotizaciones() As ActionResult
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A BÚSQUEDA DE COTIZACIONES")
            Return View()
        End Function

        <HttpPost>
        Function BuscarCotizaciones(submit As String, ByVal Optional date1 As DateTime = Nothing,
                                    ByVal Optional date2 As DateTime = Nothing) As ActionResult
            If Session("accesos") <> "NO" Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Then
                    If submit.Equals("generar") Then
                        Dim query As String = "SELECT CONVERT(NVARCHAR,A.FECHA_CREACION,103) FECHA_CREACION,A.NUMERO_COTIZACION,
                         B.NOMBRE_USUARIO, C.NOMBRE_CLIENTE, C.DIRECCION_CLIENTE,
                        C.CORREO_CLIENTE, C.TELEFONO_CLIENTE, A.NOMBRE_CONTACTO,
                        A.TELEFONO_CONTACTO, A.COMENTARIO_COTIZACION, A.SUBTOTAL_COTIZACION,
                        A.ISV_COTIZACION, A.TOTAL_COTIZACION, A.TIPO_PAGO,C.RTN,
                        CASE WHEN DATEDIFF(DAY,CAST(A.FECHA_CREACION AS DATETIME),GETDATE())>7 AND A.NUMERO_COTIZACION NOT IN (SELECT NUMERO_COTIZACION FROM TBL_ORDENES_PRODUCCION) THEN 'VENCIDA'
                        WHEN A.NUMERO_COTIZACION IN (SELECT NUMERO_COTIZACION FROM TBL_ORDENES_PRODUCCION) THEN 'ENVIADA A PRODUCCIÓN'
                        WHEN DATEDIFF(DAY,CAST(A.FECHA_CREACION AS DATETIME),GETDATE())<=7 AND A.NUMERO_COTIZACION NOT IN (SELECT NUMERO_COTIZACION FROM TBL_ORDENES_PRODUCCION) THEN 'VIGENTE' END AS ESTADO_COTIZACION
                        FROM TBL_COTIZACIONES A
		                    INNER JOIN TBL_MS_USUARIO B
                                ON A.ID_USUARIO_CREADOR=B.ID_USUARIO
				                    INNER JOIN TBL_CLIENTES C
                                        ON A.ID_CLIENTE=C.ID_CLIENTE"
                        If date1 <> Nothing Then
                            query = query + "  WHERE CAST(A.FECHA_CREACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                        End If
                        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        Dim comando As SqlCommand = New SqlCommand(query, conexion)
                        Dim lector = comando.ExecuteReader()
                        Dim model As New List(Of CotizacionesModel)
                        While (lector.Read())
                            Dim detalles = New CotizacionesModel()
                            detalles.fechaCreacion = lector("FECHA_CREACION").ToString()
                            detalles.numeroCotizacion = lector("NUMERO_COTIZACION").ToString()
                            detalles.nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                            detalles.nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                            detalles.estadoCotizacion = lector("ESTADO_COTIZACION").ToString()
                            model.Add(detalles)
                        End While
                        conexion.Close()
                        ViewBag.Message = "Datos cotizacion"
                        bitacora.registrarBitacora(Session("usuario").ToString(), "BÚSQUEDA DE COTIZACIONES")
                        Return View("BuscarCotizaciones", model)
                    Else
                        bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR BÚSQUEDA DE COTIZACIONES")
                        Dim query As String = "SELECT CONVERT(NVARCHAR,A.FECHA_CREACION,103) FECHA_CREACION,A.NUMERO_COTIZACION,
                         B.NOMBRE_USUARIO, C.NOMBRE_CLIENTE, C.DIRECCION_CLIENTE,
                        C.CORREO_CLIENTE, C.TELEFONO_CLIENTE, A.NOMBRE_CONTACTO,
                        A.TELEFONO_CONTACTO, A.COMENTARIO_COTIZACION, A.SUBTOTAL_COTIZACION,
                        A.ISV_COTIZACION, A.TOTAL_COTIZACION, A.TIPO_PAGO,C.RTN,
                        CASE WHEN DATEDIFF(DAY,CAST(A.FECHA_CREACION AS DATETIME),GETDATE())>7 AND A.NUMERO_COTIZACION NOT IN (SELECT NUMERO_COTIZACION FROM TBL_ORDENES_PRODUCCION) THEN 'VENCIDA'
                        WHEN A.NUMERO_COTIZACION IN (SELECT NUMERO_COTIZACION FROM TBL_ORDENES_PRODUCCION) THEN 'ENVIADA A PRODUCCIÓN'
                        WHEN DATEDIFF(DAY,CAST(A.FECHA_CREACION AS DATETIME),GETDATE())<=7 AND A.NUMERO_COTIZACION NOT IN (SELECT NUMERO_COTIZACION FROM TBL_ORDENES_PRODUCCION) THEN 'VIGENTE' END AS ESTADO_COTIZACION
                        FROM TBL_COTIZACIONES A
		                    INNER JOIN TBL_MS_USUARIO B
                                ON A.ID_USUARIO_CREADOR=B.ID_USUARIO
				                    INNER JOIN TBL_CLIENTES C
                                        ON A.ID_CLIENTE=C.ID_CLIENTE"
                        If date1 <> Nothing Then
                            query = query + "  WHERE CAST(A.FECHA_CREACION AS DATE) BETWEEN '" + date1.ToString("yyyy-MM-dd") +
                                        "' AND '" + date2.ToString("yyyy-MM-dd") + "'"
                        End If
                        Dim dsCotizaciones As New DsCotizaciones()
                        Dim fila As DataRow
                        Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                        conexion.Open()
                        Dim comando As SqlCommand = New SqlCommand(query, conexion)
                        Dim lector = comando.ExecuteReader()
                        Dim model As New List(Of CotizacionesModel)
                        While (lector.Read())
                            fila = dsCotizaciones.Tables("DataTable1").NewRow()
                            fila.Item("fechaCreacion") = lector("FECHA_CREACION").ToString()
                            fila.Item("numero") = lector("NUMERO_COTIZACION").ToString()
                            fila.Item("cliente") = lector("NOMBRE_CLIENTE").ToString()
                            fila.Item("estado") = lector("ESTADO_COTIZACION").ToString()
                            fila.Item("usuario") = lector("NOMBRE_USUARIO").ToString()
                            dsCotizaciones.Tables("DataTable1").Rows.Add(fila)
                        End While
                        conexion.Close()
                        Dim nombreArchivo As String = "Reporte de cotizaciones.pdf"
                        Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                        If System.IO.File.Exists(directorio) Then
                            System.IO.File.Delete(directorio)
                        End If
                        Dim crystalReport As ReportDocument = New ReportDocument()
                        crystalReport.Load(Server.MapPath("~/ReporteDeCotizaciones.rpt"))
                        crystalReport.SetDataSource(dsCotizaciones)
                        crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                        Response.ContentType = "application/octet-stream"
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                        Response.TransmitFile(directorio)
                        Response.End()
                    End If

                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function


        Function BuscarCotizacion(numeroCotizacion As String)
            Dim numCotizacion As String = Request.QueryString("numeroCotizacion")
            Session("numeroCotizacion") = numCotizacion
            'Generando PDF' 
            'CONSULTADO DATOS DE COTIZACION
            Dim fechaCreacion As String = ""
            Dim nombreUsuario As String = ""
            Dim nombreCliente As String = ""
            Dim direccionCliente As String = ""
            Dim correoCliente As String = ""
            Dim telefonoCliente As String = ""
            Dim nombreContactoCliente As String = ""
            Dim telefonoContactoCliente As String = ""
            Dim comentarioCotizacion As String = ""
            Dim subTotalCotizacion As String = ""
            Dim isvCotizacion As String = ""
            Dim totalCotizacion As String = ""
            Dim tipoPagoCotizacion As String = ""
            Dim rtnCliente As String = ""


            Dim Query As String = "SELECT CONVERT(NVARCHAR,A.FECHA_CREACION,103) FECHA_CREACION,
                         B.NOMBRE_USUARIO, C.NOMBRE_CLIENTE, C.DIRECCION_CLIENTE,
                        C.CORREO_CLIENTE, C.TELEFONO_CLIENTE, A.NOMBRE_CONTACTO,
                        A.TELEFONO_CONTACTO, A.COMENTARIO_COTIZACION, A.SUBTOTAL_COTIZACION,
                        A.ISV_COTIZACION, A.TOTAL_COTIZACION, A.TIPO_PAGO,C.RTN
                        FROM TBL_COTIZACIONES A
		                    INNER JOIN TBL_MS_USUARIO B
                                ON A.ID_USUARIO_CREADOR=B.ID_USUARIO
				                    INNER JOIN TBL_CLIENTES C
                                        ON A.ID_CLIENTE=C.ID_CLIENTE
	                    WHERE A.NUMERO_COTIZACION = " + numCotizacion
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(Query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                fechaCreacion = lector("FECHA_CREACION").ToString()
                nombreUsuario = lector("NOMBRE_USUARIO").ToString()
                nombreCliente = lector("NOMBRE_CLIENTE").ToString()
                direccionCliente = lector("DIRECCION_CLIENTE").ToString()
                correoCliente = lector("CORREO_CLIENTE").ToString()
                telefonoCliente = lector("TELEFONO_CLIENTE").ToString()
                nombreContactoCliente = lector("NOMBRE_CONTACTO").ToString()
                telefonoContactoCliente = lector("TELEFONO_CONTACTO").ToString()
                comentarioCotizacion = lector("COMENTARIO_COTIZACION").ToString()
                subTotalCotizacion = lector("SUBTOTAL_COTIZACION").ToString()
                isvCotizacion = lector("ISV_COTIZACION").ToString()
                totalCotizacion = lector("TOTAL_COTIZACION").ToString()
                tipoPagoCotizacion = lector("TIPO_PAGO").ToString()
                rtnCliente = lector("RTN").ToString()
                nombreContactoCliente = lector("NOMBRE_CONTACTO").ToString()
                telefonoContactoCliente = lector("TELEFONO_CONTACTO").ToString()
            End While
            conexion.Close()

            'OBTENIENDO DETALLE DE PRODUCTOS
            Dim detalleProductos = ""
            Dim cantidadProductos = 0

            Query = "SELECT A.*,B.NOMBRE_PRODUCTO FROM TBL_PRODUCTOS_COTIZACION A
	                    INNER JOIN TBL_PRODUCTOS B
		                    ON A.ID_PRODUCTO=B.ID_PRODUCTO
                        WHERE A.NUMERO_COTIZACION=" + numCotizacion
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(Query, conexion)
            lector = comando.ExecuteReader()
            While lector.Read()
                cantidadProductos = cantidadProductos + 1
                detalleProductos = detalleProductos + "<tr>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + cantidadProductos.ToString() + "</td>
                                       <td>" + lector("NOMBRE_PRODUCTO").ToString() + ", " + lector("COMENTARIO").ToString() + "</td>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + Double.Parse(lector("CANTIDAD_PRODUCTO").ToString()).ToString("#,##0") + "</td>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + Double.Parse(lector("PRECIO_PRODUCTO").ToString()).ToString("#,##0.#0") + "</td>
                                       <td align=" & ControlChars.Quote & " right" & ControlChars.Quote & ">" + Double.Parse(lector("SUB_TOTAL").ToString()).ToString("#,##0.#0") + "</td>
                                   </tr>"
            End While






            'INICIO
            Dim directorioLogo As String = Server.MapPath("~/Images/" + "logo3.png")
            Dim nombreArchivo As String = "Cotizacion" + "-" + numeroCotizacion.ToString() + ".pdf"
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            'document.Save(directorio)
            Dim nombreArchivoHTML As String = "Cotizacion" + "-" + numeroCotizacion.ToString() + ".html"
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
                font-size: 10;
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
        <h4><strong><i>INSTITUTO HONDUREÑO DE EDUCACIÓN POR RADIO</i></strong></h4>
        <h5><strong><i>Barrio Abajo, Avenida Lempira, Casa 305, Tel/Fax. 2237-9356, 2238-3983, 2220-6657</i></strong></h5>
        <h5><strong><i>RTN 08019999419531&nbsp;&nbsp;&nbsp;Apto. Postal 1850.&nbsp;&nbsp;&nbsp;Tegucigalpa, Honduras.</i></strong></h5>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
            <h2><strong><i><u>COTIZACIÓN</u></i></strong></h2>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & " >
        <br>
        <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
           <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & ">
               <tr>
                   <td><h5><strong><i>SEÑORES: " + nombreCliente + "</i></strong></h5></td>
                   <td><h5><strong>Documento # </strong>" + numeroCotizacion.ToString() + "</h5></td></tr>
               <tr>
                <td><h5><i><strong>DIRECCIÓN:</strong> " + direccionCliente + "</i></h5>
                    <td><h5><strong>Fecha:</strong> " + fechaCreacion + "</h5></td></tr>
               <tr>
                <td><h5><i><strong>TELÉFONO:</strong> " + telefonoCliente + "</i></h5></td>   
                <td><h5><strong>Ordenado por:</strong> " + nombreContactoCliente + "</h5></td></tr>
               <tr>
                <tr><td><h5><i><strong>RTN:</strong> " + rtnCliente + "</i></h5></td>
                <td><h5><strong>Teléfono:</strong> " + telefonoContactoCliente + "</h5></td></tr>
                <tr><td><h5><i><strong>CORREO:</strong> " + correoCliente + "</i></h5></td></tr>
           </table>
            <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & " border=" & ControlChars.Quote & "1" & ControlChars.Quote & ">
                    <tr bgcolor=" & ControlChars.Quote & "#FFE933" & ControlChars.Quote & ">
                        <td scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">No.</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Descripción</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Cantidad</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Precio unitario</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Subtotal</th>
                    </tr>" + detalleProductos + "<tr bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>Subtotal</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>" + Double.Parse(subTotalCotizacion).ToString("#,##0.#0") + "</strong></td>
                    </tr>
                    <tr  bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>15% ISV</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>" + Double.Parse(isvCotizacion).ToString("#,##0.#0") + "</strong></td>
                    </tr>
                    <tr  bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>Total</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>" + Double.Parse(totalCotizacion).ToString("#,##0.#0") + "</strong></td>
                    </tr>
            </table>
            </div>
            <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
                <table width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
                    <tr>
                        <td width=" & ControlChars.Quote & "100%" & ControlChars.Quote & " colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">
                            <h6><strong><i>OBSERVACIONES:</i></strong></h6>
                <h6><i>PAGO CON CHEQUE O CON DEPOSITO EN LA CUENTA DE AHORRO 725098221 DE BAC A NOMBRE DE ASOCIACIÓN IHER.</i></h6>
                <h6><strong><i>FAVOR NOTIFICAR AL MOMENTO DE REALIZAR SU PAGO PARA PROCEDER CON LA ELABORACIÓN DE LA FACTURA CORRESPONDIENTE, PROPORCIONANDO RTN Y NOMBRE DE LA RAZÓN.</i></strong></h6>
                        </td>
                    </tr>
                    <tr>
                        <td width=" & ControlChars.Quote & "50%" & ControlChars.Quote & "><h6>________________________</h6>
                            <h6>Vobo. Cliente</h6></td>
                        <td width=" & ControlChars.Quote & "50%" & ControlChars.Quote & "><h5><strong><i>" + nombreUsuario + "</i></strong></h6>
                            <h6><strong><i>AUXILIAR ADMINISTRATIVO</i></strong></h6>
                            <h6><strong><i>CELULAR: " + "N/D" + "</i></strong></h6>
                            <h6><strong><i>CORREO: cotizaciones@iher.hn</i></strong></h6></td>
                    </tr>
                </table>
                 <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & "  align=" & ControlChars.Quote & "center" & ControlChars.Quote & ">
                    <h5><strong><i>**** VÁLIDA POR 7 DÍAS ****</i></strong></h5>
    </div>
                
            </div>
               
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


            Session("archivoCotizacion") = "../pdf/" + nombreArchivo
            Session("nombreArchivo") = nombreArchivo
            bitacora.registrarBitacora(Session("usuario").ToString(), "VER COTIZACIÓN")
            Return RedirectToAction("VerCotizacion", "Cotizaciones")
        End Function

        Function EnviarAProduccion(numeroCotizacion As String) As ActionResult
            Dim numCotizacion As String = Request.QueryString("numeroCotizacion")
            Session("numeroCotizacionParaProduccion") = numCotizacion
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO DE ENVIAR A PRODUCCIÓN")

            Return View()
        End Function
        <HttpPost>
        Function EnviarAProduccion(lugarEntrega As String, fechaEntrega As DateTime, cantidad As String, numeroPaginas As String, tamaño As String,
                                   orientacion As String, prioridad As String, ByVal Optional materialPortada As String = "NO",
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
                                   ByVal Optional portadaTiro As String = "NO", ByVal Optional interiorTiro As String = "NO"
                                ) As ActionResult

            Dim query = "EXEC SP_NUEVA_ORDEN_PRODUCCION '" + Session("usuario").ToString() + "','" + Session("numeroCotizacionParaProduccion").ToString() + "','" + validaciones.removerEspacios(lugarEntrega) +
                             "','" + validaciones.removerEspacios(fechaEntrega.ToString("yyyy-MM-dd")) + "','" + validaciones.removerEspacios(tamaño) + "','" + validaciones.removerEspacios(cantidad) + "','" + validaciones.removerEspacios(numeroPaginas) + "','" + prioridad + "','" + validaciones.removerEspacios(orientacion) + "','" + validaciones.removerEspacios(materialPortada) +
                             "','" + validaciones.removerEspacios(gramajePortada) + "','" + validaciones.removerEspacios(colorPortada) + "','" + validaciones.removerEspacios(tamañoPortada) + "','" + validaciones.removerEspacios(materialInterior) + "','" + validaciones.removerEspacios(gramajeInterior) +
                             "','" + validaciones.removerEspacios(colorInterior) + "','" + validaciones.removerEspacios(tamañoInterior) + "','" + validaciones.removerEspacios(materialOtro) + "','" + validaciones.removerEspacios(gramajeOtro) + "','" + validaciones.removerEspacios(colorOtro) + "','" + validaciones.removerEspacios(tamañoOtro) +
                             "','" + validaciones.removerEspacios(cantidadResmasPortada) + "','" + validaciones.removerEspacios(cantidadResmasInterior) + "','" + validaciones.removerEspacios(cantidadResmasOtro) + "','" + validaciones.removerEspacios(fullColorPortada) + "','" + validaciones.removerEspacios(duotonoPortada) +
                             "','" + validaciones.removerEspacios(uniColorPortada) + "','" + validaciones.removerEspacios(pantonePortada) + "','" + validaciones.removerEspacios(cantidadTintaPortada) + "','" + validaciones.removerEspacios(fullColorInterior) + "','" + validaciones.removerEspacios(duotonoInterior) + "','" + validaciones.removerEspacios(uniColorInterior) +
                             "','" + validaciones.removerEspacios(pantoneInterior) + "','" + validaciones.removerEspacios(cantidadTintaInterior) + "','" + validaciones.removerEspacios(acabadoPortada) + "','" + validaciones.removerEspacios(cantidadAcabadoPortada) + "','" + validaciones.removerEspacios(diseñoDiseño) + "','" + validaciones.removerEspacios(diseñoImpDigital) +
                             "','" + validaciones.removerEspacios(diseñoCTP) + "','" + validaciones.removerEspacios(diseñoReimpresion) + "','" + validaciones.removerEspacios(diseñoPrensa) + "','" + validaciones.removerEspacios(tiroRetiroPortada) + "','" + validaciones.removerEspacios(tiroPortada) + "','" + validaciones.removerEspacios(tiroRetiroInterior) + "','" + validaciones.removerEspacios(tiroInterior) +
                             "','" + validaciones.removerEspacios(cantidadImprimir) + "','" + validaciones.removerEspacios(plegado) + "','" + validaciones.removerEspacios(perforado) + "','" + validaciones.removerEspacios(pegado) + "','" + validaciones.removerEspacios(grapado) + "','" + validaciones.removerEspacios(alzado) + "','" + validaciones.removerEspacios(numerado) + "','" + validaciones.removerEspacios(cortado) + "','" + validaciones.removerEspacios(empacado) +
                             "','" + validaciones.removerEspacios(observacionesEspecificas) + "','" + validaciones.removerEspacios(colorPortada2) + "','" + validaciones.removerEspacios(colorInterior2) + "','" + validaciones.removerEspacios(portadaTiro) + "','" + validaciones.removerEspacios(interiorTiro) + "'"

            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Enviado a producción"
            bitacora.registrarBitacora(Session("usuario").ToString(), "ENVÍO DE COTIZACIÓN A PRODUCCIÓN")
            Return RedirectToAction("Principal", "Inicio")
        End Function

        Function EliminarCotizacion(numeroCotizacion As String)
            Dim numCotizacion As String = Request.QueryString("numeroCotizacion")
            Session("numeroCotizacion") = numCotizacion

            Dim query = "DELETE TBL_COTIZACIONES WHERE NUMERO_COTIZACION=" + numCotizacion + ";
                        DELETE TBL_PRODUCTOS_COTIZACION WHERE NUMERO_COTIZACION=" + numCotizacion
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Cotización eliminada"
            bitacora.registrarBitacora(Session("usuario").ToString(), "ELIMINACIÓN DE COTIZACIÓN")
            Return RedirectToAction("BuscarCotizaciones", "Cotizaciones")
        End Function

        Function EditarCotizacion(numeroCotizacion As String) As ActionResult
            Dim numCotizacion As String = Request.QueryString("numeroCotizacion")
            Session("numeroCotizacion") = numCotizacion

            Dim cliente As String = Nothing
            Dim tipoPago As String = Nothing
            Dim observacion As String = Nothing
            Dim nombreContacto As String = Nothing
            Dim telefonoContacto As String = Nothing
            Dim exoneracion As String = Nothing
            Dim Query As String = "SELECT CONVERT(NVARCHAR,A.FECHA_CREACION,103) FECHA_CREACION,
                         B.NOMBRE_USUARIO, C.NOMBRE_CLIENTE, C.DIRECCION_CLIENTE,
                        C.CORREO_CLIENTE, C.TELEFONO_CLIENTE, A.NOMBRE_CONTACTO,
                        A.TELEFONO_CONTACTO, A.COMENTARIO_COTIZACION, A.SUBTOTAL_COTIZACION,
                        CASE WHEN A.ISV_COTIZACION=0 THEN 'SI' ELSE 'NO' END AS EXONERACION,
                    A.TOTAL_COTIZACION, A.TIPO_PAGO,C.RTN,A.TIPO_PAGO
                        FROM TBL_COTIZACIONES A
		                    INNER JOIN TBL_MS_USUARIO B
                                ON A.ID_USUARIO_CREADOR=B.ID_USUARIO
				                    INNER JOIN TBL_CLIENTES C
                                        ON A.ID_CLIENTE=C.ID_CLIENTE
	                    WHERE A.NUMERO_COTIZACION = " + numCotizacion
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(Query, conexion)
            Dim lector As SqlDataReader = comando.ExecuteReader()
            While lector.Read()
                cliente = lector("NOMBRE_CLIENTE").ToString()
                tipoPago = lector("TIPO_PAGO").ToString()
                observacion = lector("CORREO_CLIENTE").ToString()
                nombreContacto = lector("NOMBRE_CONTACTO").ToString()
                telefonoContacto = lector("TELEFONO_CONTACTO").ToString()
                exoneracion = lector("EXONERACION").ToString()
            End While
            conexion.Close()

            Session("cliente") = cliente
            Session("tipoPago") = tipoPago
            Session("observacion") = observacion
            Session("nombreContacto") = nombreContacto
            Session("telefonoContacto") = telefonoContacto
            Session("exoneracion") = exoneracion


            Dim cantidadProductos = 0
            Dim detalleProductos(3, 9) As String
            Dim nombreProductos As New List(Of String)
            Dim precioProductos As New List(Of String)
            Dim cantidadProducto As New List(Of String)
            Dim comentarioProductos As New List(Of String)

            Query = "SELECT A.*,B.NOMBRE_PRODUCTO FROM TBL_PRODUCTOS_COTIZACION A
	                    INNER JOIN TBL_PRODUCTOS B
		                    ON A.ID_PRODUCTO=B.ID_PRODUCTO
                        WHERE A.NUMERO_COTIZACION=" + numCotizacion
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(Query, conexion)
            lector = comando.ExecuteReader()


            While lector.Read()
                nombreProductos.Add(lector("NOMBRE_PRODUCTO").ToString())
                precioProductos.Add(lector("PRECIO_PRODUCTO").ToString())
                cantidadProducto.Add(lector("CANTIDAD_PRODUCTO").ToString())
                comentarioProductos.Add(lector("COMENTARIO").ToString())
            End While
            conexion.Close()
            TempData("nombreProductos") = nombreProductos
            TempData("precioProductos") = precioProductos
            TempData("cantidadProducto") = cantidadProducto
            TempData("comentarioProductos") = comentarioProductos


            Session("cantidadProductos") = nombreProductos.Count
            TempData("detalleProductos") = detalleProductos

            Dim clientes As New List(Of String)
            Dim productos As New List(Of String)

            Query = "SELECT NOMBRE_CLIENTE FROM TBL_CLIENTES"
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(Query, conexion)
            lector = comando.ExecuteReader()
            While lector.Read()
                clientes.Add(lector("NOMBRE_CLIENTE").ToString())
            End While
            conexion.Close()
            TempData("clientes") = clientes

            Dim listadoProductos As String = ""
            Query = "SELECT NOMBRE_PRODUCTO FROM TBL_PRODUCTOS"
            conexion = New SqlConnection(cadenaConexion)
            conexion.Open()
            comando = New SqlCommand(Query, conexion)
            lector = comando.ExecuteReader()
            While lector.Read()
                productos.Add(lector("NOMBRE_PRODUCTO").ToString())
            End While
            conexion.Close()
            TempData("productos") = productos
            bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A EDICIÓN DE COTIZACIÓN")
            Return View()
        End Function

        <HttpPost>
        Function EditarCotizacion(cliente As String, tipoPago As String, observacion As String, nombreContacto As String, telefonoContacto As String, exoneracion As String,
                                 producto_0 As String, precioProducto_0 As Double, ByVal Optional cantidadProducto_0 As Double = 0, ByVal Optional subTotal_0 As Double = 0, ByVal Optional comentario_0 As String = "NO",
                                 ByVal Optional producto_1 As String = "NO", ByVal Optional precioProducto_1 As Double = 0, ByVal Optional cantidadProducto_1 As Double = 0, ByVal Optional comentario_1 As String = "NO",
                                 ByVal Optional producto_2 As String = "NO", ByVal Optional precioProducto_2 As Double = 0, ByVal Optional cantidadProducto_2 As Double = 0, ByVal Optional comentario_2 As String = "NO",
                                 ByVal Optional producto_3 As String = "NO", ByVal Optional precioProducto_3 As Double = 0, ByVal Optional cantidadProducto_3 As Double = 0, ByVal Optional comentario_3 As String = "NO",
                                 ByVal Optional producto_4 As String = "NO", ByVal Optional precioProducto_4 As Double = 0, ByVal Optional cantidadProducto_4 As Double = 0, ByVal Optional comentario_4 As String = "NO",
                                 ByVal Optional producto_5 As String = "NO", ByVal Optional precioProducto_5 As Double = 0, ByVal Optional cantidadProducto_5 As Double = 0, ByVal Optional comentario_5 As String = "NO",
                                 ByVal Optional producto_6 As String = "NO", ByVal Optional precioProducto_6 As Double = 0, ByVal Optional cantidadProducto_6 As Double = 0, ByVal Optional comentario_6 As String = "NO",
                                 ByVal Optional producto_7 As String = "NO", ByVal Optional precioProducto_7 As Double = 0, ByVal Optional cantidadProducto_7 As Double = 0, ByVal Optional comentario_7 As String = "NO",
                                 ByVal Optional producto_8 As String = "NO", ByVal Optional precioProducto_8 As Double = 0, ByVal Optional cantidadProducto_8 As Double = 0, ByVal Optional comentario_8 As String = "NO",
                                 ByVal Optional producto_9 As String = "NO", ByVal Optional precioProducto_9 As Double = 0, ByVal Optional cantidadProducto_9 As Double = 0, ByVal Optional subTotal_9 As Double = 0, ByVal Optional comentario_9 As String = "NO") As ActionResult

            Dim query = "EXEC SP_EDITAR_COTIZACION '" + validaciones.removerEspacios(cliente) + "','" + Session("usuario").ToString() + "','" + validaciones.removerEspacios(tipoPago) + "','" + validaciones.removerEspacios(observacion) +
                "','" + validaciones.removerEspacios(nombreContacto) + "','" + validaciones.removerEspacios(telefonoContacto) + "','" + validaciones.removerEspacios(exoneracion) +
                "','" + validaciones.removerEspacios(producto_0) + "','" + validaciones.removerEspacios(precioProducto_0.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_0.ToString()) + "','" + validaciones.removerEspacios(comentario_0) +
                "','" + validaciones.removerEspacios(producto_1) + "','" + validaciones.removerEspacios(precioProducto_1.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_1.ToString()) + "','" + validaciones.removerEspacios(comentario_1) +
                "','" + validaciones.removerEspacios(producto_2) + "','" + validaciones.removerEspacios(precioProducto_2.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_2.ToString()) + "','" + validaciones.removerEspacios(comentario_2) +
                "','" + validaciones.removerEspacios(producto_3) + "','" + validaciones.removerEspacios(precioProducto_3.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_3.ToString()) + "','" + validaciones.removerEspacios(comentario_3) +
                "','" + validaciones.removerEspacios(producto_4) + "','" + validaciones.removerEspacios(precioProducto_4.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_4.ToString()) + "','" + validaciones.removerEspacios(comentario_4) +
                "','" + validaciones.removerEspacios(producto_5) + "','" + validaciones.removerEspacios(precioProducto_5.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_5.ToString()) + "','" + validaciones.removerEspacios(comentario_5) +
                "','" + validaciones.removerEspacios(producto_6) + "','" + validaciones.removerEspacios(precioProducto_6.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_6.ToString()) + "','" + validaciones.removerEspacios(comentario_6) +
                "','" + validaciones.removerEspacios(producto_7) + "','" + validaciones.removerEspacios(precioProducto_7.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_7.ToString()) + "','" + validaciones.removerEspacios(comentario_7) +
                "','" + validaciones.removerEspacios(producto_8) + "','" + validaciones.removerEspacios(precioProducto_8.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_8.ToString()) + "','" + validaciones.removerEspacios(comentario_8) +
                "','" + validaciones.removerEspacios(producto_9) + "','" + validaciones.removerEspacios(precioProducto_9.ToString()) + "','" + validaciones.removerEspacios(cantidadProducto_9.ToString()) + "','" + validaciones.removerEspacios(comentario_9) +
                "','" + Session("numeroCotizacion").ToString() + "'"
            Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
            conexion.Open()
            Dim comando As SqlCommand = New SqlCommand(query, conexion)
            comando.ExecuteNonQuery()
            conexion.Close()
            Session("mensaje") = "Cotización editada"
            bitacora.registrarBitacora(Session("usuario").ToString(), "EDICIÓN DE COTIZACIÓN")
            Return RedirectToAction("BuscarCotizaciones", "Cotizaciones")
        End Function

    End Class
End Namespace