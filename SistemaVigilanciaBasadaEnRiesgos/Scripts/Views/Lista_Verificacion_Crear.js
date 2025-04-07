var tabladata;
$(document).ready(function () {
    //activarMenu("Mantenedor");
    cargarTipoProveedor()


    ////validamos el formulario
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

    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerListaVerificacionTodos,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Nombre" },
            { "data": "Descripcion" },
            { "data": "DescripcionTipoProveedor" },
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
                "data": "ListaID", "render": function (data, type, row, meta) {
                    return "<button class='btn btn-primary btn-sm' type='button' onclick='abrirPopUpForm(" + JSON.stringify(row) + ")'><i class='fas fa-pen'></i></button>" +
                        "<button class='btn btn-danger btn-sm ml-2' type='button' onclick='eliminar(" + data + ")'><i class='fa fa-trash'></i></button>"
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


})

function cargarTipoProveedor() {  
    $.ajax({
        url: $.MisUrls.url._ObtenerTipoProveedorServicio,
        type: 'GET',
        success: function (response) {
            console.log("Respuesta del servidor:", response);

            var ddl = $("#ddlTipoProveedor");
            ddl.empty();
            ddl.append('<option value="">-- Seleccione un Tipo de Proveedor --</option>');

            if (response.data && Array.isArray(response.data) && response.data.length > 0) {
                $.each(response.data, function (index, item) {
                    ddl.append('<option value="' + item.IdTipoProveedor + '">' + item.DescripcionTipoProveedor + '</option>');
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

        $("#txtid").val(json.ListaID);

        $("#txtNombre").val(json.Nombre);
        $("#txtDescripcion").val(json.Descripcion);
        var IdTipoProveedor = $("#ddlTipoProveedor").val();
        $("#txtIdTipoProveedor").val(IdTipoProveedor);
        var valor = 0;
        valor = json.Activo == true ? 1 : 0
        $("#cboEstado").val(valor);

    } else {
        $("#txtNombre").val("");
        $("#txtDescripcion").val("");
        var IdTipoProveedor = $("#ddlTipoProveedor").val();
        $("#txtIdTipoProveedor").val(IdTipoProveedor);
        $("#cboEstado").val(1);
    }

    $('#FormModal').modal('show');

}

function Guardar() {
    if ($("#form").valid()) {
        var IdTipoProveedorServicio = $("#ddlTipoProveedor").val();

        if (!IdTipoProveedorServicio) {
            Swal.fire("Lista Verificación", "Por favor, seleccione un tipo de proveedor.", "warning");
            return;
        }

        var request = {
            objeto: {
                ListaID: $("#txtid").val(),
                Nombre: $("#txtNombre").val(),
                Descripcion: $("#txtDescripcion").val(),
                IdTipoProveedorServicio: IdTipoProveedorServicio,
                Estado: parseInt($("#cboEstado").val()) == 1 ? true : false
            }
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarListaVerificacion,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.resultado) {
                    tabladata.ajax.reload();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire("Lista Verificación", "No se pudo guardar los cambios", "warning");
                }
            },
            error: function (error) {
                console.log(error);
            },
            beforeSend: function () {
            },
        });
    }
}


function eliminar($id) {
    Swal.fire({
        title: "Lista Verificacion",
        text: "¿Desea eliminar el registro seleccionado?",
        type: "warning",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "SI",
        denyButtonText: "NO"
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            jQuery.ajax({
                url: $.MisUrls.url._EliminarListaVerificacion + "?id=" + $id,
                type: "GET",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire("Mensaje", "No se pudo eliminar la lista de verificación", "warning")
                    }
                },
                error: function (error) {
                    console.log(error)
                },
                beforeSend: function () { },
            });
        }
    });
}
