document.addEventListener('click', e => {
    const abrir = e.target.closest('[data-modal]');
    if (abrir) {
        const modal = document.getElementById(abrir.dataset.modal);
        if (modal) modal.showModal();
        return;
    }
    const fechar = e.target.closest('[data-fechar]');
    if (fechar) {
        fechar.closest('dialog').close();
    }
});

/* PRÉ-VISUALIZAÇÃO DA IMAGEM */
document.addEventListener('change', e => {
    if (!e.target.matches('.upload input[type="file"]')) return;

    const upload = e.target.closest('.upload');
    const preview = upload.querySelector('.upload-preview');
    const arquivo = e.target.files[0]; 

    // Se o usuário abriu o seletor e cancelou, volta ao estado inicial
    if (!arquivo) {
        limparPreview(upload);
        return;
    }
    preview.src = URL.createObjectURL(arquivo);
    preview.hidden = false;         
    upload.classList.add('com-imagem');  o
});


function limparPreview(upload) {
    const preview = upload.querySelector('.upload-preview');
    preview.hidden = true;
    preview.removeAttribute('src');
    upload.classList.remove('com-imagem', 'arrastando');
}


/* ARRASTAR E SOLTAR */
document.addEventListener('dragover', e => {
    const upload = e.target.closest('.upload');

    if (!upload) { e.preventDefault(); return; }
    e.preventDefault();
    upload.classList.add('arrastando'); 
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


/* LIMPAR O FORMULÁRIO AO FECHAR O MODAL */
document.addEventListener('close', e => {
    if (!e.target.matches('dialog')) return;

    const form = e.target.querySelector('form');
    if (form) form.reset();

    const upload = e.target.querySelector('.upload');
    if (upload) limparPreview(upload);
}, true);

// Bloqueia o envio se nenhuma imagem foi escolhida
document.addEventListener('submit', e => {
    const form = e.target;
    const input = form.querySelector('.upload input[type="file"]');
    if (!input) return;

    if (input.files.length === 0) {
        e.preventDefault();
        const upload = form.querySelector('.upload');
        upload.classList.add('erro');
    }
});

// Remove o destaque de erro quando uma imagem é escolhida
document.addEventListener('change', e => {
    if (e.target.matches('.upload input[type="file"]')) {
        e.target.closest('.upload').classList.remove('erro');
    }
});