<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

void Main()
{
	var cd = new ConcurrentDictionary<string, string>();
	cd["a"] = "aaa";
	cd["a"] = "aaa2";

	cd.ContainsKey("a").Dump();
	string result;
	cd.TryRemove("a", out result);
	result.Dump();
	cd.ContainsKey("a").Dump();

}

// Define other methods and classes here
