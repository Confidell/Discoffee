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