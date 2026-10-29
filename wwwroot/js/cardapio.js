const pagina = document.querySelector('.pagina');

// ABRIR MODAL E CARREGAR OPÇÕES
document.querySelectorAll('.btn-card-prod').forEach(botao => {
    botao.addEventListener('click', async () => {
        pagina.style.overflow = 'hidden';

        const modal = document.getElementById(botao.dataset.modal);

        modal.classList.add('ativa');

        const container = modal.querySelector('.container-opcoes');

        // Não carrega novamente se as opções já foram carregadas.
        if (!container || container.dataset.carregado === 'true') {
            return;
        }

        try {
            const url = container.dataset.url;

            container.innerHTML = '<p>Carregando opções...</p>';

            const resposta = await fetch(url);
            const conteudo = await resposta.text();

            if (!resposta.ok) {
                throw new Error(
                    `Erro HTTP ${resposta.status}: ${conteudo}`
                );
            }

            container.innerHTML = conteudo;
            container.dataset.carregado = 'true';
        }
        catch (erro) {
            console.error('Erro ao carregar opções:', erro);

            container.innerHTML =
                '<p>Não foi possível carregar as opções.</p>';
        }
    });
});

// FECHAR MODAL
document.querySelectorAll('.btn-fechar').forEach(botao => {
    botao.addEventListener('click', () => {
        botao.closest('.paiModal').classList.remove('ativa');

        // Só libera a página se não houver outro modal aberto.
        const algumModalAberto = document.querySelector('.paiModal.ativa');

        if (!algumModalAberto) {
            pagina.style.overflow = 'auto';
        }
    });
});

// ALTERAR QUANTIDADE DO PRODUTO
function mudar(botao, valor) {
    const seletor = botao.closest('.seletor-quantidade');
    const quantidade = seletor.querySelector('#quantidade');

    let atual = Number(quantidade.textContent);

    atual = Math.max(1, atual + valor);

    quantidade.textContent = atual;
}

// ADICIONAR PRODUTO AO CARRINHO
document.querySelectorAll('.paiModal').forEach(modal => {
    const botaoAdicionar = modal.querySelector('#adicionar-carrinho');

    if (!botaoAdicionar) {
        return;
    }

    botaoAdicionar.addEventListener('click', () => {
        adicionarAoCarrinho(modal);
    });
});


async function adicionarAoCarrinho(modal) {
    const idProduto = Number(modal.dataset.idProduto);

    const quantidade = Number(
        modal.querySelector('#quantidade').textContent
    );

    const containerOpcoes = modal.querySelector('.container-opcoes');

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
        const resposta = await fetch('/Carrinho/AdicionarItem', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dados)
        });

        const resultado = await resposta.json();

        if (!resposta.ok) {
            console.error(
                'Erro ao adicionar produto:',
                resultado.mensagem
            );

            return;
        }

        console.log(resultado.mensagem ?? 'Produto adicionado ao carrinho.');
    }
    catch (erro) {
        console.error('Erro ao comunicar com o servidor:', erro);
    }
}