﻿using System;

namespace Bikewale.Entities.UsersTestimonial
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Jan 2016
    /// Summary     :   User's Testimonial entity
    /// </summary>
    public class UsersTestimonial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HostUrl { get; set; }
        public string UserImgUrl { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
    }
}
