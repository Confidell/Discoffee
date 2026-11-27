const escolhas = {};   // ex.: { categoria: "Bebidas" }

document.addEventListener('click', e => {
    const filtro = e.target.closest('[data-filtro]');
    const opcao = e.target.closest('[data-valor]');

    // Fecha os menus que não foram clicados
    document.querySelectorAll('[data-filtro]').forEach(f => {
        if (f !== filtro) f.querySelector('.filtro-menu').hidden = true;
    });

    if (!filtro) return;
    const menu = filtro.querySelector('.filtro-menu');

    if (opcao) {
        escolhas[filtro.dataset.filtro] = opcao.dataset.valor;
        filtro.querySelector('.filtro-escolhido').textContent = opcao.textContent;
        aplicarFiltros();
    }

    menu.hidden = !menu.hidden;   // abre ou fecha
});

function aplicarFiltros() {
    document.querySelectorAll('[data-item]').forEach(card => {
        const combina = Object.entries(escolhas)
            .every(([nome, valor]) => !valor || card.dataset[nome] === valor);
        card.hidden = !combina;
    });
}
