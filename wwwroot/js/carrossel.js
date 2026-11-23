const carrosseis = document.querySelectorAll(".carrossel");

carrosseis.forEach(carrossel => {
    const imagens = carrossel.querySelectorAll("img");
    const anterior = carrossel.querySelector(".btn-anterior");
    const proximo = carrossel.querySelector(".btn-proximo");

    let imagemAtual = 0;

    function atualizarCarrossel() {
        imagens.forEach((imagem, index) => {
            imagem.classList.toggle("ativo", index === imagemAtual);
        });
    }

    proximo.addEventListener("click", () => {
        imagemAtual = (imagemAtual + 1) % imagens.length;
        atualizarCarrossel();
    });

    anterior.addEventListener("click", () => {
        imagemAtual = (imagemAtual - 1 + imagens.length) % imagens.length;
        atualizarCarrossel();
    });

    function passarSlideAutomaticamente(){
        imagemAtual = (imagemAtual + 1) % imagens.length;
        atualizarCarrossel();
    }

    setInterval(() => {

        passarSlideAutomaticamente();
    }, 5000);

    atualizarCarrossel();
});