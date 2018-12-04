using System.Collections.Generic;
using Organizer.Models;

namespace Organizer.Interfaces
{
    public interface IOrganizerService
    {
        List<OrganizerModel> GetAll();
        int Create(OrganizerCreateModel model);
        OrganizerModel GetById(int Id);
        UserWithRole GetCurrentUser(int id);

        void Update(OrganizerUpdateModel model);
        void Delete(int Id);
        int CreateAudioFile(int songId, string fileName);
        List<SongFileModel> GetAllFiles(int songId);
        void UpdateFile(OrganizerUpdateFileModel model);
        void DeleteFile(int Id);
    }
}