using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFGetStarted.AspNetCore.ExistingDb.Utilities
{
    public static class AggregateGrouping
    {
        // TKey combineFn((TKey Key, T Value) PrevKeyItem, T curItem):
        // PrevKeyItem.Key = Previous Key
        // PrevKeyItem.Value = Previous Item
        // curItem = Current Item
        // returns new Key
        // a variation of Aggregate that is a version of the APL scan operator travels 
        // along an IEnumerable returning the intermediate results, but this variation combines a pair at a time, current and previous item
        // Explanation: ScanToPairs goes through the IEnumerable starting with the first and second values and the seedKey value.
        // It passes a ValueTuple containing the current Key and current item and (separately) next item to a combineFn, 
        // and yields a ValueTuple of Key, current item. So the first result is (seedKey, FirstItem). 
        // The second result will be (combineFn((seedKey, FirstItem), SecondItem), SecondItem). And so on.
        public static IEnumerable<(TKey Key, T Value)> ScanToPairs<T, TKey>(this IEnumerable<T> src, TKey seedKey, Func<(TKey Key, T Value), T, TKey> combineFn)
        {
            using (var srce = src.GetEnumerator())
                if (srce.MoveNext())
                {
                    var prevkv = (seedKey, srce.Current);

                    while (srce.MoveNext())
                    {
                        yield return prevkv;
                        prevkv = (combineFn(prevkv, srce.Current), srce.Current);
                    }
                    yield return prevkv;
                }
        }

        // bool testFn(T prevItem, T curItem)
        // returns groups by runs of matching bool
        // Then, a GroupBy operator that groups by testing pairs with a boolean test function
        // Explanation: Using the ScanToPairs method, this method groups the IEnumerable into tuples 
        // where the key is an integer starting with 1 representing the number of the run of true 
        // testFn results from comparing the previous item to the current item. 
        // Once all the runs have been numbered, they are grouped together with GroupBy into groups of the items that belong to a run.
        public static IEnumerable<IGrouping<int, T>> GroupByPairsWhile<T>(this IEnumerable<T> src, Func<T, T, bool> testFn) =>
            src.ScanToPairs(1, (kvp, cur) => testFn(kvp.Value, cur) ? kvp.Key : kvp.Key + 1)
               .GroupBy(kvp => kvp.Key, kvp => kvp.Value);
    }
}
