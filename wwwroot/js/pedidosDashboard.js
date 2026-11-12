// ABRIR MODAIS
const abrirModal = document.querySelectorAll("[data-modal]");

abrirModal.forEach(abrir => {
    abrir.addEventListener("click", () => {
        const idModal = abrir.dataset.modal;
        const modal = document.getElementById(idModal);

        modal.showModal();
    });
});


// FECHAR MODAIS
const fecharModal = document.querySelectorAll("[data-fechar]");

fecharModal.forEach(fechar => {
    fechar.addEventListener("click", () => {
        const modal = fechar.closest("dialog");

        modal.close();
    });
});