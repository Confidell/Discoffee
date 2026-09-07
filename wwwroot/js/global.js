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

//Login/Cadastro

document.addEventListener("DOMContentLoaded", function () {
    const boxLogin = document.getElementById("box-login");
    const boxCadastro = document.getElementById("box-cadastro");

    const linkIrCadastro = document.getElementById("link-ir-cadastro");
    const linkIrLogin = document.getElementById("link-ir-login");

    if (linkIrCadastro) {
        linkIrCadastro.addEventListener("click", function (e) {
            e.preventDefault(); // Impede o recarregamento da página
            if (boxLogin && boxCadastro) {
                boxLogin.style.display = "none";
                boxCadastro.style.display = "block";
            }
        });
    }

    if (linkIrLogin) {
        linkIrLogin.addEventListener("click", function (e) {
            e.preventDefault(); // Impede o recarregamento da página
            if (boxLogin && boxCadastro) {
                boxCadastro.style.display = "none";
                boxLogin.style.display = "block";
            }
        });
    }
});