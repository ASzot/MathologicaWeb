<%@ Page Title="General Help" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GeneralHelp.aspx.cs" Inherits="MathSolverWebsite.Help.GeneralHelp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <p class="titleText">General Help</p>
    
    <script type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centeredDiv">
        <ul>
            <li class='helpTopicListItem wallOfText'>
                <p class="sectionHeading">Input</p>
                <p>
                    Mathematical expressions are entered into the textbox area on the start page. If at any time you want to navigate back to the 
                    start page simply click the Mathologica logo at the top of the screen. Math can be entered into the text box either through
                    keyboard or the tool bar on the top of the screen. The input is stricly mathematical and cannot be words or sentences. After the 
                    desired input has been entered click the solve button located below the input text box.
                </p>
                <p>
                    The math input uses <a href="http://www.mathquill.com">Mathquill</a> a contextual math expression editor. Fractions will be easily created
                    just by typing the '/' character. Likewise subscripts for variables can be created by typing in the underscore or the '_' character.
                    Fractions, exponents, subscripts, and roots can all be easily manipulated through the use of the editor just as one would with paper and 
                    pencil. A formatted fraction looks like <span class="smallMath">`x/y`</span> as opposed to an unformatted fraction which looks like `x//y`. Both are allowed in input.
                </p>
                <p id="fractionHelpParagraph">
                    Using the toolbar near the top of the input text box can allow for the entering of easy math symbols such as the square root or pi. 
                    By clicking on a symbol the displayed symbol will be inserted into the math expression at the position of the cursor.
                    Clicking on different tabs displays different symbols for use. In the first tab are a variety of operators. Some of the more distinct symbols 
                    will be discussed. There are three seperate division symbols on the operators tab of the toolbar which look different although have the same 
                    functionality. It is the user's preference to choose which one. The <span class="smallMath">`x/y`</span> creates a new formated fraction just as one would with the backslash key 
                    on the keyboard. The `/` inserts the backslash character into the expression making an unformatted fraction. The '/' character cannot be entered by 
                    keyboard to create a unformatted fraction. as that would create a formatted fraction. Finally the `\div` symbol inputs the `\div` symbol into the 
                    math input expression. All three of these options represent division.
                </p> 
                    
                <img src="../Images/Examples/ExampleFormattedFrac.PNG" class="exampleImg" />
                <span class="captionFooter">Formatted fraction.</span>
                <img src="../Images/Examples/ExampleSlashFrac.PNG" class="exampleImg" />
                <span class="captionFooter">Unformatted fraction with slash.</span>
                <img src="../Images/Examples/ExampleDivFrac.PNG" class="exampleImg" />
                <span class="captionFooter">Unformatted fraction with the `\div`</span>

                <p>
                    The `root(n)(x)` symbol creates a new root allowing the user to define the 
                    index of the root. Finally the `\circ` symbol is used for the composition of functions. A detailed description of this operation can be found 
                    <a href="../HelpTopic?Name=Composing%20Functions">here</a>. The symbols tab of the toolbar contains numerous greek characters which can be used
                    for variables or constants in math expressions. The constants tab contains constants which can be used in input. Note that constants such as `\pi` won't 
                    be formatted correctly if they are just typed into the math input text box. Instead it will be displayed as <i>pi</i>. This changes nothing for the evaluation 
                    of the math it just looks worse. To avoid this use the constants under this toolbar tab. Finally are the functions which allow the user to enter a 
                    correctly formatted function. While the user could just type in <i>sin(x)</i> on the keyboard it wouldn't look as nice as the boldface `\sin(x)`. Both are 
                    evaluated the same, one just looks better than the other.
                </p>
                <p>
                    To clear the input in a text box click the square cancel button to the far right of the input text box (circled in blue in the picture below). The clear button located to the right of the evaluation 
                    selection drop down list is used for clearing the result not the input (circled in red in the picture below).
                </p>

                <img src="../Images/Examples/ExampleClear.png" class="exampleImg" />
                <span class="captionFooter">The clear buttons. The one circled in blue is for clearing one equation. The one in red is for clearing the evaluation result.</span>

                <p>
                    An important thing to note about the input text box is that <b>INPUT CANNOT BE COPY AND PASTED IN</b>. This means that the output result from Mathologica cannot be copy and then pasted as input.
                    There are special cases where copy pasting will work, specifically if the copied text came from a Mathquill text box, but don't expect doing so to work.
                </p>
                <p>
                    The drop down list below the input text box displays how the input will be handeled. By clicking on the drop down different evaluation options 
                    can be choosen. For instance, different solve variables can be choosen in equations through this drop down. Below is an example of different evaluation
                    methods being offered. 
                </p>

                <img src="../Images/Examples/ExampleEvaluateSelection.png" class="exampleImg" />
                <span class="captionFooter">The drop down list for the potential evaluation options for a quadratic.</span>

                <p>
                    If it says <b>Invalid input</b> it means the input entered does not have correct formatting. These are statements such as `2*+3` or `(2+3*4`. 
                    If there are definite problems with the <b>Invalid input</b> please report them <a class="fakeLink" onclick="BAROMETER.show();">here</a>. 
                </p>
            </li>
            <li class="helpTopicListItem wallOfText">
                <p class="sectionHeading">System of Equations Input</p>
                <p>
                    To enter systems of equations click the square plus button located to the right of the input text box. This will create a new text box where the user can enter 
                    another equation. To delete one of the equations click the square cancel button to the right of the plus button. Once there are multiple systems of equations they can be
                    solved after selecting one of the evaluation methods from the drop down list. An example of solving a systems of two equations is detailed below.
                </p>

                <img src="../Images/Examples/ExampleSOE_0.PNG" class="exampleImg"/>
                <span class="captionFooter">First enter equation number one for the system of equations. Click the enter new equation button circled in red.</span>

                <img src="../Images/Examples/ExampleSOE_1.PNG" class="exampleImg" />
                <span class="captionFooter">A new equation line will appear.</span>

                <img src="../Images/Examples/ExampleSOE_2.PNG" class="exampleImg" />
                <span class="captionFooter">Next enter the second equation into the new equation line. Now the system of equations can be solved.</span>

                <p>
                    System of equations can also be input just by having ';' characters seperate the equations. Examples are shown <a href="../HelpTopic?Name=System+of+Equations+Elimination%2fSubstitution">here</a>.
                </p>
            </li>
            <li class="helpTopicListItem wallOfText">
                <p class="sectionHeading">Evaluating</p>
                <p>
                    To evaluate input select an option from the drop down menu and click the solve button. A result will then appear below. Work and explanations are displayed with certain problems. Not all 
                    problems solved by Mathologica provide work. For a full list of features in regards to evaluating check out the <a href="../ChangeLogForm">change log</a>. Additionally not all
                    problems can be solved by Mathologica. Problems which cannot be solved will display a message saying they cannot be solved. These messages typically say "Failure". This could imply that Mathologica is incapable of solving 
                    the problem or that the problem is unsolvable by nature. The work can be hidden or displayed by clicking on the work title button located at the top of the work. The result can be cleared by clicking
                    the clear button to the right of the evaluation selection drop down menu. 
                </p>
            </li>
        </ul>
    </div>
</asp:Content>
