using System.Collections.Generic;
using System.Linq;

namespace AOC.Day05;

public class UpdateProcessor
{
    private readonly IReadOnlyCollection<Rule> _validationRules;

    public UpdateProcessor(IReadOnlyCollection<Rule> validationRules)
    {
        _validationRules = validationRules;
    }

    public List<Update> GetValidUpdates(IReadOnlyCollection<Update> updates)
    {
        List<Update> validUpdates = [];
        foreach (Update update in updates)
        {
            bool isValid = ValidateUpdate(update);

            if (isValid)
                validUpdates.Add(update);
        }

        return validUpdates;
    }

    public List<Update> CorrectInvalidUpdates(IReadOnlyCollection<Update> invalidUpdates)
    {
        List<Update> correctedUpdates = [];

        correctedUpdates.AddRange(invalidUpdates.Select(CorrectUpdate));

        return correctedUpdates;
    }

    private Update CorrectUpdate(Update update)
    {
        List<Page> pages = update.Pages.ToList();

        for (int i = 0; i < pages.Count; i++)
        {
            IReadOnlyCollection<Rule> applicableRules = _validationRules
                                                        .Where(rule => rule.FollowingValue == pages[i].Number)
                                                        .ToList();

            int invalidPageIndex = FindInvalidPageIndex(pages[i], applicableRules, pages);

            if (invalidPageIndex == -1)
                continue;

            Swap(pages[i], pages[invalidPageIndex]);

            //we have to check the swapped element for applicableRules
            i--;
        }

        return new Update(pages);
    }

    private bool ValidateUpdate(Update update)
    {
        List<Page> pages = update.Pages;
        foreach (Page page in pages)
        {
            List<Rule> ruleSet = _validationRules
                                 .Where(rule => rule.FollowingValue == page.Number)
                                 .ToList();

            if (FindInvalidPageIndex(page, ruleSet, pages) != -1)
                return false;
        }

        return true;
    }

    private int FindInvalidPageIndex(Page currentPage, IReadOnlyCollection<Rule> applicableRules, IReadOnlyCollection<Page> allPages)
    {
        List<Page> mutablePages = allPages.ToList();
        int currentPageIndex = mutablePages.IndexOf(currentPage);

        List<int> conflictingPageIndexes = [];
        foreach (IEnumerable<Page> conflictingPages in applicableRules.Select(rule => allPages.Where(page => page.Number == rule.Value)))
        {
            conflictingPageIndexes.AddRange(conflictingPages
                                            .Select(conflictingPage => mutablePages.IndexOf(conflictingPage))
                                            .Where(conflictingPageIndex => currentPageIndex < conflictingPageIndex));
        }

        if (conflictingPageIndexes.Count == 0)
            return -1;

        return conflictingPageIndexes.Max();
    }

    private static void Swap(Page left, Page right)
    {
        (left.Number, right.Number) = (right.Number, left.Number);
    }
}