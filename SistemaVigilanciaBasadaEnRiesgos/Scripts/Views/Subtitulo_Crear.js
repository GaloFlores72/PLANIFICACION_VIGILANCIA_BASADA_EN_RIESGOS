var tabladata;

$(document).ready(function () {
    cargarListasDeVerificacion();

    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerSubtituloTodos,
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
            { "data": "ListaNombre" }, 
            { "data": "Nombre" },
            {
                "data": "Estado", "render": function (data) {
                    if (data) {
                        return '<span class="badge badge-success">Activo</span>'
                    } else {
                        return '<span class="badge badge-danger">No Activo</span>'
                    }
                }
            },
            {
                "data": "SubtituloID", "render": function (data, type, row, meta) {
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
            $.ajax({
                url: $.MisUrls.url._ObtenerSubtitulosPorListaId,
                type: 'GET',
                data: { ListaID: listaID },
                success: function (response) {
                    if (response.data) {
                        tabladata.clear().rows.add(response.data).draw();
                    }
                }
            });
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


function cargarObtenerSubtitulosPorListaId(listaID) {
    var url = $.MisUrls.url._ObtenerSubtitulosPorListaId + '?ListaID=' + listaID + '?ListaNombre=' + listaNombre;
    tabladata.ajax.url(url).load();
}

function abrirPopUpForm(json) {
    $("#txtid").val(0);

    if (json != null) {
        $("#txtid").val(json.SubtituloID);
        var listaID = json.ListaID; 
        var listaNombre = json.ListaNombre; 
        $("#txtListaNombre").val(listaNombre);
        $("#txtListaID").val(listaID);
        $("#txtNombre").val(json.Nombre);
        $("#txtDescripcion").val(json.Descripcion);
        var valor = 1;
        valor = json.Estado == true ? 1 : 0
        $("#cboEstado").val(valor);
    } else {
        var listaID = $("#ddlListas").val();
        var listaNombre = $("#ddlListas option:selected").data("nombre");
        $("#txtListaNombre").val(listaNombre);
        $("#txtListaID").val(listaID);
        $("#txtNombre").val("");
        $("#txtDescripcion").val("");
        $("#cboEstado").val(1);
    }

    $('#FormModal').modal('show');
}



function Guardar() {
    if ($("#form").valid()) {
        var listaID = $("#txtListaID").val();

        var request = {
            listaID: listaID,
            Nombre: $("#txtNombre").val(),
            Descripcion: $("#txtDescripcion").val(),
            Estado: parseInt($("#cboEstado").val()) == 1 ? true : false
        };

        if ($("#txtid").val() != "0") {
            request.SubtituloID = $("#txtid").val();
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarSubtitulo, 
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    var url = $.MisUrls.url._ObtenerSubtitulosPorListaId + '?ListaID=' + listaID ;
                    tabladata.ajax.url(url).load();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire("Subtitulo", "No se pudo guardar los cambios", "warning");
                }
            },
            error: function (error) {
                console.log(error);
                Swal.fire("Error", "Hubo un problema al guardar el subtítulo", "error");
            }
        });
    }
}



function eliminar($id) {
    Swal.fire({
        title: "Subtitulos",
        text: "¿Desea eliminar el registro seleccionado?",
        type: "warning",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "SI",
        denyButtonText: "NO"
    }).then((result) => {
        if (result.isConfirmed) {
            jQuery.ajax({
                url: $.MisUrls.url._EliminarSubtitulo + "?id=" + $id,
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
