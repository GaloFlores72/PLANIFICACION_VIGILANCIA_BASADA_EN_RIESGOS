
var tabladata;
$(document).ready(function () {
    //activarMenu("Mantenedor");


    ////validamos el formulario
    $("#form").validate({
        rules: {
            Codigo: "required",
            Nombres: "required",
            Apellidos: "required",
            Correo: "required",
            Clave: "required"
        },
        messages: {
            Codigo: "(*)",
            Nombres: "(*)",
            Apellidos: "(*)",
            Correo: "(*)",
            Clave: "(*)"
        },
        errorElement: 'span'
    });

    //OBTENER ROLES
    jQuery.ajax({
        url: $.MisUrls.url._ObtenerRoles,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            $("#cboRol").html("");

            if (data.data != null) {
                $.each(data.data, function (i, item) {

                    if (item.Activo == true) {
                        $("<option>").attr({ "value": item.IdRol }).text(item.DescripcionRol).appendTo("#cboRol");
                    }
                })
                $("#cboRol").val($("#cboRol option:first").val());
            }

        },
        error: function (error) {
            console.log(error)
        },
        beforeSend: function () {
        },
    });

    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerUsuarios,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "oRol", render: function (data) {
                    return data.DescripcionRol
                }
            },
            { "data": "CodigoUsuario" },
            { "data": "Nombres" },
            { "data": "Apellidos" },
            { "data": "Correo" },
            {
                "data": "Activo", "render": function (data) {
                    if (data) {
                        return '<span class="badge badge-success">Activo</span>'
                    } else {
                        return '<span class="badge badge-danger">No Activo</span>'
                    }
                }
            },
            {
                "data": "IdUsuario", "render": function (data, type, row, meta) {
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



function abrirPopUpForm(json) {

    $("#txtid").val(0);

    if (json != null) {

        $("#txtid").val(json.IdUsuario);

        $("#txtCodigo").val(json.CodigoUsuario);
        $("#txtNombres").val(json.Nombres);
        $("#txtApellidos").val(json.Apellidos);
        $("#txtCorreo").val(json.Correo);
        $("#txtClave").val(json.Clave);
        $("#cboTienda").val(json.IdTienda);
        $("#cboRol").val(json.IdRol);
        $("#cboEstado").val(json.Activo == true ? 1 : 0);
        $("#txtClave").prop("disabled", true);

    } else {
        $("#txtCodigo").val("");
        $("#txtNombres").val("");
        $("#txtApellidos").val("");
        $("#txtCorreo").val("");
        $("#txtClave").val("");
        $("#cboTienda").val($("#cboTienda option:first").val());
        $("#cboRol").val($("#cboRol option:first").val());
        $("#cboEstado").val(1);

        $("#txtClave").prop("disabled", false);
    }

    $('#FormModal').modal('show');

}


function Guardar() {

    if ($("#form").valid()) {

        var request = {
            objeto: {
                IdUsuario: $("#txtid").val(),
                CodigoUsuario: $("#txtCodigo").val(),
                Nombres: $("#txtNombres").val(),
                Apellidos: $("#txtApellidos").val(),
                Correo: $("#txtCorreo").val(),
                Clave: $("#txtClave").val(),
                IdRol: $("#cboRol").val(),
                Activo: parseInt($("#cboEstado").val()) == 1 ? true : false
            }
        }

        jQuery.ajax({
            url: $.MisUrls.url._GuardarUsuario,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.resultado) {
                    tabladata.ajax.reload();
                    $('#FormModal').modal('hide');
                } else {
                    Swal.fire({
                        title: "Mensaje",
                        text: "No se pudo guardar los cambios!",
                        icon: "warning"
                    });
                }
            },
            error: function (error) {
                console.log(error)
            },
            beforeSend: function () {

            },
        });

    }

}


function eliminar($id) {


    Swal.fire({
        title: "Mensaje",
        text: "¿Desea eliminar el usuario seleccionado?",
        type: "warning",
        showCancelButton: true,

        confirmButtonText: "Si",
        confirmButtonColor: "#DD6B55",

        cancelButtonText: "No",

        closeOnConfirm: true
    },

        function () {
            jQuery.ajax({
                url: $.MisUrls.url._EliminarUsuario + "?id=" + $id,
                type: "GET",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (data.resultado) {
                        tabladata.ajax.reload();
                    } else {
                        Swal.fire({
                            title: "Mensaje",
                            text: "No se pudo guardar los cambios!",
                            icon: "warning"
                        });
                    }
                },
                error: function (error) {
                    console.log(error)
                },
                beforeSend: function () {

                },
            });
        });

}