Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
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

        Function NuevaCotizacion() As ActionResult
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

            Return View()
        End Function
        <HttpPost>
        Function NuevaCotizacion(cliente As String, tipoPago As String, observacion As String, nombreContacto As String, telefonoContacto As String, exoneracion As String,
                                 producto As String, precioProducto As Double, ByVal Optional cantidadProducto As Double = 0, ByVal Optional subTotal As Double = 0, ByVal Optional comentario As String = Nothing,
                                 ByVal Optional producto_1 As String = Nothing, ByVal Optional precioProducto_1 As Double = 0, ByVal Optional cantidadProducto_1 As Double = 0, ByVal Optional comentario_1 As String = Nothing,
                                 ByVal Optional producto_2 As String = Nothing, ByVal Optional precioProducto_2 As Double = 0, ByVal Optional cantidadProducto_2 As Double = 0, ByVal Optional comentario_2 As String = Nothing,
                                 ByVal Optional producto_3 As String = Nothing, ByVal Optional precioProducto_3 As Double = 0, ByVal Optional cantidadProducto_3 As Double = 0, ByVal Optional comentario_3 As String = Nothing,
                                 ByVal Optional producto_4 As String = Nothing, ByVal Optional precioProducto_4 As Double = 0, ByVal Optional cantidadProducto_4 As Double = 0, ByVal Optional comentario_4 As String = Nothing,
                                 ByVal Optional producto_5 As String = Nothing, ByVal Optional precioProducto_5 As Double = 0, ByVal Optional cantidadProducto_5 As Double = 0, ByVal Optional comentario_5 As String = Nothing,
                                 ByVal Optional producto_6 As String = Nothing, ByVal Optional precioProducto_6 As Double = 0, ByVal Optional cantidadProducto_6 As Double = 0, ByVal Optional comentario_6 As String = Nothing,
                                 ByVal Optional producto_7 As String = Nothing, ByVal Optional precioProducto_7 As Double = 0, ByVal Optional cantidadProducto_7 As Double = 0, ByVal Optional comentario_7 As String = Nothing,
                                 ByVal Optional producto_8 As String = Nothing, ByVal Optional precioProducto_8 As Double = 0, ByVal Optional cantidadProducto_8 As Double = 0, ByVal Optional comentario_8 As String = Nothing,
                                 ByVal Optional producto_9 As String = Nothing, ByVal Optional precioProducto_9 As Double = 0, ByVal Optional cantidadProducto_9 As Double = 0, ByVal Optional subTotal_9 As Double = 0, ByVal Optional comentario_9 As String = Nothing) As ActionResult

            'Session("totalCotizacion") = cantidadProducto * precioProducto + cantidadProducto_1 * precioProducto_1 +
            '    cantidadProducto_2 * precioProducto_2 + cantidadProducto_3 * precioProducto_3 + cantidadProducto_4 * precioProducto_4 +
            '    cantidadProducto_5 * precioProducto_5 + cantidadProducto_6 * precioProducto_6 + cantidadProducto_7 * precioProducto_7 +
            '    cantidadProducto_8 * precioProducto_8 + cantidadProducto_9 * precioProducto_9

            Dim query = "EXEC SP_NUEVA_COTIZACION '" + cliente + "','" + Session("usuario").ToString() + "','" + tipoPago + "','" + observacion +
                "','" + nombreContacto + "','" + telefonoContacto + "','" + exoneracion +
                "','" + producto + "','" + precioProducto.ToString() + "','" + cantidadProducto.ToString() + "','" + comentario +
                "','" + producto_1 + "','" + precioProducto_1.ToString() + "','" + cantidadProducto_1.ToString() + "','" + comentario_1 +
                "','" + producto_2 + "','" + precioProducto_2.ToString() + "','" + cantidadProducto_2.ToString() + "','" + comentario_2 +
                "','" + producto_3 + "','" + precioProducto_3.ToString() + "','" + cantidadProducto_3.ToString() + "','" + comentario_3 +
                "','" + producto_4 + "','" + precioProducto_4.ToString() + "','" + cantidadProducto_4.ToString() + "','" + comentario_4 +
                "','" + producto_5 + "','" + precioProducto_5.ToString() + "','" + cantidadProducto_5.ToString() + "','" + comentario_5 +
                "','" + producto_6 + "','" + precioProducto_6.ToString() + "','" + cantidadProducto_6.ToString() + "','" + comentario_6 +
                "','" + producto_7 + "','" + precioProducto_7.ToString() + "','" + cantidadProducto_7.ToString() + "','" + comentario_7 +
                "','" + producto_8 + "','" + precioProducto_8.ToString() + "','" + cantidadProducto_8.ToString() + "','" + comentario_8 +
                "','" + producto_9 + "','" + precioProducto_9.ToString() + "','" + cantidadProducto_9.ToString() + "','" + comentario_9 + "'"



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
            Dim nombreArchivo As String = "Cotizacion" + "-" + numeroCotizacion.ToString() + "-" + DateTime.Now.ToString("yyyy") + ".pdf"
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            'document.Save(directorio)
            Dim nombreArchivoHTML As String = "Cotizacion" + "-" + numeroCotizacion.ToString() + "-" + DateTime.Now.ToString("yyyy") + ".html"
            Dim directorioHTML As String = Server.MapPath("/pdf/" + nombreArchivoHTML)
            Dim file As System.IO.StreamWriter
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
            Return RedirectToAction("VerCotizacion", "Cotizaciones")
        End Function
        Function VerCotizacion() As ActionResult
            Return View()
        End Function
        <HttpPost>
        Function VerCotizacion(submit As String) As ActionResult
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
            Else
                Return Json(New With {.success = False})
            End If

            conexion.Close()
        End Function



    End Class
End Namespace