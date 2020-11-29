Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Namespace Controllers
    Public Class ProveedoresController
        Inherits Controller
        Dim validaciones As Validaciones = New Validaciones()

        'Public cadenaConexion As String = "Data Source= (LocalDB)\SQLIHER ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Public cadenaConexion As String = "Data Source= " + Environment.MachineName.ToString() + " ;Initial Catalog=ImprentaIHER;Integrated Security=true;"
        'Public cadenaConexion As String = "Data Source= localhost\SQLEXPRESS ;Initial Catalog=Imprenta-IHER;Integrated Security=true;"
        Dim bitacora As Bitacora = New Bitacora()
        Function AgregarProveedor() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    bitacora.registrarBitacora(Session("usuario").ToString(), "INGRESO A MÓDULO PARA AGREGAR PROVEEDOR")
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function

        <HttpPost>
        Function AgregarProveedor(nombreProveedor As String, direccionProveedor As String,
                                    telefonoProveedor As String, correoProveedor As String,
                                nombreContactoProveedor As String, telefonoContactoProveedor As String) As ActionResult
            If Session("accesos") <> Nothing Then
                Try
                    Dim query = "EXEC SP_AGREGAR_PROVEEDOR '" + validaciones.removerEspacios(nombreProveedor) + "','" + validaciones.removerEspacios(direccionProveedor) + "','" +
                        validaciones.removerEspacios(telefonoProveedor) + "','" + validaciones.removerEspacios(correoProveedor) + "','" + validaciones.removerEspacios(nombreContactoProveedor) + "','" +
                        validaciones.removerEspacios(telefonoContactoProveedor) + "'"

                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Session("mensaje") = "Proveedor agregado"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "CREACIÓN DE PROVEEDOR: " + nombreProveedor)
                    Return View()
                Catch ex As Exception
                    Session("mensaje") = ex.ToString()
                    Return View()
                End Try
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function
        Function EditarProveedor(proveedor As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim proveedorEditar As String = Request.QueryString("proveedor")
                    Session("proveedorEditar") = proveedorEditar
                    Dim query As String = "SELECT * FROM TBL_PROVEEDORES WHERE NOMBRE_PROVEEDOR='" + proveedorEditar + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector As SqlDataReader = comando.ExecuteReader()

                    While lector.Read()
                        Session("proveedorEditar") = lector("NOMBRE_PROVEEDOR").ToString()
                        Session("direccionProveedor") = lector("DIRECCION_PROVEEDOR").ToString()
                        Session("correoProveedor") = lector("CORREO_PROVEEDOR").ToString()
                        Session("telefonoProveedor") = lector("TELEFONO_PROVEEDOR").ToString()
                        Session("nombreContactoProveedor") = lector("NOMBRE_CONTACTO").ToString()
                        Session("telefonoContactoProveedor") = lector("TELEFONO_CONTACTO").ToString()
                        Session("estadoProveedor") = lector("ESTADO_PROVEEDOR").ToString()
                    End While
                    conexion.Close()
                    bitacora.registrarBitacora(Session("usuario").ToString(), "MÓDULO PARA EDICIÓN DE PROVEEDOR: " + proveedorEditar)
                    ViewBag.Message = "Editar Proveedor"
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function EditarProveedor(nombreProveedor As String, direccionProveedor As String,
                                    telefonoProveedor As String, correoProveedor As String,
                                nombreContactoProveedor As String, telefonoContactoProveedor As String, estado As String) As ActionResult

            If Session("accesos") <> Nothing Then
                Dim query = "EXEC SP_EDITAR_PROVEEDOR '" + validaciones.removerEspacios(nombreProveedor) + "','" + validaciones.removerEspacios(direccionProveedor) + "','" +
                  validaciones.removerEspacios(telefonoProveedor) + "','" + validaciones.removerEspacios(correoProveedor) + "','" + validaciones.removerEspacios(nombreContactoProveedor) + "','" + validaciones.removerEspacios(telefonoContactoProveedor) + "','" + Session("proveedorEditar") + "','" + validaciones.removerEspacios(estado) + "'"

                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                comando.ExecuteNonQuery()
                conexion.Close()
                Session("mensaje") = "Proveedor editado"
                bitacora.registrarBitacora(Session("usuario").ToString(), "EDICIÓN DE PROVEEDOR: " + nombreProveedor)
                Return RedirectToAction("EditarProveedores", "Proveedores")
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function

        Function EditarProveedores() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim query = "SELECT * FROM TBL_PROVEEDORES"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ProveedoresModel)
                    While (lector.Read())
                        Dim detalles = New ProveedoresModel()
                        detalles.nombreProveedor = lector("NOMBRE_PROVEEDOR").ToString()
                        detalles.direccionProveedor = lector("DIRECCION_PROVEEDOR").ToString()
                        detalles.telefonoProveedor = lector("TELEFONO_PROVEEDOR").ToString()
                        detalles.correoProveedor = lector("CORREO_PROVEEDOR").ToString()
                        detalles.nombreContactoProveedor = lector("NOMBRE_CONTACTO").ToString()
                        detalles.telefonoContactoProveedor = lector("TELEFONO_CONTACTO").ToString()
                        detalles.estado = lector("ESTADO_PROVEEDOR").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos proveedor"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "VISTA EDICIÓN DE PROVEEDOR")
                    Return View("EditarProveedores", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function EliminarProveedores() As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim query = "SELECT * FROM TBL_PROVEEDORES WHERE ESTADO_PROVEEDOR='ACTIVO'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    Dim lector = comando.ExecuteReader()
                    Dim model As New List(Of ProveedoresModel)
                    While (lector.Read())
                        Dim detalles = New ProveedoresModel()
                        detalles.nombreProveedor = lector("NOMBRE_PROVEEDOR").ToString()
                        detalles.direccionProveedor = lector("DIRECCION_PROVEEDOR").ToString()
                        detalles.telefonoProveedor = lector("TELEFONO_PROVEEDOR").ToString()
                        detalles.correoProveedor = lector("CORREO_PROVEEDOR").ToString()
                        detalles.nombreContactoProveedor = lector("NOMBRE_CONTACTO").ToString()
                        detalles.telefonoContactoProveedor = lector("TELEFONO_CONTACTO").ToString()
                        model.Add(detalles)
                    End While
                    conexion.Close()
                    ViewBag.Message = "Datos proveedor"
                    bitacora.registrarBitacora(Session("usuario").ToString(), "VISTA ELIMINACIÓN DE PROVEEDOR")
                    Return View("EliminarProveedores", model)
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        <HttpPost>
        Function EliminarProveedor(proveedor As String) As ActionResult
            If Session("accesos") <> Nothing Then
                If Session("accesos").ToString().Contains("ADMINISTRACION") Or Session("accesos").ToString().Contains("ADMINISTRADOR") Then
                    Dim proveedorEliminar As String = Request.QueryString("proveedor")
                    Session("proveedorEliminar") = proveedorEliminar
                    Dim query As String = "EXEC SP_ELIMINAR_PROVEEDOR '" + proveedor + "'"
                    Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                    conexion.Open()
                    Dim comando As SqlCommand = New SqlCommand(query, conexion)
                    comando.ExecuteNonQuery()
                    conexion.Close()
                    Session("mensaje") = "Proveedor eliminado"
                    Return RedirectToAction("EditarProveedores", "Proveedores")
                    bitacora.registrarBitacora(Session("usuario").ToString(), "ELIMINACIÓN DE PROVEEDOR: " + proveedor)
                    Return View()
                Else
                    Return RedirectToAction("Login", "Cuentas")
                End If
            Else
                Return RedirectToAction("Login", "Cuentas")
            End If
        End Function
        Function ReporteProveedores() As ActionResult
            If Session("accesos") <> Nothing Then

                bitacora.registrarBitacora(Session("usuario").ToString(), "REPORTE DE PROVEEDORES")
                Dim query = "SELECT * FROM TBL_PROVEEDORES WHERE ESTADO_PROVEEDOR='ACTIVO'"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of ProveedoresModel)
                While (lector.Read())
                    Dim detalles = New ProveedoresModel()
                    detalles.nombreProveedor = lector("NOMBRE_PROVEEDOR").ToString()
                    detalles.direccionProveedor = lector("DIRECCION_PROVEEDOR").ToString()
                    detalles.telefonoProveedor = lector("TELEFONO_PROVEEDOR").ToString()
                    detalles.correoProveedor = lector("CORREO_PROVEEDOR").ToString()
                    detalles.nombreContactoProveedor = lector("NOMBRE_CONTACTO").ToString()
                    detalles.telefonoContactoProveedor = lector("TELEFONO_CONTACTO").ToString()
                    model.Add(detalles)
                End While
                conexion.Close()
                ViewBag.Message = "Datos proveedor"
                Return View("ReporteProveedores", model)
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function
        <HttpPost>
        Function ReporteProveedores(submit As String) As ActionResult
            If Session("accesos") <> Nothing Then

                bitacora.registrarBitacora(Session("usuario").ToString(), "EXPORTAR REPORTE DE PROVEEDORES")
                Dim dsProveedores As New DsProveedores()
                Dim fila As DataRow
                Dim query = "SELECT * FROM TBL_PROVEEDORES"
                Dim conexion As SqlConnection = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim comando As SqlCommand = New SqlCommand(query, conexion)
                Dim lector = comando.ExecuteReader()
                Dim model As New List(Of ProveedoresModel)
                While (lector.Read())
                    Dim detalles = New ProveedoresModel()
                    detalles.nombreProveedor = lector("NOMBRE_PROVEEDOR").ToString()
                    detalles.direccionProveedor = lector("DIRECCION_PROVEEDOR").ToString()
                    detalles.telefonoProveedor = lector("TELEFONO_PROVEEDOR").ToString()
                    detalles.correoProveedor = lector("CORREO_PROVEEDOR").ToString()
                    detalles.nombreContactoProveedor = lector("NOMBRE_CONTACTO").ToString()
                    detalles.telefonoContactoProveedor = lector("TELEFONO_CONTACTO").ToString()
                    model.Add(detalles)

                    fila = dsProveedores.Tables("DataTable1").NewRow()
                    fila.Item("nombre") = lector("NOMBRE_PROVEEDOR").ToString()
                    fila.Item("direccion") = lector("DIRECCION_PROVEEDOR").ToString()
                    fila.Item("telefono") = lector("TELEFONO_PROVEEDOR").ToString()
                    fila.Item("correo") = lector("ESTADO_PROVEEDOR").ToString()
                    dsProveedores.Tables("DataTable1").Rows.Add(fila)
                End While
                conexion.Close()
                ViewBag.Message = "Datos proveedor"
                Dim nombreArchivo As String = "Reporte de proveedores.pdf"
                Dim directorio As String = Server.MapPath("~/reportes/" + nombreArchivo)

                If System.IO.File.Exists(directorio) Then
                    System.IO.File.Delete(directorio)
                End If
                Dim crystalReport As ReportDocument = New ReportDocument()
                crystalReport.Load(Server.MapPath("~/ReporteDeProveedores.rpt"))
                crystalReport.SetDataSource(dsProveedores)
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, directorio)
                Response.ContentType = "application/octet-stream"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + nombreArchivo)
                Response.TransmitFile(directorio)
                Response.End()
                Return View("ReporteProveedores", model)
            End If
            Return RedirectToAction("Login", "Cuentas")
        End Function
    End Class
End Namespace