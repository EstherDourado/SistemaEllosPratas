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
            // Usa uma transação para garantir que Loja e Endereço sejam guardados juntos
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Procura ou cria o Estado
                    var estado = await _context.Estados.FirstOrDefaultAsync(e => e.uf == dto.uf);
                    if (estado == null)
                    {
                        estado = new EstadoModel { uf = dto.uf, nome_estado = dto.estado };
                        _context.Estados.Add(estado);
                        await _context.SaveChangesAsync(); // Guarda para obter o ID do estado
                    }

                    // 2. Procura ou cria a Cidade, associada ao Estado
                    var cidade = await _context.Cidades.FirstOrDefaultAsync(c => c.nome_cidade == dto.cidade && c.id_estado == estado.id_estado);
                    if (cidade == null)
                    {
                        cidade = new CidadeModel { nome_cidade = dto.cidade, id_estado = estado.id_estado };
                        _context.Cidades.Add(cidade);
                        await _context.SaveChangesAsync(); // Guarda para obter o ID da cidade
                    }

                    // 3. Cria a Loja
                    var novaLoja = new LojaModel
                    {
                        nome_loja = dto.nome_loja,
                        telefone = dto.telefone
                    };
                    _context.Loja.Add(novaLoja);
                    await _context.SaveChangesAsync(); // Guarda para obter o ID da loja

                    // 4. Cria o Endereço e associa-o à Loja e à Cidade
                    var novoEndereco = new EnderecoModel
                    {
                        rua = dto.rua,
                        numero = dto.numero,
                        bairro = dto.bairro,
                        cidade = dto.cidade,
                        id_loja = novaLoja.id_loja,
                        id_cidade = cidade.id_cidade
                    };
                    _context.Enderecos.Add(novoEndereco);
                    await _context.SaveChangesAsync();

                    // 5. Confirma a transação
                    await transaction.CommitAsync();

                    return novaLoja;
                }
                catch (Exception)
                {
                    // Se algo falhar, desfaz tudo
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<LojaListagemDto>> ListarLojas()
        {
            return await _context.Loja
                .Include(l => l.Endereco) // Inclui o endereço relacionado
                .ThenInclude(e => e.Cidade) // A partir do endereço, inclui a cidade
                .ThenInclude(c => c.Estado) // A partir da cidade, inclui o estado
                .Select(l => new LojaListagemDto
                {
                    id_loja = l.id_loja,
                    nome_loja = l.nome_loja,
                    telefone = l.telefone,
                    EnderecoCompleto = l.Endereco != null
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

                    // Atualiza os dados da loja
                    lojaExistente.nome_loja = dto.nome_loja;
                    lojaExistente.telefone = dto.telefone;

                    // Atualiza os dados do endereço
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
