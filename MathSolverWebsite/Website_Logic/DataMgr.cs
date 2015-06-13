using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MathSolverWebsite.Website_Logic
{
    public class DataMgr
    {
        private CloudStorageAccount _storageAccount = null;
        private CloudBlobClient _client = null;
        private CloudBlobContainer _inputsContainer = null;
        private CloudBlobContainer _messageContainer = null;

        public void Init()
        {
            // If the user's system clock is more than 15 minutes off then the blob container cannot be created.
            try
            {
                _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                _client = _storageAccount.CreateCloudBlobClient();
                _inputsContainer = _client.GetContainerReference("userinputs");
                _messageContainer = _client.GetContainerReference("messages");

                if (_inputsContainer.CreateIfNotExists())
                {
                    // Configure the container for access.
                    var permissions = _inputsContainer.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    _inputsContainer.SetPermissions(permissions);
                }
            }
            catch (Exception e)
            {
                _storageAccount = null;
                _client = null;
                _inputsContainer = null;
            }
        }

        public void CreateBlobData(string input, string selectedEval, string solveResult, bool isLoggedIn)
        {
            if (_inputsContainer == null)
                return;

            CreateBlobData(String.Format("At {0}: \n\n Input was {1} \n Selected command was {2} \n Output was: {3} \n Logged in: " + isLoggedIn.ToString(), DateTime.Now.ToString(),
                input, selectedEval, solveResult), _inputsContainer);
        }

        public void CreateBlobData(string email, string message)
        {
            if (_inputsContainer == null)
                return;

            CreateBlobData(String.Format("Email: {0} \n Message: {1}", email, message), _messageContainer);
        }

        private void CreateBlobData(string data, CloudBlobContainer container)
        {
            if (_inputsContainer == null)
                return;

            CloudBlockBlob blobData = container.GetBlockBlobReference(DateTime.Now.ToString() + ":" + Guid.NewGuid().ToString());
            blobData.UploadTextAsync(data);
        }
    }
}