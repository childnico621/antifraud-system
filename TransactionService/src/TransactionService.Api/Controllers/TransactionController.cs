using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs;
using TransactionService.Application.UseCases.CreateTransaction;
using TransactionService.Application.UseCases.GetDailyTotal;
using TransactionService.Application.UseCases.GetTransactionById;
using TransactionService.Application.UseCases.UpdateTransactionStatus;

namespace TransactionService.Api.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ICreateTransactionUseCase _createUseCase;
        private readonly IGetTransactionByIdUseCase _getUseCase;

        private readonly IUpdateTransactionStatusUseCase _updateUseCase;
        private readonly IGetDailyTotalUseCase _getDailyTotalUseCase;

        public TransactionController(
            ICreateTransactionUseCase createUseCase,
            IGetTransactionByIdUseCase getUseCase,
            IUpdateTransactionStatusUseCase updateUseCase,
            IGetDailyTotalUseCase getDailyTotalUseCase)
        {
            _createUseCase = createUseCase;
            _getUseCase = getUseCase;
            _updateUseCase = updateUseCase;
            _getDailyTotalUseCase = getDailyTotalUseCase;
        }

        /// <summary>
        /// Crear una nueva transacción
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _createUseCase.ExecuteAsync(command);
            return CreatedAtAction(nameof(GetTransaction), new { transactionExternalId = result.TransactionExternalId }, result);
        }

        /// <summary>
        /// Obtener una transacción por su TransactionExternalId
        /// </summary>
        [HttpGet("{transactionExternalId:guid}")]
        public async Task<ActionResult<TransactionResponseDto>> GetTransaction(Guid transactionExternalId)
        {
            var result = await _getUseCase.ExecuteAsync(transactionExternalId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Actualizar el estado de una transacción
        /// </summary>
        [HttpPut("{transactionExternalId:guid}/status")]
        public async Task<IActionResult> UpdateTransactionStatus(Guid transactionExternalId, [FromBody] UpdateTransactionStatusCommand command)
        {
            if (transactionExternalId != command.TransactionExternalId)
                return BadRequest("El ID en la ruta y en el cuerpo no coinciden.");

            var success = await _updateUseCase.ExecuteAsync(command);

            if (!success)
                return BadRequest("El estado proporcionado no es válido.");

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceAccountId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("daily-total")]
        public async Task<ActionResult<decimal>> GetDailyTotal([FromQuery] Guid sourceAccountId, [FromQuery] DateTime date)
        {
            var total = await _getDailyTotalUseCase.ExecuteAsync(sourceAccountId, date);
            return Ok(new { total });
        }

    }
}
