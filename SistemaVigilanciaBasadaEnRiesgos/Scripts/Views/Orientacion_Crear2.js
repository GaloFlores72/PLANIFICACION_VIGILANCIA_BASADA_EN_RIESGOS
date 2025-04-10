var tabladata;

$(document).ready(function () {
    // Deshabilita el botón Agregar desde el inicio
    $("#btnNuevo").prop("disabled", true);
    cargarListasDeVerificacion();
    // Validación en tiempo real para CódigoOrientacion (crear y editar)
    $(document).on('input', '#crearCodigoOrientacion, #txtCodigoOrientacion', function () {
        const valor = $(this).val();
        const regex = /^[A-Z0-9\-]*$/;
        if (!regex.test(valor)) {
            Swal.fire("Carácter no permitido", "Solo se permiten letras Mayusculas, números y el guion (-). No se permiten espacios ni caracteres especiales.", "warning");
            $(this).val(valor.replace(/[^A-Z0-9\-]/g, ''));
        }
    });

    // Inicializa la tabla
    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerOrientacion,
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                var preguntaID = $("#ddlPreguntas").val();
                if (preguntaID) {
                    d.preguntaID = preguntaID;
                }
            }
        },
        "columns": [
            { "data": "CodigoPeligro" },
            { "data": "Descripcion" },
            { "data": "CodigoOrientacion" },
            {
                "data": "OrientacionID", "render": function (data, type, row, meta) {
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

    // Evento cambio de lista
    $("#ddlListas").change(function () {
        var listaID = $(this).val();
        $("#btnNuevo").prop("disabled", true);
        $("#ddlSubtitulos").prop("disabled", true);
        $("#ddlPreguntas").prop("disabled", true);

        if (listaID) {
            cargarSubtitulos(listaID);
        } else {
            limpiarCombos(["#ddlSubtitulos", "#ddlPreguntas"]);
            tabladata.clear().draw();
        }
    });

    // Evento cambio de subtítulo
    $("#ddlSubtitulos").change(function () {
        var subtituloID = $(this).val();
        $("#btnNuevo").prop("disabled", true);
        $("#ddlPreguntas").prop("disabled", true);

        if (subtituloID) {
            cargarPreguntas(subtituloID);
        } else {
            limpiarCombos(["#ddlPreguntas"]);
            tabladata.clear().draw();
        }
    });

    // Evento click en buscar
    $("#btnBuscar").click(function () {
        var preguntaID = $("#ddlPreguntas").val();

        if (!preguntaID) {
            Swal.fire("Atención", "Debe seleccionar una pregunta", "warning");
            return;
        }

        var url = $.MisUrls.url._ObtenerOrientacionesPorIdPregunta + "?PreguntaID=" + preguntaID;
        tabladata.ajax.url(url).load();
        $("#btnNuevo").prop("disabled", false);
    });
});

function cargarListasDeVerificacion() {
    $.ajax({
        url: $.MisUrls.url._ObtenerListaVerificacionTodos,
        type: 'GET',
        success: function (response) {
            const ddl = $("#ddlListas");
            ddl.empty().append('<option value="">-- Seleccione una lista --</option>');
            $.each(response.data, function (i, item) {
                ddl.append(`<option value="${item.ListaID}" data-nombre="${item.Nombre}">${item.Nombre}</option>`);
            });
        }
    });
}

function cargarSubtitulos(listaID) {
    const ddl = $("#ddlSubtitulos");
    ddl.empty().append('<option value="">-- Seleccione un subtítulo --</option>').prop("disabled", false);
    $("#ddlPreguntas").prop("disabled", true);

    $.ajax({
        url: $.MisUrls.url._ObtenerSubtitulosPorListaId,
        type: 'GET',
        data: { ListaID: listaID },
        success: function (response) {
            $.each(response.data, function (i, item) {
                ddl.append(`<option value="${item.SubtituloID}" data-nombre="${item.Nombre}">${item.Nombre}</option>`);
            });
        }
    });
}

function cargarPreguntas(subtituloID) {
    const ddl = $("#ddlPreguntas");
    ddl.empty().append('<option value="">-- Seleccione una pregunta --</option>').prop("disabled", false);

    $.ajax({
        url: $.MisUrls.url._ObtenerPreguntasPorSubtitulo,
        type: 'GET',
        data: { SubtituloID: subtituloID },
        success: function (response) {
            $.each(response.data, function (i, item) {
                ddl.append(`<option value="${item.PreguntaID}" data-descripcion="${item.Descripcion}">${item.Descripcion}</option>`);
            });
        }
    });
}

function limpiarCombos(selectors) {
    selectors.forEach(function (selector) {
        $(selector).empty().append('<option value="">-- Seleccione --</option>').prop("disabled", true);
    });
}

function abrirPopUpForm(row) {
    const idOrientacion = row.OrientacionID;
    $.ajax({
        url: $.MisUrls.url._ObtenerOrientacionPorID,
        type: 'GET',
        data: { idOrientacion: idOrientacion },
        success: function (response) {
            const data = response.data;

            if (!data) {
                Swal.fire("Error", "No se pudo cargar la información", "error");
                return;
            }

            // Datos simples
            $("#txtid").val(data.OrientacionID);
            $("#txtPreguntaID").val(data.PreguntaID);
            $("#txtCodigoOrientacion").val(data.CodigoOrientacion || "");
            $("#txtCodigoPeligro").val(data.CodigoPeligro || "");
            $("#txtNombre").val(data.Nombre || "");
            $("#txtDescripcion").val(data.Descripcion || "");

            // Jerarquía XML
            if (data.oPregunta) {
                $("#txtPreguntaDescripcion").val(data.oPregunta.Descripcion || "");

                if (data.oPregunta.oSubtitulo) {
                    $("#txtSubtituloNombre").val(data.oPregunta.oSubtitulo.Nombre || "");

                    if (data.oPregunta.oSubtitulo.oListaVerificacion) {
                        $("#txtListaNombre").val(data.oPregunta.oSubtitulo.oListaVerificacion.Nombre || "");
                    }
                }
            }

            $('#FormModal').modal('show');
        },
        error: function () {
            Swal.fire("Error", "No se pudo obtener la orientación desde el servidor", "error");
        }
    });
}


function abrirModalCrearOrientacion() {
    $("#crearPreguntaID").val($("#ddlPreguntas").val());
    $("#crearListaNombre").val($("#ddlListas option:selected").text());
    $("#crearSubtituloNombre").val($("#ddlSubtitulos option:selected").text());
    $("#crearPreguntaDescripcion").val($("#ddlPreguntas option:selected").text());
    $("#crearCodigoOrientacion, #crearCodigoPeligro, #crearNombre, #crearDescripcion").val("");

    $('#ModalCrearOrientacion').modal('show');
}

function guardarNuevaOrientacion() {
    var request = {
        PreguntaID: $("#crearPreguntaID").val(),
        CodigoOrientacion: $("#crearCodigoOrientacion").val(),
        CodigoPeligro: $("#crearCodigoPeligro").val(),
        Nombre: $("#crearNombre").val(),
        Descripcion: $("#crearDescripcion").val()
    };

    $.ajax({
        url: $.MisUrls.url._GuardarOrientacion,
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json",
        success: function (data) {
            if (data.resultado === 1) {
                $('#ModalCrearOrientacion').modal('hide');
                tabladata.ajax.reload();
                Swal.fire("¡Éxito!", "Orientación registrada correctamente", "success");
            } else if (data.resultado === 2) {
                Swal.fire("Código duplicado", "Ya existe una orientación con ese código para esta pregunta", "warning");
            } else {
                Swal.fire("Error", "No se pudo registrar la orientación", "error");
            }
        },
        error: function () {
            Swal.fire("Error", "Hubo un problema al registrar la orientación", "error");
        }
    });
}

function Guardar() {
    if (!$("#form").valid()) return;

    var request = {
        OrientacionID: $("#txtid").val(),
        PreguntaID: $("#txtPreguntaID").val(),
        CodigoOrientacion: $("#txtCodigoOrientacion").val(),
        CodigoPeligro: $("#txtCodigoPeligro").val(),
        Nombre: $("#txtNombre").val(),
        Descripcion: $("#txtDescripcion").val()
    };

    $.ajax({
        url: $.MisUrls.url._GuardarOrientacion,
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json",
        success: function (data) {
            if (data.resultado === 1) {
                $('#FormModal').modal('hide');
                tabladata.ajax.reload();
                Swal.fire("¡Guardado!", "Orientación modificada correctamente", "success");
            } else if (data.resultado === 2) {
                Swal.fire("Código duplicado", "Ya existe una orientación con ese código para esta pregunta", "warning");
            } else {
                Swal.fire("Error", "No se pudo guardar la orientación", "error");
            }
        },
        error: function () {
            Swal.fire("Error", "Hubo un problema al guardar la orientación", "error");
        }
    });
}


function eliminar(id) {
    Swal.fire({
        title: "Orientaciones",
        text: "¿Desea eliminar el registro seleccionado?",
        icon: "warning",
        showDenyButton: true,
        confirmButtonText: "Sí",
        denyButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: $.MisUrls.url._EliminarOrientacion + "?id=" + id,
                type: "GET",
                success: function (data) {
                    if (data.resultado) {
                        tabladata.ajax.reload();
                        Swal.fire("Eliminado", "La orientación ha sido eliminada", "success");
                    } else {
                        Swal.fire("Error", "No se pudo eliminar la orientación", "error");
                    }
                },
                error: function () {
                    Swal.fire("Error", "Hubo un problema al eliminar la orientación", "error");
                }
            });
        }
    });
}