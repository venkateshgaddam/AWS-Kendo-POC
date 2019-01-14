using System;
﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ApplicationTracker.Models;
using ApplicationTracker.Common;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace ApplicationTracker.Controllers
{
	public partial class GridController : Controller
    {
		
		IList<Movie> result = clsDbOperations.GetMoviesResult();
        private jagdevEntities entities = new jagdevEntities();

        public ActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> Edit([DataSourceRequest]DataSourceRequest request, Movie movieModel)
        {
            Movie movieDetail = entities.Movies.Where(m => m.Movie_Id == movieModel.Movie_Id).FirstOrDefault();
            if (movieDetail != null)
            {
                movieDetail.MovieName = movieModel.MovieName;
                movieDetail.Producer = movieModel.Producer;
                movieDetail.DateOfRelease = movieModel.DateOfRelease;
                entities.Entry(movieDetail).State = System.Data.Entity.EntityState.Modified;
                await entities.SaveChangesAsync();
            }
            result = entities.Movies.Where(m => m.Movie_Id == movieModel.Movie_Id).ToList();
            //clsDbOperations.Movies.Where(m => m.Movie_Id == movieModel.Movie_Id).ToList();
            return Json(result.ToDataSourceResult(request));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([DataSourceRequest]DataSourceRequest request, int movieId)
        {
            Movie movieDetail = entities.Movies.Where(m => m.Movie_Id == movieId).FirstOrDefault();
            if (movieDetail != null)
            {
                entities.Movies.Remove(movieDetail);
                await entities.SaveChangesAsync();
            }
            return Json(entities.Movies.ToList().ToDataSourceResult(request));
        }

        public ActionResult ExportToExcel() {

            return View(result);
        
        }

        [HttpPost]
        public ActionResult ExportToExcel(FormCollection col)
        {
            var gv = new GridView();
            gv.DataSource = result;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xlsx");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }
    }
}

