using Atendimento.Application.DTOs;

namespace Atendimento.Application.Interfaces;

public interface IAutenticacaoService
{
    Task<TokenDto> LoginAsync(LoginDto dto);
}
