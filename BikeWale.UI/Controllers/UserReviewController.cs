﻿using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {
        // GET: UserReview
        [Route("user-reviews/rate-bike")]
        public ActionResult RateBike()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
        
        // GET: UserReview
        [Route("m/user-reviews/rate-bike")]
        public ActionResult RateBike_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        [Route("user-reviews/write-review")]
        public ActionResult WriteReview()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        [Route("m/user-reviews/write-review")]
        public ActionResult WriteReview_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        [Route("user-reviews/review-summary")]
        public ActionResult ReviewSummary()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
        
        [Route("m/user-reviews/review-summary")]
        public ActionResult ReviewSummary_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}