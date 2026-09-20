const form = document.querySelector(".formulario-js");

const cpf = document.querySelector(".cpf");
const telefone = document.querySelector(".tel");
const cep = document.querySelector(".cep");
const dataNascimento = document.querySelector(".dataNasc");

const campoRua = document.querySelector(".rua");
const campoBairro = document.querySelector(".bairro");
const campoCidade = document.querySelector(".cidade");
const campoEstado = document.querySelector(".estado");

// MÁSCARAS VISUAIS PARA FORMATAR CAMPOS
function formatarCPF(valor) {

    valor = valor.replace(/\D/g, "");
    valor = valor.substring(0, 11);

    valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
    valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
    valor = valor.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    return valor;
}

function formatarTelefone(valor) {

    valor = valor.replace(/\D/g, "");
    valor = valor.substring(0, 11);

    if (valor.length <= 10) {
        valor = valor.replace(/(\d{2})(\d)/, "($1) $2");
        valor = valor.replace(/(\d{4})(\d)/, "$1-$2");
    } 
    else {
        valor = valor.replace(/(\d{2})(\d)/, "($1) $2");
        valor = valor.replace(/(\d{5})(\d)/, "$1-$2");
    }

    return valor;
}

function formatarCEP(valor) {

    valor = valor.replace(/\D/g, "");
    valor = valor.substring(0, 8);

    valor = valor.replace(/(\d{5})(\d)/, "$1-$2");

    return valor;
}

function formatarData(valor) {

    valor = valor.replace(/\D/g, "");
    valor = valor.substring(0, 8);

    valor = valor.replace(/(\d{2})(\d)/, "$1/$2");
    valor = valor.replace(/(\d{2})(\d)/, "$1/$2");

    return valor;
}

// LIMPA AS MASCARAS ANTES DO ENVIO
if (form) {
    form.addEventListener("submit", function () {

        if (cpf) {
            cpf.value = cpf.value.replace(/\D/g, "");
        }

        if (telefone) {
            telefone.value = telefone.value.replace(/\D/g, "");
        }

        if (cep) {
            cep.value = cep.value.replace(/\D/g, "");
        }
    });
}

// FORMATA AO RECEBER DADOS NO INPUT COM DIGITAÇÃO
if (cpf) {
    cpf.addEventListener("input", function () {
        this.value = formatarCPF(this.value);
    });
}

if (telefone) {
    telefone.addEventListener("input", function () {
        this.value = formatarTelefone(this.value);
    });
}

if (cep) {
    cep.addEventListener("input", function () {
        this.value = formatarCEP(this.value);
    });
}

if (dataNascimento) {
    dataNascimento.addEventListener("input", function () {
        this.value = formatarData(this.value);
    });
}

// FORMATA O INPUT SE RECEBER DADO SEM DIGITAÇÃO
if (cpf) {
    cpf.value = formatarCPF(cpf.value);
}

if (telefone) {
    telefone.value = formatarTelefone(telefone.value);
}

if (cep) {
    cep.value = formatarCEP(cep.value);
}

if (dataNascimento) {
    dataNascimento.value = formatarData(dataNascimento.value);
}

// BUSCA ENDEREÇO PELA API VIACEP
async function buscarEndereco(cepValor) {

    cepValor = cepValor.replace(/\D/g, "");

    if (cepValor.length !== 8) {
        return;
    }

    try {

        campoRua.value = "Buscando...";
        campoBairro.value = "Buscando...";
        campoCidade.value = "Buscando...";
        campoEstado.value = "Buscando...";

        const resposta = await fetch(
            `https://viacep.com.br/ws/${cepValor}/json/`
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

    } 
    catch (erro) {

        campoRua.value = "Não encontrado";
        campoBairro.value = "Não encontrado";
        campoCidade.value = "Não encontrado";
        campoEstado.value = "Não encontrado";

        alert(erro.message);
    }
}

// CHAMA A API AO DIGITAR
if (cep) {
    cep.addEventListener("blur", function () {
        buscarEndereco(this.value);
    });
}

// CHAMA A API SE DECTAR CEP NO INPUT
if (cep && cep.value) {
    buscarEndereco(cep.value);
}

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