function mudarQuantidadeCarrinho(botao, variacao) {
    const formulario = botao.closest('.form-atualizar-quantidade');

    const campoQuantidade = formulario.querySelector(
        'input[name="quantidade"]'
    );

    const textoQuantidade = formulario.querySelector('.quantidade');

    let quantidade = Number(campoQuantidade.value);
    quantidade += variacao;

    if (quantidade < 1) {
        return;
    }

    // ATUALIZA O NUMERO ENVIADO AO SERVIDOR
    campoQuantidade.value = quantidade;

    // ATUALIZA O NUMERO EXIBIDO NA TELA
    textoQuantidade.textContent = quantidade;

    console.log('Valor do campo:', campoQuantidade.value);
    console.log('Dados enviados:', new FormData(formulario).get('quantidade'));

    formulario.requestSubmit();
}

document.addEventListener("DOMContentLoaded", function () {

    const tipoEntrega = document.getElementById("tipo-entrega");
    const tipoRetirada = document.getElementById("tipo-retirada");

    const valorFrete = document.getElementById("valor-frete");
    const valorTotal = document.getElementById("valor-total");

    const inputFrete = document.getElementById("frete");
    const inputDesconto = document.getElementById("desconto");
    const inputSubtotal = document.getElementById("subtotal");

    if (!tipoEntrega || !tipoRetirada) return;

    // CONVERTE OS VALORES PARA DECIMAL
    const subtotal = Number(inputSubtotal.value.replace(",", "."));
    const desconto = Number(inputDesconto.value.replace(",", ".")) || 0;

    // CALCULA O FRETE ORIGINAL
    const freteOriginal = subtotal >= 80 ? 0 : 12.50;

    function atualizarValores() {

        let frete = tipoRetirada.checked ? 0 : freteOriginal;

        let total = subtotal + frete - desconto;

        // ATUALIZA O FRETE
        valorFrete.textContent = "R$ " +
            frete.toFixed(2).replace(".", ",");

        // ATUALIZA O TOTAL
        valorTotal.textContent = "R$ " +
            total.toFixed(2).replace(".", ",");

        // ATUALIZA O VALOR ENVIADO AO SERVIDOR
        inputFrete.value = frete.toFixed(2).replace(".", ",");
    }

    tipoEntrega.addEventListener("change", atualizarValores);
    tipoRetirada.addEventListener("change", atualizarValores);

    atualizarValores();
});