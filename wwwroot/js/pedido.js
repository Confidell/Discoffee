const verDetalhes = document.querySelectorAll(".ver-detalhes");
const botoesFechar = document.querySelectorAll(".modal-pedido-fechar");
const pagina = document.querySelector('.pagina');

function fecharModalPedido(modal) {
    if (modal) {
        modal.hidden = true;
    }
    pagina.style.overflow = 'auto';
}

verDetalhes.forEach(card => {
    card.addEventListener("click", () => {
        const idPedido = card.dataset.pedidoId;

        const modal = document.getElementById(
            `modalPedido-${idPedido}`
        );

        setTimeout(() => {
            if (modal) {
                modal.hidden = false;
            }
        }, 100);

        pagina.style.overflow = 'hidden';
    });
});

// FECHAR MODAL PEDIDO
botoesFechar.forEach(botao => {
    botao.addEventListener("click", () => {
        fecharModalPedido(botao.closest("[data-modal-pedido]"));
    });
});
