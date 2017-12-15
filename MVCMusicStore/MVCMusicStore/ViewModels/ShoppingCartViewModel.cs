using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVCMusicStore.Models;

namespace MVCMusicStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        [Key]
        public int Id { get; set; }
        
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}