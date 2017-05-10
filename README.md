# Clippy

Clippy is a set of tools to manipulate data.  It generally works on the text on your clipboard and will modify that text
and place it back on your clipboard.  You will be able to copy your text in *any* text editor, run clippy to modify 
that text, and then paste it right back into your editor with the modifications made.

## Programmers toolbox for text manipulation

### Utilities
#### Clippy

Clippy is the flagship product, it is ran from the command line (run box) and works on the text stored in your
clipboard.  The general syntax is `clippy [command] <parameter1> <parameter2>...`

#### ClippyUtility

ClippyUtility is a GUI product to satisfy users who are afraid of the command line.  Usage should be self-explanatory.
It also works on the text stored in your clipboard.

#### Manip 

Manip is a command line utility that doesn't read the text on your clipboard, it modifies the standard output stream
of your command line.  Usage is through the standard pipe command and would look like 

    $> echo "hello" | manip rep "h" "H"
    Hello
    $>

Or, what has made my life more awesome is piping the buffer from [VIM](http://www.vim.org/) into manip

	:%!manip rep "bad-code" "good-code"
    
### Commands
#### Built In Commands

1. Cap - capitalizes or lower cases
2. Chunk - splits text by number of characters
3. ColumnAlign - Aligns tab delimited text into text-columns
4. Count - Counts the number of characters or lines
5. Dedupe - Deduplicates a list
6. Encode - encode/decode xml/url/base64
7. **Grep** - returns list of matches
8. NewText - get a new GUID, or current date data
9. Math - evaluates simple math expressions
10. **Rep** - replaces regex patterns or sql patterns
11. Reverse - Reverses your text
12. SetSourceData - pass in the literal text you wish to insert, useful for user-defined-functions.
13. Snippet - Obtains a predefined snippet of text
14. Sort - Sorts a list
15. **Insert** - Copy your SQL output with headers and turn it into an  insert statement
16. ToBase - Convert a base10 number to another base level (up to 36)
17. Xml - Pretty print XML (or xml-esque or partial xml)
18. Help - either print the list of functions, or specifics on a particular function if specified.


#### User Defined Functions

Oftentimes you will find yourself running a specific replace or a series of commands many times over. 
One that often turns up is getting a comma separated list of all the numbers on your clipboard.  
Clippy lets you define a user defined function for anything that you find yourself doing more than once.

`clippy --udf` will bring up a function editor, from which you can select previously ran commands, or enter new ones.


