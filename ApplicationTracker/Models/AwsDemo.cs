using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Amazon;
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
using System.Net;
using Amazon.S3.Util;
using System.Diagnostics;



namespace AWSDemo.Models
{
    public class AwsDemo
    {
        static object strEnum = new object();

        static AmazonS3Client client;
        CredentialProfile basicProfile;
        static string bucketName = "Venkat699";
        AWSCredentials awsCredentials;
        static Dictionary<string, string> dicImage = new Dictionary<string, string>();
        static Dictionary<int, string> dictionary = new Dictionary<int, string>();



        public void dictionayoperations()
        {

            //dicImage.Add("Key1", "1");
            //dicImage.Add("Key2", "2");
            //dicImage.Add("Key3", "3");
            //dicImage.Add("Key4", "4");
            //Hashtable hashtable = new Hashtable();
            //Hashtable hashtable2 = new Hashtable();
            //hashtable.Add("Area", 1);
            //hashtable.Add("Perimeter", 2);
            //hashtable.Add("Mortgage", 3);
            string jsonData = "{ 'FirstName':1, 'LastName':2 }";
            var details = JObject.Parse(jsonData);
            var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            string str = "1,2";
            //hashtable.ContainsValue(str);

            List<string> list = new List<string>();
            List<string> diclist = new List<string>();
            var strarr = str.SkipWhile(n => n == ',').ToArray();
            list = str.Split(new char[] { ',' }).ToList();
            diclist = dicImage.Values.ToList();
            dicImage.Remove(dicImage.FirstOrDefault(x => x.Key == list[0]).Value);
            for (int i = 0; i < list.Count; i++)
            {
                if (dicImage.ContainsValue(list[i]))
                {
                    dicImage.Remove(dicImage.FirstOrDefault(x => x.Value == list[i]).Key);
                }
            }
            //hashtable2 = str.Split(arr);
            //bool val = hashtable.ContainsValue(list.ToString());
            dictionary = str.ToDictionary(m => (int)m, x => x.ToString());
            str = "1,2";

            //strEnum = Enum.Parse(typeof(stringEnum), str);

            //if (dicImage.Values.Contains(list(0)))
            //{

            //}
            // dicImage = dicImage.Values.ToDictionary(x => x.ToString(), y => y);
            //List<int> list = dicImage.Values.ToList();
            //dicImage = dicImage.Values.ToList();
            //dicImage=dicImage.Where(i=>i.Value.ToString().Contains(dictionary.Values.ToString()));
            //dicImage = dicImage.Where();

        }

