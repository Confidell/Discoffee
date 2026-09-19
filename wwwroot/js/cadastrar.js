const form = document.getElementById("form-cadastro");
// DADOS PARA FORMATAÇÃO VISUAL
const cpf = document.getElementById("cpf");
const telefone = document.getElementById("tel");
const cep = document.getElementById("cep");
const dataNascimento = document.getElementById("dataNasc");
// DADOS PARA API DE CEP
const campoRua = document.getElementById("rua");
const campoBairro = document.getElementById("bairro");
const campoCidade = document.getElementById("cidade");
const campoEstado = document.getElementById("estado");

// MÁSCARAS VISUAIS
cpf.addEventListener("input", function () {

    let valor = this.value.replace(/\D/g, "");

    valor = valor.substring(0, 11);

    valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
    valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
    valor = valor.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    this.value = valor;
});

telefone.addEventListener("input", function () {

    let valor = this.value.replace(/\D/g, "");

    valor = valor.substring(0, 11);

    if (valor.length <= 10) {
        valor = valor.replace(/(\d{2})(\d)/, "($1) $2");
        valor = valor.replace(/(\d{4})(\d)/, "$1-$2");
    }
    else {
        valor = valor.replace(/(\d{2})(\d)/, "($1) $2");
        valor = valor.replace(/(\d{5})(\d)/, "$1-$2");
    }

    this.value = valor;
});

cep.addEventListener("input", function () {

    let valor = this.value.replace(/\D/g, "");

    valor = valor.substring(0, 8);

    valor = valor.replace(/(\d{5})(\d)/, "$1-$2");

    this.value = valor;
});

dataNascimento.addEventListener("input", function () {

    let valor = this.value.replace(/\D/g, "");

    valor = valor.substring(0, 8);

    valor = valor.replace(/(\d{2})(\d)/, "$1/$2");
    valor = valor.replace(/(\d{2})(\d)/, "$1/$2");

    this.value = valor;
});


// PREPARAR DADOS PARA ENVIO
form.addEventListener("submit", function () {

    // CPF
    cpf.value = cpf.value.replace(/\D/g, "");


    // TELEFONE
    telefone.value = telefone.value.replace(/\D/g, "");


    // CEP
    cep.value = cep.value.replace(/\D/g, "");
});

// CHAMA A API PARA OBTER O ENDEREÇO ATRAVÉS DO CEP
cep.addEventListener("blur", async function () {

    const cep = this.value.replace(/\D/g, "");

    if (cep.length !== 8) {
        return;
    }

    try {

        campoRua.value = "Buscando...";
        campoBairro.value = "Buscando...";
        campoCidade.value = "Buscando...";
        campoEstado.value = "Buscando...";

        const resposta = await fetch(
            `https://viacep.com.br/ws/${cep}/json/`
        );

        if (!resposta.ok) {
            throw new Error("Erro ao consultar o CEP.");
        }

        const dados = await resposta.json();

        if (dados.erro) {
            throw new Error("CEP não encontrado.");
        }

        campoRua.value = dados.logradouro;
        campoBairro.value = dados.bairro;
        campoCidade.value = dados.localidade;
        campoEstado.value = dados.uf;

    } catch (erro) {

        campoRua.value = "";
        campoBairro.value = "";
        campoCidade.value = "";
        campoEstado.value = "";

        alert(erro.message);
    }
});

// EXIBE SENHA NO INPUT
function mostrarSenha(idCampo, botao) {
    const campo = document.getElementById(idCampo);

    const olhoFechado = botao.querySelector(".icone-olho-fechado");
    const olhoAberto = botao.querySelector(".icone-olho-aberto");

    if (campo.type === "password") {
        campo.type = "text";

        olhoFechado.style.display = "block";
        olhoAberto.style.display = "none";
    } 
    else {
        campo.type = "password";

        olhoFechado.style.display = "none";
        olhoAberto.style.display = "block";
    }
}