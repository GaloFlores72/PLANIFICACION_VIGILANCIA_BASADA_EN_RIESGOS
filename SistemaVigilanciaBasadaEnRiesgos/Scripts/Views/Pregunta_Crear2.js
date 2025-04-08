var tabladata;

$(document).ready(function () {
    // Deshabilitar el botón Agregar desde el inicio
    $("#btnNuevo").prop("disabled", true);

    cargarListasDeVerificacion();

    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerPreguntas,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Referencia" },
            { "data": "Descripcion" },
            {
                "data": "PreguntaID", "render": function (data, type, row, meta) {
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
        responsive: true
    });

    // Cuando cambie ddlListas, cargar subtítulos y deshabilitar botón Agregar
    $("#ddlListas").change(function () {
        var listaID = $(this).val();
        $("#btnNuevo").prop("disabled", true); // Desactiva al cambiar

        if (listaID) {
            $("#ddlSubtitulos").prop("disabled", false);
            cargarSubtitulos(listaID);
        } else {
            $("#ddlSubtitulos").prop("disabled", true);
            $("#ddlSubtitulos").empty().append('<option value="">-- Seleccione un subtítulo --</option>');
        }
    });

    // Si cambia cualquier filtro, desactiva el botón Agregar
    $("#ddlSubtitulos").change(function () {
        $("#btnNuevo").prop("disabled", true);
    });

    // Buscar con filtros
    $("#btnBuscar").click(function () {
        const listaID = $("#ddlListas").val();
        const subtituloID = $("#ddlSubtitulos").val();

        if (!listaID) {
            Swal.fire("Atención", "Debe seleccionar una Lista de Verificación", "warning");
            return;
        }

        if (!subtituloID) {
            Swal.fire("Atención", "Debe seleccionar un Subtítulo", "warning");
            return;
        }

        const nuevaUrl = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + `?SubtituloID=${subtituloID}`;
        tabladata.ajax.url(nuevaUrl).load();

        // Habilitar botón Agregar después de aplicar filtros correctamente
        $("#btnNuevo").prop("disabled", false);
    });
});

// -------------------- Cargar Listas y Subtítulos --------------------

function cargarListasDeVerificacion() {
    $.ajax({
        url: $.MisUrls.url._ObtenerListaVerificacionTodos,
        type: 'GET',
        success: function (response) {
            const ddl = $("#ddlListas");
            ddl.empty();
            ddl.append('<option value="">-- Seleccione una lista de verificación --</option>');

            $.each(response.data, function (index, item) {
                ddl.append(`<option value="${item.ListaID}" data-nombre="${item.Nombre}">${item.Nombre}</option>`);
            });
        },
        error: function (err) {
            console.error("Error al cargar las listas de verificación:", err);
        }
    });
}

function cargarSubtitulos(listaID) {
    if (!listaID) return;

    const ddl = $("#ddlSubtitulos");
    ddl.empty().append('<option value="">-- Seleccione un subtítulo --</option>').prop("disabled", false);

    $.ajax({
        url: $.MisUrls.url._ObtenerSubtitulosPorListaId,
        type: 'GET',
        data: { ListaID: listaID },
        success: function (response) {
            if (response.data && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    ddl.append(`<option value="${item.SubtituloID}" data-nombre="${item.Nombre}">${item.Nombre}</option>`);
                });
            }
        },
        error: function (err) {
            console.error("Error al cargar subtítulos:", err);
        }
    });
}

// -------------------- MODAL CREAR --------------------

function abrirModalCrearPregunta() {
    const listaNombre = $("#ddlListas option:selected").text();
    const subtituloNombre = $("#ddlSubtitulos option:selected").text();
    const subtituloID = $("#ddlSubtitulos").val();

    $("#crearListaNombre").val(listaNombre);
    $("#crearSubtituloNombre").val(subtituloNombre);
    $("#crearSubtituloID").val(subtituloID);

    $("#crearDescripcion").val("");
    $("#crearReferencia").val("");

    $("#ModalCrearPregunta").modal("show");
}

