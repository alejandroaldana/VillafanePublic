
var ModificarModal = new bootstrap.Modal(document.getElementById('ModificarModal'), {
    keyboard: false
})
var AgregarModal = new bootstrap.Modal(document.getElementById('AgregarModal'), {
    keyboard: false
})
var EliminarModal = new bootstrap.Modal(document.getElementById('EliminarModal'), {
    keyboard: false
})

$(".modificarGrupo").on("click", function () {
    var id = $(this).data("id");
    var grupo = $(this).data("grupo");

    $("#inputModificarId").val("");
    $("#inputModificarGrupo").val("");
    $("#inputModificarId").val(id);
    $("#inputModificarGrupo").val(grupo);

    ModificarModal.toggle();

});

$("#btnAgregarGrupo").on("click", function () {
    $("#inputAgregarGrupo").val("");
    AgregarModal.toggle();
});

$(".eliminarGrupo").on("click", function () {
    var id = $(this).data("id");
    var grupo = $(this).data("grupo");

    $("#inputEliminarId").val("");
    $("#inputEliminarGrupo").val("");
    $("#inputEliminarId").val(id);
    $("#inputEliminarGrupo").val(grupo);

    EliminarModal.toggle();

});