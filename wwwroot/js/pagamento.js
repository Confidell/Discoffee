const opcoesPagamento = document.querySelectorAll('input[name="FormaPagamento"]');

const pagamentoPix = document.getElementById("pagamento-pix");
const pagamentoCartao = document.getElementById("pagamento-cartao");

const opcoesCartao = document.querySelectorAll('input[name="Cartao.IdCartao"]');

opcoesPagamento.forEach(opcao => {

    opcao.addEventListener("change", function () {

        if (this.value === "1") {
            pagamentoPix.style.display = "flex";
            pagamentoCartao.style.display = "none";
            
            opcoesCartao.forEach((opcao) => {
                opcao.checked = false;
            })
        }
        else {
            pagamentoPix.style.display = "none";
            pagamentoCartao.style.display = "flex";
        }
    });
});


// EXIBE O CAMPO CVV DO CARTÃO SELECIONADO
opcoesCartao.forEach(opcao => {

    opcao.addEventListener("change", function () {

        // OCULTA E DESABILITA TODOS OS CAMPOS CVV
        document.querySelectorAll(".campo-cvv").forEach(campo => {

            campo.style.display = "none";

            const inputCVV = campo.querySelector("input");

            inputCVV.value = "";
            inputCVV.disabled = true; // DESABILITA O INPUT
            inputCVV.required = false; // REMOVE OBRIGATORIEDADE

        });

        // EXIBE O CAMPO CVV DO CARTÃO SELECIONADO
        const campoCVV = this.closest(".opcao-cartao")
            .querySelector(".campo-cvv");

        campoCVV.style.display = "block";

        const inputCVV = campoCVV.querySelector("input");

        inputCVV.disabled = false; // ABILITA O INPUT
        inputCVV.required = true; // TORNA OBRIGATORIO
    });
});

// IMPEDE A TROCA PARA PIX DEIXANDO CVV DESABILITADO
opcoesPagamento.forEach(opcao => {
    opcao.addEventListener("change", function () {

        if (this.value === "1") {
            document.querySelectorAll(".campo-cvv").forEach(campo => {

                campo.style.display = "none";

                const inputCVV = campo.querySelector("input");

                inputCVV.value = "";
                inputCVV.disabled = true; // DESABILITA O INPUT
                inputCVV.required = false; // REMOVE OBRIGATORIEDADE
            });
        }
    });
});

// CHAMA A API VIACEP PARA A TELA DE PAGAMENTOS
const cepTexto = document.querySelector(".cep");
const enderecoCompleto = document.querySelector(".endereco-completo");

if (cepTexto && enderecoCompleto) {

    const cepValor = cepTexto.textContent.replace(/\D/g, "");

    if (cepValor.length === 8) {
        buscarEnderecoTexto(cepValor);
    }

    async function buscarEnderecoTexto(cepValor) {

        try {
            const resposta = await fetch(
                `https://viacep.com.br/ws/${cepValor}/json/`
            );

            const dados = await resposta.json();

            if (!resposta.ok || dados.erro) {
                throw new Error("CEP não encontrado.");
            }

            // EXIBE O ENDEREÇO CONCATENADO EM TEXTO
            enderecoCompleto.textContent =
                `${dados.logradouro}, ${dados.bairro}, ${dados.localidade} - ${dados.uf}`;
        }
        catch (erro) {
            enderecoCompleto.textContent = "Endereço não encontrado.";
        }
    }
}