using AdvertApi.Models;
using System;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public interface IAdvertStorageService
    {
        Task<String> Add(AdvertModel model);

        Task<bool> Confirm(ConfirmAdvertModel model);
    }
}
