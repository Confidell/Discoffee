function mudar(botao, valor) {
    const seletor = botao.closest(".seletor-quantidade");
    const quantidade = seletor.querySelector("#quantidade");

    let atual = Number(quantidade.textContent);
    atual = Math.max(1, atual + valor);

    quantidade.textContent = atual;
}

document.querySelectorAll(".paiModal").forEach(modal => {
    const botaoAdicionar = modal.querySelector("#adicionar-carrinho");

    botaoAdicionar.addEventListener("click", () => {
        adicionarAoCarrinho(modal);
    });
});

async function adicionarAoCarrinho(modal) {
    const idProduto = Number(modal.dataset.idProduto);

    const quantidade = Number(
        modal.querySelector("#quantidade").textContent
    );

    const containerOpcoes = modal.querySelector(".container-opcoes");

    const opcoesSelecionadas = containerOpcoes.querySelectorAll(
        'input[type="radio"]:checked, input[type="checkbox"]:checked'
    );

    const dados = {
        idProduto: idProduto,
        quantidade: quantidade,
        opcoes: Array.from(opcoesSelecionadas).map(opcao => ({
            idOpcao: Number(opcao.value)
        }))
    };

    try {
        const resposta = await fetch("/Carrinho/AdicionarItem", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(dados)
        });

        const resultado = await resposta.json();

        if (!resposta.ok) {
            alert(resultado.mensagem ?? "Não foi possível adicionar o produto.");
            return;
        }

        alert("Produto adicionado ao carrinho!");
    }
    catch (erro) {
        console.error(erro);
        alert("Erro ao comunicar com o servidor.");
    }
}