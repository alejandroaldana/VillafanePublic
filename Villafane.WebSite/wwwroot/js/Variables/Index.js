
var ModificarModal = new bootstrap.Modal(document.getElementById('ModificarModal'), {
    keyboard: false
})
var AgregarModal = new bootstrap.Modal(document.getElementById('AgregarModal'), {
    keyboard: false
})
var EliminarModal = new bootstrap.Modal(document.getElementById('EliminarModal'), {
    keyboard: false
})

$(".modificarVariable").on("click", function () {
    var id = $(this).data("id");
    var variable = $(this).data("variable");
    var valor = $(this).data("valor");
    $("#inputModificarId").val("");
    $("#inputModificarVariable").val("");
    $("#selectModificarValor").val("");
   
    $("#inputModificarId").val(id);
    $("#inputModificarVariable").val(variable);
    $("#selectModificarValor").val(valor);

    ModificarModal.toggle();

});

$("#btnAgregarVariable").on("click", function () {
    $("#inputAgregarVariable").val("");
    $("#selectAgregarValor").val("");
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