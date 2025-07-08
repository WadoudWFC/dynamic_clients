using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultipleHttpClient.Application.Interfaces.Admin;
using MultipleHttpClient.Application.Services.Security;

namespace MultipleHtppClient.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [RequireAdmin]
    public class AdminController : ControllerBase
    {
        private readonly IHttpAdminService _adminService;

        public AdminController(IHttpAdminService adminService)
        {
            _adminService = adminService;
        }

        #region User Management - Admin Only

        /// <summary>
        /// Get all users with optional search - Admin only
        /// </summary>
        [HttpPost("users")]
        public async Task<IActionResult> GetAllUsers([FromBody] object request)
        {

            var result = await _adminService.GetAllUsersAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Advanced user search - Admin only
        /// </summary>
        [HttpPost("users/search")]
        public async Task<IActionResult> SearchUsers([FromBody] object request)
        {

            var result = await _adminService.SearchUsersAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Load specific user - Admin only
        /// </summary>
        [HttpPost("users/load")]
        public async Task<IActionResult> LoadUser([FromBody] object request)
        {

            var result = await _adminService.LoadUserAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Create new user - Admin only
        /// </summary>
        [HttpPost("users/create")]
        public async Task<IActionResult> CreateUser([FromBody] object request)
        {

            var result = await _adminService.InsertUserAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Update existing user - Admin only
        /// </summary>
        [HttpPost("users/update")]
        public async Task<IActionResult> UpdateUser([FromBody] object request)
        {

            var result = await _adminService.UpdateUserAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Delete user - Admin only
        /// </summary>
        [HttpPost("users/delete")]
        public async Task<IActionResult> DeleteUser([FromBody] object request)
        {

            var result = await _adminService.DeleteUserAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Validate user from email link - Admin only
        /// </summary>
        [HttpPost("users/validate")]
        public async Task<IActionResult> ValidateUser([FromBody] object request)
        {
            var result = await _adminService.ValidateUserAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Resend email validation - Admin only
        /// </summary>
        [HttpPost("users/resend-validation")]
        public async Task<IActionResult> ResendEmailValidation([FromBody] object request)
        {
            var result = await _adminService.ResendEmailValidationAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        #endregion

        #region Document Management - Admin Only

        /// <summary>
        /// Upload document - Admin only
        /// </summary>
        [HttpPost("documents/insert")]
        public async Task<IActionResult> UploadDocument([FromBody] object request)
        {
            var result = await _adminService.InsertDocumentAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Get all files for a dossier - Admin only
        /// </summary>
        [HttpPost("documents/files")]
        public async Task<IActionResult> GetFiles([FromBody] object request)
        {
            var result = await _adminService.GetFilesAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Get specific file - Admin only
        /// </summary>
        [HttpPost("documents/file")]
        public async Task<IActionResult> GetFile([FromBody] object request)
        {
            var result = await _adminService.GetFileAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Update document - Admin only
        /// </summary>
        [HttpPost("documents/update")]
        public async Task<IActionResult> UpdateDocument([FromBody] object request)
        {
            var result = await _adminService.UpdateDocumentAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Delete document - Admin only
        /// </summary>
        [HttpPost("documents/delete")]
        public async Task<IActionResult> DeleteDocument([FromBody] object request)
        {
            var result = await _adminService.DeleteDocumentAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        #endregion

        #region Advanced Dossier Operations - Admin Only

        /// <summary>
        /// Delete dossier - Admin only
        /// </summary>
        [HttpPost("dossiers/delete")]
        public async Task<IActionResult> DeleteDossier([FromBody] object request)
        {
            var result = await _adminService.DeleteDossierAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Change dossier status - Admin only
        /// </summary>
        [HttpPost("dossiers/change-status")]
        public async Task<IActionResult> ChangeDossierStatus([FromBody] object request)
        {

            var result = await _adminService.ChangeStatusAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        #endregion

        #region Parameter Management - Admin Only

        /// <summary>
        /// Get zones - Admin only
        /// </summary>
        [HttpPost("parameters/zones")]
        public async Task<IActionResult> GetZones([FromBody] object request)
        {
            var result = await _adminService.GetZonesAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get type personne - Admin only
        /// </summary>
        [HttpPost("parameters/type-personne")]
        public async Task<IActionResult> GetTypePersonne([FromBody] object request)
        {
            var result = await _adminService.GetTypePersonneAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get droit profil - Admin only
        /// </summary>
        [HttpPost("parameters/droit-profil")]
        public async Task<IActionResult> GetDroitProfil([FromBody] object request)
        {
            var result = await _adminService.GetDroitProfilAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get droit - Admin only
        /// </summary>
        [HttpPost("parameters/droit")]
        public async Task<IActionResult> GetDroit([FromBody] object request)
        {
            var result = await _adminService.GetDroitAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get affectation zone user - Admin only
        /// </summary>
        [HttpPost("parameters/affectation-zone-user")]
        public async Task<IActionResult> GetAffectationZoneUser([FromBody] object request)
        {
            var result = await _adminService.GetAffectationZoneUserAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get partenaire - Admin only
        /// </summary>
        [HttpPost("parameters/partenaire")]
        public async Task<IActionResult> GetPartenaire([FromBody] object request)
        {
            var result = await _adminService.GetPartenaireAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get local dossier - Admin only
        /// </summary>
        [HttpPost("parameters/local-dossier")]
        public async Task<IActionResult> GetLocalDossier([FromBody] object request)
        {
            var result = await _adminService.GetLocalDossierAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        /// <summary>
        /// Get historique - Admin only
        /// </summary>
        [HttpPost("parameters/historique")]
        public async Task<IActionResult> GetHistorique([FromBody] object request)
        {
            var result = await _adminService.GetHistoriqueAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
        }

        #endregion
    }
}

