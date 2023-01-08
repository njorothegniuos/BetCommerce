using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Core.web.Mvc.Utils.Infrastructure
{
    public class PasswordStore
    {
        private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();
        private static readonly char[] StartingChars = new char[] { '<', '&' };

        /// <summary>Generates a random password of the specified length.</summary>
        /// <returns>A random password of the specified length.</returns>
        /// <param name="length">The number of characters in the generated password. The length must be between 1 and 128 characters. </param>
        /// <param name="numberOfNonAlphanumericCharacters">The minimum number of non-alphanumeric characters (such as @, #, !, %, &amp;, and so on) in the generated password.</param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="length" /> is less than 1 or greater than 128 -or-<paramref name="numberOfNonAlphanumericCharacters" /> is less than 0 or greater than <paramref name="length" />. </exception>
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            if (length < 1 || length > 128)
                throw new ArgumentException("password_length_incorrect", "length");
            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
                throw new ArgumentException("min_required_non_alphanumeric_characters_incorrect", "numberOfNonAlphanumericCharacters");
            string s;
            int matchIndex;
            do
            {
                var data = new byte[length];
                var chArray = new char[length];
                var num1 = 0;
                new RNGCryptoServiceProvider().GetBytes(data);
                for (var index = 0; index < length; ++index)
                {
                    var num2 = (int)data[index] % 87;
                    if (num2 < 10)
                        chArray[index] = (char)(48 + num2);
                    else if (num2 < 36)
                        chArray[index] = (char)(65 + num2 - 10);
                    else if (num2 < 62)
                    {
                        chArray[index] = (char)(97 + num2 - 36);
                    }
                    else
                    {
                        chArray[index] = Punctuations[num2 - 62];
                        ++num1;
                    }
                }
                if (num1 < numberOfNonAlphanumericCharacters)
                {
                    var random = new Random();
                    for (var index1 = 0; index1 < numberOfNonAlphanumericCharacters - num1; ++index1)
                    {
                        int index2;
                        do
                        {
                            index2 = random.Next(0, length);
                        }
                        while (!char.IsLetterOrDigit(chArray[index2]));
                        chArray[index2] = Punctuations[random.Next(0, Punctuations.Length)];
                    }
                }
                s = new string(chArray);
            }
            while (IsDangerousString(s, out matchIndex));
            return s;
        }

        internal static bool IsDangerousString(string s, out int matchIndex)
        {
            //bool inComment = false;
            matchIndex = 0;

            for (var i = 0; ;)
            {

                // Look for the start of one of our patterns 
                var n = s.IndexOfAny(StartingChars, i);

                // If not found, the string is safe
                if (n < 0) return false;

                // If it's the last char, it's safe 
                if (n == s.Length - 1) return false;

                matchIndex = n;

                switch (s[n])
                {
                    case '<':
                        // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                        if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?') return true;
                        break;
                    case '&':
                        // If the & is followed by a #, it's unsafe (e.g. &#83;) 
                        if (s[n + 1] == '#') return true;
                        break;
                }

                // Continue searching
                i = n + 1;
            }
        }

        private static bool IsAtoZ(char c)
        {
            if ((int)c >= 97 && (int)c <= 122)
                return true;
            if ((int)c >= 65)
                return (int)c <= 90;
            return false;
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
				"abcdefghijkmnopqrstuvwxyz",    // lowercase
				"0123456789",                   // digits
				"!@$?_-"                        // non-alphanumeric
			};
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                                      || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }

    public class PasswordOptions
    {
        /// <summary>Minimum required length</summary>
        public int RequiredLength { get; set; }

        /// <summary>Require a non letter or digit character</summary>
        public bool RequireNonLetterOrDigit { get; set; }

        /// <summary>Require a lower case letter ('a' - 'z')</summary>
        public bool RequireLowercase { get; set; }

        /// <summary>Require an upper case letter ('A' - 'Z')</summary>
        public bool RequireUppercase { get; set; }

        /// <summary>Require a digit ('0' - '9')</summary>
        public bool RequireDigit { get; set; }

        public bool RequireNonAlphanumeric { get; set; }

        public int RequiredUniqueChars { get; set; }
    }
}
