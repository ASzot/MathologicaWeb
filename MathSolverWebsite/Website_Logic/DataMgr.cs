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
        private CloudBlobContainer _container = null;

        public void Init()
        {
            // If the user's system clock is more than 15 minutes off then the blob container cannot be created.
            try
            {
                _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                _client = _storageAccount.CreateCloudBlobClient();
                _container = _client.GetContainerReference("userinputs");

                if (_container.CreateIfNotExists())
                {
                    // Configure the container for access.
                    var permissions = _container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    _container.SetPermissions(permissions);
                }
            }
            catch (Exception e)
            {
                _storageAccount = null;
                _client = null;
                _container = null;
            }
        }

        public void CreateBlobData(string input, string selectedEval, string solveResult)
        {
            if (_container == null)
                return;

            String finalDataStr = String.Format("At {0}: \n\n Input was {1} \n Selected command was {2} \n Output was: {3}", DateTime.Now.ToString(),
                input, selectedEval, solveResult);

            CloudBlockBlob blobData = _container.GetBlockBlobReference(DateTime.Now.ToString() + ":" + Guid.NewGuid().ToString());
            blobData.UploadTextAsync(finalDataStr);
        }
    }
}