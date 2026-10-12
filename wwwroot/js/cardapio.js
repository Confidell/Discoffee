
const pagina = document.querySelector('.pagina');

document.querySelectorAll('.btn-card-prod').forEach(botao => {
    botao.addEventListener('click', async () => {
        pagina.style.overflow = 'hidden';

        const modal = document.getElementById(botao.dataset.modal);
        modal.classList.add('ativa');

        // LOCALIZAR O CONTAINER DAS OPÇÕES

        const container = modal.querySelector('.container-opcoes');

        if (!container || container.dataset.carregado === 'true') {
            return;
        }

        try{
            const url = container.dataset.url;

            console.log("URL solicitada:", url);

            container.innerHTML = '<p>Carregando opções...</p>';

            const resposta = await fetch(url);

            console.log("Status HTTP:", resposta.status);

            const conteudo = await resposta.text();

            console.log("Resposta do servidor:", conteudo);

            if (!resposta.ok) {
                throw new Error(
                    `Erro HTTP ${resposta.status}: ${conteudo}`
                );
            }

            container.innerHTML = conteudo;
            container.dataset.carregado = 'true';

        } 
        catch(erro) {
            console.error("Erro ao carregar opções:", erro);

            container.innerHTML =
                '<p>Não foi possível carregar as opções.</p>';
        }
    });
});

document.querySelectorAll('.btn-fechar').forEach(botao => {
    botao.addEventListener('click', () => {
        pagina.style.overflow = 'auto';
        botao.closest('.paiModal').classList.remove('ativa');
    });
});
