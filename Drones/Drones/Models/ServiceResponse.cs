using System;

namespace Drones.Models
{
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Object returned
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Success flag
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// Result message
        /// </summary>
        public string Message { get; set; } = null;
    }
}
