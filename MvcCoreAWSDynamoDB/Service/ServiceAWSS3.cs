using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MvcCoreAWSDynamoDB.Service
{
    public class ServiceAWSS3
    {
        private IAmazonS3 awsClient;
        private String bucketName;

        public ServiceAWSS3(IAmazonS3 awsclient
            , IConfiguration configuration)
        {
            this.awsClient = awsclient;
            this.bucketName = configuration["AWSS3:BucketName"];
        }

        public async Task<bool> UploadFileAsync(Stream stream
            , String fileName)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,                
                Key = fileName,
                BucketName = this.bucketName               
            };


            PutObjectResponse response =
                await this.awsClient.PutObjectAsync(request);

            if(response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        public async Task<List<String>> GetS3FilesAsync()
        {
            ListVersionsResponse response =
                await this.awsClient.ListVersionsAsync
                (this.bucketName);            
            return response.Versions.Select(x => x.Key).Distinct().ToList();
        }

        public async Task<bool> DeleteFileAsync(String fileName)
        {
            var deleteobjrequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                VersionId = null
            };
            await awsClient.DeleteObjectAsync(deleteobjrequest);

            DeleteObjectResponse response =
                await this.awsClient.DeleteObjectAsync
                (this.bucketName, fileName);
            
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Stream> GetFileAsync(String fileName)
        {
            GetObjectResponse response =
                await this.awsClient.GetObjectAsync(this.bucketName, fileName);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
            else
            {
                return null;
            }
        }

    }
}
