using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;

namespace EllosPratas.Services.Loja
{
    public class LojaService(BancoContext _context) : ILojaInterface
    {
        public async Task<LojaModel> CadastrarLoja(LojaCadastroDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Busca a Cidade para pegar o nome
                    var cidade = await _context.Cidades
                        .Include(c => c.Estado)
                        .FirstOrDefaultAsync(c => c.Id_cidade == dto.Id_cidade);

                    if (cidade == null)
                    {
                        throw new Exception("Cidade não encontrada.");
                    }

                    // 2. Cria a Loja SEM id_endereco (será preenchido depois se necessário)
                    var novaLoja = new LojaModel
                    {
                        Nome_loja = dto.Nome_loja,
                        Telefone = dto.Telefone,
                        Id_endereco = "0" // Temporário
                    };
                    _context.Loja.Add(novaLoja);
                    await _context.SaveChangesAsync(); // Salva para obter o id_loja

                    // 3. Cria o Endereço associado à Loja
                    var novoEndereco = new EnderecoModel
                    {
                        Rua = dto.Rua,
                        Numero = dto.Numero,
                        Bairro = dto.Bairro,
                        Cidade = cidade.Nome_cidade,
                        Id_loja = novaLoja.Id_loja, // Agora temos o id_loja válido
                        Id_cidade = cidade.Id_cidade
                    };
                    _context.Enderecos.Add(novoEndereco);
                    await _context.SaveChangesAsync(); // Salva o endereço

                    // 4. Atualiza o id_endereco na Loja
                    novaLoja.Id_endereco = novoEndereco.Id_endereco.ToString();
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
                    Id_loja = l.Id_loja,
                    Nome_loja = l.Nome_loja,
                    Telefone = l.Telefone,
                    EnderecoCompleto = (l.Endereco != null && l.Endereco.Cidade != null && l.Endereco.Cidade.Estado != null)
                        ? $"{l.Endereco.Rua}, {l.Endereco.Numero} - {l.Endereco.Bairro}, {l.Endereco.Cidade.Nome_cidade} - {l.Endereco.Cidade.Estado.Uf}"
                        : "Endereço não cadastrado"
                })
                .ToListAsync();
        }

        public async Task<LojaEdicaoDto?> ObterLojaParaEdicao(int id)
        {
            return await _context.Loja
                .Where(l => l.Id_loja == id)
                .Select(l => new LojaEdicaoDto
                {
                    Id_loja = l.Id_loja,
                    Nome_loja = l.Nome_loja,
                    Telefone = l.Telefone,
                    Id_endereco = l.Endereco.Id_endereco,
                    Rua = l.Endereco.Rua,
                    Numero = l.Endereco.Numero,
                    Bairro = l.Endereco.Bairro,
                    Id_cidade = l.Endereco.Id_cidade,
                    Id_estado = l.Endereco.Cidade.Id_estado
                })
                .FirstOrDefaultAsync();
        }

        public async Task<LojaModel> AtualizarLoja(LojaEdicaoDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lojaExistente = await _context.Loja.FindAsync(dto.Id_loja);
                    if (lojaExistente == null) throw new Exception("Loja não encontrada.");

                    var enderecoExistente = await _context.Enderecos.FindAsync(dto.Id_endereco);
                    if (enderecoExistente == null) throw new Exception("Endereço não encontrado.");

                    var cidade = await _context.Cidades.FindAsync(dto.Id_cidade);
                    if (cidade == null) throw new Exception("Cidade não encontrada.");

                    lojaExistente.Nome_loja = dto.Nome_loja;
                    lojaExistente.Telefone = dto.Telefone;

                    enderecoExistente.Rua = dto.Rua;
                    enderecoExistente.Numero = dto.Numero;
                    enderecoExistente.Bairro = dto.Bairro;
                    enderecoExistente.Id_cidade = dto.Id_cidade;
                    enderecoExistente.Cidade = cidade.Nome_cidade;

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