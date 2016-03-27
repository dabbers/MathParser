MathParser
==========

C# Math Parser that performs basic calculations and unit conversions. Uses open exchange rates for currency conversion.

This is a C# math parser that reads in human formatted strings of math equations and calculates the result. This is a math calculator that does simple-ish expressions.

It can also do unit conversions. It can convert feet, meters, hours, seconds, etc. 

This solution also includes a small asp.net page that lets you use the math parser library as a Web API. This project also shows how to setup and use Math parser (it's really easy)




            MathParser mp = new MathParser("https://openexchangerates.org/api/latest.json?app_id=REPLACE YOUR ID", Server.MapPath("~"));

            var res = mp.Evaluate(expression);
            Response.Write(mp.GetInterpretation() + " = " + res.ToString());



First you create your math parser, providing the url for currency conversion. Then you call the Evauluate on a string expression (ie: "1 + 1").

The .GetInterpretation() fetches the interpretation for the most recently parsed expression. Calling res.ToString() will convert the value to its reduced form (or unit chosen, ie: 24 inches will be reduced to 2 feet if the desired unit is not specified).


If you find a math bug, please file a new issue with the input query, the output of both GetInterpretation and res.ToString(). Then provide the expected result .


The res.value property is the value of the result in the base unit. For digital capacity (TB to gb to mb etc) that is in bytes. I suppose it makes sense to request the value in its reduced form. That's a todo feature :).



Hopefully this is useful to people!
