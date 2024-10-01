
//
// Summary:
//     A collection of helper methods for working with collections.
internal static class CollectionHelpers
{
    //
    // Summary:
    //     Computes the hash code for a collection of items.
    //
    // Parameters:
    //   collection:
    //     The collection of items.
    //
    // Type parameters:
    //   T:
    //     The type of items in the collection.
    //
    // Returns:
    //     The computed hash code.
    //
    // Remarks:
    //     This method provides commutative, stable, and order-independent hashing for collections.
    public static int GetHashCode<T>(IEnumerable<T> collection)
    {
        return collection?.Where((T e) => e != null).Aggregate(0, (int current, T element) => current + element.GetHashCode()) ?? 0;
    }

    //
    // Summary:
    //     Determines whether two collections are equal.
    //
    // Parameters:
    //   left:
    //     The first collection.
    //
    //   right:
    //     The second collection.
    //
    //   orderSignificant:
    //     Indicates whether the order of elements is significant in the comparison.
    //
    // Type parameters:
    //   T:
    //     The type of items in the collections.
    //
    // Returns:
    //     True if the collections are equal; otherwise, false.
    public static bool Equals<T>(IEnumerable<T> left, IEnumerable<T> right, bool orderSignificant = false)
    {
        if (left == right)
        {
            return true;
        }

        if (left == null && right != null)
        {
            return false;
        }

        if (left != null && right == null)
        {
            return false;
        }

        if (orderSignificant)
        {
            return left.SequenceEqual(right);
        }

        try
        {
            return left.OrderBy((T l) => l).SequenceEqual(right.OrderBy((T r) => r));
        }
        catch (Exception)
        {
            HashSet<T> hashSet = new HashSet<T>(left);
            HashSet<T> equals = new HashSet<T>(right);
            return hashSet.SetEquals(equals);
        }
    }

    //
    // Summary:
    //     Adds a range of elements to the collection.
    //
    // Parameters:
    //   destination:
    //     The destination collection.
    //
    //   source:
    //     The source collection containing elements to add.
    //
    // Type parameters:
    //   T:
    //     The type of elements in the collection.
    public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
    {
        foreach (T item in source)
        {
            destination.Add(item);
        }
    }
}