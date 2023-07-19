
var AgregarModal = new bootstrap.Modal(document.getElementById('AgregarModal'), {
    keyboard: false
})
var AgregarEscalaModal = new bootstrap.Modal(document.getElementById('ModalAgregarEscala'), {
    keyboard: false
})

$("#btnAgregar").on("click", function () {
   
    AgregarModal.toggle();
});
$("#btnAgregarEscala").on("click", function () {
    $("#inputAgregarEscalaDescripcion").val("");
    $("#inputAgregarEscalaMin").val("");
    $("#inputAgregarEscalaMax").val("");
    AgregarEscalaModal.toggle();
});

$("#btnGuardarEscala").on("click", function () {

    var desc = $("#inputAgregarEscalaDescripcion").val();
    var min = $("#inputAgregarEscalaMin").val();
    var max = $("#inputAgregarEscalaMax").val();
    if (!desc) {
        alert("Debes agregar una descripción");
        return;
    }
    if (!min) {
        alert("Debes agregar un mínimo");
        return;
    }
    if (!max) {
        alert("Debes agregar un máximo");
        return;
    }

    $.ajax({
        url: '/Indicadores/AgregarEscala',
        data: {
            descripcion: desc,
            min: min,
            max: max
        },
        method: 'POST',
        success: function (data) {
            $("#selectAgregarEscala").append("<option value='" + data + "'>" + desc + "</option>");
            AgregarEscalaModal.toggle();
        },
        error: function (e) {
            alert("No se pudo crear la escala");
            AgregarEscalaModal.toggle();
        }
        })

   
});
//var EliminarModal = new bootstrap.Modal(document.getElementById('EliminarModal'), {
//    keyboard: false
//})

//var ModificarModal = new bootstrap.Modal(document.getElementById('ModificarModal'), {
//    keyboard: false
//})

//$(".modificarGrupo").on("click", function () {
//    var id = $(this).data("id");
//    var grupo = $(this).data("grupo");

//    $("#inputModificarId").val("");
//    $("#inputModificarGrupo").val("");
//    $("#inputModificarId").val(id);
//    $("#inputModificarGrupo").val(grupo);

//    ModificarModal.toggle();

//});



//$(".eliminarGrupo").on("click", function () {
//    var id = $(this).data("id");
//    var grupo = $(this).data("grupo");

//    $("#inputEliminarId").val("");
//    $("#inputEliminarGrupo").val("");
//    $("#inputEliminarId").val(id);
//    $("#inputEliminarGrupo").val(grupo);

//    EliminarModal.toggle();

//});