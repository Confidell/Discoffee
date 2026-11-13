const pagina = document.querySelector('.pagina');

// ABRIR MODAL E CARREGAR OPÇÕES
document.querySelectorAll('.btn-card-prod').forEach(botao => {
    botao.addEventListener('click', async () => {
        pagina.style.overflow = 'hidden';

        const modal = document.getElementById(botao.dataset.modal);

        modal.classList.add('ativa');

        const container = modal.querySelector('.container-opcoes');

        // EVITA CARREGAR OPÇÕES DUAS VEZES
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

        const ModalAberto = document.querySelector('.paiModal.ativa');

        if (!ModalAberto) {
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

        const botao = modal.querySelector('#adicionar-carrinho');
        const modalAtivo = document.querySelector('.paiModal.ativa');

        const textoOriginal = 'Adicionar ao carrinho';
        
        botao.classList.add('adicionado');
        botao.innerHTML = `
            <svg xmlns="http://www.w3.org/2000/svg"
                width="16" height="16"
                fill="currentColor"
                class="bi bi-bookmark-check-fill"
                viewBox="0 0 16 16">

                <path fill-rule="evenodd"
                    d="M2 15.5V2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v13.5a.5.5 0 0 1-.74.439L8 13.069l-5.26 2.87A.5.5 0 0 1 2 15.5m8.854-9.646a.5.5 0 0 0-.708-.708L7.5 7.793 6.354 6.646a.5.5 0 1 0-.708.708l1.5 1.5a.5.5 0 0 0 .708 0z"/>
            </svg>

            ${resultado.mensagem}
        `;

        setTimeout(() => {
            botao.textContent = textoOriginal;
            botao.classList.remove('adicionado');

            setTimeout(() => {
                modalAtivo.classList.remove('ativa');
                pagina.style.overflow = 'auto';
            }, 50);

            // RECARREGA A PÁGINA PARA ATUALIZAR O CONTATOR DE ITENS NO CARRINHO NO HEADER
            window.location.reload();
        }, 1500);
    }
    catch (erro) {
        console.error('Erro ao comunicar com o servidor:', erro);
    }
}