document.querySelectorAll('.btn-info').forEach(botao => {
    botao.addEventListener('click', () => {
        const modal = document.getElementById(botao.dataset.modal);
        modal.classList.add('ativa');
    });
});

document.querySelectorAll('.btn-fechar').forEach(botao => {
    botao.addEventListener('click', () => {
        botao.closest('.paiModal').classList.remove('ativa');
    });
});
