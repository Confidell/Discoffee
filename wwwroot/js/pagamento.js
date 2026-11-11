const opcoesPagamento = document.querySelectorAll(
    'input[name="FormaPagamento"]'
);

const pagamentoPix = document.getElementById("pagamento-pix");
const pagamentoCartao = document.getElementById("pagamento-cartao");

opcoesPagamento.forEach(opcao => {

    opcao.addEventListener("change", function () {

        if (this.value === "Pix") {
            pagamentoPix.style.display = "flex";
            pagamentoCartao.style.display = "none";
        }
        else {
            pagamentoPix.style.display = "none";
            pagamentoCartao.style.display = "flex";
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