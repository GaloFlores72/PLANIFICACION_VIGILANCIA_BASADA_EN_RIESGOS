﻿//var table = $('#tablaLV').DataTable(); // Inicializa DataTables
$(document).ready(function () {

    $('#OrganizacionID').on('change', function () {
        var _oid = $(this).val();
        $.get($.MisUrls.url._ObtieneOrganizacionPorOid, { Id: _oid }, function (data) {
            if (data) {
                $('#oOrganizaciones_OrganizacionID').val(data.OrganizacionID);
                $('#oOrganizacion_Nombre').val(data.Nombre);
                $('#oOrganizacion_Direccion').val(data.Direccion);
                $('#oOrganizacion_GerenteResponsable').val(data.GerenteResponsable);
                $('#oOrganizacion_NCertificadoOMA').val(data.NCertificadoOMA);
                $('#oOrganizacion_Correo').val(data.Correo);
                $('#oOrganizacion_Telefono').val(data.Telefono);
            }
        });

    });

    $(".color-selector").change(function () {
        // Obtener el valor seleccionado
        var estadoId = $(this).val();
        var valores = "";
        var _comentario = "";
        $(this).parents("tr").find("td").each(function () {
            valores += $(this).html() + "\n";
        });

        let fila = $(this).closest("tr"); // Obtiene la fila padre
        let _detalleRespuestaId = fila.find("td:eq(0)").text(); // Primera columna (DetalleRespuestaID)
        let _respuestaOrientacionId = fila.find("td:eq(1)").text(); // Segunda columna (RespuestaOrientacionID)       
        //Ajax Post
        if (_detalleRespuestaId > 0 && _respuestaOrientacionId > 0) {
            jQuery.ajax({
                type: "GET",
                url: $.MisUrls.url._CambiaOrientacionEstado,
                data: { detalleRespuestaId: _detalleRespuestaId, respuestaOrientacionId: _respuestaOrientacionId, estadoId: estadoId, comentario: _comentario },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                   
                    window.location.reload();
                },
                failure: function (response) {                  
                    MensajeIco("Lista Verificación", response.responseText, "error");     
                },
                error: function (response) {          
                    MensajeIco("Lista Verificación", response.responseText, "error");                    
                }
            });
        }       
    });

})

function modalConstataciones(_id) {
    if (_id > 0) {
        jQuery.ajax({
            type: "GET",
            url: $.MisUrls.url._ObtenerConstantacionPorOrientacionId,
            data: { id: _id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != undefined && data != null) {
                    $("#orientacionExamen").val(data.Descripcion);
                    $("#codigoPeligro").val(data.CodigoPeligro);
                    $("#OOrientacionID").val(data.OrientacionID);  
                    $('#COrientacionID').val(data.OrientacionID);
                    $("#tbConstataciones tbody").html("");
                    $.each(data.oContataciones, function (i, row) {
                        // Convertir la fecha a formato legible
                        let fechaFormateada = formatearFecha(row["FechaConstatacion"]);
                        $("<tr>").append(
                            $("<td>").text(row["ConstatacionID"]),
                            $("<td>").text(row["oArea"].Nombre),
                            $("<td>").text(row["DescripcionConstatacion"]),
                            $("<td>").text(fechaFormateada),
                            $("<td class'text-center'>").html('<a href="#" class="btn btn-bottom btn-sm btn-outline-primary" onclick="modalConstatacionNuevo(' + row["ConstatacionID"] + ')">Editar</a>  <a href="#" class="btn btn-outline-danger btn-sm" onclick="ConstatacionEliminar(' + row["ConstatacionID"] +')">Eliminar</a>')
                        ).appendTo("#tbConstataciones tbody");
                    })
                    $('#modalConstataciones').modal('show');
                }
                
            },
            failure: function (response) {
                MensajeIco("Constatación", response.responseText, "error");
            },
            error: function (response) {
                MensajeIco("Constatación", response.responseText, "error");
            }
        });
               
    }
}

function ConstatacionEliminar(_id) {
    var idOrientacion = $('#OOrientacionID').val(); 
    Swal.fire({
        title: 'Constatación',
        html: "¿Desea eliminar la constatación seleccionado?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#C5C7CF',
        confirmButtonText: 'Si',
        cancelButtonText: "No",
    }).then((result) => {
        if (result.isConfirmed) {
            jQuery.ajax({
                type: "POST",
                url: $.MisUrls.url._EliminarConstacion,
                data: { id: _id },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (resultado) {
                        modalConstataciones(idOrientacion);
                    }
                },
                failure: function (response) {
                    MensajeIco("Constatación", response.responseText, "error");
                },
                error: function (response) {
                    MensajeIco("Constatación", response.responseText, "error");
                }
            });
        }
    })
}

