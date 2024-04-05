using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using Complaint_Report_Registering_API.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> CreateComplaint(ComplaintPostDTO complaint, string userId)
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
            };
            context.Complaints.Add(newComplaint);
            await context.SaveChangesAsync();
            return true;
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
            .FirstOrDefaultAsync(c => c.ComplaintId == complaintId);
            if (complaint == null) return new ComplaintGetDTO { };
            var complaintToDisplay = new ComplaintGetDTO
            {
                ComplaintId = complaint!.ComplaintId,
                Title = complaint!.Title,
                ComplaintType = complaint.ComplaintType?.Type,
                Status = complaint.Status?.Type,
                Description = complaint.Description,
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
                User = new ProfileDTO
                {
                    UserId = complaint.ApplicationUser?.Id,
                    FirstName = complaint.ApplicationUser?.FirstName,
                    LastName = complaint.ApplicationUser?.LastName,
                    Email = complaint.ApplicationUser?.Email,
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
            .ToListAsync();
            var complaintsToDisplay = complaints.Select(c => new ComplaintGetDTO
            {
                ComplaintId = c.ComplaintId,
                Title = c.Title,
                ComplaintType = c.ComplaintType?.Type,
                Status = c.Status?.Type,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                User = new ProfileDTO
                {
                    UserId = c.ApplicationUser?.Id,
                    FirstName = c.ApplicationUser?.FirstName,
                    LastName = c.ApplicationUser?.LastName,
                    Email = c.ApplicationUser?.Email,
                }
            }).ToList();
            return complaintsToDisplay;
        }
    }
}
