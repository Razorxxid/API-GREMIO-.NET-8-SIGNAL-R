using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Groups;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Core.Services.Interfaces;
using PWA_GREMIO_API.Infraestructure;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Group = PWA_GREMIO_API.Core.Entities.Groups.Group;


namespace PWA_GREMIO_API.Core.Services.Implementations
{
    [Authorize]
    public class GroupHubService : Hub<IClientService>
    {

        private readonly IUnitOfWork _unitOfWork;


        public GroupHubService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;


        }

        // Adds the user to a group and creates a UserOfGroup entry if it doesn't exist
        public async Task AddToGroup(int groupId)
        {

            try
            {

                var groupRepository = _unitOfWork.GetRepository<Group, int?>();
                var UserOfGroupRepository = _unitOfWork.GetRepository<UserOfGroup, int?>();
                var userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>();
                var userAuthRRepository = _unitOfWork.GetRepository<UserAuth, int?>();

                string? email = Context.UserIdentifier;

                int? userId = await userAuthRRepository.GetProyected(f => f.Email == email, f => f.Id);

                Group? groupDb = await groupRepository.Get(groupId);
               
                if (groupDb is null)
                {
                    Console.WriteLine("Group not found in db GroupDb");
                    return;
                }

                await Groups.AddToGroupAsync(Context.ConnectionId, groupDb.Name);

                var searchInGroup =  groupDb.UsersOfGroup.Find(f => f.UserSignalRId == userId);

                if (searchInGroup is null) { 
                    Console.WriteLine("Group not found in db UsersOfGroups");
                    return;
                }

                UserSignalR? userSignalR = await userSignalRRepository.Get(userId);

                if (userSignalR is null)
                {
                    Console.WriteLine("User not found in db");
                    return;
                }

                if (searchInGroup is not null)
                {
                    Console.WriteLine("user is assigned to the group");
                    return;

                } else
                {
                  
                    UserOfGroup userOfGroupDb = new()
                    {
                        Group = groupDb,
                        UserSignalRId = (int)userId,
                        GroupId = groupId,
                        UserSignalR = userSignalR,
                        RoleInGroup = "NormalUser"
                    };

                    groupDb.UsersOfGroup.Add(userOfGroupDb);

                    groupRepository.Update(groupDb);

                    await _unitOfWork.Commit();

                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        // Removes the user from a group and deletes the UserOfGroup entry if it exists
        public async Task RemoveFromGroup(int groupId)
        {


            try
            {

                var groupRepository = _unitOfWork.GetRepository<Group, int?>();
                var userAuthRRepository = _unitOfWork.GetRepository<UserAuth, int?>();

                string? email = Context.UserIdentifier;



                int? userId = await userAuthRRepository.GetProyected(f => f.Email == email, f => f.Id);

                Group? groupDb = await groupRepository.Get(groupId);

                UserOfGroup? searchInGroup = groupDb?.UsersOfGroup.Find(f => f.UserSignalRId == userId);


                if (groupDb is null)
                {
                    Console.WriteLine("Group not found in db");
                    return;
                }

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupDb.Name);

                if (searchInGroup is not null)
                {
                    groupDb.UsersOfGroup.Remove(searchInGroup);
                    groupRepository.Update(groupDb);
                    await _unitOfWork.Commit();
                }
                else
                {
                    Console.WriteLine("user is not assigned in DB group");
                    return;
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Sends an announcement to all groups specified in the DestinationGroupsIds of the SendAnnoucementDto
        public async Task SendAnnoucementToGroups(SendAnnoucementDto annoucement)
        {
            var annoucementRepository = _unitOfWork.GetRepository<AnnoucementEntity, int?>();
            var annoucementOfGroupRepository = _unitOfWork.GetRepository<AnnoucementOfGroup, int?>();
            var userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>();
            var userAuthRRepository = _unitOfWork.GetRepository<UserAuth, int?>();

            string? email = Context.UserIdentifier;

            int? userId = await userAuthRRepository.GetProyected(f => f.Email == email, f => f.Id);

            int? userSignalRId = await userSignalRRepository.GetProyected(f => f.UserAuthId == userId, f => f.Id);
            var userSignalR = await userSignalRRepository.GetProyected(f => f.Id == userSignalRId, f => f);

            if (userSignalRId is null)
            {
                Console.WriteLine("Author User not found in db");
                return;
            }

            var annoucementEntity = new AnnoucementEntity
            {
             
                AuthorUserSignalRId = userSignalRId,
                Title = annoucement.Title,
                Text = annoucement.Text,
                Image_url = annoucement.ImageUrl,
                DestinationGroupsIds = annoucement.DestinationGroupsIds,
                DateOfExpiration = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(30),
                TimeOfExpiration = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(23).AddMinutes(59),
                CreatedBy = userSignalRId

            };

            await annoucementRepository.Add(annoucementEntity);


            await _unitOfWork.Commit();


            var receiveAnnoucementDTO = new ReceiveAnnoucementDto
            {
                Id = annoucementEntity.Id,
                Title = annoucementEntity.Title,
                Text = annoucementEntity.Text,
                ImageUrl = annoucementEntity.Image_url,
              
            };

            if (annoucement.DestinationGroupsIds is not null)
            {
                IEnumerable<AnnoucementOfGroup> newDbAnnoucementsOfGroup = new List<AnnoucementOfGroup>();
                var groupRepository = _unitOfWork.GetRepository<Group, int?>();

                IEnumerable<Group?> groupsDb = await groupRepository.GetProyectedMany(f => annoucement.DestinationGroupsIds.Contains(f.Id), f => f);
                if (groupsDb is null)
                {
                    Console.WriteLine("Groups not found in db");
                    return;
                }
                foreach (var group in annoucement.DestinationGroupsIds)
                {

                    var groupEntity = groupsDb.FirstOrDefault(item => item.Id == group); 


                    if (annoucementEntity is null)
                    {
                        Console.WriteLine("Annoucement not found in db");
                        return;
                    }

                    if(userSignalRId is null) return;

                    newDbAnnoucementsOfGroup.Append(new AnnoucementOfGroup
                    {
                      
                        UserSignalR = userSignalR,
                        UserSignalRId = userSignalRId,
                        AnnoucementId = annoucementEntity.Id,
                        GroupId = groupEntity.Id,
                    });

                

                    await Clients.Group(groupEntity.Name)
                   .SendAsync("ReceiveMessage", receiveAnnoucementDTO);
                    Console.WriteLine("SendingMessageTo: " + groupEntity.Name);

                }

                await annoucementOfGroupRepository.AddMany(newDbAnnoucementsOfGroup);

                await _unitOfWork.Commit();


            }

        }



        // Removes an announcement from all groups it was sent to
        public async Task RemoveAnnoucementFromAllGroups(int annoucementId)
        {

            try {

                var userAuthRRepository = _unitOfWork.GetRepository<UserAuth, int?>();
                var userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>(); 

                string? email = Context.UserIdentifier;

                int? userId = await userAuthRRepository.GetProyected(f => f.Email == email, f => f.Id);

                int? userSignalRId = await userSignalRRepository.GetProyected(f => f.UserAuthId == userId, f => f.Id);

                if (userId is null)
                {
                    Console.WriteLine("user Identifier not found");
                    return;
                }

                var annoucementRepository = _unitOfWork.GetRepository<AnnoucementEntity, int?>();

                AnnoucementEntity? annoucement = await annoucementRepository.Get(annoucementId);

                if (annoucement is null)
                {
                    Console.WriteLine("Annoucement not found in db");
                    return;
                } else if (annoucement.AuthorUserSignalRId != userSignalRId)
                {
                    Console.WriteLine("Created by " + annoucement.CreatedBy);
                    Console.WriteLine("UserSignalRId " + userSignalRId);
                    Console.WriteLine("User is not the owner of the annoucement");
                    return;
                } else
                {
                    if (annoucement.DestinationGroupsIds is null)
                    {
                        Console.WriteLine("Annoucement has no groups");
                        return;
                    }
                   
                    var groupRepository = _unitOfWork.GetRepository<Group, int?>();

                    IEnumerable<string?> groupsNamesToDelete = await groupRepository.GetProyectedMany(f => annoucement.DestinationGroupsIds.Contains(f.Id), f => f.Name);

                    foreach (var group in groupsNamesToDelete)
                    {
                      if (group is not null)
                      await  Clients.Group(group)
                            .RemoveMessageFromList( annoucementId);
                    }

                    var annoucementOfGroupRepository = _unitOfWork.GetRepository<AnnoucementOfGroup, int?>();

                    IEnumerable<AnnoucementOfGroup?> groupsDb = await annoucementOfGroupRepository.GetProyectedMany(f => f.AnnoucementId == annoucementId, f => f);

                    List<AnnoucementOfGroup?> groupListToDelete = groupsDb.ToList();

                    if (groupListToDelete.Count() == 0)
                    {
                        Console.WriteLine("Groups not found correctly");
                        return;
                    }
                    annoucementOfGroupRepository.DeleteMany(groupListToDelete);


                    await _unitOfWork.Commit();


                    annoucementRepository.Delete(annoucement);
                  
                    await _unitOfWork.Commit();

                }


            } catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Called when a user connects to the hub, reconnection logic is implemented here
        public override async Task OnConnectedAsync()
        {
            try
            {
                var userAuthRRepository = _unitOfWork.GetRepository<UserAuth, int?>();

                string? email = Context.UserIdentifier;

                int? userId = await userAuthRRepository.GetProyected(f => f.Email == email, f => f.Id);

                if (userId is null)
                {
                    Console.WriteLine("User Identifier not found");
                    return;
                }
                var _userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>();
                var groupRepository = _unitOfWork.GetRepository<Group, int?>();


                var userIdSignalR = await _userSignalRRepository.GetProyected(f => f.UserAuthId == userId, f => f.Id);

           

                // Reconectar al usuario a sus grupos


                var usersOfGroupsRepository = _unitOfWork.GetRepository<UserOfGroup, int?>();
                var groupsOfUser = await usersOfGroupsRepository.GetProyectedMany(f => f.UserSignalRId == userIdSignalR, f => f.Group.Name);

                
                if (groupsOfUser is null)
                {
                    Console.WriteLine("User has no groups");
                    return;
                }
                foreach (var groupname in groupsOfUser)
                {
                    if (groupname is not null)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, groupname);

                        Console.WriteLine("User reconnected to group " + groupname);
                    }
                }


               

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }


        

    }
}