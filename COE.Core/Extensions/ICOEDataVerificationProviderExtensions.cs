using Atata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Atata
{
    public static class ICOEDataVerificationProviderExtensions
    {
        private const string NullString = "null";

        [ThreadStatic]
        private static List<string> _ExceptionResults;

        private static List<string> ExceptionResults {
            get {
                if (_ExceptionResults == null)
                {
                    _ExceptionResults = new List<string>();
                }
                return _ExceptionResults;
            }
            set {
                _ExceptionResults = value;
            }
        }

        internal static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        internal static T CheckNotNull<T>(this T value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);

            return value;
        }

        internal static string CheckNotNullOrEmpty(this string value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (value == string.Empty)
                throw new ArgumentException(ConcatMessage("Should not be empty string.", errorMessage), argumentName);

            return value;
        }

        internal static string CheckNotNullOrWhitespace(this string value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(ConcatMessage("Should not be empty string or whitespace.", errorMessage), argumentName);

            return value;
        }

        internal static IEnumerable<T> CheckNotNullOrEmpty<T>(this IEnumerable<T> collection, string argumentName, string errorMessage = null)
        {
            if (collection == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (!collection.Any())
                throw new ArgumentException(ConcatMessage("Collection should contain at least one element.", errorMessage), argumentName);

            return collection;
        }

        private static string ConcatMessage(string primaryMessage, string secondaryMessage)
        {
            return string.IsNullOrEmpty(secondaryMessage)
                ? primaryMessage
                : $"{primaryMessage} {secondaryMessage}";
        }

        internal static string GetShouldText(this TermMatch match)
        {
            switch (match)
            {
                case TermMatch.Contains:
                    return "contain";
                case TermMatch.Equals:
                    return "equal";
                case TermMatch.StartsWith:
                    return "start with";
                case TermMatch.EndsWith:
                    return "end with";
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match));
            }
        }

        internal static string ConvertValueToString<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue value)
            where TOwner : PageObject<TOwner>
        {
            return TermResolver.ToString(value, provider.ValueTermOptions);
        }

        private static Exception CreateAssertionException<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, TData actual, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            string formattedMessage = args != null && args.Any()
                ? message.FormatWith(args.Select(x => ObjectToString(x)).ToArray())
                : message;

            return should.CreateAssertionException(formattedMessage, ObjectToString(actual));
        }

        internal static Exception CreateAssertionException<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, string expected, string actual)
            where TOwner : PageObject<TOwner>
        {
            string errorMessage = new StringBuilder().
                AppendLine($"Invalid {should.DataProvider.Component.ComponentFullName} {should.DataProvider.ProviderName}.").
                AppendLine($"Expected: {should.GetShouldText()} {expected}").
                AppendLine($"Actual: {actual}").
                ToString();

            var exceptionType = AtataContext.Current.AssertionExceptionType;

            return exceptionType != null
                ? (Exception)Activator.CreateInstance(exceptionType, errorMessage)
                : new AssertionException(errorMessage);
        }


        private static string CollectionToString(IEnumerable collection)
        {
            return CollectionToString(collection.Cast<object>());
        }

        private static string CollectionToString(IEnumerable<object> collection)
        {
            if (collection == null)
                return NullString;
            if (!collection.Any())
                return "<empty>";
            else if (collection.Count() == 1)
                return ObjectToString(collection.First());
            else
                return "<{0}>".FormatWith(string.Join(", ", collection.Select(ObjectToString).ToArray()));
        }

        private static string ObjectToString(object value)
        {
            if (Equals(value, null))
                return NullString;
            else if (value is string)
                return "\"{0}\"".FormatWith(value);
            else if (value is ValueType)
                return value.ToString();
            else if (value is IEnumerable enumerableValue)
                return CollectionToString(enumerableValue);
            else
                return "{{{0}}}".FormatWith(value.ToString());
        }

        private static string BuildVerificationConstraintMessage<TData, TOwner>(IDataVerificationProvider<TData, TOwner> should, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                string formattedMessage;

                if (args != null && args.Any())
                {
                    string[] convertedArgs = args.
                        Select(x => $"\"{should.DataProvider.ConvertValueToString(x) ?? NullString}\"").
                        ToArray();

                    formattedMessage = message.FormatWith(convertedArgs);
                }
                else
                {
                    formattedMessage = message;
                }

                return $"{should.GetShouldText()} {formattedMessage}";
            }
            else
            {
                return null;
            }
        }

        public static TOwner ThrowSoftAsserts<TOwner>(this IPageObject<TOwner> page)
            where TOwner : PageObject<TOwner>
        {
            AtataContext.Current.Log.Info($"ThrowSoftAssert: Exceptions count is {ExceptionResults.Count}");
            if (ExceptionResults.Count>0) {
                Exception ex = new Exception(ExceptionResults.Count.ToString()+" exceptions: \n" +ExceptionResults.Aggregate((i, j) => i + "\n" + j));
                ExceptionResults.Clear();
                throw ex;
            }
            return page.Owner;
        }

        public static TOwner SatisfySoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, Predicate<TData> predicate, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            string verificationConstraintMessage = BuildVerificationConstraintMessage(should, message, args);

            AtataContext.Current.Log.Start(new VerificationLogSection(should.DataProvider.Component, should.DataProvider.ProviderName, verificationConstraintMessage));

            TData actual = default(TData);

            bool doesSatisfy = AtataContext.Current.Driver.Try().Until(
                _ =>
                {
                    actual = should.DataProvider.Value;
                    return predicate(actual) != should.IsNegation;
                },
                should.GetRetryOptions());

            if (!doesSatisfy)
            {
                Exception ex = CreateAssertionException(should, actual, message, args);
                ExceptionResults.AddExecptionMessage(ex.Message);
            }

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        public static TOwner SatisfySoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, Predicate<IEnumerable<TData>> predicate, string message, params TData[] args)
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            string expectedMessage = (args != null && args.Any()) ? message?.FormatWith(CollectionToString(args)) : message;
            string verificationConstraintMessage = $"{should.GetShouldText()} {expectedMessage}";
                        AtataContext.Current.Log.Start(new VerificationLogSection(should.DataProvider.Component, should.DataProvider.ProviderName, verificationConstraintMessage));

            IEnumerable<TData> actual = null;

            bool doesSatisfy = AtataContext.Current.Driver.Try().Until(
                _ =>
                {
                    actual = should.DataProvider.Value?.Select(x => x.Value).ToArray();
                    return predicate(actual) != should.IsNegation;
                },
                should.GetRetryOptions());

            if (!doesSatisfy)
            {
                Exception ex = should.CreateAssertionException(expectedMessage, CollectionToString(actual));
                ExceptionResults.AddExecptionMessage(ex.Message);
            }

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        private static List<string> AddExecptionMessage(this List<string> list, string message) {
            list.Add((list.Count + 1).ToString() + ") Exception message: " + message);
            return list;
        }

        public static TOwner EqualSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => Equals(actual, expected), "equal {0}", expected);
        }

        public static TOwner BeTrueSoft<TOwner>(this IDataVerificationProvider<bool, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual, "be true");
        }

        public static TOwner BeTrueSoft<TOwner>(this IDataVerificationProvider<bool?, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual == true, "be true");
        }

        public static TOwner BeFalseSoft<TOwner>(this IDataVerificationProvider<bool, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => !actual, "be false");
        }

        public static TOwner BeFalseSoft<TOwner>(this IDataVerificationProvider<bool?, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual == false, "be false");
        }

        public static TOwner BeNullSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => Equals(actual, null), "be null");
        }

        public static TOwner BeNullOrEmptySoft<TOwner>(this IDataVerificationProvider<string, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => string.IsNullOrEmpty(actual), "be null or empty");
        }

        public static TOwner BeNullOrWhiteSpaceSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => string.IsNullOrWhiteSpace(actual), "be null or white-space");
        }

        public static TOwner EqualIgnoringCaseSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => string.Equals(expected, actual, StringComparison.CurrentCultureIgnoreCase), "equal {0} ignoring case", expected);
        }

        public static TOwner ContainSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.SatisfySoft(actual => actual != null && actual.Contains(expected), "contain {0}", expected);
        }

        public static TOwner ContainIgnoringCaseSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.SatisfySoft(actual => actual != null && actual.ToUpper().Contains(expected.ToUpper()), "contain {0} ignoring case", expected);
        }

        public static TOwner StartWithSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.SatisfySoft(actual => actual != null && actual.StartsWith(expected), "start with {0}", expected);
        }

        public static TOwner StartWithIgnoringCaseSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.SatisfySoft(actual => actual != null && actual.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase), "start with {0} ignoring case", expected);
        }

        public static TOwner EndWithSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.SatisfySoft(actual => actual != null && actual.EndsWith(expected), "end with {0}", expected);
        }

        public static TOwner EndWithIgnoringCaseSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNull(nameof(expected));

            return should.SatisfySoft(actual => actual != null && actual.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase), "end with {0} ignoring case", expected);
        }

        public static TOwner MatchSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, string pattern)
            where TOwner : PageObject<TOwner>
        {
            pattern.CheckNotNull(nameof(pattern));

            return should.SatisfySoft(actual => actual != null && Regex.IsMatch(actual, pattern), $"match pattern \"{pattern}\"");
        }

        public static TOwner BeGreaterSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeGreaterSoft<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.Value.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeGreaterOrEqualSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);
        }

        public static TOwner BeGreaterOrEqualSoft<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.Value.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);
        }

        public static TOwner BeLessSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.CompareTo(expected) < 0, "be less than {0}", expected);
        }

        public static TOwner BeLessSoft<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.Value.CompareTo(expected) < 0, "be less than {0}", expected);
        }

        public static TOwner BeLessOrEqualSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);
        }

        public static TOwner BeLessOrEqualSoft<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.Value.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);
        }

        public static TOwner BeInRangeSoft<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData from, TData to)
            where TData : IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && actual.CompareTo(from) >= 0 && actual.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }

        public static TOwner BeInRangeSoft<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData from, TData to)
            where TData : struct, IComparable<TData>, IComparable
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && actual.Value.CompareTo(from) >= 0 && actual.Value.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }

        public static TOwner BeGreaterDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => DateTime.Compare(actual, expected)>0, "be greater than date {0}", expected);
        }

        public static TOwner BeGreaterDateTime<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => DateTime.Compare(actual, expected) > 0, "be greater than date {0}", expected);
        }

        public static TOwner BeGreaterDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && (DateTime.Compare(actual.Value, expected) > 0), "be greater than date {0}", expected);
        }

        public static TOwner BeGreaterDateTime<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && DateTime.Compare(actual.Value, expected) > 0, "be greater than date {0}", expected);
        }

        public static TOwner BeGreaterOrEqualDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => DateTime.Compare(actual, expected) >= 0, "be greater or equal than date {0}", expected);
        }

        public static TOwner BeGreaterOrEqualDateTime<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => DateTime.Compare(actual, expected) >= 0, "be greater or equal than date {0}", expected);
        }

        public static TOwner BeGreaterOrEqualDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && (DateTime.Compare(actual.Value, expected) >= 0), "be greater or equal than date {0}", expected);
        }

        public static TOwner BeGreaterOrEqualDateTime<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && DateTime.Compare(actual.Value, expected) >= 0, "be greater or equal than date {0}", expected);
        }

        public static TOwner BeLessDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => DateTime.Compare(actual, expected) < 0, "be less than date {0}", expected);
        }

        public static TOwner BeLessDateTime<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => DateTime.Compare(actual, expected) < 0, "be less than date {0}", expected);
        }

        public static TOwner BeLessDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && (DateTime.Compare(actual.Value, expected) < 0), "be less than date {0}", expected);
        }

        public static TOwner BeLessDateTime<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && DateTime.Compare(actual.Value, expected) < 0, "be less than date {0}", expected);
        }

        public static TOwner BeLessOrEqualDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => DateTime.Compare(actual, expected) <= 0, "be less or equal than date {0}", expected);
        }

        public static TOwner BeLessOrEqualDateTime<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => DateTime.Compare(actual, expected) <= 0, "be less or equal than date {0}", expected);
        }

        public static TOwner BeLessOrEqualDateTimeSoft<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && (DateTime.Compare(actual.Value, expected) <= 0), "be less or equal than date {0}", expected);
        }

        public static TOwner BeLessOrEqualDateTime<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(actual => actual != null && DateTime.Compare(actual.Value, expected) <= 0, "be less or equal than date {0}", expected);
        }

        public static TOwner EqualDateSoft<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => Equals(actual.Date, expected.Date), "equal date {0}", expected);
        }

        public static TOwner EqualDateSoft<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && Equals(actual.Value.Date, expected.Date), "equal date {0}", expected);
        }

        public static TOwner MatchAnySoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            var predicate = match.GetPredicate();

            string message = new StringBuilder().
                Append($"{match.GetShouldText()} ").
                AppendIf(expected.Length > 1, "any of: ").
                AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}")).
                ToString();

            return should.SatisfySoft(actual => actual != null && expected.Any(x => predicate(actual, x)), message, expected);
        }

        public static TOwner ContainAllSoft<TOwner>(this IDataVerificationProvider<string, TOwner> should, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            string message = new StringBuilder().
                Append($"contain ").
                AppendIf(expected.Length > 1, "all of: ").
                AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}")).
                ToString();

            return should.SatisfySoft(actual => actual != null && expected.All(x => actual.Contains(x)), message, expected);
        }

        public static TOwner BeEmptySoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft( actual => actual != null && !actual.Any(), "be empty");
        }

        public static TOwner HaveCountSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, int expected)
            where TOwner : PageObject<TOwner>
        {
            return should.SatisfySoft(actual => actual != null && actual.Count() == expected, $"have count {expected}");
        }

        public static TOwner BeEquivalentSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {CollectionToString(expected)}");
        }

        public static TOwner BeEquivalentSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {CollectionToString(expected)}");
        }

        public static TOwner EqualSequenceSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {CollectionToString(expected)}");
        }

        public static TOwner EqualSequenceSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {CollectionToString(expected)}");
        }

        public static TOwner ContainSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && should.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {CollectionToString(expected)}");
        }

        public static TOwner ContainSoft<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<TData, TOwner>>, TOwner> should, params TData[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && should.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {CollectionToString(expected)}");
        }

        public static TOwner ContainSoft<TOwner>(this IDataVerificationProvider<IEnumerable<string>, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && should.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {CollectionToString(expected)}");
        }

        public static TOwner ContainSoft<TOwner>(this IDataVerificationProvider<IEnumerable<IDataProvider<string, TOwner>>, TOwner> should, TermMatch match, params string[] expected)
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual => actual != null && should.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {CollectionToString(expected)}");
        }

        public static TOwner ContainSoft<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, Expression<Func<TControl, bool>> predicateExpression)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            predicateExpression.CheckNotNull(nameof(predicateExpression));
            var predicate = predicateExpression.Compile();

            return should.SatisfySoft(
                actual => actual != null && actual.Any(predicate),
                $"contain \"{UIComponentResolver.ResolveControlName<TControl, TOwner>(predicateExpression)}\" {UIComponentResolver.ResolveControlTypeName<TControl>()}");
        }

        public static TOwner ContainHavingContentSoft<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, TermMatch match, params string[] expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.SatisfySoft(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Content.Value).ToArray();
                    return should.IsNegation
                        ? expected.Any(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                        : expected.All(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)));
                },
                $"contain having content that {match.ToString(TermCase.MidSentence)} {CollectionToString(expected)}");
        }

        public static TOwner BeVisibleSoft<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsVisible.Should.WithSettings(should).BeTrueSoft();
        }

        public static TOwner BeHiddenSoft<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsVisible.Should.WithSettings(should).BeFalse();
        }

        public static TOwner BeEnabledSoft<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsEnabled.Should.WithSettings(should).BeTrueSoft();
        }

        public static TOwner BeDisabledSoft<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsEnabled.Should.WithSettings(should).BeFalseSoft();
        }

        public static TOwner BeReadOnlySoft<TData, TControl, TOwner>(this IFieldVerificationProvider<TData, TControl, TOwner> should)
            where TControl : EditableField<TData, TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsReadOnly.Should.WithSettings(should).BeTrueSoft();
        }

        public static TOwner BeCheckedSoft<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Should.WithSettings(should).BeTrueSoft();
        }

        public static TOwner BeUncheckedSoft<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Should.WithSettings(should).BeFalseSoft();
        }

        public static TOwner HaveClassSoft<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should, params string[] classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Attributes.Class.Should.WithSettings(should).ContainSoft(classNames);
        }

        public static TOwner ExistSoft<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            string expectedMessage = "exist";

            AtataContext.Current.Log.Start(new VerificationLogSection(should.Component, $"{should.GetShouldText()} {expectedMessage}"));

            SearchOptions searchOptions = new SearchOptions
            {
                IsSafely = false,
                Timeout = should.Timeout ?? AtataContext.Current.VerificationTimeout,
                RetryInterval = should.RetryInterval ?? AtataContext.Current.VerificationRetryInterval
            };

            try
            {
                StaleSafely.Execute(
                    options =>
                    {
                        if (should.IsNegation)
                            should.Component.Missing(options);
                        else
                            should.Component.Exists(options);
                    },
                    searchOptions);
            }
            catch (Exception exception)
            {
                AtataContext.Current.Log.Info("NOT Exists!");
                StringBuilder messageBuilder = new StringBuilder().
                    Append($"Invalid {should.Component.ComponentFullName} presence.").
                    AppendLine().
                    Append($"Expected: {should.GetShouldText()} {expectedMessage}");

                Exception ex = VerificationUtils.CreateAssertionException(messageBuilder.ToString(), exception);
                ExceptionResults.AddExecptionMessage(ex.Message);
            }
            AtataContext.Current.Log.Info("component styles: "+should.Component.Content);
            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }
    }
}
