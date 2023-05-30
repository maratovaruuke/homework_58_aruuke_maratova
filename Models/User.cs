using Microsoft.Extensions.Hosting;
using homework_59_aruuke_maratova.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
﻿using System.Collections.Generic;


namespace homework_59_aruuke_maratova.Models        
{
    public class User : IdentityUser
    {
        public byte[] Avatar { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public virtual int GenderId
        {
            get => (int)this.Gender;
            set => Gender = (Gender)value;
        }
        [EnumDataType(typeof(Gender))]
        [Required]
        public Gender Gender { get; set; }

        public List<Post> Posts { get; set; }
        public List<Follow> Followers { get; set; }
        public List<Follow> Following { get; set; }
        public User()
        {
            Posts = new List<Post>();
            Followers = new List<Follow>();
            Following = new List<Follow>();
        }
    }
}
