using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.Runtime;
using Amazon.EC2;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.EC2.Model;
using System.Web.Helpers;
using Newtonsoft.Json;
using System.Data;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;
using Amazon.IdentityManagement.Model;


namespace AWSDemo.Models
{
    public class UploadBucket
    {

        static string bucketName = "azv-s3str-dppl";
        public string dest = "";
        public DirectoryInfo dInfo;
        static string keyName = "F_Entertainment.pdf";
        static string filePath = "C:\\Users\\vgaddam\\Desktop\\12 Forward Entertainment [RemitMelanie Wester]-2017630.pdf";
        static string destPath = "C:\\Users\\vgaddam\\Desktop";
        static string directorypath = ConfigurationManager.AppSettings["DirectoryPath"].ToString();
        static AmazonS3Client client;
        CredentialProfile basicProfile;
        AWSCredentials awsCredentials;
        //AWSS3Provider fs = new AWSS3Provider();
        public bool upload = true;
        static string AccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        static string SecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        static string AWSURL = ConfigurationManager.AppSettings["AWSServiceURL"];
        public AWSCredentials credentials = new BasicAWSCredentials(AccessKey, SecretKey);
        public AmazonS3Config config = new AmazonS3Config();
        public IAmazonS3 s3Client;



        //public AETN.CloudWrapper.FileStore.AWSS3Provider clsProvide = new AETN.CloudWrapper.FileStore.AWSS3Provider(AccessKey, SecretKey, AWSURL);



        public void GetObjects(int id = 0)
        {
            try
            {
                //var res = fs.GetFile(bucketName, @"Archive_Docs/LIFETIME/MISC/I+WANT+A+BABY+AKA+REAL+WOMEN+SYNOPSIS.PDF");
                //var client = new AmazonIdentityManagementServiceClient(credentials);
                //var response = client.GetUser(new GetUserRequest
                //{
                //    //UserName = "ppas-cabbie-dev"
                //    UserName = "Admin"
                //});

                //User user = response.User;
                //var requestgroups = new ListGroupsForUserRequest(user.UserName);
                //var groups = client.ListGroupsForUser(requestgroups);

                //var perm = client.ListPolicies(new ListPoliciesRequest { });
                //string name = "";
                //foreach (var item in groups.Groups)
                //{
                //    name = item.GroupName;
                //}
                //var requestPolicies = new ListUserPoliciesRequest
                //{
                //    UserName = user.UserName
                //};
                //var p = new ListGroupPoliciesRequest
                //{
                //    GroupName = name,
                //    MaxItems=10
                //};
                //var poli = client.ListGroupPolicies(p);
                //var responsePolicies = client.ListUserPolicies(requestPolicies);
                //string policyname ="";
                //foreach (var policy in responsePolicies.PolicyNames)
                //{
                //    policyname = policy[0].ToString();
                //}
                config.RegionEndpoint = RegionEndpoint.USEast1;
                config.ServiceURL = ConfigurationManager.AppSettings["AWSServiceURL"].ToString();
                s3Client = new AmazonS3Client(credentials, config);
                using (s3Client)
                {
                    //Uri uri = s3Client.GetPreSignedURL(
                    ListObjectsRequest req = new ListObjectsRequest
                    {
                        BucketName = bucketName
                    };
                    var result = s3Client.ListObjects(req);
                    //S3Bucket bucket = s3Client.getbucket
                    List<string> keysList = new List<string>();
                    
                    //for (int i = 0; i < result.S3Objects.Count; i++)
                    //{
                    //    string fileName = result.S3Objects[i].Key;
                    //    if (fileName.Contains("/"))
                    //    {
                    //        fileName = fileName.Substring(fileName.LastIndexOf("/") + 1);
                    //    }
                    //    keysList.Add(fileName);
                    //    if (i == id)
                    //    {
                    //        keyName = "TestingFile.pdf";
                    //        // UploadImages(id);
                    //    }
                    //}

                    //GetObjectRequest request = new GetObjectRequest
                    //{
                    //    BucketName = bucketName,
                    //    Key = keyName
                    //};

                    //using (GetObjectResponse response = s3Client.GetObject(request))
                    //{
                    keysList.Add("Forward_Entertainment.pdf");
                    keysList.Add("Forward_Entertainment1.pdf");
                    keysList.Add("Forward_Entertainment2.pdf");
                        ListObjectsRequest listRequest = new ListObjectsRequest
                        {
                            BucketName = bucketName
                        };
                        List<string> keyNames = new List<string>();
                        S3Bucket bucket = new S3Bucket();
                        Dictionary<string,object> dicObj = new Dictionary<string,object>();
                         //s3Client.DownloadToFilePath(bucketName, keyName, destPath,dicObj);
                        var listResponse = s3Client.ListObjects(listRequest);
                        for (int i = 0; i < listResponse.S3Objects.Count; i++)
                        {
                            keyNames.Add(listResponse.S3Objects[i].Key.ToString());
                            if (!Directory.Exists(directorypath))
                            {
                                dInfo = new DirectoryInfo(directorypath);
                                dInfo.Create();
                            }
                            GetObjectRequest getrequest = new GetObjectRequest
                            {
                                BucketName = bucketName,
                                Key = keyNames[i]
                            };
                            GetObjectResponse response = s3Client.GetObject(getrequest);
                            dest = Path.Combine(directorypath, keyNames[i]);
                            if (!System.IO.File.Exists(dest))
                            {
                                response.WriteResponseStreamToFile(dest);
                            }
                            
                        }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }

        }

        public void UploadImages(int id = 0)
        {
            try
            {
                // var result = clsProvide.ListFile("azv-s3str-ddeb");
                config.RegionEndpoint = RegionEndpoint.USEast1;
                config.ServiceURL = ConfigurationManager.AppSettings["AWSServiceURL"].ToString();
                client = new AmazonS3Client(credentials, config);
                //string str = "Test";

                using (client)
                {
                    PutObjectRequest putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        FilePath = filePath,
                        ContentType = MimeMapping.GetMimeMapping(filePath)
                    };

                    //GetPreSignedUrlRequest getRequest = new GetPreSignedUrlRequest
                    //{
                    //    BucketName = bucketName,
                    //    Key = keyName,
                    //    Verb=HttpVerb.GET,
                    //    Expires = DateTime.Now.AddYears(3)
                    //};
                    //string Url = s3Client.GetPreSignedURL(getRequest);

                    PutObjectResponse response = client.PutObject(putRequest);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }

        }
    }
}