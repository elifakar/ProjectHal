function GuzergahAdd(projeId) {
    var url = "/Proje/_GuzergahCreate/" + projeId;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function GuzergahEdit(id) {
    var url = "/Proje/_GuzergahEdit/" + id;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function ProjeAracAdd(projeId) {
    var url = "/Proje/_AracAdd/" + projeId;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function ProjeAracEdit(id) {
    var url = "/Proje/_AracEdit/" + id;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function TedarikciTevkifatAdd(tedarikciId) {
    var url = "/Tedarikci/_TevkifatAdd/" + tedarikciId;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function TedarikciTevkifatEdit(id) {
    var url = "/Tedarikci/_TevkifatEdit/" + id;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function AracSurucuAdd(aracId) {
    var url = "/Arac/_SurucuAdd/" + aracId;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function AracSurucuEdit(id) {
    var url = "/Arac/_SurucuEdit/" + id;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}

function PaketDetail(id) {
    var url = "/Paket/_Detail/" + id;
    $("#modalContent").load(url, function () {
        $("#modal").modal("show");
    })
}