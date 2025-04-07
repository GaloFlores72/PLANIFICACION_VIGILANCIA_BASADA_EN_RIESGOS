//Editado
var tabladata;

$(document).ready(function () {
    // Cargar la lista de proveedores al iniciar la página
    cargarTipoProveedor();

    // Validación del formulario
    $("#form").validate({
        rules: {
            Nombre: "required",
            Descripcion: "required"
        },
        messages: {
            Nombre: "(*)",
            Descripcion: "(*)"
        },
        errorElement: 'span'
    });

    // Inicializar la tabla vacía
    tabladata = $('#tbdata').DataTable({
        "data": [], // Inicialmente vacía
        "columns": [
            { "data": "Nombre" },
            { "data": "Descripcion" },
            { "data": "DescripcionTipoProveedor" },
            {
                "data": "Estado", "render": function (data) {
                    return data
                        ? '<span class="badge badge-success">Activo</span>'
                        : '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                "data": "ListaID", "render": function (data, type, row, meta) {
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

    // Asignar evento de clic al botón "Buscar"
    $("#btnBuscar").click(function () {
        filtrarListaVerificacion();
    });

    // Asignar evento de clic al botón "Guardar"
    $("#btnGuardarCambios").click(function () {
        guardarListaVerificacion();
    });
});

function abrirPopUpForm(json) {
    console.log("Datos recibidos:", json);

    $("#txtid").val(0);

    if (json != null) {
        $("#txtid").val(json.ListaID);
        $("#txtNombre").val(json.Nombre);
        $("#txtDescripcion").val(json.Descripcion);
        $("#ddlTipoProveedor").val(json.IdTipoProveedorServicio).change();
        $("#cboEstado").val(json.Estado ? "1" : "0");
    } else {
        $("#txtNombre").val("");
        $("#txtDescripcion").val("");
        $("#ddlTipoProveedor").val("").change(); // Reiniciar selección del proveedor
        $("#cboEstado").val("1");
    }

    // Mostrar el modal
    $('#FormModal').modal('show');
}


function cargarTipoProveedor() {
    $.ajax({
        url: $.MisUrls.url._ObtenerTipoProveedorServicio,
        type: 'GET',
        success: function (response) {
            console.log("Respuesta del servidor:", response);

            var ddlPopup = $("#ddlTipoProveedor");
            var ddlProveedor = $("#cboProveedor");

            ddlPopup.empty();
            ddlProveedor.empty();

            ddlPopup.append('<option value="">-- Seleccione un Tipo de Proveedor --</option>');
            ddlProveedor.append('<option value="">-- Seleccione un Tipo de Proveedor --</option>');

            if (response.data && Array.isArray(response.data) && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    var option = '<option value="' + item.IdTipoProveedor + '">' + item.DescripcionTipoProveedor + '</option>';
                    ddlPopup.append(option);
                    ddlProveedor.append(option);
                });
            } else {
                console.log("No se encontraron proveedores.");
            }
        },
        error: function (err) {
            console.error("Error al cargar los proveedores:", err);
        }
    });
}


function filtrarListaVerificacion() {
    var idProveedor = $("#cboProveedor").val(); // Obtener el proveedor seleccionado

    $.ajax({
        url: $.MisUrls.url._ObtenerListaVerificacionPorProveedor,
        type: 'GET',
        data: { IdTipoProveedor: idProveedor },
        success: function (response) {
            console.log("Datos filtrados recibidos:", response);

            // Limpiar la tabla antes de agregar nuevos datos
            tabladata.clear().draw();

            if (response.data && Array.isArray(response.data) && response.data.length > 0) {
                tabladata.rows.add(response.data).draw(); // Agregar datos filtrados
            } else {
                console.log("No se encontraron registros.");
            }
        },
        error: function (err) {
            console.error("Error al cargar la lista de verificación:", err);
        }
    });
}

function guardarListaVerificacion() {
    if (!$("#form").valid()) {
        return;
    }

    var request = {
        ListaID: parseInt($("#txtid").val()) || 0,
        Nombre: $("#txtNombre").val().trim(),
        Descripcion: $("#txtDescripcion").val().trim(),
        IdTipoProveedorServicio: parseInt($("#ddlTipoProveedor").val()),
        Estado: $("#cboEstado").val() == "1" ? true : false
    };

    console.log("Enviando datos:", request); // Debug para revisar en la consola

    $.ajax({
        url: $.MisUrls.url._GuardarListaVerificacion,
        type: 'POST',
        data: JSON.stringify(request),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("Respuesta del servidor:", response);
            if (response.resultado) {
                $('#FormModal').modal('hide');
                Swal.fire("Éxito", "La lista de verificación ha sido guardada correctamente.", "success");
                filtrarListaVerificacion();
            } else {
                Swal.fire("Error", "No se pudo guardar la lista de verificación.", "error");
            }
        },
        error: function (err) {
            console.error("Error en la petición AJAX:", err);
            Swal.fire("Error", "Ocurrió un error en la conexión con el servidor.", "error");
        }
    });
}


function eliminar(id) {
    Swal.fire({
        title: "¿Está seguro?",
        text: "Esta acción no se puede revertir.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: $.MisUrls.url._EliminarListaVerificacion + "?id=" + id,
                type: 'GET',
                success: function (response) {
                    if (response.resultado) {
                        Swal.fire("Eliminado", "La lista de verificación ha sido eliminada.", "success");
                        filtrarListaVerificacion();
                    } else {
                        Swal.fire("Error", "No se pudo eliminar la lista de verificación.", "error");
                    }
                },
                error: function (err) {
                    console.error("Error al eliminar la lista de verificación:", err);
                }
            });
        }
    });
}
