document.addEventListener('DOMContentLoaded', function () {

    const ctx = document.getElementById('graficoVendas');

    const meses = JSON.parse(ctx.dataset.meses);
    const valores = JSON.parse(ctx.dataset.valores);

    const totalAnual = valores.reduce((total, valor) => total + valor, 0);

    const formatarMoeda = valor => valor.toLocaleString('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    });

    new Chart(ctx, {
        type: 'line',

        data: {
            labels: meses,

            datasets: [{
                label: 'Total anual: ' + formatarMoeda(totalAnual),
                data: valores,

                borderColor: '#0015B8',
                backgroundColor: 'rgba(0, 86, 255, 0.12)',

                borderWidth: 3,
                pointBackgroundColor: '#0056FF',
                pointBorderColor: '#FFFFFF',
                pointBorderWidth: 2,
                pointRadius: 5,
                pointHoverRadius: 7,

                tension: 0.3,
                fill: true
            }]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false,

            plugins: {
                legend: {
                    display: true,
                },

                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return formatarMoeda(context.parsed.y);
                        }
                    }
                }
            },

            scales: {
                x: {
                    grid: {
                        display: false
                    }
                },

                y: {
                    beginAtZero: true,

                    ticks: {
                        callback: function(value) {
                            return formatarMoeda(value);
                        }
                    }
                }
            }
        }
    });


    // Gráfico de tipo de pedido
    const ctxPedidos = document.getElementById('graficoPedidos');

    const pedidosRetirada = Number(ctxPedidos.dataset.retirada);
    const pedidosEntrega = Number(ctxPedidos.dataset.entrega);

    new Chart(ctxPedidos, {
        type: 'doughnut',

        data: {
            labels: ['Retirada', 'Entrega'],

            datasets: [{
                data: [pedidosRetirada, pedidosEntrega],

                backgroundColor: [
                    '#9007DA',
                    '#0015B8'
                ],

                borderWidth: 0,
                hoverOffset: 8
            }]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false,

            plugins: {
                legend: {
                    position: 'bottom'
                },

                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.label + ': ' + context.raw + ' pedidos';
                        }
                    }
                }
            },

            cutout: '70%'
        }
    });


    // Gráfico de produtos mais vendidos
    const ctxProdutos = document.getElementById('graficoProdutos');

    const nomesProdutos = JSON.parse(ctxProdutos.dataset.nomes);
    const quantidadesProdutos = JSON.parse(ctxProdutos.dataset.quantidades);

    new Chart(ctxProdutos, {
        type: 'pie',

        data: {
            labels: nomesProdutos,

            datasets: [{
                label: 'Unidades vendidas',
                data: quantidadesProdutos,

                backgroundColor: [
                    '#FD0DC1',
                    '#9007DA',
                    '#0015B8',
                    '#F50000',
                    '#FF7900'
                ],

                borderWidth: 2,
                borderColor: '#FFFFFF',

                hoverOffset: 8
            }]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false,

            plugins: {
                legend: {
                    display: true,
                    position: 'bottom'
                },

                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.label + ': ' + context.raw + ' unidades';
                        }
                    }
                }
            }
        }
    });


    // Gráfico de pedidos por status
    const ctxStatus = document.getElementById('graficoStatus');

    const nomesStatus = JSON.parse(ctxStatus.dataset.nomes);
    const quantidadesStatus = JSON.parse(ctxStatus.dataset.quantidades);

    new Chart(ctxStatus, {
        type: 'bar',

        data: {
            labels: nomesStatus,

            datasets: [{
                label: 'Quantidade de pedidos',
                data: quantidadesStatus,

                backgroundColor: [
                    '#FF7900',
                    '#FFBA00',
                    '#FD0DC1',
                    '#0015B8',
                    '#9007DA',
                    '#F50000'
                ],

                borderRadius: 5,
                barThickness: 25
            }]
        },

        options: {
            indexAxis: 'y',
            responsive: true,
            maintainAspectRatio: false,

            plugins: {
                legend: {
                    display: false
                },

                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.raw + ' pedidos';
                        }
                    }
                }
            },

            scales: {
                x: {
                    beginAtZero: true,

                    grid: {
                        display: false
                    },

                    ticks: {
                        precision: 0
                    }
                },

                y: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
});