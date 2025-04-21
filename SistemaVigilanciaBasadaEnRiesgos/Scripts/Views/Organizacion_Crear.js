let tabladata;

$(document).ready(function () {

    tabladata = $('#tablaVisualizacion').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerOrganizaciones,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Nombre" },
            { "data": "Direccion" },
            { "data": "Correo" },
            { "data": "Telefono" },
            {
                "data": "OrganizacionID",
                "render": function (data, type, row) {
                    return `
                        <button class='btn btn-primary btn-sm' type='button' onclick='abrirPopUpEditar(${data})'><i class='fas fa-pen'></i></button>
                        <button class='btn btn-danger btn-sm ml-2' type='button' onclick='eliminar(${data})'><i class='fa fa-trash'></i></button>
                    `;
                },
                "orderable": false,
                "searchable": false
            }
        ],
        "language": {
            "url": $.MisUrls.urls.Url_datatable_spanish
        },
        responsive: true
    });

    // Nuevo
    $("#btnNuevo").click(function () {
        limpiarModalNuevo();
        $("#formNuevoRegistro").modal("show");
    });

    // Guardar nuevo
    $("#btnGuardarNuevo").click(function () {
        if (!validarFormulario("nuevo")) return;

        let entidad = {
            OrganizacionID: 0,
            Nombre: $("#nuevoNombre").val(),
            Direccion: $("#nuevoDireccion").val(),
            GerenteResponsable: $("#nuevoGerente").val(),
            NCertificadoOMA: $("#nuevoOMA").val(),
            Correo: $("#nuevoCorreo").val(),
            Telefono: $("#nuevoTelefono").val()
        };

        $.ajax({
            url: $.MisUrls.url._GuardarOrganizacion,
            type: "POST",
            data: JSON.stringify(entidad),
            contentType: "application/json; charset=utf-8",
            success: function (resp) {
                if (resp.resultado === 1) {
                    Swal.fire("¡Éxito!", "Organización registrada correctamente", "success");
                    $("#formNuevoRegistro").modal("hide");
                    tabladata.ajax.reload();
                } else {
                    Swal.fire("Error", "No se pudo registrar", "error");
                }
            }
        });
    });

    // Guardar edición
    $("#btnGuardarEditar").click(function () {
        if (!validarFormulario("editar")) return;

        let entidad = {
            OrganizacionID: $("#editarID").val(),
            Nombre: $("#editarNombre").val(),
            Direccion: $("#editarDireccion").val(),
            GerenteResponsable: $("#editarGerente").val(),
            NCertificadoOMA: $("#editarOMA").val(),
            Correo: $("#editarCorreo").val(),
            Telefono: $("#editarTelefono").val()
        };

        $.ajax({
            url: $.MisUrls.url._GuardarOrganizacion,
            type: "POST",
            data: JSON.stringify(entidad),
            contentType: "application/json; charset=utf-8",
            success: function (resp) {
                if (resp.resultado === 1) {
                    Swal.fire("¡Éxito!", "Organización actualizada correctamente", "success");
                    $("#formEditarRegistro").modal("hide");
                    tabladata.ajax.reload();
                } else {
                    Swal.fire("Error", "No se pudo actualizar", "error");
                }
            }
        });
    });

    // Validaciones en tiempo real
    inicializarValidacionesCampo("nuevo");
    inicializarValidacionesCampo("editar");
});

// Editar
function abrirPopUpEditar(id) {
    $.ajax({
        url: $.MisUrls.url._ObtenerOrganizacionPorId,
        type: "GET",
        data: { idOrganizacion: id },
        success: function (resp) {
            const o = resp.data;
            $("#editarID").val(o.OrganizacionID);
            $("#editarNombre").val(o.Nombre);
            $("#editarDireccion").val(o.Direccion);
            $("#editarGerente").val(o.GerenteResponsable);
            $("#editarOMA").val(o.NCertificadoOMA);
            $("#editarCorreo").val(o.Correo);
            $("#editarTelefono").val(o.Telefono);

            limpiarErrores("editar");
            $("#formEditarRegistro").modal("show");
        }
    });
}

// Eliminar
function eliminar(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: "Esta acción no se puede deshacer",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: $.MisUrls.url._EliminarOrganizacion,
                type: "POST",
                data: { id: id },
                success: function (resp) {
                    if (resp.resultado === 1) {
                        Swal.fire("Eliminado", "Organización eliminada con éxito", "success");
                    } else if (resp.resultado === 2) {
                        Swal.fire("Advertencia", "No se puede eliminar. Existen datos relacionados.", "warning");
                    } else {
                        Swal.fire("Error", "No se pudo eliminar", "error");
                    }
                    tabladata.ajax.reload();
                }
            });
        }
    });
}

// Validar formulario antes de enviar
function validarFormulario(prefijo) {
    let valido = true;

    $(`#${prefijo}Nombre, #${prefijo}Direccion, #${prefijo}Gerente, #${prefijo}OMA`).each(function () {
        if ($(this).val().trim() === "") {
            $(this).addClass("is-invalid");
            valido = false;
        } else {
            $(this).removeClass("is-invalid");
        }
    });

    const correo = $(`#${prefijo}Correo`);
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(correo.val())) {
        correo.addClass("is-invalid");
        valido = false;
    } else {
        correo.removeClass("is-invalid");
    }

    const telefono = $(`#${prefijo}Telefono`);
    if (telefono.val() && !/^\d+$/.test(telefono.val())) {
        telefono.addClass("is-invalid");
        valido = false;
    } else {
        telefono.removeClass("is-invalid");
    }

    return valido;
}

// Validaciones en blur
function inicializarValidacionesCampo(prefijo) {
    $(`#${prefijo}Correo`).on("blur", function () {
        const email = $(this).val();
        const isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
        $(this).toggleClass("is-invalid", !isValid);
    });

    $(`#${prefijo}Telefono`).on("blur", function () {
        const value = $(this).val();
        const isValid = value === "" || /^[\d\s\-]+$/.test(value); 
        $(this).toggleClass("is-invalid", !isValid);
    });

    $(`#${prefijo}Nombre, #${prefijo}Direccion, #${prefijo}Gerente, #${prefijo}OMA`).on("blur", function () {
        const isValid = $(this).val().trim() !== "";
        $(this).toggleClass("is-invalid", !isValid);
    });
}

// Limpieza modal
function limpiarModalNuevo() {
    $("#formNuevo input").val("").removeClass("is-invalid");
}

function limpiarErrores(prefijo) {
    $(`#formEditarRegistro input`).removeClass("is-invalid");
}
