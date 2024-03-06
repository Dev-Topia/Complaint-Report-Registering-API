using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
using Microsoft.EntityFrameworkCore;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Repositories
{
    public class ComplaintRepository(AppDbContext context) : IComplaint
    {
        public async Task<GeneralResponse> RegsiterComplaint(Complaint complaint)
        {
            if (complaint is null) return new GeneralResponse(false, "Model is empty!");
            context.Complaint.Add(complaint);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Register Successfully!");
        }
        public async Task<ArrayResponse> ViewAllComplaint()
        {
            var complaints = await context.Complaint.ToListAsync();
            return new ArrayResponse(true, complaints);
        }
        public async Task<ObjectResponse> ViewComplaint(string id)
        {
            var complaint = await context.Complaint.FindAsync(new Guid(id));
            if (complaint == null)
            {
                return new ObjectResponse(false, "Complaint not found");
            }
            var complaintGetDTO = new ComplaintGetDTO
            {
                Id = complaint.Id,
                Title = complaint.Title,
                Type = complaint.Type,
                Description = complaint.Description,
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
            };
            return new ObjectResponse(true, complaintGetDTO);
        }
        public async Task<GeneralResponse> DeleteComplaint(string id)
        {
            var complaint = await context.Complaint.FindAsync(new Guid(id));
            if (complaint == null)
            {
                return new GeneralResponse(false, "Complaint not found!");
            }
            context.Complaint.Remove(complaint);
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Complaint deleted successfully!");
        }
        public async Task<GeneralResponse> EditComplaint(string id, ComplaintPostDTO updatedComplaint)
        {
            var complaint = await context.Complaint.FindAsync(new Guid(id));
            if (complaint == null)
            {
                return new GeneralResponse(false, "Complaint not found!");
            }
            complaint.Title = updatedComplaint.Title;
            complaint.Type = updatedComplaint.Type;
            complaint.Description = updatedComplaint.Description;
            complaint.UpdatedAt = DateTime.UtcNow;
            context.Complaint.Update(complaint);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Complaint updated successfully");
        }
    }
}