// Función para formatear la fecha correctamente
function formatearFecha(fecha) {
    if (!fecha) return "Sin fecha"; // Manejo de valores nulos o vacíos

    let fechaObj = new Date(fecha);
    if (isNaN(fechaObj.getTime())) return "Fecha inválida"; // Verifica si la fecha es válida

    // Formato: dd/mm/yyyy hh:mm:ss
    let dia = fechaObj.getDate().toString().padStart(2, "0");
    let mes = (fechaObj.getMonth() + 1).toString().padStart(2, "0"); // +1 porque enero es 0
    let anio = fechaObj.getFullYear();
    let horas = fechaObj.getHours().toString().padStart(2, "0");
    let minutos = fechaObj.getMinutes().toString().padStart(2, "0");
    let segundos = fechaObj.getSeconds().toString().padStart(2, "0");

    return `${dia}/${mes}/${anio} ${horas}:${minutos}:${segundos}`;
}

function formatearFechaFormato(fecha) {
    if (!fecha) return "Sin fecha"; // Manejo de valores nulos o vacíos

    let fechaObj = new Date(fecha);
    if (isNaN(fechaObj.getTime())) return "Fecha inválida"; // Verifica si la fecha es válida

    // Formato: dd/mm/yyyy hh:mm:ss
    let dia = fechaObj.getDate().toString().padStart(2, "0");
    let mes = (fechaObj.getMonth() + 1).toString().padStart(2, "0"); // +1 porque enero es 0
    let anio = fechaObj.getFullYear();
    let horas = fechaObj.getHours().toString().padStart(2, "0");
    let minutos = fechaObj.getMinutes().toString().padStart(2, "0");
    let segundos = fechaObj.getSeconds().toString().padStart(2, "0");
    return `${anio}-${mes}-${dia}`;
    //return `${dia}/${mes}/${anio}`;
}

function modalConstatacionNuevo(id) {
    if (id > 0) {        
        jQuery.ajax({
            type: "GET",
            url: $.MisUrls.url._ObtenerOrientacionPorId,
            data: { id: id},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != undefined && data != null) {
                    let fehaConstatacion = formatearFechaFormato(data.FechaConstatacion);
                    $('#ConstatacionID').val(data.ConstatacionID);
                    $('#COrientacionID').val(data.OrientacionID);
                    $('#AreaID').val(data.AreaID);
                    $('#FechaConstatacion').val(fehaConstatacion);
                    $("#PresuntaInfraccion").prop("checked")
                    $('#DescripcionConstatacion').val(data.DescripcionConstatacion);
                    $("#AfectaSO").prop("checked")
                    $('#NotaAfectaSO').val(data.NotaAfectaSO);
                    $('#EvidenviaNombre').val(null);
                    $('#EvidenciaArchivo').val(null);
                    $("#tbEvidencia tbody").html(""); 
                }

            },
            failure: function (response) {
                MensajeIco("Constatación", response.responseText, "error");
            },
            error: function (response) {
                MensajeIco("Constatación", response.responseText, "error");
            }
        });
    }
    else {
        $('#AreaID').val(0);
        $('#FechaConstatacion').val(null);
        $('#DescripcionConstatacion').val(null);
        $('#NotaAfectaSO').val(""); 
        $('#EvidenviaNombre').val(null); 
        $('#EvidenciaArchivo').val(null); 
        $("#tbEvidencia tbody").html(""); 
    }
    $('#modalConstataciones').modal('hide');
    $('#modalConstatacion').modal('show');
    
}

