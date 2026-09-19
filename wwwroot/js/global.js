// HEADER
let lastScroll = 0//Ultima posição do scroll

window.addEventListener('scroll', () => {
    let header = document.querySelector('header')//Chama o header
    const currentScroll = window.scrollY //Atual posição do scroll

        if(currentScroll > lastScroll){
            header.classList.add('hidden')//Ao rolar para baixo
        }
        else{
            header.classList.remove('hidden')//Ao rolar para cima
        }
        lastScroll = currentScroll //Atualiza a ultima posição do scroll

        header.classList.toggle('scroll', window.scrollY > 10)
})

// POPUP DE ERRROS
function fecharPopup() {
    const popup = document.getElementById("popup-erros");

    popup.classList.remove("ativo");

    setTimeout(() => {
        popup.remove();
    }, 300);
}