var tabladata;

$(document).ready(function () {
    $("#form").validate({
        rules: {
            Descripcion: "required"
        },
        messages: {
            Descripcion: "(*)"
        },
        errorElement: 'span'
    });

    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerTipoProveedorServicio, // URL correcta desde _Layout.cshtml
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "DescripcionTipoProveedor" },
            {
                "data": "Estado",
                "render": function (data) {
                    return data
                        ? '<span class="badge badge-success">Activo</span>'
                        : '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                "data": "IdTipoProveedor",
                "render": function (data, type, row, meta) {
                    return "<button class='btn btn-primary btn-sm' type='button' onclick='abrirPopUpForm(" + JSON.stringify(row) + ")'><i class='fas fa-pen'></i></button>" +
                        "<button class='btn btn-danger btn-sm ml-2' type='button' onclick='eliminar(" + data + ")'><i class='fa fa-trash'></i></button>";
                },
                "orderable": false,
                "searchable": false,
                "width": "90px"
            }
        ],
        "language": {
            "url": $.MisUrls.url.Url_datatable_spanish
        },
        responsive: true
    });

});

// Función para abrir el modal con los datos cargados
function abrirPopUpForm(json) {
    $("#txtid").val(0);

    if (json != null) {
        $("#txtid").val(json.IdTipoProveedor);
        $("#txtDescripcion").val(json.DescripcionTipoProveedor);
        $("#cboEstado").val(json.Estado ? 1 : 0);
    } else {
        $("#txtDescripcion").val("");
        $("#cboEstado").val(1);
    }

    $('#FormModal').modal('show');
}

// Función para guardar o actualizar un tipo de proveedor
function Guardar() {
    if ($("#form").valid()) {
        var request = {
            IdTipoProveedor: $("#txtid").val() ? parseInt($("#txtid").val()) : 0,
            DescripcionTipoProveedor: $("#txtDescripcion").val(),
            Estado: $("#cboEstado").val() == "1"
        };

        jQuery.ajax({
            url: $.MisUrls.url._GuardarTipoProveedor, // URL correcta desde _Layout.cshtml
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    Swal.fire("Éxito", "Registro guardado correctamente", "success");
                    tabladata.ajax.reload();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire("Error", "No se pudo guardar los cambios", "warning");
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}

// Función para eliminar un tipo de proveedor
function eliminar(id) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "No podrás revertir esto",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            jQuery.ajax({
                url: $.MisUrls.url._EliminarTipoProveedor, // URL correcta desde _Layout.cshtml
                type: "POST",
                data: JSON.stringify({ id: id }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.resultado) {
                        Swal.fire("Eliminado", "El proveedor ha sido eliminado.", "success");
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire("Error", "No se pudo eliminar el proveedor", "warning");
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
}
