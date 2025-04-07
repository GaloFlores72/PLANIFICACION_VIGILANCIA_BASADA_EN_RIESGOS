var tabladata;

$(window).on('load', function () {
    cargarListasDeVerificacion();
    cargarEstadosdeImplementacion();

        tabladata = $('#tbdata').DataTable({
            "ajax": {
                "url": $.MisUrls.url._ObtenerOrientacionEstadosTodos,
                "type": "GET",
                "datatype": "json",
            } ,
            "columns": [
                { "data": "OrientacionEstadoID" },
                { "data": "DescripcionOrientacion" },
                { "data": "DescripcionEstado" },
                {
                    "data": "OrientacionEstadoID", "render": function (data, type, row, meta) {
                        return "<button class='btn btn-primary btn-sm' type='button' onclick='abrirPopUpForm(" + JSON.stringify(row) + ")'><i class='fas fa-pen'></i></button>" +
                            "<button class='btn btn-danger btn-sm ml-2' type='button' onclick='eliminar(" + data + ")'><i class='fa fa-trash'></i></button>";
                    },
                    "orderable": false,
                    "searchable": false,
                    "width": "90px"
                }
            ],
            "language": {
                "url": $.MisUrls.urls.Url_datatable_spanish
            },
            responsive: true,
            "initComplete": function (settings, json) {
                console.log("DataTable ha sido inicializada correctamente.");
            }
        });

    $("#ddlListas").change(function () {
        var listaID = $(this).val();
        if (listaID) {
            cargarSubtitulos(listaID);
        }
    });

    $("#ddlSubtitulos").change(function () {
        var subtituloID = $(this).val();
        if (subtituloID) {
            cargarPreguntas(subtituloID);
        }
    });

    $("#ddlPreguntas").change(function () {
        var preguntaID = $(this).val();
        if (preguntaID) {
            cargarOrientaciones(preguntaID);
        }
    });

    $("#ddlOrientacion").change(function () {
        var orientacionID = $(this).val();
        console.log(orientacionID)
        if (orientacionID) {
            tabladata.ajax.url($.MisUrls.url._ObtenerOrientacionEstadoPorOrientacionID + "?OrientacionID=" + orientacionID).load();
        }
    });
    
});

function cargarListasDeVerificacion() {
    $.ajax({
        url: $.MisUrls.url._ObtenerListaVerificacionTodos,
        type: 'GET',
        success: function (response) {
            var ddl = $("#ddlListas");
            ddl.empty();
            ddl.append('<option value="">-- Seleccione una lista de verificación --</option>');

            $.each(response.data, function (index, item) {
                ddl.append('<option value="' + item.ListaID + '" data-nombre="' + item.Nombre + '">' + item.Nombre + '</option>');
            });
        },
        error: function (err) {
            console.error("Error al cargar las listas de verificación:", err);
        }
    });
}

function cargarSubtitulos(listaID) {
    if (!listaID) {
        console.error("ListaID no está definido.");
        return;
    }

    $("#ddlSubtitulos").empty();
    $("#ddlSubtitulos").append('<option value="">-- Seleccione un subtítulo --</option>');
    $("#ddlSubtitulos").prop("disabled", false);

    $.ajax({
        url: $.MisUrls.url._ObtenerSubtitulosPorListaId,
        type: 'GET',
        data: { ListaID: listaID },
        success: function (response) {
            if (response.data && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    $("#ddlSubtitulos").append('<option value="' + item.SubtituloID + '" data-nombre="' + item.Nombre + '">' + item.Nombre + '</option>');
                });
            } else {
                $("#ddlSubtitulos").prop("disabled", true); 
                console.error("No se encontraron subtítulos.");
            }
        },
    });
}


function cargarPreguntas(subtituloID) {
    if (!subtituloID) {
        console.error("subtituloID no está definido.");
        return;
    }

    $("#ddlPreguntas").empty();
    $("#ddlPreguntas").append('<option value="">-- Seleccione una pregunta --</option>');
    $("#ddlPreguntas").prop("disabled", false);

    $.ajax({
        url: $.MisUrls.url._ObtenerPreguntasPorSubtitulo,
        type: 'GET',
        data: { SubtituloID: subtituloID },
        success: function (response) {
            if (response.data && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    $("#ddlPreguntas").append('<option value="' + item.PreguntaID + '" data-descripcion="' + item.Descripcion + '">' + item.Descripcion + '</option>');
                });
            } else {
                console.error("No se encontraron preguntas.");
            }
        },
    });
}

function cargarOrientaciones(preguntaID) {
    if (!preguntaID) {
        console.error("preguntaID no está definido.");
        return;
    }

    $("#ddlOrientacion").empty();
    $("#ddlOrientacion").append('<option value="">-- Seleccione una orientación --</option>');
    $("#ddlOrientacion").prop("disabled", false);

    $.ajax({
        url: $.MisUrls.url._ObtenerOrientacionesPorIdPregunta,
        type: 'GET',
        data: { PreguntaID: preguntaID },
        success: function (response) {
            if (response.data && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    $("#ddlOrientacion").append('<option value="' + item.OrientacionID + '" data-descripcion="' + item.Descripcion + '">' + item.Descripcion + '</option>');
                });
            } else {
                console.error("No se encontraron orientaciones.");
            }
        },
    });
}

function cargarEstadosdeImplementacion() {
    $.ajax({
        url: $.MisUrls.url._ObtenerEstadoDeImplementacion,
        type: 'GET',
        success: function (response) {
            console.log("Respuesta del servidor:", response); 

            var ddl = $("#ddlEstadosImplementacion");
            ddl.empty();
            ddl.append('<option value="">-- Seleccione un Estado de Implementación --</option>');

            if (response.data && Array.isArray(response.data) && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    ddl.append('<option value="' + item.EstadoID + '">' + item.Descripcion + '</option>');
                });
            } else {
                console.log("No se encontraron estados de implementación.");
            }
        },
        error: function (err) {
            console.error("Error al cargar los Estado de Implementación:", err);
        }
    });
}




function abrirPopUpForm(json) {
    console.log(json);

    $("#txtid").val(0);
    

    if (json != null) {
        $("#txtid").val(json.OrientacionEstadoID);
        var orientacionID = $("#ddlOrientacion").val();
        $("#txtOrientacionID").val(orientacionID);
        $("#txtEstadoID").val(json.EstadoID);
    } else {
        var orientacionID = $("#ddlOrientacion").val();
        $("#txtOrientacionID").val(orientacionID);
        $("#txtEstadoID").val("");
    }

    $('#FormModal').modal('show');
}

function Guardar() {
    if ($("#form").valid()) {
        var orientacionID = $("#ddlOrientacion").val();
        var EstadoID = $("#ddlEstadosImplementacion").val();
        var OrientacionEstadoID = $("#txtid").val(); 

        var request = {
            orientacionID: orientacionID,
            EstadoID: EstadoID,
        };

        if (OrientacionEstadoID != "0") {
            console.log(OrientacionEstadoID);
            request.OrientacionEstadoID = OrientacionEstadoID; 
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarOrientacionEstado,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    tabladata.ajax.reload();  // Recarga los datos en la tabla
                    $('#FormModal').modal('hide');  // Cierra el modal
                } else {
                    Swal.fire("Orientacion Estado", "No se pudo guardar los cambios", "warning");
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}


