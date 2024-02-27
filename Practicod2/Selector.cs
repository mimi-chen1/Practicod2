using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Practicod2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new List<string>();
        }

        public static Selector FromQueryString(string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                return null;
            }

            // Check for spaces
            if (queryString.Contains(' '))
            {
                return BuildSelectorTreeWithSpaces(queryString);
            }
            else
            {
                return ParseSelectorString(queryString);
            }
        }

        private static Selector BuildSelectorTreeWithSpaces(string selectorString)
        {
            // Split the selector string into parts
            string[] parts = selectorString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Create the root selector
            Selector rootSelector = null;
            Selector currentSelector = null;

            // Iterate over the parts and build the tree
            foreach (var part in parts)
            {
                Selector childSelector = ParseSelectorString(part);
                if (childSelector != null)
                {
                    if (currentSelector == null)
                    {
                        // First iteration, set the rootSelector
                        rootSelector = childSelector;
                        currentSelector = childSelector;
                    }
                    else
                    {
                        // Set the Child property for the current selector
                        currentSelector.Child = childSelector;
                        currentSelector = childSelector;
                    }
                }
            }

            return rootSelector;
        }

        private static Selector ParseSelectorString(string selectorString)
        {
            if (string.IsNullOrWhiteSpace(selectorString))
            {
                return null;
            }

            Selector selector = new Selector();
            string[] parts = Regex.Split(selectorString, @"(?=[#.])");

            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    if (part.StartsWith("#"))
                    {
                        if (selector.Child != null)
                        {
                            selector.Child.Id = part.Substring(1);
                        }
                        else
                        {
                            selector.Id = part.Substring(1);
                        }
                    }
                    else if (part.StartsWith("."))
                    {
                        // השורה הזו משמשת להוספת כל ה־Classes מה־part ל־List
                        selector.Classes.AddRange(part.Substring(1).Split('.'));
                    }
                    else
                    {
                        selector.TagName = part.ToLower();
                    }
                }
            }

            return selector;
        }


        private static bool IsValidHtmlTag(string tagName)
        {
            // This is a simplified check, but it should be sufficient for most cases.
            return Regex.IsMatch(tagName, "^[a-zA-Z]+$");
        }
    }
}


