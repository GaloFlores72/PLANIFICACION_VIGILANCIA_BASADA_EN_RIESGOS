var tabladata;

$(document).ready(function () {
    cargarListasDeVerificacion();

    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerPreguntas,
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                var listaID = $("#ddlListas").val();
                if (listaID) {
                    d.listaID = listaID;
                }
            }
        },
        "columns": [
            { "data": "Referencia" },
            { "data": "Descripcion" },
            {
                "data": "Estado"
            },
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

    $("#ddlListas").change(function () {
        var listaID = $(this).val();
        if (listaID) {
            tabladata.ajax.url($.MisUrls.url._ObtenerPreguntas + "?ListaID=" + listaID).load();
            cargarSubtitulos(listaID);
        } else {
            tabladata.ajax.url($.MisUrls.url._ObtenerPreguntas).load();
        }
    });

    $("#ddlSubtitulos").change(function () {
        var subtituloID = $(this).val();
        if (subtituloID) {
            tabladata.ajax.url($.MisUrls.url._ObtenerPreguntasPorSubtitulo + "?subtituloID=" + subtituloID).load();
        }
        console.log(subtituloID);
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


function abrirPopUpForm(json) {
    $("#txtid").val(0);

    if (json != null) {
        $("#txtid").val(json.PreguntaID);
        var subtituloID = json.SubtituloID;
        $("#txtSubtituloID").val(subtituloID);
        $("#txtDescripcion").val(json.Descripcion);
        $("#txtReferencia").val(json.Referencia);
        $("#txtEstadisticas").val(json.Estadisticas);
        $("#cboEstado").val(json.Estado);
    } else {
        var subtituloID = $("#ddlSubtitulos").val();
        $("#txtSubtituloID").val(subtituloID);
        $("#txtDescripcion").val("");
        $("#txtReferencia").val("");
        $("#txtEstadisticas").val("");
        $("#cboEstado").val("");
    }

    $('#FormModal').modal('show');
}


function Guardar() {
    if ($("#form").valid()) {
        var subtituloID = $("#txtSubtituloID").val()

        var request = {
            subtituloID: subtituloID,
            Descripcion: $("#txtDescripcion").val(),
            Referencia: $("#txtReferencia").val(),
            Estadisticas: $("#txtEstadisticas").val(),
            Estado: $("#cboEstado").val()
        };

        if ($("#txtid").val() != "0") {
            request.PreguntaID = $("#txtid").val();
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarPregunta,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log(data);
                if (data.resultado) {
                    var url = $.MisUrls.url._ObtenerPreguntasPorSubtitulo + '?SubtituloID=' + subtituloID;
                    tabladata.ajax.url(url).load();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire("Pregunta", "No se pudo guardar los cambios", "warning");
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
        title: "Preguntas",
        text: "¿Desea eliminar el registro seleccionado?",
        type: "warning",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "SI",
        denyButtonText: "NO"
    }).then((result) => {
        if (result.isConfirmed) {
            jQuery.ajax({
                url: $.MisUrls.url._EliminarPregunta + "?id=" + $id,
                type: "GET",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire("Mensaje", "No se pudo eliminar el subtitulo", "warning")
                    }
                },
                error: function (error) {
                    console.log(error)
                }
            });
        }
    });
}