        public void demo()
        {
            try
            {
                var sharedFile = new SharedCredentialsFile();
                if (sharedFile.TryGetProfile("basic_profile", out basicProfile) &&
                 AWSCredentialsFactory.TryGetAWSCredentials(basicProfile, sharedFile, out awsCredentials))
                {
                    using (client = new AmazonS3Client(awsCredentials, basicProfile.Region))
                    {
                        // Create a new configuration request and add two rules    
                        CORSConfiguration configuration = new CORSConfiguration
                        {
                            Rules = new System.Collections.Generic.List<CORSRule>
                        {
                          new CORSRule
                          {
                            Id = "CORSRule1",
                            AllowedMethods = new List<string> {"PUT", "POST", "DELETE"},
                            AllowedOrigins = new List<string> {"http://*.example.com"}
                          },
                          new CORSRule
                          {
                            Id = "CORSRule2",
                            AllowedMethods = new List<string> {"GET"},
                            AllowedOrigins = new List<string> {"*"},
                            MaxAgeSeconds = 3000,
                            ExposeHeaders = new List<string> {"x-amz-server-side-encryption"}
                          }
                        }
                        };

                        // Add the configuration to the bucket 
                        PutCORSConfiguration(configuration);

                        // Retrieve an existing configuration 
                        configuration = GetCORSConfiguration();

                        // Add a new rule.
                        configuration.Rules.Add(new CORSRule
                        {
                            Id = "CORSRule3",
                            AllowedMethods = new List<string> { "HEAD" },
                            AllowedOrigins = new List<string> { "http://www.example.com" }
                        });

                        // Add the configuration to the bucket 
                        PutCORSConfiguration(configuration);

                        // Verify that there are now three rules
                        configuration = GetCORSConfiguration();
                        Console.WriteLine();
                        Console.WriteLine("Expected # of rulest=3; found:{0}", configuration.Rules.Count);
                        Console.WriteLine();
                        Console.WriteLine("Pause before configuration delete. To continue, click Enter...");
                        Console.ReadKey();

                        // Delete the configuration
                        DeleteCORSConfiguration();

                        // Retrieve a nonexistent configuration
                        configuration = GetCORSConfiguration();
                        Debug.Assert(configuration == null);
                    }

                    Console.WriteLine("Example complete.");
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                Console.ReadKey();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }



        public static void PutCORSConfiguration(CORSConfiguration configuration)
        {

            PutCORSConfigurationRequest request = new PutCORSConfigurationRequest
            {
                BucketName = bucketName,
                Configuration = configuration
            };

            var response = client.PutCORSConfiguration(request);
        }

        public static CORSConfiguration GetCORSConfiguration()
        {
            GetCORSConfigurationRequest request = new GetCORSConfigurationRequest
            {
                BucketName = bucketName

            };
            var response = client.GetCORSConfiguration(request);
            var configuration = response.Configuration;
            PrintCORSRules(configuration);
            return configuration;
        }

        public static void DeleteCORSConfiguration()
        {
            DeleteCORSConfigurationRequest request = new DeleteCORSConfigurationRequest
            {
                BucketName = bucketName
            };
            client.DeleteCORSConfiguration(request);
        }

        public static void PrintCORSRules(CORSConfiguration configuration)
        {
            Console.WriteLine();

            if (configuration == null)
            {
                Console.WriteLine("\nConfiguration is null");
                return;
            }

            Console.WriteLine("Configuration has {0} rules:", configuration.Rules.Count);
            foreach (CORSRule rule in configuration.Rules)
            {
                Console.WriteLine("Rule ID: {0}", rule.Id);
                Console.WriteLine("MaxAgeSeconds: {0}", rule.MaxAgeSeconds);
                Console.WriteLine("AllowedMethod: {0}", string.Join(", ", rule.AllowedMethods.ToArray()));
                Console.WriteLine("AllowedOrigins: {0}", string.Join(", ", rule.AllowedOrigins.ToArray()));
                Console.WriteLine("AllowedHeaders: {0}", string.Join(", ", rule.AllowedHeaders.ToArray()));
                Console.WriteLine("ExposeHeader: {0}", string.Join(", ", rule.ExposeHeaders.ToArray()));
            }
        }

        public void EnumerateSecurityGroups()
        {


            try
            {
                var sharedFile = new SharedCredentialsFile();
                if (sharedFile.TryGetProfile("basic_profile", out basicProfile) &&
                 AWSCredentialsFactory.TryGetAWSCredentials(basicProfile, sharedFile, out awsCredentials))
                {

                    AmazonEC2Client amazonEC2 = new AmazonEC2Client(awsCredentials, basicProfile.Region);
                    var request = new DescribeSecurityGroupsRequest();
                    var response = amazonEC2.DescribeSecurityGroups(request);
                    //Json.Write(response, writer);
                    var jsonResponse = JsonConvert.SerializeObject(response);

                    List<SecurityGroup> securityGroup = response.SecurityGroups;
                    List<SecurityGroup> outSecurityGroup = new List<SecurityGroup>();

                    foreach (var item in securityGroup)
                    {
                        outSecurityGroup.Add(item);
                    }
                    outSecurityGroup.RemoveRange(0, 2);

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }

        }


        public void ListBuckets()
        {
            try
            {
                CredentialProfile basicProfile;
                AWSCredentials awsCredentials;
                var sharedFile = new SharedCredentialsFile();
                if (sharedFile.TryGetProfile("basic_profile", out basicProfile) &&
                 AWSCredentialsFactory.TryGetAWSCredentials(basicProfile, sharedFile, out awsCredentials))
                {
                    using (var client = new AmazonS3Client(awsCredentials, basicProfile.Region))
                    {
                        var response = client.ListBuckets();
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }
        }


    }
}