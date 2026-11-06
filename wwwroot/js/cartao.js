document.addEventListener("DOMContentLoaded", function () {

    const numeroCartaoForm = document.getElementById("numero-cartao-form");
    const nomeCartaoForm = document.getElementById("nome-cartao-form");
    const apelidoCartaoForm = document.getElementById("apelido-cartao-form");
    const validadeCartaoForm = document.getElementById("validade-cartao-form");
    const cpfTitularForm = document.getElementById("cpf-titular-form");
    const numeroCartaoVisu = document.getElementById("numero-cartao-visu");
    const nomeCartaoVisu = document.getElementById("nome-cartao-visu");
    const apelidoCartaoVisu = document.getElementById("apelido-cartao-visu");
    const validadeCartaoVisu = document.getElementById("validade-cartao-visu");
    const bandeira = document.getElementById("bandeira");
    const cartao = document.getElementById("cartao");

    // CONFIGURAÇÃO DAS BANDEIRAS
    const bandeiras = {

        visa: {
            imagem: "/Assets/Icons/Logo_Visa.svg",
            classe: "cartao-visa"
        },

        mastercard: {
            imagem: "/Assets/Icons/Logo_Mastercard.svg",
            classe: "cartao-mastercard"
        },

        elo: {
            imagem: "/Assets/Icons/Logo_Elo.svg",
            classe: "cartao-elo"
        },

        amex: {
            imagem: "/Assets/Icons/Logo_Amex.svg",
            classe: "cartao-amex"
        }

    };

    // NÚMERO DO CARTÃO
    numeroCartaoForm.addEventListener("input", async function () {

        let valor = this.value.replace(/\D/g, "");

        valor = valor.substring(0, 16);

        valor = valor.replace(/(\d{4})(?=\d)/g, "$1 ");

        this.value = valor;

        numeroCartaoVisu.textContent =
            valor || "0000 0000 0000 0000";


        // RETIRA APENAS OS NUMEROS
        const numeroLimpo = valor.replace(/\D/g, "");

        // CONSULTA COM APENAS 6 DIGITOS
        if (numeroLimpo.length >= 6) {
            await identificarBandeira(numeroLimpo);
        } 
        else{
            resetarBandeira();
        }

    });

    // IDENTIFICAR BANDEIRA
    function identificarBandeira(numero) {
        // DEIXA APENAS NUMEROS
        numero = numero.replace(/\D/g, "");

        let tipo = null;

        // VISA
        if (/^4/.test(numero)) {
            tipo = "visa";
        }

        // MASTERCARD
        else if (
            /^(5[1-5]|2(2[2-9]|[3-6]\d|7[01]|720))/.test(numero)
        ) 
        {
            tipo = "mastercard";
        }

        // AMEX
        else if (/^3[47]/.test(numero)) {
            tipo = "amex";
        }

        // ELO
        else if (
            /^(4011|431274|438935|451416|457393|457631|5067|5090|627780|636297|636368)/.test(numero)
        ) 
        {
            tipo = "elo";
        }

        if (tipo) {
            alterarBandeira(tipo);
        }
        else{
            resetarBandeira();
        }
    }


    // ALTERA BANDEIRA
    function alterarBandeira(tipo) {

        const dados = bandeiras[tipo];

        if (!dados) {
            resetarBandeira();
            return;
        }


        // TROCA A LOGO
        bandeira.src = dados.imagem;

        bandeira.alt =
            `Bandeira ${tipo}`;

        // REMOVE OS ESTILOS ANTERIORES
        Object.values(bandeiras).forEach(bandeiraDados => {

            cartao.classList.remove(
                bandeiraDados.classe
            );
        });

        // ADICIONA O ESTILO DA BANDEIRA ATUAL
        cartao.classList.add(
            dados.classe
        );
    }


    // RESETAR BANDEIRA
    function resetarBandeira() {

        bandeira.src =
            "/Assets/Icons/Logo.svg";

        bandeira.alt =
            "Logo do Cartão";


        Object.values(bandeiras).forEach(bandeiraDados => {

            cartao.classList.remove(
                bandeiraDados.classe
            );
        });

        document.getElementById(
            "tipo-cartao-visu"
        ).textContent = "Tipo";
    }

    // NOME IMPRESSO
    nomeCartaoForm.addEventListener("input", function () {

        let valor = this.value.replace(
            /[^a-zA-ZÀ-ÿ\s]/g,
            ""
        );

        valor = valor.substring(0, 30);

        valor = valor.replace(/\s{2,}/g, " ");

        this.value = valor;

        nomeCartaoVisu.textContent =
            valor.toUpperCase() || "NOME IMPRESSO";
    });

    // APELIDO
    apelidoCartaoForm.addEventListener("input", function () {

        let valor = this.value.replace(
            /[^a-zA-ZÀ-ÿ0-9\s]/g,
            ""
        );

        valor = valor.substring(0, 20);

        valor = valor.replace(/\s{2,}/g, " ");

        this.value = valor;

        apelidoCartaoVisu.textContent =
            valor || "APELIDO";
    });

    // VALIDADE
    validadeCartaoForm.addEventListener("input", function () {

        let valor = this.value.replace(/\D/g, "");

        valor = valor.substring(0, 4);

        if (valor.length > 2) {

            valor =
                valor.substring(0, 2) +
                "/" +
                valor.substring(2);

        }

        this.value = valor;

        validadeCartaoVisu.textContent =
            valor || "MM/AA";
    });

    // CPF
    cpfTitularForm.addEventListener("input", function () {

        let valor = this.value.replace(/\D/g, "");

        valor = valor.substring(0, 11);

        if (valor.length > 9) {

            valor = valor.replace(
                /(\d{3})(\d{3})(\d{3})(\d{2})/,
                "$1.$2.$3-$4"
            );

        } 
        else if (valor.length > 6) {

            valor = valor.replace(
                /(\d{3})(\d{3})(\d{1,3})/,
                "$1.$2.$3"
            );

        } 
        else if (valor.length > 3) {

            valor = valor.replace(
                /(\d{3})(\d{1,3})/,
                "$1.$2"
            );
        }

        this.value = valor;

    });
});

