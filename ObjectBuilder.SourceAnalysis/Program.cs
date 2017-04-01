using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace ObjectBuilder.SourceAnalysis
{
	class Program
	{
		static void Main2(string[] args)
		{
			Builder builder = new Builder();
			TestObject obj = builder.BuildUp<TestObject>(new Locator(), null, null);
			obj.SayHello();
			Console.ReadLine();
		} 
	}

	public class TestObject
	{
		public void SayHello()
		{
			Console.WriteLine("TEST");
		}
	} 
}
