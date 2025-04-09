var tabladata;

$(document).ready(function () {
    // Deshabilita el botón Agregar desde el inicio
    $("#btnNuevo").prop("disabled", true);
    cargarListasDeVerificacion();

    // Validación en tiempo real para CódigoPregunta (crear y editar)
    $(document).on('input', '#crearCodigoPregunta, #txtCodigoPregunta', function () {
        const valor = $(this).val();
        const regex = /^[A-Z0-9\-]*$/;
        if (!regex.test(valor)) {
            Swal.fire("Carácter no permitido", "Solo se permiten letras Mayusculas, números y el guion (-). No se permiten espacios ni caracteres especiales.", "warning");
            $(this).val(valor.replace(/[^A-Z0-9\-]/g, ''));
        }
    });

    // Inicializa la tabla de preguntas
    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerPreguntas,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Referencia" },
            { "data": "Descripcion" },
            { "data": "CodigoPregunta" },
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

    // Carga subtítulos cuando se selecciona una lista
    $("#ddlListas").change(function () {
        var listaID = $(this).val();
        $("#btnNuevo").prop("disabled", true);

        if (listaID) {
            $("#ddlSubtitulos").prop("disabled", false);
            cargarSubtitulos(listaID);
        } else {
            $("#ddlSubtitulos").prop("disabled", true);
            $("#ddlSubtitulos").empty().append('<option value="">-- Seleccione un subtítulo --</option>');
        }
    });

    // Desactiva el botón Agregar cuando cambia subtítulo
    $("#ddlSubtitulos").change(function () {
        $("#btnNuevo").prop("disabled", true);
    });

    // Buscar preguntas por subtítulo
    $("#btnBuscar").click(function () {
        const listaID = $("#ddlListas").val();
        const subtituloID = $("#ddlSubtitulos").val();

        if (!listaID || !subtituloID) {
            Swal.fire("Atención", "Debe seleccionar Lista y Subtítulo", "warning");
            return;
        }

        const nuevaUrl = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + `?SubtituloID=${subtituloID}`;
        tabladata.ajax.url(nuevaUrl).load();
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
        },
        error: function (err) {
            console.error("Error al cargar listas:", err);
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
            $.each(response.data, function (i, item) {
                ddl.append(`<option value="${item.SubtituloID}" data-nombre="${item.Nombre}">${item.Nombre}</option>`);
            });
        },
        error: function (err) {
            console.error("Error al cargar subtítulos:", err);
        }
    });
}

function abrirModalCrearPregunta() {
    $("#crearListaNombre").val($("#ddlListas option:selected").text());
    $("#crearSubtituloNombre").val($("#ddlSubtitulos option:selected").text());
    $("#crearSubtituloID").val($("#ddlSubtitulos").val());
    $("#crearDescripcion, #crearReferencia, #crearCodigoPregunta").val("");
    $("#ModalCrearPregunta").modal("show");
}

function guardarNuevaPregunta() {
    const subtituloID = $("#crearSubtituloID").val();
    const descripcion = $("#crearDescripcion").val();
    const referencia = $("#crearReferencia").val();
    const codigo = $("#crearCodigoPregunta").val();

    if (!subtituloID || !codigo) {
        Swal.fire("Error", "Debe ingresar código y seleccionar subtítulo", "error");
        return;
    }

    const request = {
        SubtituloID: subtituloID,
        Descripcion: descripcion,
        Referencia: referencia,
        Estado: "No Satisfactorio",
        Estadisticas: 0,
        CodigoPregunta: codigo
    };

    $.ajax({
        url: $.MisUrls.url._GuardarPregunta,
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json",
        success: function (data) {
            if (data.resultado === 1) {
                const url = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + "?SubtituloID=" + subtituloID;
                tabladata.ajax.url(url).load();
                $("#ModalCrearPregunta").modal("hide");
                Swal.fire("¡Éxito!", "Pregunta guardada correctamente", "success");
            } else if (data.resultado === 2) {
                Swal.fire("Duplicado", "Ya existe ese código de pregunta", "warning");
            } else {
                Swal.fire("Error", "No se pudo guardar", "error");
            }
        },
        error: function (error) {
            console.error(error);
            Swal.fire("Error", "Error al guardar", "error");
        }
    });
}

function abrirPopUpForm(row) {
    const idPregunta = row.PreguntaID;
    $.ajax({
        url: $.MisUrls.url._ObtenerPreguntaPorId,
        type: 'GET',
        data: { idPregunta: idPregunta },
        success: function (response) {
            const pregunta = response.data;
            if (!pregunta) {
                Swal.fire("Error", "No se pudo cargar la información", "error");
                return;
            }
            $("#txtid").val(pregunta.PreguntaID);
            $("#txtSubtituloID").val(pregunta.SubtituloID);
            $("#txtDescripcion").val(pregunta.Descripcion);
            $("#txtReferencia").val(pregunta.Referencia);
            $("#txtCodigoPregunta").val(pregunta.CodigoPregunta);
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
            Swal.fire("Error", "Error al obtener los datos", "error");
        }
    });
}

function Guardar() {
    if (!$("#form").valid()) return;
    const subtituloID = $("#txtSubtituloID").val();
    const request = {
        SubtituloID: subtituloID,
        Descripcion: $("#txtDescripcion").val(),
        Referencia: $("#txtReferencia").val(),
        CodigoPregunta: $("#txtCodigoPregunta").val(),
        Estado: "No Satisfactorio",
        Estadisticas: 0
    };
    if ($("#txtid").val() != "0") {
        request.PreguntaID = $("#txtid").val();
    }

    $.ajax({
        url: $.MisUrls.url._GuardarPregunta,
        type: "POST",
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.resultado === 1) {
                const url = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + '?SubtituloID=' + subtituloID;
                tabladata.ajax.url(url).load();
                $('#FormModal').modal('hide');
                Swal.fire("¡Editado!", "Los cambios se guardaron correctamente", "success");
            } else if (data.resultado === 2) {
                Swal.fire("Duplicado", "Ya existe ese código de pregunta", "warning");
            } else {
                Swal.fire("Error", "No se pudo guardar los cambios", "error");
            }
        },
        error: function (error) {
            console.log(error);
            Swal.fire("Error", "Hubo un problema al guardar la pregunta", "error");
        }
    });
}

function eliminar(id) {
    Swal.fire({
        title: "Preguntas",
        text: "¿Desea eliminar el registro seleccionado?",
        icon: "warning",
        showDenyButton: true,
        confirmButtonText: "SI",
        denyButtonText: "NO"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: $.MisUrls.url._EliminarPregunta + "?id=" + id,
                type: "GET",
                success: function (data) {
                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire("Mensaje", "No se pudo eliminar el registro", "warning");
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
}
