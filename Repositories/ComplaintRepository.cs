using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.DTOs.GetDTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using Complaint_Report_Registering_API.Entities;
using Microsoft.EntityFrameworkCore;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Repositories
{
    public class ComplaintRepository(AppDbContext context) : IComplaint
    {
        public async Task<bool> AddComplaintType(ComplaintTypePostDTO complaintType)
        {
            var findComplaintType = await context.ComplaintTypes
            .FirstOrDefaultAsync(ct => ct.Type == complaintType.ComplaintType);
            if (findComplaintType != null)
            {
                return false;
            }
            context.ComplaintTypes.Add(new ComplaintType
            {
                Type = complaintType.ComplaintType,
            });
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddStatus(StatusPostDTO status)
        {
            var findStatus = await context.Status
            .FirstOrDefaultAsync(s => s.Type == status.Status);
            if (findStatus != null)
            {
                return false;
            }
            context.Status.Add(new Status
            {
                Type = status.Status,
            });
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<CreateComplaintResponse> CreateComplaint(ComplaintPostDTO complaint, string userId)
        {
            var newComplaint = new Complaint
            {
                Title = complaint.Title,
                ComplaintTypeId = complaint.ComplaintTypeId,
                StatusId = 1,
                Description = complaint.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ApplicationUserId = userId,
                FileUrl = complaint.FileUrl,
                DepartmentId = complaint.DepartmentId
            };
            context.Complaints.Add(newComplaint);
            await context.SaveChangesAsync();
            return new CreateComplaintResponse(true, newComplaint.ComplaintId);
        }

        public async Task<bool> EditComplaint(ComplaintPostDTO complaint, int complaintId)
        {
            try
            {
                var complaintToUpdate = await context.Complaints.FindAsync(complaintId);
                if (complaintToUpdate == null)
                {
                    return false;
                }
                complaintToUpdate.Title = complaint.Title;
                complaintToUpdate.ComplaintTypeId = complaint.ComplaintTypeId;
                complaintToUpdate.StatusId = 1;
                complaintToUpdate.Description = complaint.Description;
                complaintToUpdate.FileUrl = complaint.FileUrl;
                complaintToUpdate.UpdatedAt = DateTime.UtcNow;
                context.Complaints.Update(complaintToUpdate);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> RemoveComplaint(int complaintId)
        {
            try
            {
                var complaint = await context.Complaints.FindAsync(complaintId);
                if (complaint == null)
                {
                    return false;
                }
                context.Complaints.Remove(complaint);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> FindComplaint(int complaintId)
        {
            var complaint = await context.Complaints.FindAsync(complaintId);
            if (complaint == null)
            {
                return false;
            }
            return true;
        }

        public async Task<ComplaintGetDTO> ViewComplaint(int complaintId)
        {
            var complaint = await context.Complaints
            .Include(c => c.ComplaintType)
            .Include(c => c.Status)
            .Include(c => c.ApplicationUser)
            .ThenInclude(u => u!.Department)
            .FirstOrDefaultAsync(c => c.ComplaintId == complaintId);
            if (complaint == null) return new ComplaintGetDTO { };
            var complaintToDisplay = new ComplaintGetDTO
            {
                ComplaintId = complaint!.ComplaintId,
                Title = complaint!.Title,
                ComplaintType = complaint.ComplaintType?.Type,
                Status = complaint.Status?.Type,
                Description = complaint.Description,
                FileUrl = complaint.FileUrl,
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
                Department = complaint.Department!.DepartmentName,
                User = new ProfileDTO
                {
                    UserId = complaint.ApplicationUser?.Id,
                    FirstName = complaint.ApplicationUser?.FirstName,
                    LastName = complaint.ApplicationUser?.LastName,
                    Email = complaint.ApplicationUser?.Email,
                    ImageUrl = complaint.ApplicationUser?.ImageUrl,
                    Department = complaint.ApplicationUser!.Department!.DepartmentName,
                }
            };
            return complaintToDisplay;
        }

        public async Task<List<ComplaintGetDTO>> ViewComplaints()
        {
            var complaints = await context.Complaints
            .Include(c => c.ComplaintType)
            .Include(c => c.Status)
            .Include(c => c.ApplicationUser)
            .ThenInclude(u => u!.Department)
            .ToListAsync();
            var complaintsToDisplay = complaints.Select(c => new ComplaintGetDTO
            {
                ComplaintId = c.ComplaintId,
                Title = c.Title,
                ComplaintType = c.ComplaintType?.Type,
                Status = c.Status?.Type,
                Description = c.Description,
                FileUrl = c.FileUrl,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Department = c.Department!.DepartmentName,
                User = new ProfileDTO
                {
                    UserId = c.ApplicationUser?.Id,
                    FirstName = c.ApplicationUser?.FirstName,
                    LastName = c.ApplicationUser?.LastName,
                    Email = c.ApplicationUser?.Email,
                    ImageUrl = c.ApplicationUser?.ImageUrl,
                    Department = c.ApplicationUser!.Department!.DepartmentName,
                }
            }).ToList();
            return complaintsToDisplay;
        }

        public async Task<bool> FindComplaintType(int complaintTypeId)
        {
            var complaintType = await context.ComplaintTypes.FindAsync(complaintTypeId);
            if (complaintType == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<ComplaintTypeGetDTO>> ViewComplaintTypes()
        {
            var complaintTypes = await context.ComplaintTypes.ToListAsync();
            var complaintTypesToDisplay = complaintTypes.Select(ct => new ComplaintTypeGetDTO
            {
                ComplaintTypeId = ct.ComplaintTypeId,
                ComplaintType = ct.Type,
            }).ToList();
            return complaintTypesToDisplay;
        }

        public async Task<GeneralResponse> RemoveComplaintType(int complaintTypeId)
        {
            try
            {
                var findComplaint = await context.Complaints.FirstOrDefaultAsync(c => c.ComplaintTypeId == complaintTypeId);
                if (findComplaint != null)
                {
                    return new GeneralResponse(false, "There is complaint associated with the type");
                }
                var complaintType = await context.ComplaintTypes.FindAsync(complaintTypeId);
                if (complaintType == null)
                {
                    return new GeneralResponse(false, "Complaint type not found");
                }
                context.ComplaintTypes.Remove(complaintType);
                await context.SaveChangesAsync();
                return new GeneralResponse(true, "Complaint type deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new GeneralResponse(false, "Something went wrong");
            }
        }
    }
}