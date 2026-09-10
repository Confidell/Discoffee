//Login/Cadastro

document.addEventListener("DOMContentLoaded", function () {

    const formLogin = document.getElementById("form-login");
    const formCadastro = document.getElementById("form-cadastro");

    const linkIrCadastro = document.getElementById("link-cadastro");
    const linkIrLogin = document.getElementById("link-login");


    // Ir para o cadastro
    if (linkIrCadastro) {
        linkIrCadastro.addEventListener("click", function (e) {

            e.preventDefault();

            if (formLogin && formCadastro) {
                formLogin.style.display = "none";
                formCadastro.style.display = "flex";
            }

        });
    }


    // Voltar para o login
    if (linkIrLogin) {
        linkIrLogin.addEventListener("click", function (e) {

            e.preventDefault();

            if (formLogin && formCadastro) {
                formCadastro.style.display = "none";
                formLogin.style.display = "flex";
            }

        });
    }

});