using Atata;
using COE.Examples.Components;

/// <summary>
/// Extensions package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Extensions
{
    /// <summary>
    /// Validation Message Extension class
    /// </summary>
    public static class ValidationMessageExtensions
    {
        /// <summary>
        ///   Bes the required.
        /// </summary>
        /// <typeparam name="TOwner">
        ///   The type of the owner.
        /// </typeparam>
        /// <param name="should">
        ///   The should.
        /// </param>
        /// <returns></returns>
        public static TOwner BeRequired<TOwner>(this IFieldVerificationProvider<string, ValidationMessage<TOwner>, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.EndWithIgnoringCase("field is required.");
        }

        /// <summary>
        ///   Haves the incorrect format.
        /// </summary>
        /// <typeparam name="TOwner">
        ///   The type of the owner.
        /// </typeparam>
        /// <param name="should">
        ///   The should.
        /// </param>
        /// <returns></returns>
        public static TOwner HaveIncorrectFormat<TOwner>(this IFieldVerificationProvider<string, ValidationMessage<TOwner>, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.Equal("The email field must be a valid email.");
        }

        /// <summary>
        ///   Haves the minimum length.
        /// </summary>
        /// <typeparam name="TOwner">
        ///   The type of the owner.
        /// </typeparam>
        /// <param name="should">
        ///   The should.
        /// </param>
        /// <param name="length">
        ///   The length.
        /// </param>
        /// <returns></returns>
        public static TOwner HaveMinLength<TOwner>(this IFieldVerificationProvider<string, ValidationMessage<TOwner>, TOwner> should, int length)
            where TOwner : PageObject<TOwner>
        {
            return should.EndWithIgnoringCase($"field must be at least {length} characters.");
        }

        /// <summary>
        ///   Haves the maximum length.
        /// </summary>
        /// <typeparam name="TOwner">
        ///   The type of the owner.
        /// </typeparam>
        /// <param name="should">
        ///   The should.
        /// </param>
        /// <param name="length">
        ///   The length.
        /// </param>
        /// <returns></returns>
        public static TOwner HaveMaxLength<TOwner>(this IFieldVerificationProvider<string, ValidationMessage<TOwner>, TOwner> should, int length)
            where TOwner : PageObject<TOwner>
        {
            return should.Equal($"maximum length is {length}");
        }
    }
}
