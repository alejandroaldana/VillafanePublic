
var ModificarModal = new bootstrap.Modal(document.getElementById('ModificarModal'), {
    keyboard: false
})
var AgregarModal = new bootstrap.Modal(document.getElementById('AgregarModal'), {
    keyboard: false
})
var EliminarModal = new bootstrap.Modal(document.getElementById('EliminarModal'), {
    keyboard: false
})

$(".modificarPeriodo").on("click", function () {
    var id = $(this).data("id");
    var periodo = $(this).data("periodo");
    var orden = $(this).data("orden");

    $("#inputModificarId").val("");
    $("#inputModificarPeriodo").val("");
    $("#inputModificarOrden").val("");
    $("#inputModificarId").val(id);
    $("#inputModificarPeriodo").val(periodo);
    $("#inputModificarOrden").val(orden);

    ModificarModal.toggle();

});

$("#btnAgregarPeriodo").on("click", function () {
    $("#inputAgregarPeriodo").val("");
    $("#inputAgregarOrden").val("");
    AgregarModal.toggle();
});

$(".eliminarPeriodo").on("click", function () {
    var id = $(this).data("id");
   

    $("#inputEliminarId").val("");
    $("#inputEliminarId").val(id);
    
   

    EliminarModal.toggle();

});