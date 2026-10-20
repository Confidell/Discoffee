const botoesDetalhes = document.querySelectorAll(".ver-detalhes");
const botoesFechar = document.querySelectorAll(".modal-pedido-fechar");
const pagina = document.querySelector('.pagina');

function fecharModalPedido(modal) {
    if (modal) {
        modal.hidden = true;
    }
    pagina.style.overflow = 'auto';
}

botoesDetalhes.forEach(botao => {
    botao.addEventListener("click", () => {
        const idPedido = botao.dataset.pedidoId;

        const modal = document.getElementById(
            `modalPedido-${idPedido}`
        );

        if (modal) {
            modal.hidden = false;
        }

        pagina.style.overflow = 'hidden';
    });
});

// Fechar ao clicar no X
botoesFechar.forEach(botao => {
    botao.addEventListener("click", () => {
        fecharModalPedido(botao.closest("[data-modal-pedido]"));
    });
});
