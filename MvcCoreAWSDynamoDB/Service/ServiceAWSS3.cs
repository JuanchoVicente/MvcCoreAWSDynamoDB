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
        private String bucketName;
        private IAmazonS3 awsClient;

        public ServiceAWSS3(IAmazonS3 awsclient
            , IConfiguration configuration)
        {
            this.awsClient = awsclient;
            this.bucketName = configuration["AWS:Bucktname"];
        }

        public async Task<bool> UploadFile(Stream stream
            , String fileName)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                Key = fileName,
                BucketName = bucketName
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

        public async Task<List<String>> GetS3Files()
        {
            ListVersionsResponse response =
                await this.awsClient.ListVersionsAsync
                (this.bucketName);
            return response.Versions.Select(x => x.Key).ToList();
        }

        public async Task<bool> DeleteFile(String fileName)
        {
            DeleteObjectResponse response =
                await this.awsClient.DeleteObjectAsync
                    (this.bucketName, fileName);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Stream> GetFile(String fileName)
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
