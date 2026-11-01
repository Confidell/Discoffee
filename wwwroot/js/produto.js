document.addEventListener('click', e => {
    const abrir = e.target.closest('[data-modal]');
    if (abrir) {
        const modal = document.getElementById(abrir.dataset.modal);
        if (modal) {
            limparModal(modal);  
            modal.showModal();
        }
        return;
    }

    const fechar = e.target.closest('[data-fechar]');
    if (fechar) {
        const modal = fechar.closest('dialog');
        limparModal(modal);
        modal.close();
    }
});

function limparModal(modal) {
    modal.querySelector('form')?.reset();
    const upload = modal.querySelector('.upload');
    if (upload) limparPreview(upload);
}

/* PRÉ-VISUALIZAÇÃO DA IMAGEM */
document.addEventListener('change', e => {
    if (!e.target.matches('.upload input[type="file"]')) return;

    const upload = e.target.closest('.upload');
    const preview = upload.querySelector('.upload-preview');
    const arquivo = e.target.files[0]; 

    upload.classList.remove('erro');

    // Se o usuário abriu o seletor e cancelou, volta ao estado inicial
    if (!arquivo) {
        limparPreview(upload);
        return;
    }
    preview.src = URL.createObjectURL(arquivo);
    preview.hidden = false;         
    upload.classList.add('com-imagem');  
});


function limparPreview(upload) {
    const preview = upload.querySelector('.upload-preview');
    upload.classList.remove('arrastando', 'erro');

    // EDIÇÃO: volta para a imagem atual do produto
    if (upload.dataset.original) {
        preview.src = upload.dataset.original;
        preview.hidden = false;
        upload.classList.add('com-imagem');
        return;
    }

    // CADASTRO: volta ao estado vazio
    preview.hidden = true;
    preview.removeAttribute('src');
    upload.classList.remove('com-imagem');
}

/* ARRASTAR E SOLTAR */
document.addEventListener('dragover', e => {
    e.preventDefault();
    e.target.closest('.upload')?.classList.add('arrastando');
});

// "dragleave": o arquivo saiu da área
document.addEventListener('dragleave', e => {
    const upload = e.target.closest('.upload');
    if (upload && !upload.contains(e.relatedTarget)) {
        upload.classList.remove('arrastando');
    }
});

// "drop": o usuário soltou o arquivo
document.addEventListener('drop', e => {
    e.preventDefault(); 

    const upload = e.target.closest('.upload');
    if (!upload) return;
    upload.classList.remove('arrastando');

    const arquivo = e.dataTransfer.files[0];
    if (!arquivo || !arquivo.type.startsWith('image/')) return;

    const input = upload.querySelector('input[type="file"]');
    const dados = new DataTransfer();
    dados.items.add(arquivo);
    input.files = dados.files;

    input.dispatchEvent(new Event('change', { bubbles: true }));
});

/* ATIVAR / DESATIVAR PRODUTO */
document.addEventListener('change', async e => {
    const toggle = e.target.closest('.toggle-status');
    if (!toggle) return;

    // O token de segurança já existe nos formulários dos modais
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    const dados = new URLSearchParams({
        id: toggle.dataset.id,
        disponivel: toggle.checked,
        __RequestVerificationToken: token
    });

    try {
        const resposta = await fetch('/Gerencia/Produtos/AlterarDisponibilidade', {
            method: 'POST',
            body: dados
        });
        if (!resposta.ok) throw new Error();
    } catch {
        // Não salvou: volta o interruptor para o estado anterior
        toggle.checked = !toggle.checked;
    }
});

// Bloqueia o envio se nenhuma imagem foi escolhida
document.addEventListener('submit', e => {
    const form = e.target;
    const upload = form.querySelector('.upload');
    const input = upload?.querySelector('input[type="file"]');
    if (!input) return;

    if (input.files.length === 0 && !upload.dataset.original) {
        e.preventDefault();
        upload.classList.add('erro');
    }
});