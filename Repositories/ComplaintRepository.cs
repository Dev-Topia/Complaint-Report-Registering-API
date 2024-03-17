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
        public async Task<ListResponse> ViewAllComplaint()
        {
            var complaints = await context.Complaints
            .Include(c => c.ComplaintType)
            .Include(c => c.StatusType)
            .Include(c => c.ApplicationUser)
            .ToListAsync();

            var complaintGetDTOs = complaints.Select(c => new ComplaintGetDTO
            {
                Id = c.Id,
                Title = c.Title,
                Type = c.ComplaintType?.Type,
                Status = c.StatusType?.Type,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                User = new ProfileDTO
                {
                    Id = c.ApplicationUser?.Id,
                    FirstName = c.ApplicationUser?.FirstName,
                    LastName = c.ApplicationUser?.LastName,
                    Email = c.ApplicationUser?.Email
                }
            }).ToList();

            return new ListResponse(true, complaintGetDTOs);
        }

        public async Task<ObjectResponse> ViewComplaint(string id)
        {
            var complaint = await context.Complaints
            .Include(c => c.ComplaintType)
            .Include(c => c.StatusType)
            .Include(c => c.ApplicationUser)
            .FirstOrDefaultAsync(c => c.Id == new Guid(id));
            if (complaint == null)
            {
                return new ObjectResponse(false, "Complaints not found");
            }
            var complaintGetDTO = new ComplaintGetDTO
            {
                Id = complaint.Id,
                Title = complaint.Title,
                Type = complaint.ComplaintType?.Type,
                Status = complaint.StatusType?.Type,
                Description = complaint.Description,
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
                User = new ProfileDTO
                {
                    Id = complaint.ApplicationUser?.Id,
                    FirstName = complaint.ApplicationUser?.FirstName,
                    LastName = complaint.ApplicationUser?.LastName,
                    Email = complaint.ApplicationUser?.Email
                }
            };
            return new ObjectResponse(true, complaintGetDTO);
        }

        public async Task<GeneralResponse> RegsiterComplaint(ComplaintPostDTO complaintPostDTO, string userId)
        {
            if (complaintPostDTO is null) return new GeneralResponse(false, "Model is empty!");
            var newComplaint = new Complaint
            {
                Id = Guid.NewGuid(),
                Title = complaintPostDTO.Title,
                ComplaintTypeId = complaintPostDTO.ComplaintTypeId,
                StatusTypeId = complaintPostDTO.StatusTypeId,
                Description = complaintPostDTO.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ApplicationUserId = userId,
                FileUrl = complaintPostDTO.FileUrl,
            };
            context.Complaints.Add(newComplaint);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Register Successfully!");
        }

        public async Task<GeneralResponse> DeleteComplaint(string id)
        {
            var complaint = await context.Complaints.FindAsync(new Guid(id));
            if (complaint == null)
            {
                return new GeneralResponse(false, "Complaints not found!");
            }
            context.Complaints.Remove(complaint);
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Complaints deleted successfully!");
        }

        public async Task<GeneralResponse> EditComplaint(string id, ComplaintPostDTO updatedComplaint)
        {
            var complaint = await context.Complaints.FindAsync(new Guid(id));
            if (complaint == null)
            {
                return new GeneralResponse(false, "Complaints not found!");
            }
            complaint.Title = updatedComplaint.Title;
            complaint.ComplaintTypeId = updatedComplaint.ComplaintTypeId;
            complaint.StatusTypeId = updatedComplaint.StatusTypeId;
            complaint.Description = updatedComplaint.Description;
            complaint.UpdatedAt = DateTime.UtcNow;
            context.Complaints.Update(complaint);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Complaints updated successfully");
        }

        public async Task<GeneralResponse> AddComplaintType(string typeName)
        {
            var newComplaintType = new ComplaintType
            {
                Id = Guid.NewGuid(),
                Type = typeName,
            };
            context.ComplaintTypes.Add(newComplaintType);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Complaint Type Added Successfully!");
        }

        public async Task<GeneralResponse> AddStatusType(string typeName)
        {
            var newStatusType = new StatusType
            {
                Id = Guid.NewGuid(),
                Type = typeName
            };
            context.StatusTypes.Add(newStatusType);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Status Type Added Successfully!");
        }
    }
}