function camposCostatacionCambiaColor() {
    $("#AreaID").removeClass("border-danger");
    $("#FechaConstatacion").removeClass("border-danger");
    $("#DescripcionConstatacion").removeClass("border-danger");
    $('#NotaAfectaSO').removeClass("border-danger");
}
function validaCamposConstatacion() {
    camposCostatacionCambiaColor();
    var _AreaID = $('#AreaID').val();
    var _FechaConstatacion = $('#FechaConstatacion').val();   
    var _DescripcionConstatacion = $('#DescripcionConstatacion').val();   
    var _NotaAfectaSO = $('#NotaAfectaSO').val(); 
    if (_AreaID == 0) {
        MensajeIco("Constatación", "Seleccione el Área es obligatorio ingresar", "warning");
        $('#AreaID').addClass("border-danger");
        return false;
    }

    if (_FechaConstatacion.trim() === "" || isNaN(new Date(_FechaConstatacion).getTime())) {
        MensajeIco("Constatación", "Fecha de constatación es obligatorio ingresar", "warning");
        $('#FechaConstatacion').addClass("border-danger");
        return false;
    }
    if (ValidaCampoVacio(_DescripcionConstatacion)) {
        MensajeIco("Constatación", "Descripción de la constatación en blanco es obligatorio ingresar", "warning");
        $('#DescripcionConstatacion').addClass("border-danger");
        return false;
    }

    if ($("#AfectaSO").is(":checked")) {
        if (ValidaCampoVacio(_NotaAfectaSO)) {
            MensajeIco("Constatación", "Nota afecta seguridad operacional en blanco es obligatorio ingresar", "warning");
            $('#NotaAfectaSO').addClass("border-danger");
            return false;
        }
    }
    else {
        $('#NotaAfectaSO').val("");
    }

    return true;
}
function grabarContatacion() {
    if (validaCamposConstatacion()) {
        var _constatacionID = $('#ConstatacionID').val();
        var _cOrientacionID = $('#COrientacionID').val();
        var _areaID = $('#AreaID').val();
        var _fechaConstatacion = $('#FechaConstatacion').val();
        var _presuntaInfraccion = $("#PresuntaInfraccion").prop("checked")
        var _descripcionConstatacion = $('#DescripcionConstatacion').val();
        var _afectaSO = $("#AfectaSO").prop("checked")
        var _notaAfectaSO = $('#NotaAfectaSO').val();

        var request = {
            objeto: {
                ConstatacionID: parseInt(_constatacionID),
                OrientacionID: parseInt(_cOrientacionID),
                AreaID: parseInt(_areaID),
                FechaConstatacion: _fechaConstatacion,
                PresuntaInfraccion: _presuntaInfraccion,
                DescripcionConstatacion: _descripcionConstatacion,
                AfectaSO: _afectaSO,
                NotaAfectaSO: _notaAfectaSO
            }
        }
        $("#loadImagenEnviar").css("display", "block");
        jQuery.ajax({
            url: $.MisUrls.url._GuardarConstacion,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    //tabladata.ajax.reload();
                    salirConstatacion();
                    modalConstataciones(_cOrientacionID);
                } else {
                    MensajeIco("Mensaje", "No se pudo guardar los cambios", "warning")
                }
                $("#loadImagenEnviar").css("display", "none");
            },
            error: function (error) {
                MensajeIco("Error", error, "error")              
                $("#loadImagenEnviar").css("display", "none");
            },            
            beforeSend: function () {},
        });        
    }
}

function filtrarTabla() {

    let input = $("#buscar").val().toLowerCase(); // Obtener texto de búsqueda
    let encontrado = false; // Para saber si hay coincidencias

    $("#tablaLV tbody tr").each(function () {
        let textoFila = $(this).text().toLowerCase(); // Obtener el texto de la fila

        if (textoFila.includes(input)) {
            $(this).show().addClass("resaltado"); // Mostrar y resaltar
            if (!encontrado) {
                $('html, body').animate({ scrollTop: $(this).offset().top - 100 }, 500); // Desplazar a la primera coincidencia
                encontrado = true;
            }
        } else {
            $(this).hide().removeClass("resaltado"); // Ocultar y quitar resaltado
        }
    });
}
function salirConstatacion() {
    $('#modalConstataciones').modal('show');
    $('#modalConstatacion').modal('hide');
}

function cambia_Color(elemento) {

    var selectedColor = elemento.val();
    elemento.style.backgroundColor = '#ffcccc'; // Color cuando el mouse está sobre la celda
}

function MensajeIco(titulo, mensaje, icono) {
    Swal.fire({
        title: titulo,
        text: mensaje,
        icon: icono,
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Aceptar',
        allowOutsideClick: false,
    });
}

function ValidaCampoVacio(_campo) {
    if (_campo == null || _campo == undefined || _campo == "") { return true }
    else { return false }
}