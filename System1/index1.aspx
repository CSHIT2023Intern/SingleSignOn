<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index1.aspx.cs" Inherits="SingleSignOn.index1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="style1.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js"></script>
    <title>System 1's Index</title>

    <script>
        var categories = ["活動資訊", "其他", "課程/考試", "學雜費/獎學金", "總務資訊", "傑出表現", "徵人求才", "人事資訊", "維修/停用", "校園宣導", "圖書館資訊"];
        var places = ["圖書資訊處", "教務處", "秘書室", "總務處", "學生事務處", "高教深耕辦公室", "教學資源暨教師成長中心", "研究發展處", "人事室"];

        function getRandomDate(start, end) {
            var startDate = new Date(start).getTime();
            var endDate = new Date(end).getTime();
            var randomTime = Math.random() * (endDate - startDate) + startDate;
            return new Date(randomTime);
        }

        function formatDate(date) {
            var year = date.getFullYear();
            var month = ("0" + (date.getMonth() + 1)).slice(-2);
            var day = ("0" + date.getDate()).slice(-2);
            return year + "-" + month + "-" + day;
        }

        function generateTable() {
            var table = document.getElementById("myTable");
            var rowsPerPage = 10;

            var currentDate = new Date();
            var endDate = formatDate(currentDate); // 將當前日期格式化

            for (var i = 0; i < rowsPerPage; i++) {
                var row = table.insertRow();
                var categoryCell = row.insertCell();
                var contentCell = row.insertCell();
                var dateCell = row.insertCell();
                var placeCell = row.insertCell();

                var randomIndex = Math.floor(Math.random() * categories.length);
                var randomCategory = categories[randomIndex];
                categoryCell.textContent = randomCategory;

                var randomPlaceIndex = Math.floor(Math.random() * places.length);
                var randomPlace = places[randomPlaceIndex];
                placeCell.textContent = randomPlace;

                var startDate = "2023-06-01";
                var randomDate = getRandomDate(startDate, endDate);
                var formattedDate = formatDate(randomDate);
                dateCell.textContent = formattedDate;
            }
        }

        window.onload = generateTable;
    </script>

</head>
<body style="height: 97.7vh;">

    <form id="form1" runat="server">
        <div style="background-color: black; width: 100%; height: 7%; margin-left: 0px;">
            &nbsp;&nbsp;
        <asp:Label ID="title1" runat="server" Text="學生資訊系統"></asp:Label>

            <asp:Button ID="logoutbtn" runat="server" Text="登出" OnClick="LogoutButton_Click" />
            <asp:Label ID="welcomeLabel" runat="server" Text=""></asp:Label>
        </div>

        <br />

        <!-- Sidebar -->
        <div style="width: 17%; height: 75%; float: left;">
            <div class="accordion" id="accordionExample">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="headingOne">
                        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="false" aria-controls="collapseOne">
                            選課作業
                        </button>
                    </h2>
                    <div id="collapseOne" class="accordion-collapse collapse" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <%--<asp:Button ID="web2btn" runat="server" Text="學生選課系統" OnClick="Web2btn_Click" />--%>
                        </div>
                    </div>
                </div>

                <div class="accordion-item">
                    <h2 class="accordion-header" id="headingTwo">
                        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                            線上學習
                        </button>
                    </h2>
                    <div id="collapseTwo" class="accordion-collapse collapse" aria-labelledby="headingTwo" data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <%--<asp:Button ID="web3btn" runat="server" Text="數位學習系統(LMS)" OnClick="Web3btn_Click" />--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- OuterTab -->
        <div style="width: 81%; height: 100%; float: right;">
            <ul class="nav nav-tabs" id="myTab" role="tablist" style="width: 95%;">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#home" type="button" role="tab" aria-controls="home" aria-selected="true">首頁</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="profile-tab" data-bs-toggle="tab" data-bs-target="#profile" type="button" role="tab" aria-controls="profile" aria-selected="false">個人主頁</button>
                </li>
            </ul>

            <div class="tab-content" id="myTabContent">
                <br />
                <!-- InnerTab - 首頁 -->
                <div class="tab-pane fade show active" id="home" role="tabpanel" aria-labelledby="home-tab">
                    <ul class="nav nav-tabs" id="innerTab" role="tablist" style="width: 95%;">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="announcement-tab" data-bs-toggle="tab" data-bs-target="#announcement" type="button" role="tab" aria-controls="announcement" aria-selected="true">公告(20)</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="todolist-tab" data-bs-toggle="tab" data-bs-target="#todolist" type="button" role="tab" aria-controls="todolist" aria-selected="false" style="color: red;">待辦事項(1)</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="progress-tab" data-bs-toggle="tab" data-bs-target="#progress" type="button" role="tab" aria-controls="progress" aria-selected="false">申請進度(0)</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="alarm-tab" data-bs-toggle="tab" data-bs-target="#alarm" type="button" role="tab" aria-controls="alarm" aria-selected="false" style="color: red;">預警(1)</button>
                        </li>
                    </ul>

                    <!-- InnerTabContent - 首頁 -->
                    <div class="tab-content" id="innerTabContent">
                        <div class="tab-pane fade show active" id="announcement" role="tabpanel" aria-labelledby="announcement-tab">
                            <br />
                            <table id="myTable" class="table table-striped table-hover" style="width: 95%;">
                                <thead>
                                    <tr>
                                        <th scope="col" style="width: 15%;">公告類別</th>
                                        <th scope="col">公告內容</th>
                                        <th scope="col" style="width: 15%;">公告日期</th>
                                        <th scope="col" style="width: 15%;">公告單位</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>

                        <div class="tab-pane fade" id="todolist" role="tabpanel" aria-labelledby="todolist-tab">
                        </div>

                        <div class="tab-pane fade" id="progress" role="tabpanel" aria-labelledby="progress-tab">
                        </div>

                        <div class="tab-pane fade" id="alarm" role="tabpanel" aria-labelledby="alarm-tab">
                        </div>
                    </div>
                </div>

                <!-- InnerTab - 個人主頁 -->
                <div class="tab-pane fade" id="profile" role="tabpanel" aria-labelledby="profile-tab">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <button class="nav-link active" id="todo-tab" data-bs-toggle="tab" data-bs-target="#todo" type="button" role="tab" aria-controls="todo" aria-selected="true">待辦<span class="badge bg-secondary">0</span></button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="handling-tab" data-bs-toggle="tab" data-bs-target="#handling" type="button" role="tab" aria-controls="handling" aria-selected="false">經辦<span class="badge bg-secondary">0</span></button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="notice-tab" data-bs-toggle="tab" data-bs-target="#notice" type="button" role="tab" aria-controls="notice" aria-selected="false">通知<span class="badge bg-secondary">0</span></button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="timeout-tab" data-bs-toggle="tab" data-bs-target="#timeout" type="button" role="tab" aria-controls="timeout" aria-selected="false">逾時<span class="badge bg-secondary">0</span></button>
                        </li>
                    </ul>

                    <!-- InnerTabContent - 個人主頁 -->
                    <div class="tab-content" id="innerprofileTabContent">
                        <div class="tab-pane fade show active" id="todo" role="tabpanel" aria-labelledby="todo-tab">
                        </div>

                        <div class="tab-pane fade" id="handling" role="tabpanel" aria-labelledby="handling-tab">
                        </div>

                        <div class="tab-pane fade" id="notice" role="tabpanel" aria-labelledby="notice-tab">
                        </div>

                        <div class="tab-pane fade" id="timeout" role="tabpanel" aria-labelledby="timeout-tab">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.min.js"></script>
</body>
</html>
