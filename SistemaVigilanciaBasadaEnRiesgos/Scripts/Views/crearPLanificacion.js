$(document).ready(function () {
    ////validamos el formulario
    $("#form").validate({
        rules: {
            Direccion: "required",
            Telefono: "required"
        },
        messages: {
            Direccion: "(*)",
            Telefono: "(*)"

        },
        errorElement: 'span'
    });


    $('#IdTipoProveedorServicio').on('change', function () {
        var _oid = $(this).val();
        if (_oid != "") {
            $("#IdListaVerificacion").html("");
            $("<option>").attr({ "value": "0" }).text("SELECCIONE ").appendTo("#IdListaVerificacion");
            $.get($.MisUrls.url._ObtieneTipoProveedorServicioPorOid, { Id: _oid }, function (data) {
                if (data.oListaDeVerificacion != null) {
                    $.each(data.oListaDeVerificacion, function (i, item) {
                        $("<option>").attr({ "value": item.ListaID }).text(item.Nombre + " - " + item.Descripcion).appendTo("#IdListaVerificacion");
                    });
                }
            });
        }


    });

    $('#OrganizacionID').on('change', function () {
        var _oid = $(this).val();
        if (_oid != "") {
            $.get($.MisUrls.url._ObtieneOrganizacionPorOid, { Id: _oid }, function (data) {
                if (data != null) {
                    $("#Direccion").val(data.Direccion);
                    $("#Telefono").val(data.Telefono);
                    $("#Correo").val(data.Correo);
                    $("#GerenteResponsable").val(data.GerenteResponsable);

                }
            });
        }


    });

});



function Crear() {


    $('#modalCrear').modal('show');
}


function Guardar() {
    if ($("#form").valid()) {
        var _IdRespuesta = parseInt($("#RespuestaID").val());
        var _IdTipoProveedorServicio = $('#IdTipoProveedorServicio option:selected').val();
        var _ListaID = $("#IdListaVerificacion").val();
        var _OrganizacionID = $("#OrganizacionID").val();
        var _Certificado = $("#Certificado").val();
        var _UsuarioID = $("#UsuarioID").val();
        var _FechaInicio = $("#FechaInicio").val();
        var _FechaFin = $("#FechaFin").val();

        // Organizacion
        var _Nombre = $('#OrganizacionID option:selected').text();
        var _Direccion = $("#Direccion").val();
        var _Telefono = $("#Telefono").val();
        var _Correo = $("#Correo").val();
        var _GerenteResponsable = $("#GerenteResponsable").val();

        var _suarioRespuestaLV = new Array();

        $("#tbInspectores TBODY TR").each(function () {
            var row = $(this);
            var customer = {};
            customer.IdUsuarioRespuesta = 0;
            customer.IdUsuario = row.find("TD").eq(0).html();
            customer.IdRespuestaLV = _IdRespuesta;
            _suarioRespuestaLV.push(customer);
        });


        var request = {
            objeto: {
                RespuestaID: _IdRespuesta,
                IdTipoProveedorServicio: _IdTipoProveedorServicio,
                ListaID: _ListaID,
                OrganizacionID: _OrganizacionID,
                Certificado: _Certificado,
                UsuarioID: _UsuarioID,
                FechaInicio: _FechaInicio,
                FechaFin: _FechaFin,
                oOrganizacion: {
                    OrganizacionID: _OrganizacionID,
                    Nombre: _Nombre,
                    Direccion: _Direccion,
                    Telefono: _Telefono,
                    Correo: _Correo,
                    GerenteResponsable: _GerenteResponsable
                },
                oUsuarioRespuestaLV: _suarioRespuestaLV
            }
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarEncabezadoRespuesta,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    // tabladata.ajax.reload();                                   
                    Swal.fire({
                        title: 'Planificación',
                        text: "La información se guardo correctamente",
                        icon: 'warning',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#C5C7CF',
                        confirmButtonText: 'Aceptar',
                        
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                            $('#modalCrear').modal('hide');    
                        }
                    })
                } else {
                    mensajeGeneralIco("Mensaje", data.mensajeError, "warning")                    
                }
            },
            error: function (error) {
                mensajeGeneralIco("Error", data.mensajeError, "error")    
                console.log(error)
            },
            beforeSend: function () {

            },
        });

    }
}

function agregarInpectores() {
    var _numVuelo = 0;
    var _oidUsuario = $('#InspectorID').val();
    var _onombre = $('#InspectorID option:selected').text();
    if (_oidUsuario != "0") {
        $("#tbInspectores TBODY TR").each(function () {
            _numVuelo++;
        });

        if (_numVuelo > 0) {
            var existe = false;
            var _idUsuario = 0;
            $("#tbInspectores TBODY TR").each(function () {
                var row = $(this);
                _idUsuario = row.find("TD").eq(0).html();
                if (_oidUsuario == _idUsuario) {
                    existe = true;
                    return;
                }
            });

            if (!existe) {
                $("<tr>").append(
                    $("<td>").text(_oidUsuario),
                    $("<td>").text(_onombre),
                    $("<td>").html('<a href="#" class="skiplink-text" onclick="EliminarInspector(this);"><i class="ti-trash"></i>Eliminar</a>')
                ).appendTo("#tbInspectores tbody");
                $("#InspectorID").val("0");
            }
        }
        else {
            $("<tr>").append(
                $("<td>").text(_oidUsuario),
                $("<td>").text(_onombre),
                $("<td>").html('<a href="#" class="skiplink-text" onclick="EliminarInspector(this);"><i class="ti-trash"></i>Eliminar</a>')
            ).appendTo("#tbInspectores tbody");
            $("#InspectorID").val("0");
        }


    }
}

function EliminarInspector(_row) {
    if (_row != null) {
        Swal.fire({
            title: 'Eliminar',
            text: "¿Desea eliminar el inspector seleccionado?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#C5C7CF',
            confirmButtonText: 'Si',
            cancelButtonText: "No",
        }).then((result) => {
            if (result.isConfirmed) {
                var d = _row.parentNode.parentNode.rowIndex;
                document.getElementById('tbInspectores').deleteRow(d);
            }
        })
    }


}

function mensajeGeneralIco(titulo, contenido, _ico) {
    Swal.fire({
        icon: _ico,
        title: "<p style='width: 100 %;'>" + titulo + "</p>",
        html: "<ul >" + contenido + "</ul>",
        confirmButtonText: 'Aceptar'
    });
}