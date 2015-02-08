<%@ Page Title="Change Log" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangeLogForm.aspx.cs" Inherits="MathSolverWebsite.ChangeLogForm" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <%
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!(b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)))) {
                Response.Write(
                    "<div>" +
                        "<div id='adPanel'>" + 
                            "<script async src='//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js'></script>" + 
                            "<!-- Mathologica Default Ad -->" + 
                            "<ins class='adsbygoogle'" + 
                                    "style='display:inline-block; width:728px; height:90px'" + 
                                    "data-ad-client='ca-pub-3516117000150402'" + 
                                    "data-ad-slot='4392690570'></ins>" + 
                            "<script>" + 
                                "(adsbygoogle = window.adsbygoogle || []).push({});" + 
                            "</script>" + 
                        "</div>" + 
                    "</div>");
            }
        %>


    <p class="pageTitle">Change Log</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centeredDiv">
        <p>
            Below are a list of changes which have been made to the Mathologica program over time. 
            This will include an extensive list of all features. Minor fixes are not included, 
            only additions of major features to the math solving system are.
        </p>

        <p class="sectionHeading">Update #3</p>
        <p class="dateHeading">January 20, 2015</p>
        <ul class="changeLogList">
            <li>Added color themes under the user preferences menu.</li>
            <li>Changed the menu bar at the top to no longer contain a link to the change log.</li>
        </ul>

        <p class="sectionHeading">Update #2</p>
        <p class="dateHeading">December 23, 2014</p>
        <ul class="changeLogList">
            <li>
                Slight changes to the user interface with adding the edit menu.
                <ul>
                    <li>New equation lines are no longer added and deleted through the new line and remove line in the edit menu.</li>
                    <li>The clear button previously next to the evaluation option drop down is no longer there. Instead go to the edit menu and it is clear result.</li>
                </ul>

            </li>
            <li>A major bug fix with persisting data.</li>
        </ul>

        <p class="sectionHeading">A Sum of Change Pushing the Limits</p>
        <p class="dateHeading">December 4, 2014</p>
        <ul class="changeLogList">
            <li>The major changes in this update were derivatives, limits, and summations.</li>
            <li>There were also numerous bug fixes and design changes.</li>
            <li>Check out <a href="/HelpPage.aspx">here</a> for the example topics on many of the added features.</li>
        </ul>

        <p class="sectionHeading">Initial Release</p>
        <p class="dateHeading">November 19, 2014</p>
        <ul class="changeLogList">
            <li>
                Input is with MathQuill and allows for easy manipulation of math expressions. A variety of symbols are supported.
                <ul>
                    <li>Variables can be Greek characters.</li>
                    <li>Variables can have subscripts.</li>
                    <li>Several equations can be input at once with the add line button.</li>
                    <li>Input on a given line can be cleared with the clear button located on the far right of the input box.</li>
                </ul>
            </li>
            <li>
                Ability to solve equality equations for single variables. The different scenarios of solving are detailed below.
                <ul>
                    <li>
                        <b>Quadratics</b> can be solved through a variety of methods. Quadratics are recognized to have further features.
                        <ul>
                            <li>Solve by the quadratic equation.</li>
                            <li>Solve by complete the square.</li>
                            <li>Solve by factoring.</li>
                        </ul>
                    </li>
                    <li><b>Linear equations</b>.</li>
                    <li><b>Absolute value</b> equations can be solved with all valid solutions being returned.</li>
                    <li>
                        <b>Cubic equations</b> can be solved through a couple methods explained below.
                        <ul>
                            <li>Can be solved by factoring in special cases.</li>
                            <li>Solved using synthetic division with the possible rational roots of a polynomial.</li>
                        </ul>
                    </li>
                    <li><b>Exponent equations</b> can be solved in situations where a maximum of two of the variable being solved for are present in a power.</li>
                    <li><b>Factored equations</b> can be solved. The list of factors will be solved independently giving all solutions.</li>
                    <li><b>Fractional equations</b> can be solved, including situations where complex fractions are present.</li>
                    <li><b>Logarithm equations</b> can be solved.</li>
                    <li><b>Radical equations</b> can be solved. This can include multiple radicals and in some cases these radicals can have different root indices.</li>
                    <li>
                        <b>Power equations</b> can be solved. This is where the variable being solved for is raised to an exponent.
                        <ul>
                            <li>Using De Moivre's theorem all of the roots can be calculated.</li>
                        </ul>
                    </li>
                    <li>
                        <b>Trig Equations</b> can be solved. Multiple trig terms can be included. A variety of solve tactics are used.
                        <ul>
                            <li>General solutions.</li>
                            <li>Can work in radians or degrees.</li>
                            <li>Using trig identities to simplify.</li>
                            <li>Using trig identities to cancel.</li>
                        </ul>
                    </li>
                </ul>
            </li>
            <li>
                Can solve inequalities.
                <ul>
                    <li><b>Compound inequalities.</b></li>
                    <li>Regular inequalities.</li>
                    <li>Polynomial inequalities with real rational roots.</li>
                    <li>Absolute value inequalities.</li>
                    <li>Fractional inequalities.</li>
                </ul>
            </li>
            <li>
                Can work with a variety of different input.
                <ul>
                    <li>
                        General equations.
                        <ul>
                            <li>Solving system of equations for a set of variables. Solve method can be determined.</li>
                            <li>Solving single equation for any variable.</li>
                            <li>Finding the domain of any variable.</li>
                        </ul>
                    </li>
                    <li>
                        Quadratics.
                        <ul>
                            <li>Can factor.</li>
                            <li>Find the vertex.</li>
                            <li>Convert to vertex form.</li>
                            <li>Find the zeros. Solve method can be determined.</li>
                            <li>Find the axis of symmetry.</li>
                        </ul>
                    </li>
                    <li>
                        Logarithms.
                        <ul>
                            <li>Compounding and expanding logarithm expressions.</li>
                            <li>Finding the vertical asymptote.</li>
                        </ul>
                    </li>
                    <li>
                        Functions.
                        <ul>
                            <li>Assigning functions.</li>
                            <li>Finding inverses.</li>
                        </ul>
                    </li>
                    <li>
                        Equality checking.
                        <ul>
                            <li>Check if two expressions are equal.</li>
                        </ul>
                    </li>
                    <li>
                        Sinusoidal terms.
                        <ul>
                            <li>Find the period.</li>
                            <li>Find the amplitude.</li>
                            <li>Find the phase shift (horizontal shift).</li>
                        </ul>
                    </li>
                </ul>
            </li>
            <li>
                System of equations.
                <ul>
                    <li>Solve by elimination.</li>
                    <li>Solve by substitution.</li>
                </ul>
            </li>
            <li>
                Functions.
                <ul>
                    <li>Functions can be assigned and used by inputing values to the defined function.</li>
                    <li>Functions can combined with any of the operators and with the composition operator which takes the second function and inputs it to the first.</li>
                    <li>Find the inverse of functions.</li>
                </ul>
            </li>
            <li>
                Simplifying.
                <ul>
                    <li>Using order of operations.</li>
                    <li>Evaluating functions. Includes evaluating user functions.</li>
                    <li>Composition of functions.</li>
                    <li>Expanding and collapsing logarithms.</li>
                    <li>Basic trigonometric identities to simplify.</li>
                    <li>Trigonometric cancellations.</li>
                    <li>Working with imaginary numbers.</li>
                </ul>
            </li>
        </ul>
    </div>


    <%
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!(b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)))) {
                Response.Write(
                    "<div>" +
                        "<div id='adPanel'>" + 
                            "<script async src='//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js'></script>" + 
                            "<!-- Mathologica Default Ad -->" + 
                            "<ins class='adsbygoogle'" + 
                                    "style='display:inline-block; width:728px; height:90px'" + 
                                    "data-ad-client='ca-pub-3516117000150402'" + 
                                    "data-ad-slot='4392690570'></ins>" + 
                            "<script>" + 
                                "(adsbygoogle = window.adsbygoogle || []).push({});" + 
                            "</script>" + 
                        "</div>" + 
                    "</div>");
            }
        %>
</asp:Content>
