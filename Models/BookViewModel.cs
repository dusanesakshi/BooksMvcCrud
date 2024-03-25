﻿using System.ComponentModel.DataAnnotations;

namespace FreshMvcCurd.Models
{
    public class BookViewModel
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Should be greater than or equal to 1")]
        public int price { get; set; }
    }
}
