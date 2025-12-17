function filtrarTabla() {

    const inicio = document.getElementById('fechaInicio').value;
    const fin = document.getElementById('fechaFin').value;

    const filas = document.querySelectorAll('#tablaVentas tbody tr');

    filas.forEach(fila => {

        const fechaFila = fila.getAttribute('data-fecha');
        let mostrar = true;

        if (inicio && fechaFila < inicio) mostrar = false;
        if (fin && fechaFila > fin) mostrar = false;

        fila.style.display = mostrar ? '' : 'none';
    });
}

function exportarExcel() {

    const filas = document.querySelectorAll('#tablaVentas tbody tr');
    let tabla =
        "<table border='1'>" +
        document.querySelector('#tablaVentas thead').outerHTML +
        "<tbody>";

    filas.forEach(fila => {
        if (fila.style.display !== 'none') {
            tabla += fila.outerHTML;
        }
    });

    tabla += "</tbody></table>";

    const html =
        "<html>" +
        "<head><meta charset='UTF-8'></head>" +
        "<body>" +
        tabla +
        "</body>" +
        "</html>";

    const blob = new Blob([html], {
        type: "application/vnd.ms-excel"
    });

    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = "Reporte_Ventas.xls";
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
}
