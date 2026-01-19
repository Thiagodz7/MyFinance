using MassTransit;
using MyFinance.Domain.Events;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Worker.Consumers
{
    // IConsumer<T>: Diz "Eu sei ler mensagens do tipo T"
    public class LancamentoCriadoConsumer : IConsumer<LancamentoCriadoEvent>
    {
        // Injetamos o repositório
        private readonly IContaRepository _contaRepository;
        private readonly IUnitOfWork _uow;
        //private readonly ICategoriaRepository _categoriaRepository;
        public LancamentoCriadoConsumer(IContaRepository contaRepository, IUnitOfWork uow)
        {
            _contaRepository = contaRepository;
            _uow = uow;
            //_categoriaRepository = categoriaRepository;
        }

        public async Task Consume(ConsumeContext<LancamentoCriadoEvent> context)
        {
            var evento = context.Message;

            Console.WriteLine($"[PROCESSANDO] Atualizando saldo para conta {evento.ContaId}...");

            // 1. Busca a conta no banco
            var conta = await _contaRepository.GetByIdAsync(evento.ContaId);

            if (conta != null)
            {
                // 2. Aplica a regra de negócio (Soma ou Subtrai)
                // O método AtualizarSaldo já encapsula a lógica "SaldoAtual += Valor"
                conta.AtualizarSaldo(evento.Valor);

                // 3. Salva a alteração
                await _contaRepository.UpdateAsync(conta);

                await _uow.CommitAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCESSO] Saldo atualizado! Novo saldo: R$ {conta.SaldoAtual}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERRO] Conta {evento.ContaId} não encontrada.");
                Console.ResetColor();
            }
        }
    }
}