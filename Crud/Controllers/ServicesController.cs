using Crud.Contracts;
using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.Models.Entities.Services;
using Crud.Service;
using Crud.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Crud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly IServicesService _serviceService;
        public readonly ApplicationDBContext dbContext;

        public ServicesController(ApplicationDBContext dbContext, IServicesService _serviceService)
        {
            this.dbContext = dbContext;
            this._serviceService = _serviceService;
        }

        /*Drone Services*/
        [HttpPost("post-drone-service")]
        public async Task<IActionResult> DroneService([FromBody] DroneViewModel request)
        {
            var result = await _serviceService.DroneServiceAsync(request);
            return Ok(result);
        }

        [HttpGet("get-drone-services-data")]
        public async Task<IActionResult> GetContactus()
        {
            var result = await _serviceService.GetDroneServiceDataAsync();
            return Ok(result);
        }

        [HttpPost("send-reply")]
        public async Task<IActionResult> SendReply([FromBody] SendReplyRequest request)
        {
            var result = await _serviceService.SendReplyDroneService(request);
            return Ok(result);
        }

        /*SM Consulting Services*/
        [HttpPost("post-sm-consulting-service")]
        public async Task<IActionResult> SMConsultingService([FromBody] SocialMediaConsultingViewModel request)
        {
            var result = await _serviceService.SMConsultingServiceAsync(request);
            return Ok(result);
        }

        [HttpGet("get-sm-consulting-services-data")]
        public async Task<IActionResult> GetSMConsultingService()
        {
            var result = await _serviceService.GetSMConsultingData();
            return Ok(result);
        }

        [HttpPost("send-reply-sm-consulting")]
        public async Task<IActionResult> SendReplySMConsultingService([FromBody] SendReplyRequest request)
        {
            var result = await _serviceService.SendReplySMConsulting(request);
            return Ok(result);
        }


        /*SM Creation Services*/
        [HttpPost("post-sm-creation-service")]
        public async Task<IActionResult> SMCreationService([FromBody] SocialMediaCreationViewModel request)
        {
            var result = await _serviceService.SMCreationServiceAsync(request);
            return Ok(result);
        }

        [HttpGet("get-sm-creation-services-data")]
        public async Task<IActionResult> GetSMCreationService()
        {
            var result = await _serviceService.GetSMCreationData();
            return Ok(result);
        }

        [HttpPost("send-reply-sm-creation")]
        public async Task<IActionResult> SendReplySMCreationService([FromBody] SendReplyRequest request)
        {
            var result = await _serviceService.SendReplySMCreation(request);
            return Ok(result);
        }


        /*Video Editing Services*/
        [HttpPost("post-video-editing-service")]
        public async Task<IActionResult> VideoEditingService([FromBody] VideoEditingViewModel request)
        {
            var result = await _serviceService.VideoEditingServiceAsync(request);
            return Ok(result);
        }

        [HttpGet("get-video-editing-services-data")]
        public async Task<IActionResult> GetVideoEditingService()
        {
            var result = await _serviceService.GetVideoEditingData();
            return Ok(result);
        }

        [HttpPost("send-reply-video-editing")]
        public async Task<IActionResult> SendReplyVideoEditingService([FromBody] SendReplyRequest request)
        {
            var result = await _serviceService.SendReplyVideoEditing(request);
            return Ok(result);
        }


        /*Software Services*/
        [HttpPost("post-software-service")]
        public async Task<IActionResult> SoftwareService([FromBody] WebAppViewModel request)
        {
            var result = await _serviceService.SoftwareServiceAsync(request);
            return Ok(result);
        }

        [HttpGet("get-software-services-data")]
        public async Task<IActionResult> GetSoftwareService()
        {
            var result = await _serviceService.GetSoftwareData();
            return Ok(result);
        }

        [HttpPost("send-reply-software")]
        public async Task<IActionResult> SendReplySoftwareService([FromBody] SendReplyRequest request)
        {
            var result = await _serviceService.SendReplySoftware(request);
            return Ok(result);
        }

    }
}
