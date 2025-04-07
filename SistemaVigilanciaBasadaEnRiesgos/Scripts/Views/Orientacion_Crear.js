var tabladata;

$(document).ready(function () {
    cargarListasDeVerificacion();

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

    $("#ddlListas").change(function () {
        var listaID = $(this).val();
        if (listaID) {
            tabladata.ajax.url($.MisUrls.url._ObtenerOrientacion + "?ListaID=" + listaID).load();
            cargarSubtitulos(listaID);
        } else {
            tabladata.ajax.url($.MisUrls.url._ObtenerOrientacion).load();
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
            tabladata.ajax.url($.MisUrls.url._ObtenerOrientacionesPorIdPregunta + "?PreguntaID=" + preguntaID).load();

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
                console.error("No se encontraron subtítulos.");
            }
        },
    });
}

function cargarPreguntas(subtituloID) {
    if (!subtituloID) {
        console.log(preguntaID);
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

function abrirPopUpForm(json) {
    $("#txtid").val(0);

    if (json != null) {
        $("#txtid").val(json.OrientacionID);
        var preguntaID = json.PreguntaID;
        $("#txtPreguntaID").val(preguntaID);
        $("#txtCodigoPeligro").val(json.CodigoPeligro);
        $("#txtNombre").val(json.Nombre);
        $("#txtDescripcion").val(json.Descripcion);
    } else {
        var preguntaID = $("#ddlPreguntas").val();
        $("#txtPreguntaID").val(preguntaID);
        $("#txtCodigoPeligro").val("");
        $("#txtNombre").val("");
        $("#txtDescripcion").val("");
    }

    $('#FormModal').modal('show');
}

function Guardar() {
    if ($("#form").valid()) {
        var preguntaID = $("#txtPreguntaID").val()

        var request = {
            preguntaID: preguntaID,
            CodigoPeligro: $("#txtCodigoPeligro").val(),
            Nombre: $("#txtNombre").val(),
            Descripcion: $("#txtDescripcion").val()
        };

        if ($("#txtid").val() != "0") {
            request.OrientacionID = $("#txtid").val();
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarOrientacion,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log(data);
                if (data.resultado) {
                    var url = $.MisUrls.url._ObtenerOrientacionesPorIdPregunta + "?PreguntaID=" + preguntaID;
                    tabladata.ajax.url(url).load();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire("Orientacion", "No se pudo guardar los cambios", "warning");
                }
            },
            error: function (error) {
                console.log(error);
                Swal.fire("Error", "Hubo un problema al guardar pregunta", "error");
            }
        });
    }
}
function eliminar($id) {
    Swal.fire({
        title: "Orientaciones",
        text: "¿Desea eliminar el registro seleccionado?",
        icon: "warning",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "Sí",
        denyButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            jQuery.ajax({
                url: $.MisUrls.url._EliminarOrientacion + "?id=" + $id,
                type: "GET",
                success: function (data) {
                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire("Error", "No se pudo eliminar la orientación", "warning");
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
}
