using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApplicationTracker.Models;

namespace ApplicationTracker.Common
{
    public static class clsDbOperations
    {
        private static jagdevEntities entites = new jagdevEntities();

        public static async Task<int> AddUser(Models.Movie movieModel)
        {

            try
            {
                using (entites)
                {
                    entites.Movies.Add(movieModel);
                    await entites.SaveChangesAsync();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool CheckUser(LoginModel User, ref bool isUservalid)
        {
            try
            {
                using (jagdevEntities mEntities = new jagdevEntities())
                {
                    var user = true;
                        //mEntities.TempUsers.Where(u => u.UserName == User.UserName && u.Password == User.Password).First();
                    if (user != null)
                    {
                        isUservalid = true;
                        return isUservalid;
                    }
                    else
                    {
                        isUservalid = false;
                        return isUservalid;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the movies Result
        /// </summary>
        /// <returns></returns>
        public static IList<Movie> GetMoviesResult()
        {
            try
            {
                List<Movie> movieModel = new List<Movie>();

                using (jagdevEntities entites = new jagdevEntities())
                {
                    movieModel = entites.Movies.ToList();
                    return movieModel; 
                }

            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// This method will converts image in byte array format and returns to its caller.
        /// use System.IO namespace regarding streaming concept.
        /// </summary>
        /// <param name="imageLocation"></param>
        /// <returns></returns>
        //public byte[] ReadImageFile(string imageLocation)
        //{
        //    byte[] imageData = null;
        //    FileInfo fileInfo = new FileInfo(imageLocation);
        //    long imageFileLength = fileInfo.Length;
        //    FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    imageData = br.ReadBytes((int)imageFileLength);
        //    return imageData;
        //}


        public static ICollection<MoviesModel> Movies = new List<MoviesModel>()
        {
    
            new MoviesModel(){
                MovieName="Movie1",
                Producer="P1",
                DateOfRelease=DateTime.Now.Date,
                Color="Red"
            },
            new MoviesModel(){
                MovieName="Movie2",
                Producer="P2",
                DateOfRelease=DateTime.Now.AddDays(-85).Date,
                Color="Blue"
            },new MoviesModel(){
                MovieName="Movie3",
                Producer="P3",
                DateOfRelease=DateTime.Now.AddMonths(65).Date,
                Color="Green"
            },new MoviesModel(){
                MovieName="Movie4",
                Producer="P4",
                DateOfRelease=DateTime.Now.AddYears(-7).Date,
                Color="Yellow"
            }
            ,new MoviesModel(){
                MovieName="Movie5",
                Producer="P5",
                DateOfRelease=DateTime.Now.AddYears(-9).Date,
                Color="Grey"
            }
            ,new MoviesModel(){
                MovieName="Movie6",
                Producer="P6",
                DateOfRelease=DateTime.Now.AddYears(-10).Date,
                Color="Black"
            },new MoviesModel(){
                MovieName="Movie7",
                Producer="P4",
                DateOfRelease=DateTime.Now.AddDays(-27).Date,
                Color="Purple"
            }
    
    };


        internal static void AddMovie(MoviesModel model)
        {
            model.Movie_Id = Movies.Count + 1;
            Movies.Add(model);
        }
    }
}