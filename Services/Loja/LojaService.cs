using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;

namespace EllosPratas.Services.Loja
{
    public class LojaService : ILojaInterface
    {
        private readonly BancoContext _context;

        public LojaService(BancoContext context)
        {
            _context = context;
        }

        public async Task<LojaModel> CadastrarLoja(LojaCadastroDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Busca a Cidade para pegar o nome
                    var cidade = await _context.Cidades
                        .Include(c => c.Estado)
                        .FirstOrDefaultAsync(c => c.id_cidade == dto.id_cidade);

                    if (cidade == null)
                    {
                        throw new Exception("Cidade não encontrada.");
                    }

                    // 2. Cria a Loja SEM id_endereco (será preenchido depois se necessário)
                    var novaLoja = new LojaModel
                    {
                        nome_loja = dto.nome_loja,
                        telefone = dto.telefone,
                        id_endereco = 0 // Temporário
                    };
                    _context.Loja.Add(novaLoja);
                    await _context.SaveChangesAsync(); // Salva para obter o id_loja

                    // 3. Cria o Endereço associado à Loja
                    var novoEndereco = new EnderecoModel
                    {
                        rua = dto.rua,
                        numero = dto.numero,
                        bairro = dto.bairro,
                        cidade = cidade.nome_cidade,
                        id_loja = novaLoja.id_loja, // Agora temos o id_loja válido
                        id_cidade = cidade.id_cidade
                    };
                    _context.Enderecos.Add(novoEndereco);
                    await _context.SaveChangesAsync(); // Salva o endereço

                    // 4. Atualiza o id_endereco na Loja
                    novaLoja.id_endereco = novoEndereco.id_endereco;
                    await _context.SaveChangesAsync();

                    // 5. Confirma a transação
                    await transaction.CommitAsync();

                    return novaLoja;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<LojaListagemDto>> ListarLojas()
        {
            return await _context.Loja
                .Include(l => l.Endereco)
                    .ThenInclude(e => e.Cidade)
                        .ThenInclude(c => c.Estado)
                .Select(l => new LojaListagemDto
                {
                    id_loja = l.id_loja,
                    nome_loja = l.nome_loja,
                    telefone = l.telefone,
                    EnderecoCompleto = (l.Endereco != null && l.Endereco.Cidade != null && l.Endereco.Cidade.Estado != null)
                        ? $"{l.Endereco.rua}, {l.Endereco.numero} - {l.Endereco.bairro}, {l.Endereco.Cidade.nome_cidade} - {l.Endereco.Cidade.Estado.uf}"
                        : "Endereço não cadastrado"
                })
                .ToListAsync();
        }

        public async Task<LojaEdicaoDto?> ObterLojaParaEdicao(int id)
        {
            return await _context.Loja
                .Where(l => l.id_loja == id)
                .Select(l => new LojaEdicaoDto
                {
                    id_loja = l.id_loja,
                    nome_loja = l.nome_loja,
                    telefone = l.telefone,
                    id_endereco = l.Endereco.id_endereco,
                    rua = l.Endereco.rua,
                    numero = l.Endereco.numero,
                    bairro = l.Endereco.bairro,
                    id_cidade = l.Endereco.id_cidade,
                    id_estado = l.Endereco.Cidade.id_estado
                })
                .FirstOrDefaultAsync();
        }

        public async Task<LojaModel> AtualizarLoja(LojaEdicaoDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lojaExistente = await _context.Loja.FindAsync(dto.id_loja);
                    if (lojaExistente == null) throw new Exception("Loja não encontrada.");

                    var enderecoExistente = await _context.Enderecos.FindAsync(dto.id_endereco);
                    if (enderecoExistente == null) throw new Exception("Endereço não encontrado.");

                    var cidade = await _context.Cidades.FindAsync(dto.id_cidade);
                    if (cidade == null) throw new Exception("Cidade não encontrada.");

                    lojaExistente.nome_loja = dto.nome_loja;
                    lojaExistente.telefone = dto.telefone;

                    enderecoExistente.rua = dto.rua;
                    enderecoExistente.numero = dto.numero;
                    enderecoExistente.bairro = dto.bairro;
                    enderecoExistente.id_cidade = dto.id_cidade;
                    enderecoExistente.cidade = cidade.nome_cidade;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return lojaExistente;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}