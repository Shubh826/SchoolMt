using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class LoginMDL
    {
        [Required(ErrorMessage = "Please Enter User Name.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }

        public string ForgetPwdUserName { get; set; }

        public string EmailID { get; set; }
        public bool RememberMe { get; set; }
        public string FullName { get; set; }
    }

    public class ResetPassword
    {
        [Required]
        public string Password { get; set; }

        public string ResetCode { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public string EmailID { get; set; }
    }
}
