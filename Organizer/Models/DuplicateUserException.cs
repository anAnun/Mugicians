﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException()
        {
        }
    }
}