function guardarNuevaPregunta() {
    const subtituloID = $("#crearSubtituloID").val();
    const descripcion = $("#crearDescripcion").val();
    const referencia = $("#crearReferencia").val();

    if (!subtituloID) {
        Swal.fire("Error", "Debe tener un subtítulo seleccionado", "error");
        return;
    }

    const request = {
        SubtituloID: subtituloID,
        Descripcion: descripcion,
        Referencia: referencia,
        Estado: "No Satisfactorio", //valores por defecto
        Estadisticas: 0 //valores por defecto
    };

    $.ajax({
        url: $.MisUrls.url._GuardarPregunta,
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json",
        success: function (data) {
            if (data.resultado) {
                const url = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + "?SubtituloID=" + subtituloID;
                tabladata.ajax.url(url).load();
                $("#ModalCrearPregunta").modal("hide");
                Swal.fire({
                    title: "¡Éxito!",
                    text: "La pregunta fue guardada correctamente.",
                    icon: "success",
                    confirmButtonText: "Aceptar"
                });
            } else {
                Swal.fire("Error", "No se pudo guardar la pregunta", "warning");
            }
        },
        error: function (error) {
            console.error(error);
            Swal.fire("Error", "Ocurrió un error al guardar", "error");
        }
    });
}


// -------------------- MODAL EDITAR --------------------

function abrirPopUpForm(row) {
    const idPregunta = row.PreguntaID;

    $.ajax({
        url: $.MisUrls.url._ObtenerPreguntaPorId,
        type: 'GET',
        data: { idPregunta: idPregunta },
        success: function (response) {
            const pregunta = response.data;

            if (!pregunta) {
                Swal.fire("Error", "No se pudo cargar la información de la pregunta", "error");
                return;
            }

            $("#txtid").val(pregunta.PreguntaID);
            $("#txtSubtituloID").val(pregunta.SubtituloID);
            $("#txtDescripcion").val(pregunta.Descripcion);
            $("#txtReferencia").val(pregunta.Referencia);

            // Aquí llenas los campos de Lista y Subtítulo relacionados (solo visuales, deshabilitados)
            if (pregunta.oSubtitulo) {
                $("#txtSubtituloNombre").val(pregunta.oSubtitulo.Nombre);

                if (pregunta.oSubtitulo.oListaVerificacion) {
                    $("#txtListaNombre").val(pregunta.oSubtitulo.oListaVerificacion.Nombre);
                }
            }

            $('#FormModal').modal('show');
        },
        error: function (err) {
            console.error(err);
            Swal.fire("Error", "Error al obtener los datos de la pregunta", "error");
        }
    });
}


function Guardar() {
    if ($("#form").valid()) {
        const subtituloID = $("#txtSubtituloID").val();

        if (!subtituloID) {
            Swal.fire("Error", "No se puede guardar sin un Subtítulo válido.", "error");
            return;
        }

        const request = {
            SubtituloID: subtituloID,
            Descripcion: $("#txtDescripcion").val(),
            Referencia: $("#txtReferencia").val()
        };

        if ($("#txtid").val() != "0") {
            request.PreguntaID = $("#txtid").val();
        }

        $.ajax({
            url: $.MisUrls.url._GuardarPregunta,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    const url = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + '?SubtituloID=' + subtituloID;
                    tabladata.ajax.url(url).load();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire("Pregunta", "No se pudo guardar los cambios", "warning");
                }
            },
            error: function (error) {
                console.log(error);
                Swal.fire("Error", "Hubo un problema al guardar la pregunta", "error");
            }
        });
    }
}

// -------------------- ELIMINAR --------------------

function eliminar(id) {
    Swal.fire({
        title: "Preguntas",
        text: "¿Desea eliminar el registro seleccionado?",
        icon: "warning",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "SI",
        denyButtonText: "NO"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: $.MisUrls.url._EliminarPregunta + "?id=" + id,
                type: "GET",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire("Mensaje", "No se pudo eliminar el subtítulo", "warning");
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
}
