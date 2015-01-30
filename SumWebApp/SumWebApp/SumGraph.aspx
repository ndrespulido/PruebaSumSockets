<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SumGraph.aspx.cs" Inherits="SumWebApp.SumGraph" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/StockTicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {

            var dps = []; // dataPoints

            var chart = new CanvasJS.Chart("chartContainer", {
                title: {
                    text: "Sum Servers Updates"
                },
                data: [{
                    type: "line",
                    dataPoints: dps
                }]
            });

            var xVal = 0;
            var yVal = 100;
            var updateInterval = 20;
            var dataLength = 500; // number of dataPoints visible at any point

            var updateChart = function (count) {
                count = count || 1;
                // count is number of times loop runs to generate random dataPoints.

                for (var j = 0; j < count; j++) {
                    yVal = parseInt($("#lblSumServerValue").html());
                    dps.push({
                        x: xVal,
                        y: yVal
                    });
                    xVal++;
                };
                if (dps.length > dataLength) {
                    dps.shift();
                }

                chart.render();

            };

            // generates first set of dataPoints
            updateChart(dataLength);

            // update chart after specified time. 
            setInterval(function () { updateChart() }, updateInterval);

        }
    </script>
    <script type="text/javascript" src="Scripts/canvasjs.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:Button ID="btnStart" runat="server" Text="Start" OnClick="btnStart_Click" />

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

            <asp:Timer ID="tmrUpdate" runat="server" Interval="1000"
                OnTick="tmrUpdate_Tick">
            </asp:Timer>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="tmrUpdate" EventName="Tick" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="lblSumServerValue" runat="server" Text=""></asp:Label>
                   
                </ContentTemplate>
            </asp:UpdatePanel>
             <div id="chartContainer" style="height: 300px; width: 100%;">
                    </div>
        </div>
    </form>
</body>
</html>
