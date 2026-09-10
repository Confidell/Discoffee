// document.addEventListener("DOMContentLoaded", function () {

//     const formLogin = document.getElementById("form-login");
//     const formCadastro = document.getElementById("form-cadastro");

//     const linkIrCadastro = document.getElementById("link-cadastro");
//     const linkIrLogin = document.getElementById("link-login");

//     // IR PARA CADASTRO
//     if (linkIrCadastro) {

//         linkIrCadastro.addEventListener("click", function(e) {

//             e.preventDefault();

//             formLogin.classList.add('form-oculto');
//             formCadastro.classList.remove('form-oculto');
//         });
//     }

//     // IR PARA LOGIN
//     if (linkIrLogin) {

//         linkIrLogin.addEventListener("click", function(e) {

//             e.preventDefault();

//             formCadastro.classList.remove('form-oculto');
//             formLogin.classList.add('form-oculto');

//         });
//     }
// });