const btnAdicionarCartao = document.querySelector(".btn-adicionar-cartao");
const btnCancelar = document.querySelector(".btn-cancelar");
const formAdicionarCartao = document.querySelector(".adicionar-cartao");

btnAdicionarCartao.addEventListener("click", () => {

    formAdicionarCartao.classList.add("exibir");
});

btnCancelar.addEventListener("click", () => {

    formAdicionarCartao.classList.remove("exibir");
});

// IDENTIFICAR BANDEIRAS DOS CARTÕES CADASTRADOS
document.addEventListener("DOMContentLoaded", function () {

    const cartoesCadastrados = document.querySelectorAll(".card-cartao-cadastrado");

    const bandeirasCadastradas = {
        visa: "/Assets/Icons/Logo_Visa.svg",
        mastercard: "/Assets/Icons/Logo_Mastercard.svg",
        elo: "/Assets/Icons/Logo_Elo.svg",
        amex: "/Assets/Icons/Logo_Amex.svg"
    };

    cartoesCadastrados.forEach(card => {

        const numeroCartao = card.querySelector(
            ".card-cartao-info p"
        );

        const imagemBandeira = card.querySelector(
            ".bandeira-cadastrada"
        );

        if (!numeroCartao || !imagemBandeira) return;

        const numero = numeroCartao.textContent.replace(/\D/g, "");

        let tipo = null;

        // VISA
        if (/^4/.test(numero)) {
            tipo = "visa";
        }

        // MASTERCARD
        else if (/^(5[1-5]|2(2[2-9]|[3-6]\d|7[01]|720))/.test(numero)) {
            tipo = "mastercard";
        }

        // AMEX
        else if (/^3[47]/.test(numero)) {
            tipo = "amex";
        }

        // ELO
        else if (/^(4011|431274|438935|451416|457393|457631|5067|5090|627780|636297|636368)/.test(numero)) {
            tipo = "elo";
        }

        // ALTERAR IMAGEM
        if (tipo) {

            imagemBandeira.src = bandeirasCadastradas[tipo];
            imagemBandeira.alt = `Bandeira ${tipo}`;
        }
    });
});