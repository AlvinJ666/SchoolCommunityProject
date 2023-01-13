using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab4.Data;
using lab4.Models;
using Azure.Storage.Blobs;
using lab4.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Azure;
using System.IO;

namespace lab4.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly SchoolCommunityContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "advertisements";

        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: Advertisements
        public IActionResult Index(string id)
        {
            var community = _context.Communities.Where(m => m.ID == id).FirstOrDefault();
            var viewModel = new AdsViewModel();
            viewModel.Community = community;
            var adsCommunity = _context.AdvertisementCommunity.Where(m => m.CommunityID == id).Include(m => m.Advertisement);

            viewModel.Advertisements = adsCommunity.Select(m => m.Advertisement);

            return View(viewModel);
        }

        // GET: Advertisements/Create
        public IActionResult Create(string id)
        {
            return View(_context.Communities.Find(id));
        }

        // POST: Advertisements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,Url")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advertisement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advertisement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string CommunityID, IFormFile file)
        {

            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName.ToLower());
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            }

            try
            {
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(file.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }

                // add the photo to the database if it uploaded successfully
                var image = new Advertisement();
                image.Url = blockBlob.Uri.AbsoluteUri;
                image.FileName = file.FileName;

                _context.Advertisements.Add(image);
                _context.SaveChanges();

                var adsCommunity = new AdsCommunity();
                adsCommunity.AdvertisementID = image.Id;
                adsCommunity.CommunityID = CommunityID;

                _context.AdvertisementCommunity.Add(adsCommunity);
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {
                View("Error");
            }

            return RedirectToAction("Index", new { id = CommunityID });
        }

        // GET: Advertisements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // POST: Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            string communityID = _context.AdvertisementCommunity.Where(m => m.AdvertisementID == id).First().CommunityID;


            BlobContainerClient containerClient;
            // Get the container and return a container client object
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(advertisement.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.Advertisements.Remove(advertisement);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction("Index", new { id = communityID });
        }
    }
}
