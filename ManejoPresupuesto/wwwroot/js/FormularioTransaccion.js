function incializarFormularioTransacciones(urlObtenerCategorias) {
    $("#TipoOperacionId").change(async function () {
        const valorSeleccionado = $(this).val();
        // console.log('Valor: ' + valorSeleccionado);
        const respuesta = await fetch(urlObtenerCategorias, {
            method: 'POST',
            body: valorSeleccionado,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await respuesta.json();
        //console.log(json);
        const opciones = json.map(categoria => `<option value=${categoria.value}>${categoria.text}</option>`);
        $("#CategoriaId").html(opciones);
    })
}