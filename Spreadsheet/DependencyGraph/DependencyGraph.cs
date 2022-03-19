//Author: Devin White
using System;
using System.Collections.Generic;
/// <summary>
/// Author: Devin White
/// </summary>
namespace SpreadsheetUtilities
{

	/// <summary>
	/// (s1,t1) is an ordered pair of strings
	/// t1 depends on s1; s1 must be evaluated before t1
	/// 
	/// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
	/// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
	/// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
	/// set, and the element is already in the set, the set remains unchanged.
	/// 
	/// Given a DependencyGraph DG:
	/// 
	///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
	///        (The set of things that depend on s)    
	///        
	///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
	///        (The set of things that s depends on) 
	//
	// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
	//     dependents("a") = {"b", "c"}
	//     dependents("b") = {"d"}
	//     dependents("c") = {}
	//     dependents("d") = {"d"}
	//     dependees("a") = {}
	//     dependees("b") = {"a"}
	//     dependees("c") = {"a"}
	//     dependees("d") = {"b", "d"}
	/// </summary>
	public class DependencyGraph
	{
		private Dictionary<string, HashSet<string>> dependents;
		private Dictionary<string, HashSet<string>> dependees;

		/// <summary>
		/// Creates an empty DependencyGraph.
		/// </summary>
		public DependencyGraph()
		{
			dependents = new Dictionary<string, HashSet<string>>();
			dependees = new Dictionary<string, HashSet<string>>();
		}


		/// <summary>
		/// Returns The number of ordered pairs in the DependencyGraph.
		/// </summary>
		public int Size
		{
			get
			{
				int counter = 0; // counter tokeep track of value size rather than keys
				foreach (HashSet<string> Dependent in dependents.Values)
				{
					counter += Dependent.Count;
				}
				return counter;
			}
		}


		/// <summary>
		/// The size of dependees(s).
		/// This property is an example of an indexer.  If dg is a DependencyGraph, you would
		/// invoke it like this:
		/// dg["a"]
		/// It should return the size of dependees("a")
		/// </summary>
		public int this[string s]
		{
			get
			{
				if (dependees.ContainsKey(s))
				{
					return dependees[s].Count;
				}
				else return 0;
			}
		}

		/// <summary>
		/// Reports whether dependents(s) is non-empty.
		/// </summary>
		public bool HasDependents(string s)
		{
			if (dependents.ContainsKey(s) && dependents[s].Count > 0)
			{
				return true;
			}
			else return false;
		}


		/// <summary>
		/// Reports whether dependees(s) is non-empty.
		/// </summary>
		public bool HasDependees(string s)
		{
			if (dependees.ContainsKey(s) && dependees[s].Count > 0)
			{
				return true;
			}
			else return false;
		}


		/// <summary>
		/// Enumerates dependents(s).
		/// </summary>
		public IEnumerable<string> GetDependents(string s)
		{
			return GetValues(s, dependents);
		}


		/// <summary>
		/// Enumerates dependees(s).
		/// </summary>
		public IEnumerable<string> GetDependees(string s)
		{
			return GetValues(s, dependees);
		}

		/// <summary>
		/// Helper method for Get methods
		/// </summary>
		/// <param name="s">Key of dependents/Dependees to retrieve</param>
		/// <param name="retrievedGraph">The graph to retrieve the values from</param>
		/// <returns>IEnumerable of the provided graph with the provided key</returns>
		private IEnumerable<string> GetValues(string s, Dictionary<string, HashSet<string>> retrievedGraph)
		{
			HashSet<string> valuesofKey = new HashSet<string>();
			if (retrievedGraph.ContainsKey(s))
			{
				foreach (string value in retrievedGraph[s])
				{
					valuesofKey.Add(value);
				}
				return valuesofKey;
			}
			else return new HashSet<string>();
		}


		/// <summary>
		/// Adds the ordered pair (s,t), if it doesn't exist
		/// This should be thought of as:
		///   t depends on s
		/// </summary>
		/// <param name="s"> s must be evaluated first. T depends on S</param>
		/// <param name="t"> t cannot be evaluated until s is</param>        /// 
		public void AddDependency(string s, string t)
		{
			if (dependents.ContainsKey(s))
			{
				dependents[s].Add(t);
			}
			else dependents.Add(s, new HashSet<string>() { t });

			if (dependees.ContainsKey(t))
			{
				dependees[t].Add(s);
			}
			else dependees.Add(t, new HashSet<string>() { s });

			if (!dependees.ContainsKey(s))
			{
				dependees.Add(s, new HashSet<string>());
			}

			if (!dependents.ContainsKey(t))
			{
				dependents.Add(t, new HashSet<string>());
			}
		}


		/// <summary>
		/// Removes the ordered pair (s,t), if it exists
		/// If ordered pair doesn't exist, does nothing.
		/// </summary>
		/// <param name="s"> Dependent to be removed</param>
		/// <param name="t">Dependee to be removed</param>
		public void RemoveDependency(string s, string t)
		{
			if (dependents.ContainsKey(s))
			{
				if (dependents[s].Contains(t))
				{
					dependents[s].Remove(t);
				}
			}

			if (dependees.ContainsKey(t))
			{
				if (dependees[t].Contains(s))
				{
					dependees[t].Remove(s);
					dependents[s].Remove(t);
				}
			}
		}


		/// <summary>
		/// Removes all existing ordered pairs of the form (s,r).  Then, for each
		/// t in newDependents, adds the ordered pair (s,t).
		/// </summary>
		public void ReplaceDependents(string s, IEnumerable<string> newDependents)
		{
			AlterGraphs(s, newDependents, dependents, dependees);
		}

		/// <summary>
		/// Removes all existing ordered pairs of the form (r,s).  Then, for each 
		/// t in newDependees, adds the ordered pair (t,s).
		/// </summary>
		public void ReplaceDependees(string s, IEnumerable<string> newDependees)
		{
			AlterGraphs(s, newDependees, dependees, dependents);
		}
		/// <summary>
		/// Helper Method for replacing values in the graphs
		/// </summary>
		/// <param name="s"> Key where dependencies are replaces</param>
		/// <param name="newValues">Set of new dependencies to add</param>
		/// <param name="graph1">Main Dictionary altered</param>
		/// <param name="graph2">Secondary dictionary altered</param>
		private void AlterGraphs(string s, IEnumerable<string> newValues, Dictionary<string, HashSet<string>> mainGraph, Dictionary<string, HashSet<string>> secondaryGraph)
		{
			if (mainGraph.ContainsKey(s)) // clears the values in key s
			{
				mainGraph[s].Clear();
				foreach (HashSet<string> value in secondaryGraph.Values)
				{
					if (value.Contains(s))
					{
						value.Remove(s);
					}
				}
			}
			else mainGraph.Add(s, new HashSet<String>());

			foreach (string values in newValues) // removes s from any values it's in
			{
				mainGraph[s].Add(values);

				if (secondaryGraph.ContainsKey(values))
				{
					secondaryGraph[values].Add(s);
				}
				else secondaryGraph.Add(values, new HashSet<string> { s });
			}
		}
	}

}