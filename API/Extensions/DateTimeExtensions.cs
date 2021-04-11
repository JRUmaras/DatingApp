using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates age of a person with respect to a reference date.
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <param name="referenceDate">Date in reference to which the age will be calculated.</param>
        /// <returns>Age in years.</returns>
        public static int CalculateAge(this DateTime dateOfBirth, DateTime referenceDate)
        {
            var deltaYears = referenceDate.Year - dateOfBirth.Date.Year;

            // Check if the person has already had his or hers birthday on the ref year
            return referenceDate >= dateOfBirth.Date.AddYears(deltaYears)
                ? deltaYears
                : deltaYears - 1;
        }
    }
}
