namespace HuntFlowWIUT.Web.Helpers
{
    public static class VacancyFormHelper
    {
        // Define a HashSet for quick lookups.
        private static readonly HashSet<int> AcademicDivisions = new HashSet<int>
        {
            4931, 4930, 4973, 4978, 4979, 4980, 4981, 4983, 4984, 4982
        };

        /// <summary>
        /// Determines if the provided account division should use the second form.
        /// </summary>
        /// <param name="accountDivision">The account division of the vacancy.</param>
        /// <returns>True if the second form should be used; otherwise, false.</returns>
        public static bool IsAcademicDivision(int accountDivision)
        {
            return AcademicDivisions.Contains(accountDivision);
        }
    }
}
