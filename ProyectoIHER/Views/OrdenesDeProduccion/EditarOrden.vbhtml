@Code
    ViewData("Title") = "Enviar a producción | Imprenta IHER"
    Layout = "~/Views/Shared/_Layout.vbhtml"
    @ModelType IEnumerable(Of ProyectoIHER.EditarOrdenModel)
End Code

<div class="ibox float-e-margins">
    <div class="ibox-title">
        <h3> <strong>Editar orden: @Session("numeroOrden").ToString()</strong></h3>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content">
        @Using Html.BeginForm("EditarOrden", "OrdenesDeProduccion", FormMethod.Post, New With {.id = "form"})
            @For Each item In Model
                @<div id="wizard">
                    <h1>Datos complementarios</h1>
                    <div class="step-content">
                        <div class="row">
                            <div class="col-lg-12">
                                <Label class="font-normal"><strong>Descripción del trabajo:</strong></Label>
                                <textarea class="form-control" type="text" id="descripcionDelTrabajo" name="descripcionDelTrabajo" rows="3" required onkeyup="this.value = this.value.toUpperCase();">@item.descripcionTrabajo</textarea>
                            </div>
                            <div class="col-lg-6">
                                <br>
                                <Label class="font-normal"><strong>Lugar de entrega:</strong></Label>
                                <input id="lugarEntrega" name="lugarEntrega" type="text" class="form-control required" onkeyup="this.value = this.value.toUpperCase();" value="@item.lugar_entrega">
                            </div>
                            <div class="col-lg-2">
                                <br>
                                <Label class="font-normal"><strong>Fecha de entrega:</strong></Label>
                                <input type="date" class="form-control" required id="fechaEntrega" name="fechaEntrega" value="@item.fecha_entrega" />
                            </div>
                            <div class="col-lg-2">
                                <br>
                                <Label class="font-normal"><strong>Cantidad:</strong></Label>
                                <input type="number" class="form-control" required value="@item.cantidad" min="0" id="cantidad" name="cantidad" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div class="col-lg-2">
                                <br>
                                <Label class="font-normal"><strong>Número de páginas:</strong></Label>
                                <input type="number" class="form-control" required value="@item.numero_paginas" min="0" id="numeroPaginas" name="numeroPaginas" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div class="col-lg-2">
                                <br>
                                <Label class="font-normal"><strong>Orientación:</strong></Label>
                                <select class="form-control" required id="orientacion" name="orientacion">
                                    <option value=""> ---SELECCIONE - --</option>
                                    @If item.orientacion.ToString().Equals("VERTICAL") Then
                                        @<option value="VERTICAL" selected>VERTICAL</option>
                                        @<option value="HORIZONTAL"> HORIZONTAL</option>
                                    Else
                                        @<option value="VERTICAL">VERTICAL</option>
                                        @<option value="HORIZONTAL" selected> HORIZONTAL</option>
                                    End If

                                </select>
                            </div>
                            <div Class="col-lg-2">
                                <br>
                                <Label Class="font-normal"><strong>Prioridad:</strong></Label>
                                <select Class="form-control" required id="prioridad" name="prioridad">
                                    <option value=""> ---SELECCIONE - --</option>
                                    @If item.prioridad.ToString().Equals("URGENTE") Then
                                        @<option value="URGENTE" selected>URGENTE</option>
                                        @<option value="NORMAL"> NORMAL</option>
                                    Else
                                        @<option value="URGENTE">URGENTE</option>
                                        @<option value="NORMAL" selected> NORMAL</option>
                                    End If

                                </select>
                            </div>
                            <div Class="col-lg-2">
                                <br>
                                <Label Class="font-normal"><strong>Tamaño:</strong></Label>
                                <input type="text" Class="form-control" required id="tamaño" name="tamaño" onkeyup="this.value = this.value.toUpperCase();" value="@item.tamaño" />
                            </div>
                        </div>
                    </div>
                    <h1> Material</h1>
                    <div Class="step-content">
                        <div Class="row">
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Portada:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.material_portada" id="materialPortada" name="materialPortada" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Gramaje:</strong></Label>
                                <input type="number" Class="form-control" required value="@item.gramaje_portada" id="gramajePortada" name="gramajePortada" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Color:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.color_portada" id="colorPortada" name="colorPortada" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Tamaño:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.tamaño_portada" id="tamañoPortada" name="tamañoPortada" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>

                        </div>
                        <div Class="row">
                            <br>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Interior:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.material_interior" id="materialInterior" name="materialInterior" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Gramaje:</strong></Label>
                                <input type="number" Class="form-control" required value="@item.gramaje_interior" id="gramajeInterior" name="gramajeInterior" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Color:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.color_interior" id="colorInterior" name="colorInterior" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Tamaño:</strong></Label>
                                <input type="text" Class="form-control" required id="tamañoInterior" name="tamañoInterior" onkeyup="this.value = this.value.toUpperCase();" value="@item.tamaño_interior" />
                            </div>

                        </div>
                        <div Class="row">
                            <br>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Otro:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.material_otro" id="materialOtro" name="materialOtro" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Gramaje:</strong></Label>
                                <input type="number" Class="form-control" required value="@item.gramaje_otro" id="gramajeOtro" name="gramajeOtro" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Color:</strong></Label>
                                <input type="text" Class="form-control" required value="@item.color_otro" id="colorOtro" name="colorOtro" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                            <div Class="col-lg-3">
                                <Label Class="font-normal"><strong>Tamaño:</strong></Label>
                                <input type="text" Class="form-control" required id="tamañoOtro" name="tamañoOtro" onkeyup="this.value = this.value.toUpperCase();" value="@item.tamaño_otro" />
                            </div>

                        </div>
                        <div Class="row">
                            <br>
                            <div Class="col-lg-12">
                                <h3> Cantidad de resmas a utilizar:</h3>
                            </div>
                            <div Class="col-lg-4">
                                <Label Class="font-normal"><strong>Portada:</strong></Label>
                                <input type="number" Class="form-control" required id="cantidadResmasPortada" name="cantidadResmasPortada" onkeyup="this.value = this.value.toUpperCase();" value="@item.cantidad_resmas_portada"/>
                            </div>
                            <div Class="col-lg-4">
                                <Label Class="font-normal"><strong>Interior:</strong></Label>
                                <input type="number" Class="form-control" required id="cantidadResmasInterior" name="cantidadResmasInterior" onkeyup="this.value = this.value.toUpperCase();" value="@item.cantidad_resmas_interior"/>
                            </div>
                            <div Class="col-lg-4">
                                <Label Class="font-normal"><strong>Otro:</strong></Label>
                                <input type="number" Class="form-control" required id="cantidadResmasOtro" name="cantidadResmasOtro" onkeyup="this.value = this.value.toUpperCase();" value="@item.cantidad_resmas_otro" />
                            </div>
                        </div>
                    </div>
                    <h1> Color</h1>
                    <div Class="step-content">
                        <div Class="row">
                            <div Class="col-lg-4">
                                <h3> Portada :        </h3>
                                @If item.colorPortada.ToString().Equals("FULL COLOR") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="FULL COLOR" checked>
                                        <Label for="colorPortada2"> Full Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="DUOTONO">
                                        <Label for="colorPortada2"> Duotono</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="UN COLOR">
                                        <Label for="colorPortada2"> Un Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="PANTONE">
                                        <Label for="colorPortada2"> Pantone</Label><br>
                                    </div>
                                ElseIf item.colorPortada.ToString().Equals("DUOTONO") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="FULL COLOR">
                                        <Label for="colorPortada2"> Full Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="DUOTONO" checked>
                                        <Label for="colorPortada2"> Duotono</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="UN COLOR">
                                        <Label for="colorPortada2"> Un Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="PANTONE">
                                        <Label for="colorPortada2"> Pantone</Label><br>
                                    </div>
                                ElseIf item.colorPortada.ToString().Equals("UN COLOR") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="FULL COLOR">
                                        <Label for="colorPortada2"> Full Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="DUOTONO">
                                        <Label for="colorPortada2"> Duotono</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="UN COLOR" checked>
                                        <Label for="colorPortada2"> Un Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="PANTONE">
                                        <Label for="colorPortada2"> Pantone</Label><br>
                                    </div>
                                ElseIf item.colorPortada.ToString().Equals("PANTONE") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="FULL COLOR">
                                        <Label for="colorPortada2"> Full Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="DUOTONO">
                                        <Label for="colorPortada2"> Duotono</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="UN COLOR">
                                        <Label for="colorPortada2"> Un Color</Label><br>
                                        <input type="radio" id="colorPortada2" name="colorPortada2" value="PANTONE" checked>
                                        <Label for="colorPortada2"> Pantone</Label><br>
                                    </div>
                                End If
                                <div Class="col-lg-12">
                                    <Label Class="font-normal"><strong>Cantidad de tinta y código Pantone:</strong></Label>
                                    <input Class="form-control" type="text" required id="cantidadTintaPortada" name="cantidadTintaPortada" onkeyup="this.value = this.value.toUpperCase();" value="@item.cantidad_tinta_portada"/>
                                </div>

                            </div>
                            <div Class="col-lg-4">
                                <h3> Interior :    </h3>
                                @If item.colorInterior.ToString().Equals("FULL COLOR") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="FULL COLOR" checked>
                                        <Label for="colorInterior2">Full Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="DUOTONO">
                                        <Label for="colorInterior2">Duotono</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="UN COLOR">
                                        <Label for="colorInterior2">Un Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="PANTONE">
                                        <Label for="colorInterior2">Pantone</Label><br>
                                    </div>
                                ElseIf item.colorInterior.ToString().Equals("DUOTONO") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="FULL COLOR">
                                        <Label for="colorInterior2">Full Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="DUOTONO" checked>
                                        <Label for="colorInterior2">Duotono</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="UN COLOR">
                                        <Label for="colorInterior2">Un Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="PANTONE">
                                        <Label for="colorInterior2">Pantone</Label><br>
                                    </div>
                                ElseIf item.colorInterior.ToString().Equals("UN COLOR") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="FULL COLOR">
                                        <Label for="colorInterior2">Full Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="DUOTONO">
                                        <Label for="colorInterior2">Duotono</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="UN COLOR" checked>
                                        <Label for="colorInterior2">Un Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="PANTONE">
                                        <Label for="colorInterior2">Pantone</Label><br>
                                    </div>
                                ElseIf item.colorInterior.ToString().Equals("PANTONE") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="FULL COLOR">
                                        <Label for="colorInterior2">Full Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="DUOTONO">
                                        <Label for="colorInterior2">Duotono</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="UN COLOR">
                                        <Label for="colorInterior2">Un Color</Label><br>
                                        <input type="radio" id="colorInterior2" name="colorInterior2" value="PANTONE" checked>
                                        <Label for="colorInterior2">Pantone</Label><br>
                                    </div>
                                End If

                                <div Class="col-lg-12">
                                    <Label Class="font-normal"><strong>Cantidad de tinta y código Pantone:</strong></Label>
                                    <input Class="form-control" type="text" required id="cantidadTintaInterior" name="cantidadTintaInterior" onkeyup="this.value = this.value.toUpperCase();" value="@item.cantidad_tinta_interior" />
                                </div>
                            </div>
                            <div Class="col-lg-4">
                                <h3> Acabado de portada:</h3>
                                @If item.acabado_de_portada.ToString().Equals("BARNIZ MATE") Then
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="acabadoPortada" name="acabadoPortada" value="BARNIZ MATE" checked>
                                        <Label for="acabadoPortada">Barníz mate</Label><br>
                                        <input type="radio" id="acabadoPortada" name="acabadoPortada" value="BARNIZ BRILLANTE">
                                        <Label for="acabadoPortada">Barníz brillante</Label><br>
                                    </div>
                                Else
                                    @<div Class="col-lg-12">
                                        <input type="radio" id="acabadoPortada" name="acabadoPortada" value="BARNIZ MATE">
                                        <Label for="acabadoPortada">Barníz mate</Label><br>
                                        <input type="radio" id="acabadoPortada" name="acabadoPortada" value="BARNIZ BRILLANTE" checked>
                                        <Label for="acabadoPortada">Barníz brillante</Label><br>
                                    </div>
                                End IF
                                <div Class="col-lg-12">
                                    <Label Class="font-normal"><strong>Cantidad:</strong></Label>
                                    <input Class="form-control" type="text" required id="cantidadAcabadoPortada" name="cantidadAcabadoPortada" onkeyup="this.value = this.value.toUpperCase();" value="@item.cantidad_acabado_de_portada" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <h1> Diseño e impresión</h1>
                    <div Class="step-content">
                        <div Class="row">
                            <div Class="col-lg-4">
                                <h3> Diseño :        </h3>
                                <Label Class="font-normal"><strong>Destino final:</strong></Label>
                                <div Class="col-lg-12">
                                    @If item.diseño_diseño.ToString().Equals("SI") Then
                                        @<input type="checkbox" id="diseñoDiseño" name="diseñoDiseño" value="SI" checked>
                                    Else
                                        @<input type="checkbox" id="diseñoDiseño" name="diseñoDiseño" value="SI" >
                                    End If
                                    <Label for="diseñoDiseño">Diseño</Label><br>

                                    @If item.diseño_imp_digital.ToString().Equals("SI") Then
                                        @<input type="checkbox" id="diseñoImpDigital" name="diseñoImpDigital" value="SI" checked>
                                    Else
                                        @<input type="checkbox" id="diseñoImpDigital" name="diseñoImpDigital" value="SI" >
                                    End If
                                    <Label for="diseñoImpDigital">Imp. digital</Label><br>

                                    @If item.diseño_ctp.ToString().Equals("SI") Then
                                        @<input type="checkbox" id="diseñoCTP" name="diseñoCTP" value="SI" checked>
                                    Else
                                        @<input type="checkbox" id="diseñoCTP" name="diseñoCTP" value="SI" >
                                    End If
                                    <Label for="diseñoCTP">CTP</Label><br>
                                    @If item.diseño_reimpresion.ToString().Equals("SI") Then
                                        @<input type="checkbox" id="diseñoReimpresion" name="diseñoReimpresion" value="SI" checked>
                                    Else
                                        @<input type="checkbox" id="diseñoReimpresion" name="diseñoReimpresion" value="SI" >
                                    End If
                                    <Label for="diseñoReimpresion">Reimpresión</Label><br>
                                    @If item.diseño_prensa.ToString().Equals("SI") Then
                                        @<input type="checkbox" id="diseñoPrensa" name="diseñoPrensa" value="SI" checked>
                                    Else
                                        @<input type="checkbox" id="diseñoPrensa" name="diseñoPrensa" value="SI" >
                                    End If
                                    <Label for="diseñoPrensa">Prensa</Label><br>
                                </div>
                            </div>
                            <div Class="col-lg-4">
                                <h3> Impresión :        </h3>
                                <Label class="font-normal"><strong>Portada:</strong></Label>
                                <div class="col-lg-12">
                                    @If item.tiroPortada.ToString().Equals("TIRO/RETIRO") Then
                                        @<input type="radio" id="portadaTiro" name="portadaTiro" value="TIRO/RETIRO" checked>
                                    Else
                                        @<input type="radio" id="portadaTiro" name="portadaTiro" value="TIRO/RETIRO">
                                    End If
                                    <Label for="portadaTiro">Tiro/Retiro</Label><br>

                                    @If item.tiroPortada.ToString().Equals("TIRO") Then
                                        @<input type="radio" id="portadaTiro" name="portadaTiro" value="TIRO" checked>
                                    Else
                                        @<input type="radio" id="portadaTiro" name="portadaTiro" value="TIRO">
                                    End If
                                    <Label for="portadaTiro">Tiro</Label><br>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <Label class="font-normal"><strong>Interior:</strong></Label>
                                <div class="col-lg-12">
                                    @If item.tiroInterior.ToString().Equals("TIRO/RETIRO") Then
                                        @<input type="radio" id="interiorTiro" name="interiorTiro" value="TIRO/RETIRO" checked>
                                    Else
                                        @<input type="radio" id="interiorTiro" name="interiorTiro" value="TIRO/RETIRO">
                                    End If
                                    <Label for="interiorTiro">Tiro/Retiro</Label><br>

                                    @If item.tiroInterior.ToString().Equals("TIRO") Then
                                        @<input type="radio" id="interiorTiro" name="interiorTiro" value="TIRO" checked>
                                    Else
                                        @<input type="radio" id="interiorTiro" name="interiorTiro" value="TIRO">
                                    End If
                                    <Label for="interiorTiro">Tiro</Label><br>
                                </div>
                            </div>
                            <div class="col-lg-4">

                            </div>
                            <div class="col-lg-8">
                                <Label class="font-normal"><strong>Cantidad a imprimir (Ya incluye excedente):</strong></Label>
                                <input class="form-control" type="number" value="@item.cantidad_imprimir" required id="cantidadImprimir" name="cantidadImprimir" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                        </div>
                    </div>
                    <h1> Encuadernación</h1>
                    <div class="step-content">
                        <div class="col-lg-4">
                            <div class="col-lg-12">

                                @If item.encuadernacion_plegado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="plegado" name="plegado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="plegado" name="plegado" value="SI" >
                                End If
                                <Label for="plegado">Plegado</Label><br>
                                @If item.encuadernacion_perforado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="perforado" name="perforado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="perforado" name="perforado" value="SI" >
                                End If
                                <Label for="perforado">Perforado</Label><br>
                                @If item.encuadernacion_pegado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="pegado" name="pegado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="pegado" name="pegado" value="SI" >
                                End If
                                <Label for="pegado">Pegado</Label><br>
                                @If item.encuadernacion_grapado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="grapado" name="grapado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="grapado" name="grapado" value="SI" >
                                End If
                                <Label for="grapado">Grapado</Label><br>
                                @If item.encuadernacion_alzado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="alzado" name="alzado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="alzado" name="alzado" value="SI" >
                                End If
                                <Label for="alzado">Alzado</Label><br>
                                @If item.encuadernacion_numerado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="numerado" name="numerado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="numerado" name="numerado" value="SI" >
                                End If
                                <Label for="numerado">Numerado</Label><br>
                                @If item.encuadernacion_cortado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="cortado" name="cortado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="cortado" name="cortado" value="SI" >
                                End If
                                <Label for="cortado">Cortado</Label><br>
                                @If item.encuadernacion_empacado.ToString().Equals("SI") Then
                                    @<input type="checkbox" id="empacado" name="empacado" value="SI" checked>
                                Else
                                    @<input type="checkbox" id="empacado" name="empacado" value="SI" >
                                End If
                                <Label for="empacado">Empacado</Label><br>
                            </div>
                        </div>
                    </div>
                    <h1> Observaciones específicas</h1>
                    <div class="step-content">
                        <div class="row">
                            <div class="col-lg-12">
                                <Label class="font-normal"><strong>Observaciones:</strong></Label>
                                <textarea class="form-control" type="text" id="observacionesEspecificas" name="observacionesEspecificas" rows="3" required onkeyup="this.value = this.value.toUpperCase();">@item.observaciones_especificas</textarea>
                            </div>
                            <div class="col-lg-12">
                                <br>
                                <Label class="font-normal"><strong>Comentario de edición:</strong></Label>
                                <input class="form-control" type="text" required id="comentarioDeEdicion" name="comentarioDeEdicion" onkeyup="this.value = this.value.toUpperCase();" />
                            </div>
                        </div>
                    </div>
                </div>
            Next
        End Using
    </div>
