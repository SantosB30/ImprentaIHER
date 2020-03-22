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
        Function NuevaCotizacion(cliente As String, tipoPago As String, observacion As String,
                                 producto As String, precioProducto As Double, ByVal Optional cantidadProducto As Double = 0, ByVal Optional subTotal As Double = 0, ByVal Optional comentario As String = Nothing,
                                 ByVal Optional producto_1 As String = Nothing, ByVal Optional precioProducto_1 As Double = 0, ByVal Optional cantidadProducto_1 As Double = 0, ByVal Optional subTotal_1 As Double = 0, ByVal Optional comentario_1 As String = Nothing,
                                 ByVal Optional producto_2 As String = Nothing, ByVal Optional precioProducto_2 As Double = 0, ByVal Optional cantidadProducto_2 As Double = 0, ByVal Optional subTotal_2 As Double = 0, ByVal Optional comentario_2 As String = Nothing,
                                 ByVal Optional producto_3 As String = Nothing, ByVal Optional precioProducto_3 As Double = 0, ByVal Optional cantidadProducto_3 As Double = 0, ByVal Optional subTotal_3 As Double = 0, ByVal Optional comentario_3 As String = Nothing,
                                 ByVal Optional producto_4 As String = Nothing, ByVal Optional precioProducto_4 As Double = 0, ByVal Optional cantidadProducto_4 As Double = 0, ByVal Optional subTotal_4 As Double = 0, ByVal Optional comentario_4 As String = Nothing,
                                 ByVal Optional producto_5 As String = Nothing, ByVal Optional precioProducto_5 As Double = 0, ByVal Optional cantidadProducto_5 As Double = 0, ByVal Optional subTotal_5 As Double = 0, ByVal Optional comentario_5 As String = Nothing,
                                 ByVal Optional producto_6 As String = Nothing, ByVal Optional precioProducto_6 As Double = 0, ByVal Optional cantidadProducto_6 As Double = 0, ByVal Optional subTotal_6 As Double = 0, ByVal Optional comentario_6 As String = Nothing,
                                 ByVal Optional producto_7 As String = Nothing, ByVal Optional precioProducto_7 As Double = 0, ByVal Optional cantidadProducto_7 As Double = 0, ByVal Optional subTotal_7 As Double = 0, ByVal Optional comentario_7 As String = Nothing,
                                 ByVal Optional producto_8 As String = Nothing, ByVal Optional precioProducto_8 As Double = 0, ByVal Optional cantidadProducto_8 As Double = 0, ByVal Optional subTotal_8 As Double = 0, ByVal Optional comentario_8 As String = Nothing,
                                 ByVal Optional producto_9 As String = Nothing, ByVal Optional precioProducto_9 As Double = 0, ByVal Optional cantidadProducto_9 As Double = 0, ByVal Optional subTotal_9 As Double = 0, ByVal Optional comentario_9 As String = Nothing) As ActionResult

            Session("totalCotizacion") = cantidadProducto * precioProducto + cantidadProducto_1 * precioProducto_1 +
                cantidadProducto_2 * precioProducto_2 + cantidadProducto_3 * precioProducto_3 + cantidadProducto_4 * precioProducto_4 +
                cantidadProducto_5 * precioProducto_5 + cantidadProducto_6 * precioProducto_6 + cantidadProducto_7 * precioProducto_7 +
                cantidadProducto_8 * precioProducto_8 + cantidadProducto_9 * precioProducto_9


            'Generando PDF' 'FALTA AGREGAR LOS CAMPOS DINÁMICOS (INFORMACIÓN DE LA COTIZACIÓN),

            'INICIO
            Dim directorioLogo As String = Server.MapPath("~/Images/" + "logo3.png")
            Dim nombreArchivo As String = "Cotizacion" + cliente + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf"
            Dim directorio As String = Server.MapPath("/pdf/" + nombreArchivo)
            'document.Save(directorio)
            Dim nombreArchivoHTML As String = "Cotizacion" + cliente + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".html"
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
        <br>
            <h2><strong><i><u>COTIZACIÓN</u></i></strong></h2>
    </div>
    <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & " >
        <br>
        <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
           <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & ">
               <tr>
                   <td><h5><strong><i>SEÑORES: INDUSTRIA PAPELERA HONDUREÑA S.A.</i></strong></h5></td>
                   <td><h5><strong>Documento # </strong>0188-2019</h5></td></tr>
               <tr>
                <td><h5><i><strong>DIRECCIÓN:</strong> TEGUCIGALPA, F.M.</i></h5>
                    <td><h5><strong>Fecha:</strong>19/03/2020</h5></td></tr>
               <tr>
                <td><h5><i><strong>TELÉFONO:</strong> 9990-7839 / 9953-0045</i></h5></td>   
                <td><h5><strong>Ordenado por:</strong> USUARIO</h5></td></tr>
               <tr>
                <tr><td><h5><i><strong>RTN:</strong> 0501-9995-135935</i></h5></td>
                <td><h5><strong>Teléfono:</strong> 99999999</h5></td></tr>
                <tr><td><h5><i><strong>CORREO:</strong> fabiolamedrano@cuadernoscopan.hn</i></h5></td></tr>
           </table>
            <table class=" & ControlChars.Quote & "table" & ControlChars.Quote & " border=" & ControlChars.Quote & "1" & ControlChars.Quote & ">
                    <tr bgcolor=" & ControlChars.Quote & "#FFE933" & ControlChars.Quote & ">
                        <td scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">No.</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Descripción</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Cantidad</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Precio unitario</th>
                        <th scope=" & ControlChars.Quote & "col" & ControlChars.Quote & ">Subtotal</th>
                    </tr>
                    <tr>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & ">1</td>
                        <td>Stickers tamaño 3''x5.75'', en vinil adhesivo troquelado, full color, a una cara.</td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & ">300</td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & ">4.85</td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & ">1,455.00</td>
                    </tr>
                    
                    <tr bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>Subtotal</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>1,455.00</strong></td>
                    </tr>
                    <tr  bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>15%</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>218.25</strong></td>
                    </tr>
                    <tr  bgcolor=" & ControlChars.Quote & "#E0DFD7" & ControlChars.Quote & ">
                        <td colspan=" & ControlChars.Quote & "4" & ControlChars.Quote & " align=" & ControlChars.Quote & "center" & ControlChars.Quote & "><strong>Total</strong></td>
                        <td align=" & ControlChars.Quote & "right" & ControlChars.Quote & "><strong>1,673.25</strong></td>
                    </tr>
            </table>
            </div>
            <div class=" & ControlChars.Quote & "col-md-12" & ControlChars.Quote & ">
                <table width=" & ControlChars.Quote & "100%" & ControlChars.Quote & ">
                    <tr>
                        <td width=" & ControlChars.Quote & "100%" & ControlChars.Quote & " colspan=" & ControlChars.Quote & "2" & ControlChars.Quote & ">
                            <h6><strong><i>OBSERVACIONES:</i></strong></h6>
                <h6><i>PAGO CON CHEQUE O CON DEPOSITO EN LA CUENTA DE AHORRO 9999999 DE BAC A NOMBRE DE ASOCIACIÓN IHER.</i></h6>
                <h6><strong><i>FAVOR NOTIFICAR AL MOMENTO DE REALIZAR SU PAGO PARA PROCEDER CON LA ELABORACIÓN DE LA FACTURA CORRESPONDIENTE, PROPORCIONANDO RTN Y NOMBRE DE LA RAZÓN.</i></strong></h6>
                        </td>
                    </tr>
                    <tr>
                        <td width=" & ControlChars.Quote & "50%" & ControlChars.Quote & "><h6>________________________</h6>
                            <h6>Vobo. Cliente</h6></td>
                        <td width=" & ControlChars.Quote & "50%" & ControlChars.Quote & "><h5><strong><i>USUARIO</i></strong></h6>
                            <h6><strong><i>AUXILIAR ADMINISTRATIVO</i></strong></h6>
                            <h6><strong><i>CELULAR: 9999-9999</i></strong></h6>
                            <h6><strong><i>CORREO: cotizaciones@iher.hn</i></strong></h6></td>
                    </tr>
                </table>
                <br>
                
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