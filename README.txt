----------------------
CSS Assignments
----------------------

This is a ASP.NET MVC Web Application designed for CSS Assignments.
This projects contains both user interface and REST API.

-------------------------
Rule Calculation
-------------------------

There are two approach to run project and calculate rules.
Both approaches require json string type input.

Example input:

[
	{
	"ID": 1956738,
	"variableA": 20,
	"variableB": 33,
	"variableC": 46,
	"variableD": 53
	},
	{
	"ID": 1956739,
	"variableA": 20,
	"variableB": 33,
	"variableC": 46,
	"variableD": 53
	},
	{
	"ID": 1956740,
	"variableA": 20,
	"variableB": 33,
	"variableC": 46,
	"variableD": 53
	}
]

1. Approach: User Interface Usage:

There are two screens which are used for data loading and showing the result list.
When you run the project "Load Data" screen appeares as a default. (Default routing)

	* Enter the input which is in json format and including pre-defined class and variable definitions and press "Load and Calculate" button.
	* This button redirects to the Result List screen which shows the calculated result of each rules according to given operation and entitiy parameters. 

2. Approach: REST API

	* One can call the load and calculate operation as a rest service. This service takes json strint format input mentioned above.

Example usage: #HostName#/api/RuleService/LoadDataAndCalculate

	* This service returns the jsons string contains list of rule results.
