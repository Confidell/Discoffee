const iconeEditar = document.querySelector(".icone-editar");
const botoes = document.querySelector(".controles-perfil .botoes");
const iconeCancelar = document.querySelector(".icone-cancelar");

const inputs = document.querySelectorAll(".edit-input");

iconeEditar.addEventListener('click', () => {

    botoes.classList.add('exibir');
    iconeEditar.classList.add('ocultar');

    inputs.forEach((i) => {
        i.removeAttribute("disabled");
    });
});

iconeCancelar.addEventListener('click', () => {

    botoes.classList.remove('exibir');
    iconeEditar.classList.remove('ocultar');

    inputs.forEach((i) => {
        i.setAttribute("disabled", true);
    });
});