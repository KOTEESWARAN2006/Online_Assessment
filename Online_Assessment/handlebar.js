// Use `var` instead of `const` for variable declarations
var Handlebars = require('handlebars');

// Define a simple template
var source = "<p>Hello, {{name}}!</p>";
var template = Handlebars.compile(source);

// Provide data for the template
var data = { name: "koteeswaran" };

// Render the template with data
var result = template(data);

// Output the rendered result
console.log(result);  // Should print: <p>Hello, World!</p>
