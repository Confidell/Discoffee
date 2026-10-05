document.addEventListener("DOMContentLoaded", function () {

    // =========================
    // ELEMENTOS
    // =========================

    const numeroCartaoForm =
        document.getElementById("numero-cartao-form");

    const nomeCartaoForm =
        document.getElementById("nome-cartao-form");

    const apelidoCartaoForm =
        document.getElementById("apelido-cartao-form");

    const validadeCartaoForm =
        document.getElementById("validade-cartao-form");

    const cpfTitularForm =
        document.getElementById("cpf-titular-form");


    const numeroCartaoVisu =
        document.getElementById("numero-cartao-visu");

    const nomeCartaoVisu =
        document.getElementById("nome-cartao-visu");

    const apelidoCartaoVisu =
        document.getElementById("apelido-cartao-visu");

    const validadeCartaoVisu =
        document.getElementById("validade-cartao-visu");

    const bandeira =
        document.getElementById("bandeira");

    const cartao =
        document.getElementById("cartao");


    // =========================
    // CONFIGURAÇÃO DAS BANDEIRAS
    // =========================

    const bandeiras = {

        visa: {
            imagem: "/Assets/Icons/Bandeiras/visa.svg",
            classe: "cartao-visa"
        },

        mastercard: {
            imagem: "/Assets/Icons/Bandeiras/mastercard.svg",
            classe: "cartao-mastercard"
        },

        elo: {
            imagem: "/Assets/Icons/Bandeiras/elo.svg",
            classe: "cartao-elo"
        },

        amex: {
            imagem: "/Assets/Icons/Bandeiras/amex.svg",
            classe: "cartao-amex"
        }

    };


    // =========================
    // NÚMERO DO CARTÃO
    // =========================

    numeroCartaoForm.addEventListener("input", async function () {

        let valor = this.value.replace(/\D/g, "");

        valor = valor.substring(0, 16);

        valor = valor.replace(/(\d{4})(?=\d)/g, "$1 ");

        this.value = valor;

        numeroCartaoVisu.textContent =
            valor || "0000 0000 0000 0000";


        // Retira apenas os números
        const numeroLimpo = valor.replace(/\D/g, "");


        // Só consulta quando tiver pelo menos 6 números
        if (numeroLimpo.length >= 6) {

            await identificarBandeira(numeroLimpo);

        } else {

            resetarBandeira();

        }

    });


    // =========================
    // IDENTIFICAR BANDEIRA
    // =========================

    async function identificarBandeira(numero) {

        // Pega os primeiros 8 números
        // quando disponíveis
        const bin = numero.substring(0, 8);

        try {

            const resposta = await fetch(
                `https://lookup.binlist.net/${bin}`,
                {
                    headers: {
                        "Accept-Version": "3"
                    }
                }
            );


            if (!resposta.ok) {

                resetarBandeira();

                return;
            }


            const dados = await resposta.json();

            console.log("Dados do cartão:", dados);


            const esquema =
                dados.scheme?.toLowerCase();


            if (bandeiras[esquema]) {

                alterarBandeira(esquema);

            } else {

                resetarBandeira();

            }


        } catch (erro) {

            console.error(
                "Erro ao consultar a bandeira:",
                erro
            );

            resetarBandeira();
        }
    }


    // =========================
    // ALTERAR BANDEIRA
    // =========================

    function alterarBandeira(tipo) {

        const dados = bandeiras[tipo];

        if (!dados) {
            resetarBandeira();
            return;
        }


        // Troca o logo
        bandeira.src = dados.imagem;

        bandeira.alt =
            `Bandeira ${tipo}`;


        // Remove estilos anteriores
        Object.values(bandeiras).forEach(bandeiraDados => {

            cartao.classList.remove(
                bandeiraDados.classe
            );

        });


        // Adiciona estilo da nova bandeira
        cartao.classList.add(
            dados.classe
        );


        // Atualiza o "Tipo"
        document.getElementById(
            "tipo-cartao-visu"
        ).textContent = tipo
            .charAt(0)
            .toUpperCase() + tipo.slice(1);

    }


    // =========================
    // RESETAR BANDEIRA
    // =========================

    function resetarBandeira() {

        bandeira.src =
            "/Assets/Icons/Bandeiras/default.svg";

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


    // =========================
    // NOME IMPRESSO
    // =========================

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


    // =========================
    // APELIDO
    // =========================

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


    // =========================
    // VALIDADE
    // =========================

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


    // =========================
    // CPF
    // =========================

    cpfTitularForm.addEventListener("input", function () {

        let valor = this.value.replace(/\D/g, "");

        valor = valor.substring(0, 11);

        if (valor.length > 9) {

            valor = valor.replace(
                /(\d{3})(\d{3})(\d{3})(\d{2})/,
                "$1.$2.$3-$4"
            );

        } else if (valor.length > 6) {

            valor = valor.replace(
                /(\d{3})(\d{3})(\d{1,3})/,
                "$1.$2.$3"
            );

        } else if (valor.length > 3) {

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