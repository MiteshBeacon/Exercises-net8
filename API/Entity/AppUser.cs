﻿namespace API.Entity
{
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        //public byte[] PasswordHas { get; set; }
        //public byte[] PasswordSalt { get; set; }
    }
}
