﻿using System.Collections.Generic;

namespace PetroGastStation.Common.Responses
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public List<T> ResultList { get; set; }
    }
}
