const opcoesPagamento = document.querySelectorAll(
    'input[name="FormaPagamento"]'
);

const pagamentoPix = document.getElementById("pagamento-pix");
const pagamentoCartao = document.getElementById("pagamento-cartao");

opcoesPagamento.forEach(opcao => {

    opcao.addEventListener("change", function () {

        if (this.value === "Pix") {
            pagamentoPix.style.display = "block";
            pagamentoCartao.style.display = "none";
        }
        else {
            pagamentoPix.style.display = "none";
            pagamentoCartao.style.display = "block";
        }

    });

});