</div>
    @Section Styles
        @Styles.Render("~/plugins/wizardStepsStyles")
        @Styles.Render("~/plugins/awesomeCheckboxStyles")
        @Styles.Render("~/plugins/clockpickerStyles")
        @Styles.Render("~/plugins/dateRangeStyles")
        @Styles.Render("~/Content/plugins/iCheck/iCheckStyles")
        @Styles.Render("~/Content/plugins/chosen/chosenStyles")
        @Styles.Render("~/plugins/switcheryStyles")
        @Styles.Render("~/plugins/jasnyBootstrapStyles")
        @Styles.Render("~/plugins/nouiSliderStyles")
        @Styles.Render("~/plugins/dataPickerStyles")
        @Styles.Render("~/Content/plugins/ionRangeSlider/ionRangeStyles")
        @Styles.Render("~/plugins/imagecropperStyles")
        @Styles.Render("~/Content/plugins/colorpicker/colorpickerStyles")
        @Styles.Render("~/plugins/select2Styles")
        @Styles.Render("~/plugins/touchSpinStyles")
        @Styles.Render("~/plugins/tagInputsStyles")
        @Styles.Render("~/plugins/duallistStyles")
    End Section

    @Section Scripts
        @Scripts.Render("~/plugins/dataPicker")
        @Scripts.Render("~/plugins/ionRange")
        @Scripts.Render("~/plugins/nouiSlider")
        @Scripts.Render("~/plugins/jasnyBootstrap")
        @Scripts.Render("~/plugins/switchery")
        @Scripts.Render("~/plugins/chosen")
        @Scripts.Render("~/plugins/knob")
        @Scripts.Render("~/plugins/imagecropper")
        @Scripts.Render("~/plugins/colorpicker")
        @Scripts.Render("~/plugins/clockpicker")
        @Scripts.Render("~/plugins/dateRange")
        @Scripts.Render("~/plugins/select2")
        @Scripts.Render("~/plugins/touchSpin")
        @Scripts.Render("~/plugins/tagInputs")
        @Scripts.Render("~/plugins/duallist")
        <script>
    console.log("@ViewData("prueba")")
        </script>
        <script type="text/javascript">
            $(document).ready(function () {


                $('#data_1 .input-group.date').datepicker({
                    todayBtn: "linked",
                    keyboardNavigation: false,
                    forceParse: false,
                    calendarWeeks: true,
                    autoclose: true,
                    dateFormat: 'dd/mm/yyyy'
                });

                $('input[name="daterange"]').daterangepicker();


                $('#data_2 .input-group.date').datepicker({
                    startView: 1,
                    todayBtn: "linked",
                    keyboardNavigation: false,
                    forceParse: false,
                    autoclose: true,
                    format: 'dd/mm/yyyy'
                });

            });

        </script>
        @Scripts.Render("~/plugins/wizardSteps")
        @Scripts.Render("~/plugins/iCheck")
        @Scripts.Render("~/plugins/validate")
        <script type="text/javascript">
            var settings = {
                onFinished: function (event, currentIndex) {
                    $("#form").submit();
                },
                labels: {
                    cancel: "Cancelar",
                    current: "Paso actual:",
                    pagination: "Paginación",
                    finish: "Finalizar",
                    next: "Siguiente",
                    previous: "Anterior",
                    loading: "Cargando ..."
                }


            };

            $(document).ready(function () {
                $("#wizard").steps(settings);
                $("#form").steps({
                    onFinished: function (event, currentIndex) {
                        var form = $(this);
                        form.submit();
                    }
                })

            });

        </script>
    End Section
