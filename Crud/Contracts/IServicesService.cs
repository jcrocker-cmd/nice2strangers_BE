using Crud.Models;
using Crud.Models.Entities;
using Crud.Models.Entities.Services;
using Crud.ViewModel;

namespace Crud.Contracts
{
    public interface IServicesService
    {
        /*Drone Services*/
        Task<DroneModel> DroneServiceAsync(DroneViewModel request);
        Task<List<DroneViewModel>> GetDroneServiceDataAsync();
        Task<string> SendReplyDroneService(SendReplyRequest request);

        /*SM Consulting*/
        Task<SocialMediaConsultingModel> SMConsultingServiceAsync(SocialMediaConsultingViewModel request);
        Task<List<SocialMediaConsultingViewModel>> GetSMConsultingData();
        Task<string> SendReplySMConsulting(SendReplyRequest request);

        /*SM Creation*/
        Task<SocialMediaCreationModel> SMCreationServiceAsync(SocialMediaCreationViewModel request);
        Task<List<SocialMediaCreationViewModel>> GetSMCreationData();
        Task<string> SendReplySMCreation(SendReplyRequest request);

        /*Video Editing*/
        Task<VideoEditingModel> VideoEditingServiceAsync(VideoEditingViewModel request);
        Task<List<VideoEditingViewModel>> GetVideoEditingData();
        Task<string> SendReplyVideoEditing(SendReplyRequest request);


        /*Software Services*/
        Task<WebAppModel> SoftwareServiceAsync(WebAppViewModel request);
        Task<List<WebAppViewModel>> GetSoftwareData();
        Task<string> SendReplySoftware(SendReplyRequest request);
    }
}
