using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Auth.Commands;

public class SignUpCommandHandler(IUserRepository userRepository, IHashService hashService, IUnitOfWork unitOfWork)
    : IRequestHandler<SignUpCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(SignUpCommand request, CancellationToken ct)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, ct))
            throw new BusinessException("Ya existe una cuenta registrada con ese email.");

        var hash = hashService.ComputeHash(request.Password);
        var user = new User(request.Email, request.Name, hash, "Customer");
        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return new RegisterResponse(user.Id, user.Email, user.Name, user.Role);
    }
}
